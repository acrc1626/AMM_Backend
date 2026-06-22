/**
 * censos_load.js — Carga de endpoints críticos de Censos
 *
 * Propósito: validar que GET /api/Censos y POST /api/Censos cumplen los SLOs
 * bajo carga normal (10 VUs) y carga media (50 VUs).
 *
 * Ejecución:
 *   k6 run --env AMM_PASS=<password> perf/scripts/censos_load.js
 *   k6 run --env BASE_URL=https://api.prod.com --env AMM_PASS=<pass> perf/scripts/censos_load.js
 */

import http from 'k6/http';
import { check, group, sleep } from 'k6';
import { Counter, Rate, Trend } from 'k6/metrics';

// ── Configuración ──────────────────────────────────────────────────────────────
const BASE_URL = __ENV.BASE_URL || 'https://localhost:7043';
const CREDS = {
  correo:   __ENV.AMM_USER || 'qa@amm.local',
  password: __ENV.AMM_PASS || '2d6afed9-f9f5-483a-a987-c8ae1024d09f',
};

// ── Métricas personalizadas ────────────────────────────────────────────────────
const censosCreados      = new Counter('censos_creados_total');
const tasaErroresCensos  = new Rate('tasa_errores_censos');
const latenciaCensoGet   = new Trend('latencia_censo_get_ms',  true);
const latenciaCensoPost  = new Trend('latencia_censo_post_ms', true);

// ── Escenarios y SLOs ─────────────────────────────────────────────────────────
export const options = {
  insecureSkipTLSVerify: true,

  scenarios: {
    // Escenario 1: carga normal — 10 VUs, ramp-up 30 s, duración 60 s
    carga_normal: {
      executor:  'ramping-vus',
      startTime: '0s',
      stages: [
        { duration: '30s', target: 10 },  // ramp-up
        { duration: '60s', target: 10 },  // sostenido
        { duration: '5s',  target: 0  },  // ramp-down
      ],
      gracefulRampDown: '5s',
    },

    // Escenario 2: carga media — 50 VUs, ramp-up 10 s, duración 60 s
    // Arranca cuando termina carga_normal (30+60+5 = 95 s)
    carga_media: {
      executor:  'ramping-vus',
      startTime: '95s',
      stages: [
        { duration: '10s', target: 50 },  // ramp-up agresivo
        { duration: '60s', target: 50 },  // sostenido
        { duration: '5s',  target: 0  },  // ramp-down
      ],
      gracefulRampDown: '5s',
    },
  },

  thresholds: {
    // SLOs principales
    http_req_duration:     ['p(95)<300', 'p(99)<800'],
    // Tasa de error HTTP < 1 %
    http_req_failed:       ['rate<0.01'],
    // Checks deben pasar > 99 %
    checks:                ['rate>0.99'],
    // Al menos un censo creado por escenario
    censos_creados_total:  ['count>0'],
    // Errores de negocio < 1 %
    tasa_errores_censos:   ['rate<0.01'],
    // Tendencias individuales por endpoint
    latencia_censo_get_ms:  ['p(95)<300'],
    latencia_censo_post_ms: ['p(95)<300'],
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
// Obtiene el token JWT compartido por todos los VUs.
// ─────────────────────────────────────────────────────────────────────────────
export function setup() {
  // Arrange
  const payload = JSON.stringify(CREDS);

  // Act
  const res = http.post(`${BASE_URL}/api/auth/login`, payload, BASE_PARAMS);

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

  console.log(`✅ Login OK. Token expira: ${res.json('expira')}. Escenarios listos.`);
  return { token: res.json('token') };
}

// ─────────────────────────────────────────────────────────────────────────────
// default(): ejecutado por cada VU en cada iteración.
// Flujo: GET /api/censos/paged → POST /api/Censos
// ─────────────────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) {
    console.error('Sin token — abortando iteración');
    return;
  }

  const ap = authParams(data.token);

  // ── 1. Consulta de censos ──────────────────────────────────────────────────
  group('censos-get', () => {
    // Arrange — headers ya incluyen Bearer token

    // Act — endpoint paginado para evitar full-table-scan (DEF-CARGA-001)
    const res = http.get(`${BASE_URL}/api/censos/paged?page=1&pageSize=20`, ap);
    latenciaCensoGet.add(res.timings.duration);

    // Assert — PagedResult<CensoDto>: { items: [], total, page, pageSize }
    const ok = check(res, {
      '[censos/get] 200':        (r) => r.status === 200,
      '[censos/get] items array': (r) => Array.isArray(r.json('items')),
    });

    if (!ok) tasaErroresCensos.add(1);
  });

  sleep(0.3);

  // ── 2. Creación de censo ───────────────────────────────────────────────────
  group('censos-post', () => {
    // Arrange
    const body = JSON.stringify({
      tipoEntornoId: 1,
      pacienteId:    1,
      ubicacionId:   null,
      departamentoId: null,
      municipioId:   null,
      fecha:         '2026-06-17T00:00:00.000Z',
      observacion:   `Censo k6 VU-${__VU}-I-${__ITER}`,
      estadoId:      1,
    });

    // Act
    const res = http.post(`${BASE_URL}/api/Censos`, body, ap);
    latenciaCensoPost.add(res.timings.duration);

    // Assert
    const ok = check(res, {
      '[censos/post] 201':        (r) => r.status === 201,
      '[censos/post] id asignado': (r) => r.json('id') > 0,
      '[censos/post] paciente ok': (r) => r.json('pacienteId') === 1,
    });

    if (ok) censosCreados.add(1);
    else     tasaErroresCensos.add(1);
  });

  sleep(0.5);
}

// ─────────────────────────────────────────────────────────────────────────────
// teardown(): se ejecuta UNA vez al finalizar todos los VUs.
// ─────────────────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — censos_load.js');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  Escenario 1 : carga_normal  → 10 VUs, ramp 30s, dur 60s');
  console.log('  Escenario 2 : carga_media   → 50 VUs, ramp 10s, dur 60s');
  console.log('  Token activo: ' + (data?.token ? 'SÍ' : 'NO ❌'));
  console.log('  SLOs evaluados:');
  console.log('    p(95) < 300 ms   → http_req_duration');
  console.log('    p(99) < 800 ms   → http_req_duration');
  console.log('    error rate < 1%  → http_req_failed');
  console.log('    checks   > 99%   → checks');
  console.log('  Exportar JSON:');
  console.log('    k6 run --out json=perf/results/censos_load.json perf/scripts/censos_load.js');
  console.log('═══════════════════════════════════════════════════════════');
}
