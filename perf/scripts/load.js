/**
 * load.js — Carga normal (ramp-up → 40 VUs → ramp-down, ~9 min)
 *
 * Propósito: simular la carga concurrente esperada en producción.
 * Valida que el sistema cumple los SLOs bajo uso cotidiano.
 *
 * Ejecución:
 *   k6 run --env AMM_PASS=<password> perf/scripts/load.js
 *   k6 run --env BASE_URL=https://api.prod.com --env AMM_PASS=<pass> perf/scripts/load.js
 */

import http from 'k6/http';
import { check, group, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { Counter, Rate, Trend } from 'k6/metrics';

// ── Configuración ─────────────────────────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://localhost:7043';
const CREDS = {
  correo:   __ENV.AMM_USER || 'qa@amm.local',
  password: __ENV.AMM_PASS || '2d6afed9-f9f5-483a-a987-c8ae1024d09f',
};

const pacientes = new SharedArray('pacientes', () =>
  papaparse.parse(open('../data/pacientes.csv'), { header: true, skipEmptyLines: true }).data
);

// ── Métricas personalizadas ───────────────────────────────────────────────────
const escriturasExitosas = new Counter('escrituras_exitosas');
const tasaErrores        = new Rate('tasa_errores_negocio');
const latenciaEscritura  = new Trend('latencia_escritura_ms', true);

// ── Escenario de carga: ramp-up gradual ───────────────────────────────────────
export const options = {
  insecureSkipTLSVerify: true,

  stages: [
    { duration: '2m', target: 10 },  // calentamiento
    { duration: '3m', target: 40 },  // ramp-up a carga normal
    { duration: '4m', target: 40 },  // carga sostenida
    { duration: '2m', target: 0  },  // ramp-down
  ],

  thresholds: {
    http_req_duration:   ['p(95)<300', 'p(99)<800'],
    http_req_failed:     ['rate<0.02'],
    checks:              ['rate>0.95'],
    escrituras_exitosas: ['count>0'],
    tasa_errores_negocio:['rate<0.05'],
  },
};

const BASE_PARAMS = { headers: { 'Content-Type': 'application/json' }, timeout: '15s' };

function authParams(token) {
  return {
    headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
    timeout: '15s',
  };
}

// ── setup ─────────────────────────────────────────────────────────────────────
export function setup() {
  // Arrange
  const payload = JSON.stringify(CREDS);

  // Act
  const res = http.post(`${BASE_URL}/api/auth/login`, payload, BASE_PARAMS);

  // Assert
  check(res, {
    '[setup] login 200':  (r) => r.status === 200,
    '[setup] token ok':   (r) => Boolean(r.json('token')),
  });

  if (res.status !== 200) {
    console.error(`❌ Login falló: ${res.status}`);
    return { token: null };
  }

  console.log(`✅ Token obtenido. Iniciando carga con ${pacientes.length} filas CSV.`);
  return { token: res.json('token') };
}

// ── default ───────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) return;

  const ap = authParams(data.token);

  // ── Flujo de lectura (70% del tráfico real) ──────────────────────────────────
  group('lectura', () => {
    // Arrange — batch de catálogos + listados
    const reqs = [
      ['GET', `${BASE_URL}/api/pacientes`,    null, ap],
      ['GET', `${BASE_URL}/api/censos`,       null, ap],
      ['GET', `${BASE_URL}/api/estados`,      null, ap],
    ];

    // Act
    const [pacRes, cenRes, estRes] = http.batch(reqs);

    // Assert
    check(pacRes, { '[load/pacientes] 200': (r) => r.status === 200 });
    check(cenRes, { '[load/censos] 200':    (r) => r.status === 200 });
    check(estRes, { '[load/estados] 200':   (r) => r.status === 200 });
  });

  sleep(0.5);

  // ── Flujo de escritura (30% del tráfico) ─────────────────────────────────────
  if (__ITER % 3 === 0) {
    group('escritura', () => {
      // Arrange
      const fila     = pacientes[(__VU * 7 + __ITER) % pacientes.length];
      const docUnico = `${fila.documento}-v${__VU}i${__ITER}`;

      const body = JSON.stringify({
        tipoDocumentoId:     1,
        documento:           docUnico,
        primerNombre:        fila.nombre,
        primerApellido:      'LoadTest',
        sexoId:              parseInt(fila.vivo) === 1 ? 1 : 2,
        fechaNacimiento:     edadAFecha(parseInt(fila.edad)),
        pertenenciaEtnicaId: parseInt(fila.etnia) > 0 ? parseInt(fila.etnia) : null,
      });

      // Act
      const res = http.post(`${BASE_URL}/api/pacientes`, body, ap);
      latenciaEscritura.add(res.timings.duration);

      // Assert
      const ok = check(res, { '[load/crear] 201': (r) => r.status === 201 });
      if (ok) escriturasExitosas.add(1);
      else tasaErrores.add(1);
    });
  }

  sleep(1 + Math.random());
}

// ── teardown ──────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — load.js (Carga Normal)');
  console.log('  Escenario: ramp-up a 40 VUs, 4 min sostenido');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  Revisar thresholds en la salida de k6.');
  console.log('  Exportar JSON: k6 run --out json=perf/results/load.json ...');
  console.log('═══════════════════════════════════════════════════════════');
}

function edadAFecha(edad) {
  const d = new Date();
  d.setFullYear(d.getFullYear() - edad);
  return d.toISOString().split('T')[0];
}
