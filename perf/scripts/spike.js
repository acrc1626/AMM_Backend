/**
 * spike.js — Prueba de pico (spike súbito 10 → 300 VUs → 10, ~15 min)
 *
 * Propósito: simular un evento de tráfico repentino (ej. jornada de vacunación
 * masiva, alerta epidemiológica). Valida que el sistema se recupera después
 * del pico sin quedarse en estado degradado.
 *
 * Ejecución:
 *   k6 run --env AMM_PASS=<password> perf/scripts/spike.js
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

const erroresPico       = new Counter('errores_durante_pico');
const recuperacionOk    = new Rate('recuperacion_post_pico');

// ── Escenario spike ───────────────────────────────────────────────────────────
export const options = {
  insecureSkipTLSVerify: true,

  stages: [
    { duration: '2m',  target: 10  },  // carga normal de referencia
    { duration: '30s', target: 300 },  // ← PICO súbito
    { duration: '3m',  target: 300 },  // sostener el pico
    { duration: '30s', target: 10  },  // ← caída rápida
    { duration: '5m',  target: 10  },  // recuperación — ¿vuelven los SLOs?
    { duration: '2m',  target: 0   },  // ramp-down
  ],

  thresholds: {
    // SLOs normales: se espera que fallen durante el pico
    http_req_duration: ['p(95)<300', 'p(99)<800'],
    http_req_failed:   ['rate<0.10'],
    checks:            ['rate>0.85'],
    // Clave: la recuperación post-pico debe ser buena
    recuperacion_post_pico: ['rate>0.90'],
  },
};

const BASE_PARAMS = { headers: { 'Content-Type': 'application/json' }, timeout: '20s' };

function authParams(token) {
  return {
    headers: { 'Content-Type': 'application/json', Authorization: `Bearer ${token}` },
    timeout: '20s',
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
    console.error(`❌ Login falló antes del spike: ${res.status}`);
    return { token: null };
  }

  console.log('✅ Spike test listo. El pico ocurrirá a los 2 min.');
  return { token: res.json('token') };
}

// ── default ───────────────────────────────────────────────────────────────────
export default function (data) {
  if (!data?.token) return;

  const ap = authParams(data.token);

  // ── Flujo representativo de un usuario real ───────────────────────────────────
  group('consulta-epidemiologica', () => {
    // Arrange — simula operador que consulta pacientes durante alerta

    // Act
    const res = http.get(`${BASE_URL}/api/pacientes`, ap);

    // Assert
    const ok = check(res, {
      '[spike/pacientes] 200':         (r) => r.status === 200,
      '[spike/pacientes] responde OK':  (r) => r.status < 500,
    });
    if (!ok) erroresPico.add(1);
  });

  sleep(0.2);

  group('registro-caso', () => {
    // Arrange
    const fila     = pacientes[(__VU * 3 + __ITER) % pacientes.length];
    const docUnico = `${fila.documento}-sp${__VU}i${__ITER}`;

    const body = JSON.stringify({
      tipoDocumentoId:     1,
      documento:           docUnico,
      primerNombre:        fila.nombre,
      primerApellido:      'SpikeTest',
      sexoId:              parseInt(fila.vivo) === 1 ? 1 : 2,
      fechaNacimiento:     edadAFecha(parseInt(fila.edad)),
      pertenenciaEtnicaId: null,
    });

    // Act
    const res = http.post(`${BASE_URL}/api/pacientes`, body, ap);

    // Assert — post-pico, monitorear recuperación
    const ok = check(res, {
      '[spike/crear] no 5xx': (r) => r.status !== 500,
      '[spike/crear] 201 o 400': (r) => r.status === 201 || r.status === 400,
    });
    recuperacionOk.add(ok ? 1 : 0);
  });

  sleep(0.5 + Math.random() * 0.3);
}

// ── teardown ──────────────────────────────────────────────────────────────────
export function teardown(data) {
  console.log('');
  console.log('═══════════════════════════════════════════════════════════');
  console.log('  RESUMEN — spike.js');
  console.log('  Analizar:');
  console.log('  1. ¿Cuántos errores durante el pico? (errores_durante_pico)');
  console.log('  2. ¿Se recuperó la latencia p95 post-pico? (stage 5)');
  console.log('  3. ¿La métrica recuperacion_post_pico > 90%?');
  console.log('  Documentar en defectos_rendimiento.md si no cumple.');
  console.log('═══════════════════════════════════════════════════════════');
}

function edadAFecha(edad) {
  const d = new Date();
  d.setFullYear(d.getFullYear() - edad);
  return d.toISOString().split('T')[0];
}
