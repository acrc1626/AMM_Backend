/**
 * soak.js — Prueba de resistencia / soak (15 VUs, 60 min por defecto)
 *
 * Propósito: detectar degradación con el tiempo — memory leaks, connection pool
 * exhaustion, file descriptor leaks o acumulación de datos en cache sin TTL.
 *
 * Nota: por defecto la duración es 60 min. Para una prueba rápida usa:
 *   k6 run --env SOAK_DURATION=10m --env AMM_PASS=<pass> perf/scripts/soak.js
 *
 * Ejecución completa (producción):
 *   k6 run --env SOAK_DURATION=60m --env AMM_PASS=<pass> perf/scripts/soak.js
 */

import http from 'k6/http';
import { check, group, sleep } from 'k6';
import { SharedArray } from 'k6/data';
import papaparse from 'https://jslib.k6.io/papaparse/5.1.1/index.js';
import { Counter, Trend } from 'k6/metrics';

// ── Configuración ─────────────────────────────────────────────────────────────
const BASE_URL      = __ENV.BASE_URL      || 'https://localhost:7043';
const SOAK_DURATION = __ENV.SOAK_DURATION || '60m';
const CREDS = {
  correo:   __ENV.AMM_USER || 'qa@amm.local',
  password: __ENV.AMM_PASS || '2d6afed9-f9f5-483a-a987-c8ae1024d09f',
};

const pacientes = new SharedArray('pacientes', () =>
  papaparse.parse(open('../data/pacientes.csv'), { header: true, skipEmptyLines: true }).data
);

// Métricas para detectar degradación en el tiempo
const latenciaTendencia = new Trend('latencia_tendencia_ms', true);
const escriturasTotal   = new Counter('escrituras_soak_total');

// ── Escenario soak: carga constante baja durante mucho tiempo ─────────────────
export const options = {
  insecureSkipTLSVerify: true,

  stages: [
    { duration: '5m',          target: 15 },  // calentamiento
    { duration: SOAK_DURATION, target: 15 },  // carga sostenida
    { duration: '5m',          target: 0  },  // cierre limpio
  ],

  thresholds: {
    // En soak, los SLOs deben mantenerse ESTABLES durante toda la prueba
    http_req_duration:   ['p(95)<300', 'p(99)<800'],
    http_req_failed:     ['rate<0.01'],
    checks:              ['rate>0.99'],
    escrituras_soak_total: ['count>10'],
  },
};

const BASE_PARAMS = { headers: { 'Content-Type': 'application/json' }, timeout: '10s' };

function authParams(token) {
  return {
    headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
    timeout: '10s',
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

  console.log(`✅ Soak test iniciado. Duración: ${SOAK_DURATION}. VUs: 15`);
  console.log('   Monitorear memoria del servidor durante la prueba.');
  return { token: res.json('token') };
}

// ── default ───────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) return;

  const ap = authParams(data.token);

  // ── Flujo mixto: lectura (60%) + escritura (40%) ─────────────────────────────

  group('lectura-sostenida', () => {
    // Arrange — consultas representativas de uso diario

    // Act
    const pacRes = http.get(`${BASE_URL}/api/pacientes`, ap);
    latenciaTendencia.add(pacRes.timings.duration);

    // Assert
    check(pacRes, { '[soak/pacientes] 200': (r) => r.status === 200 });

    sleep(0.5);

    // Act
    const cenRes = http.get(`${BASE_URL}/api/censos`, ap);
    latenciaTendencia.add(cenRes.timings.duration);

    // Assert
    check(cenRes, { '[soak/censos] 200': (r) => r.status === 200 });
  });

  sleep(1);

  group('escritura-sostenida', () => {
    // Arrange — solo 40% de las iteraciones crean registros
    if (__ITER % 5 !== 0) return;

    const fila     = pacientes[(__VU * 5 + __ITER) % pacientes.length];
    const docUnico = `${fila.documento}-sk${__VU}i${__ITER}`;

    const body = JSON.stringify({
      tipoDocumentoId:     1,
      documento:           docUnico,
      primerNombre:        fila.nombre,
      primerApellido:      'SoakTest',
      sexoId:              1,
      fechaNacimiento:     edadAFecha(parseInt(fila.edad)),
      pertenenciaEtnicaId: null,
    });

    // Act
    const res = http.post(`${BASE_URL}/api/pacientes`, body, ap);
    latenciaTendencia.add(res.timings.duration);

    // Assert
    const ok = check(res, { '[soak/crear] 201': (r) => r.status === 201 });
    if (ok) escriturasTotal.add(1);
  });

  // Think time realista entre peticiones
  sleep(2 + Math.random() * 2);
}

// ── teardown ──────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — soak.js (Resistencia)');
  console.log(`  Duración total: ${SOAK_DURATION}`);
  console.log('  Indicadores de degradación a analizar:');
  console.log('  1. latencia_tendencia_ms: ¿aumentó durante la prueba?');
  console.log('  2. http_req_failed: ¿aumentó hacia el final?');
  console.log('  3. Memoria del servidor: revisar métricas de SO/APM.');
  console.log('  Comparar p95 de la primera hora vs. la última.');
  console.log('═══════════════════════════════════════════════════════════');
}

function edadAFecha(edad) {
  const d = new Date();
  d.setFullYear(d.getFullYear() - edad);
  return d.toISOString().split('T')[0];
}
