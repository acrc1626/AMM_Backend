/**
 * baseline.js — Smoke test (1 VU, 2 min)
 *
 * Propósito: verificar que todos los endpoints críticos responden correctamente
 * antes de ejecutar pruebas de carga mayores. Si este script falla, no ejecutes
 * los demás.
 *
 * Ejecución:
 *   k6 run --env AMM_PASS=<password> perf/scripts/baseline.js
 */

import http from 'k6/http';
import { check, group, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { Counter, Trend } from 'k6/metrics';

// ── Configuración ─────────────────────────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://localhost:7043';
const CREDS = {
  correo:   __ENV.AMM_USER || 'qa@amm.local',
  password: __ENV.AMM_PASS || '2d6afed9-f9f5-483a-a987-c8ae1024d09f',
};

// ── Datos parametrizados (cargados una vez, compartidos entre VUs) ─────────────
const pacientes = new SharedArray('pacientes', () =>
  papaparse.parse(open('../data/pacientes.csv'), { header: true, skipEmptyLines: true }).data
);

// ── Métricas personalizadas ───────────────────────────────────────────────────
const pacientesCreados = new Counter('pacientes_creados_total');
const latenciaLogin    = new Trend('latencia_login_ms', true);

// ── SLOs y configuración del escenario ────────────────────────────────────────
export const options = {
  vus: 1,
  duration: '2m',
  insecureSkipTLSVerify: true,

  thresholds: {
    // SLO principal: p95 < 300 ms, p99 < 800 ms
    http_req_duration:      ['p(95)<300', 'p(99)<800'],
    // Tasa de error HTTP < 1%
    http_req_failed:        ['rate<0.01'],
    // Checks deben pasar al menos 99%
    checks:                 ['rate>0.99'],
    // Ningún error de login en smoke test
    pacientes_creados_total: ['count>0'],
  },
};

// ── Parámetros HTTP base ───────────────────────────────────────────────────────
const BASE_PARAMS = { headers: { 'Content-Type': 'application/json' }, timeout: '10s' };

function authParams(token) {
  return {
    headers: {
      'Content-Type':  'application/json',
      'Authorization': `Bearer ${token}`,
    },
    timeout: '10s',
  };
}

// ─────────────────────────────────────────────────────────────────────────────
// setup(): se ejecuta UNA vez antes de que comiencen los VUs.
// Obtiene el token JWT y lo retorna para que lo usen todos los VUs.
// ─────────────────────────────────────────────────────────────────────────────
export function setup() {
  // Arrange
  const payload = JSON.stringify(CREDS);

  // Act
  const res = http.post(`${BASE_URL}/api/auth/login`, payload, BASE_PARAMS);
  latenciaLogin.add(res.timings.duration);

  // Assert
  const ok = check(res, {
    '[setup] login → 200':        (r) => r.status === 200,
    '[setup] token en respuesta': (r) => Boolean(r.json('token')),
    '[setup] expira definido':    (r) => Boolean(r.json('expira')),
  });

  if (!ok) {
    console.error(`❌ Setup falló. Status: ${res.status}. Body: ${res.body}`);
    return { token: null };
  }

  const token  = res.json('token');
  const expira = res.json('expira');
  const runId = String(Date.now()).slice(-5);
  console.log(`✅ Login OK. Token expira: ${expira}. RunId: ${runId}. VUs listos.`);
  return { token, runId };
}

// ─────────────────────────────────────────────────────────────────────────────
// default(): ejecutado por cada VU en cada iteración.
// ─────────────────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) {
    console.error('Sin token — abortando iteración');
    return;
  }

  const ap = authParams(data.token);

  // ── 1. Identidad del usuario autenticado ────────────────────────────────────
  group('auth', () => {
    // Arrange — el token ya está en ap.headers

    // Act
    const res = http.get(`${BASE_URL}/api/auth/me`, ap);

    // Assert
    check(res, {
      '[auth/me] 200': (r) => r.status === 200,
      '[auth/me] email presente': (r) => Boolean(r.json('email')),
    });
  });

  sleep(0.3);

  // ── 2. Catálogos de referencia (batch paralelo) ──────────────────────────────
  group('catalogos', () => {
    // Arrange
    const endpoints = [
      `${BASE_URL}/api/estados`,
      `${BASE_URL}/api/tiposdocumento`,
      `${BASE_URL}/api/etnias`,
      `${BASE_URL}/api/sexos`,
      `${BASE_URL}/api/tiposentorno`,
    ];

    // Act — requests paralelos
    const responses = http.batch(endpoints.map((url) => ['GET', url, null, ap]));

    // Assert
    responses.forEach((r, i) => {
      check(r, { [`[catalogos][${i}] 200`]: (resp) => resp.status === 200 });
    });
  });

  sleep(0.3);

  // ── 3. Listado de pacientes ──────────────────────────────────────────────────
  group('pacientes-listado', () => {
    // Act
    const res = http.get(`${BASE_URL}/api/pacientes`, ap);

    // Assert
    check(res, {
      '[pacientes] 200':      (r) => r.status === 200,
      '[pacientes] es array': (r) => Array.isArray(r.json()),
    });
  });

  sleep(0.3);

  // ── 4. Creación de paciente (parametrizado con CSV) ──────────────────────────
  group('pacientes-crear', () => {
    // Arrange — fila del CSV único por VU e iteración
    // docUnico incluye runId (5 dígitos del timestamp) para evitar
    // colisiones de unique constraint entre ejecuciones sucesivas.
    // Longitud máx: "CC-PERF-200-99999-9" = 20 chars (límite DTO: 30).
    const fila    = pacientes[(__VU - 1 + __ITER) % pacientes.length];
    const docUnico = `${fila.documento}-${data.runId}-${__ITER}`;

    const body = JSON.stringify({
      tipoDocumentoId:     1,
      documento:           docUnico,
      primerNombre:        fila.nombre,
      primerApellido:      'PerfTest',
      sexoId:              1,
      fechaNacimiento:     edadAFecha(parseInt(fila.edad)),
      pertenenciaEtnicaId: null,
    });

    // Act
    const res = http.post(`${BASE_URL}/api/pacientes`, body, ap);

    // Assert
    const ok = check(res, {
      '[crear-paciente] 201': (r) => r.status === 201,
      '[crear-paciente] id asignado': (r) => r.json('id') > 0,
    });
    if (ok) pacientesCreados.add(1);
  });

  sleep(0.5);

  // ── 5. Censos (lectura paginada) ─────────────────────────────────────────────
  // Se usa el endpoint paginado para evitar full-table-scan (DEF-CARGA-001).
  // Escabiosis excluido del baseline: la tabla ESCABIOSIS no existe en BD —
  // requiere migración pendiente antes de poder habilitarse aquí.
  group('censos', () => {
    const res = http.get(`${BASE_URL}/api/censos/paged?page=1&pageSize=10`, ap);
    check(res, { '[censos/paged] 200': (r) => r.status === 200 });
  });

  sleep(1);
}

// ─────────────────────────────────────────────────────────────────────────────
// teardown(): se ejecuta UNA vez al finalizar todos los VUs.
// ─────────────────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — baseline.js (Smoke Test)');
  console.log('═══════════════════════════════════════════════════════════');
  console.log(`  Token disponible  : ${data?.token ? 'SÍ' : 'NO ❌'}`);
  console.log(`  Datos CSV         : ${pacientes.length} pacientes cargados`);
  console.log('  SLOs evaluados    :');
  console.log('    p(95) < 300 ms  → ver sección "http_req_duration" del reporte');
  console.log('    p(99) < 800 ms  → ver sección "http_req_duration" del reporte');
  console.log('  Resultados        : perf/results/');
  console.log('═══════════════════════════════════════════════════════════');
}

// ── Helper: calcula fecha de nacimiento a partir de edad ─────────────────────
function edadAFecha(edad) {
  const d = new Date();
  d.setFullYear(d.getFullYear() - edad);
  return d.toISOString().split('T')[0];
}
