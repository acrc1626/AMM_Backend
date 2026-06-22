/**
 * stress.js — Prueba de estrés (ramp-up progresivo hasta 200 VUs, ~22 min)
 *
 * Propósito: encontrar el punto de quiebre del sistema. La carga excede
 * deliberadamente la capacidad esperada. Se espera que los SLOs fallen
 * en algún punto; el objetivo es identificar cuándo.
 *
 * Ejecución:
 *   k6 run --env AMM_PASS=<password> perf/scripts/stress.js
 */

import http from 'k6/http';
import { check, group, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { Counter, Rate } from 'k6/metrics';

// ── Configuración ─────────────────────────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://localhost:7043';
const CREDS = {
  correo:   __ENV.AMM_USER || 'qa@amm.local',
  password: __ENV.AMM_PASS || '2d6afed9-f9f5-483a-a987-c8ae1024d09f',
};

const pacientes = new SharedArray('pacientes', () =>
  papaparse.parse(open('../data/pacientes.csv'), { header: true, skipEmptyLines: true }).data
);

const errores5xx = new Counter('errores_5xx');
const tasa5xx    = new Rate('tasa_5xx');

// ── Escenario de estrés progresivo ────────────────────────────────────────────
export const options = {
  insecureSkipTLSVerify: true,

  stages: [
    { duration: '2m', target: 20  },  // calentamiento
    { duration: '3m', target: 50  },  // carga normal
    { duration: '3m', target: 100 },  // primer nivel de estrés
    { duration: '3m', target: 150 },  // estrés elevado
    { duration: '3m', target: 200 },  // estrés máximo
    { duration: '5m', target: 200 },  // sostener para medir degradación
    { duration: '3m', target: 0   },  // recuperación
  ],

  thresholds: {
    // Umbrales más permisivos que en load (buscamos el quiebre, no pasar SLOs)
    http_req_duration: ['p(95)<1000', 'p(99)<3000'],
    http_req_failed:   ['rate<0.15'],
    checks:            ['rate>0.80'],
    tasa_5xx:          ['rate<0.10'],
  },
};

const BASE_PARAMS = { headers: { 'Content-Type': 'application/json' }, timeout: '30s' };

function authParams(token) {
  return {
    headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
    timeout: '30s',
  };
}

// ── setup ─────────────────────────────────────────────────────────────────────
export function setup() {
  // Arrange
  const payload = JSON.stringify(CREDS);

  // Act
  const res = http.post(`${BASE_URL}/api/auth/login`, payload, BASE_PARAMS);

  // Assert
  check(res, { '[setup] login 200': (r) => r.status === 200 });

  if (res.status !== 200) {
    console.error(`❌ Login falló: ${res.status}`);
    return { token: null };
  }

  console.log('✅ Stress test iniciado. Monitoreando degradación...');
  return { token: res.json('token') };
}

// ── default ───────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) return;

  const ap = authParams(data.token);

  // ── Lectura intensiva ─────────────────────────────────────────────────────────
  group('lectura-intensiva', () => {
    // Arrange
    const reqs = [
      ['GET', `${BASE_URL}/api/pacientes`,     null, ap],
      ['GET', `${BASE_URL}/api/censos`,        null, ap],
      ['GET', `${BASE_URL}/api/escabiosis`,    null, ap],
      ['GET', `${BASE_URL}/api/geohelmintiasis`, null, ap],
    ];

    // Act
    const responses = http.batch(reqs);

    // Assert + conteo de 5xx
    responses.forEach((r, i) => {
      const ok = check(r, { [`[stress/batch-${i}] <500`]: (resp) => resp.status < 500 });
      if (!ok) {
        errores5xx.add(1);
        tasa5xx.add(1);
      } else {
        tasa5xx.add(0);
      }
    });
  });

  sleep(0.2);

  // ── Escritura bajo estrés ─────────────────────────────────────────────────────
  group('escritura-estres', () => {
    // Arrange
    const fila     = pacientes[(__VU + __ITER) % pacientes.length];
    const docUnico = `${fila.documento}-s${__VU}i${__ITER}`;

    const body = JSON.stringify({
      tipoDocumentoId:     1,
      documento:           docUnico,
      primerNombre:        fila.nombre,
      primerApellido:      'StressTest',
      sexoId:              1,
      fechaNacimiento:     edadAFecha(parseInt(fila.edad)),
      pertenenciaEtnicaId: null,
    });

    // Act
    const res = http.post(`${BASE_URL}/api/pacientes`, body, ap);

    // Assert
    check(res, { '[stress/crear] no 5xx': (r) => r.status < 500 });
    if (res.status >= 500) errores5xx.add(1);
  });

  sleep(0.3 + Math.random() * 0.5);
}

// ── teardown ──────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — stress.js');
  console.log('  Observar en qué stage comenzaron a aparecer errores 5xx.');
  console.log('  Comparar latencias p95/p99 por etapa en el reporte HTML.');
  console.log('  Documentar punto de quiebre en defectos_rendimiento.md');
  console.log('═══════════════════════════════════════════════════════════');
}

function edadAFecha(edad) {
  const d = new Date();
  d.setFullYear(d.getFullYear() - edad);
  return d.toISOString().split('T')[0];
}
