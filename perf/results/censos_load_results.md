# Resultados — censos_load.js

- **Fecha:** 2026-06-17
- **Duración total:** 2m55s
- **VUs máximos:** 50 (carga_normal: 10 VUs · carga_media: 50 VUs)
- **Script:** `perf/scripts/censos_load.js`
- **Entorno:** `https://localhost:7043`

## Resumen de métricas

| Endpoint              | Estado | p95       | Tasa de éxito |
|-----------------------|--------|-----------|---------------|
| POST /api/Censos      | ✅     | 66ms      | 100%          |
| GET /api/Censos       | ❌     | 10.000ms  | 5.4% (2/416 timeout) |

- **Checks globales:** 6.68% exitosos (58/868)
- **http_req_failed:** 94.62% (405/428 requests)

## Defectos detectados

- [DEF-CARGA-001](../defectos_rendimiento.md#def-carga-001) — GET /api/Censos: 94.6% timeout bajo carga concurrente
