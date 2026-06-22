/**
 * regression.js — Prueba de regresión de rendimiento (5 min, apta para CI)
 *
 * Propósito: detectar regresiones de rendimiento en cada PR o merge a main.
 * Rápido (<5 min), umbrales estrictos, falla rápido si hay degradación.
 * Ejecutar DESPUÉS del baseline y ANTES de mergear cambios significativos.
 *
 * Integración CI (GitHub Actions):
 *   k6 run --env AMM_PASS=${{ secrets.AMM_PERF_PASS }} \
 *          --env BASE_URL=${{ vars.PERF_BASE_URL }} \
 *          perf/scripts/regression.js
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

const regresionesDetectadas = new Counter('regresiones_detectadas');
const tasaExito             = new Rate('tasa_exito_endpoints');

// ── Escenario CI: rápido y determinista ──────────────────────────────────────
export const options = {
  insecureSkipTLSVerify: true,

  stages: [
    { duration: '1m', target: 20 },  // ramp-up
    { duration: '3m', target: 20 },  // carga constante
    { duration: '1m', target: 0  },  // ramp-down
  ],

  // Umbrales estrictos: si algo falla aquí, el CI bloquea el merge
  thresholds: {
    http_req_duration:        ['p(95)<300',  'p(99)<800'],
    http_req_failed:          ['rate<0.01'],
    checks:                   ['rate>0.98'],
    regresiones_detectadas:   ['count<1'],
    tasa_exito_endpoints:     ['rate>0.99'],
  },
};

const BASE_PARAMS = { headers: { 'Content-Type': 'application/json' }, timeout: '10s' };

function authParams(token) {
  return {
    headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
    timeout: '10s',
  };
}

// ── Endpoints críticos a validar en cada regresión ────────────────────────────
const ENDPOINTS_CRITICOS = [
  { label: 'login',        path: '/api/auth/login', auth: false },
  { label: 'me',           path: '/api/auth/me',    auth: true  },
  { label: 'pacientes',    path: '/api/pacientes',  auth: true  },
  { label: 'censos',       path: '/api/censos',     auth: true  },
  { label: 'estados',      path: '/api/estados',    auth: true  },
  { label: 'escabiosis',   path: '/api/escabiosis', auth: true  },
];

// ── setup ─────────────────────────────────────────────────────────────────────
export function setup() {
  // Arrange
  const payload = JSON.stringify(CREDS);

  // Act
  const res = http.post(`${BASE_URL}/api/auth/login`, payload, BASE_PARAMS);

  // Assert
  check(res, { '[setup] login 200': (r) => r.status === 200 });

  if (res.status !== 200) {
    console.error(`❌ Regresión detectada en login: ${res.status}`);
    return { token: null };
  }

  console.log('✅ Regression test iniciado. Umbrales estrictos activos.');
  return { token: res.json('token') };
}

// ── default ───────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) return;

  const ap = authParams(data.token);

  // ── Validar todos los endpoints críticos ─────────────────────────────────────
  group('endpoints-criticos', () => {
    ENDPOINTS_CRITICOS.filter((e) => e.auth).forEach((endpoint) => {
      // Arrange
      const params = endpoint.auth ? ap : BASE_PARAMS;

      // Act
      const res = http.get(`${BASE_URL}${endpoint.path}`, params);

      // Assert
      const ok = check(res, {
        [`[reg/${endpoint.label}] 200`]:    (r) => r.status === 200,
        [`[reg/${endpoint.label}] <300ms`]: (r) => r.timings.duration < 300,
      });

      tasaExito.add(ok ? 1 : 0);
      if (!ok) {
        regresionesDetectadas.add(1);
        console.warn(`⚠️  Regresión en ${endpoint.label}: ${res.status} / ${res.timings.duration}ms`);
      }
    });
  });

  sleep(0.5);

  // ── Operación de escritura (ruta crítica) ─────────────────────────────────────
  group('escritura-critica', () => {
    // Arrange
    const fila     = pacientes[(__VU * 11 + __ITER) % pacientes.length];
    const docUnico = `${fila.documento}-r${__VU}i${__ITER}`;

    const body = JSON.stringify({
      tipoDocumentoId:     1,
      documento:           docUnico,
      primerNombre:        fila.nombre,
      primerApellido:      'RegressionTest',
      sexoId:              1,
      fechaNacimiento:     edadAFecha(parseInt(fila.edad)),
      pertenenciaEtnicaId: null,
    });

    // Act
    const res = http.post(`${BASE_URL}/api/pacientes`, body, ap);

    // Assert
    const ok = check(res, {
      '[reg/crear] 201':         (r) => r.status === 201,
      '[reg/crear] < 300ms':     (r) => r.timings.duration < 300,
    });

    tasaExito.add(ok ? 1 : 0);
    if (!ok) {
      regresionesDetectadas.add(1);
      console.warn(`⚠️  Regresión en POST /pacientes: ${res.status} / ${res.timings.duration}ms`);
    }
  });

  sleep(1);
}

// ── teardown ──────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — regression.js (CI Gate)');
  console.log('  Si regresiones_detectadas > 0 → BLOQUEAR el merge.');
  console.log('  Endpoints validados:');
  ENDPOINTS_CRITICOS.forEach((e) => console.log(`    - ${e.path}`));
  console.log('═══════════════════════════════════════════════════════════');
}

function edadAFecha(edad) {
  const d = new Date();
  d.setFullYear(d.getFullYear() - edad);
  return d.toISOString().split('T')[0];
}
