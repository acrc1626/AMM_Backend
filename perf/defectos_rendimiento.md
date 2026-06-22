# Registro de Defectos de Rendimiento — AMM Backend

Plantilla para documentar hallazgos de las pruebas de carga.
Cada defecto debe incluir el script que lo detectó, las métricas observadas
y los criterios de aceptación (SLOs).

---

## Formato de defecto

| Campo            | Descripción                                                    |
|------------------|----------------------------------------------------------------|
| **ID**           | PERF-NNN (secuencial)                                          |
| **Script**       | baseline / load / stress / spike / soak / regression           |
| **Severidad**    | Crítica / Alta / Media / Baja                                  |
| **Endpoint**     | Ruta HTTP afectada                                             |
| **Observado**    | Métrica real obtenida (ej. p95 = 850 ms)                       |
| **Esperado**     | SLO definido (ej. p95 < 300 ms)                                |
| **VUs**          | Concurrencia al momento del defecto                            |
| **Fecha**        | YYYY-MM-DD                                                     |
| **Estado**       | Abierto / En análisis / Resuelto / Aceptado                    |
| **Notas**        | Contexto adicional, posible causa raíz, PR de solución         |

---

## Defectos registrados

<!-- Copiar el bloque siguiente por cada defecto encontrado -->

<!--
### PERF-001

| Campo       | Valor                                |
|-------------|--------------------------------------|
| Script      | stress.js                            |
| Severidad   | Alta                                 |
| Endpoint    | POST /api/pacientes                  |
| Observado   | p95 = 1 200 ms @ 150 VUs             |
| Esperado    | p95 < 300 ms                         |
| VUs         | 150                                  |
| Fecha       | 2026-06-16                           |
| Estado      | Abierto                              |
| Notas       | La latencia se dispara al superar 120 VUs. Posible lock en la tabla PACIENTE o pool de conexiones agotado. |
-->

## DEF-CARGA-001

- **ID:** DEF-CARGA-001
- **Script:** censos_load.js
- **Endpoint afectado:** GET /api/Censos
- **Escenario:** Carga Normal (10 VUs) y Carga Media (50 VUs)
- **Evidencia:** 
  - Checks exitosos: 6.68% (58/868)
  - GET timeouts: 94.6% (405/416 requests)
  - p95 latencia GET: 10.000ms (techo del timeout)
  - p95 latencia POST: 66ms (sin problemas)
- **Causa raíz:** El endpoint GET /api/Censos retorna 
  todos los registros sin paginación. Bajo concurrencia, 
  la query satura el pool de conexiones de EF Core 
  generando timeouts en cascada.
- **Impacto:** Alto — endpoint principal del flujo 
  epidemiológico. Inutilizable bajo carga moderada.
- **Mejora propuesta:** Implementar paginación 
  GET /api/Censos?page=1&pageSize=20 con índice 
  en columna fecha para reducir el volumen de datos 
  por request y liberar el pool de conexiones.
- **Estado:** 🔴 Identificado — pendiente de corrección

---

## SLOs de referencia

| Métrica                 | Umbral     | Script(s) que lo evalúan          |
|-------------------------|------------|-----------------------------------|
| p95 latencia            | < 300 ms   | todos                             |
| p99 latencia            | < 800 ms   | todos                             |
| Tasa de error HTTP      | < 1%       | baseline, load, regression        |
| Tasa de error HTTP      | < 10%      | stress, spike                     |
| Tasa de checks exitosos | > 99%      | baseline, regression              |
| Recuperación post-pico  | > 90%      | spike                             |

---

## Historial de ejecuciones

| Fecha      | Script      | p95 (ms) | p99 (ms) | Error rate | VUs max | Resultado |
|------------|-------------|----------|----------|------------|---------|-----------|
|            |             |          |          |            |         |           |

---

## Notas de ambiente

- **URL base**: `https://localhost:7043`
- **Usuario de prueba**: `qa@amm.local`
- **Versión k6**: ejecutar `k6 version` antes de cada sesión
- **Exportar resultados**: `k6 run --out json=perf/results/YYYY-MM-DD-<script>.json ...`
