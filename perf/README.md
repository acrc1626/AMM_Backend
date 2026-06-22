# Pruebas de Rendimiento — AMM Backend

Suite de pruebas de carga con [k6](https://k6.io/) para validar los SLOs
del sistema de vigilancia epidemiológica AMM.

---

## Estructura

```
perf/
├── scripts/
│   ├── baseline.js    → Smoke test (1 VU, 2 min) — ejecutar primero
│   ├── load.js        → Carga normal (40 VUs, ~9 min)
│   ├── stress.js      → Estrés progresivo (hasta 200 VUs, ~22 min)
│   ├── spike.js       → Pico súbito (300 VUs en 30 s, ~15 min)
│   ├── soak.js        → Resistencia (15 VUs, 60 min)
│   └── regression.js  → Gate de CI (20 VUs, 5 min)
├── data/
│   └── pacientes.csv  → 200 filas para parametrizar POST /api/pacientes
├── results/           → Reportes JSON/HTML generados (en .gitignore)
└── defectos_rendimiento.md → Registro de hallazgos
```

---

## Prerrequisitos

| Herramienta | Versión mínima | Instalación                            |
|-------------|----------------|----------------------------------------|
| k6          | 2.0+           | `winget install GrafanaLabs.k6`        |
| API corriendo | —            | `dotnet run` en `src/AMM.Api`          |
| Usuario QA  | —              | creado por `DevSeeder` al arrancar     |

Verifica k6:
```bash
k6 version
```

---

## Variables de entorno

| Variable       | Default                                    | Descripción              |
|----------------|--------------------------------------------|--------------------------|
| `BASE_URL`     | `https://localhost:7043`                   | URL base de la API       |
| `AMM_USER`     | `qa@amm.local`                             | Correo del usuario de prueba |
| `AMM_PASS`     | `2d6afed9-f9f5-483a-a987-c8ae1024d09f`    | Password (DevSeeder)     |
| `SOAK_DURATION`| `60m`                                      | Duración del soak test   |

> **Nota de seguridad**: no expongas `AMM_PASS` en el código fuente en
> ambientes distintos a local/staging. Usar secrets en CI.

---

## SLOs definidos

| Métrica             | Umbral        |
|---------------------|---------------|
| Latencia p95        | < 300 ms      |
| Latencia p99        | < 800 ms      |
| Tasa de error HTTP  | < 1% (normal) |
| Checks exitosos     | > 99%         |

---

## Ejecución

### 1. Smoke test (siempre primero)
```bash
k6 run perf/scripts/baseline.js
```
Con password explícito:
```bash
k6 run --env AMM_PASS=<password> perf/scripts/baseline.js
```

### 2. Carga normal
```bash
k6 run --env AMM_PASS=<password> perf/scripts/load.js
```

### 3. Estrés
```bash
k6 run --env AMM_PASS=<password> perf/scripts/stress.js
```

### 4. Pico
```bash
k6 run --env AMM_PASS=<password> perf/scripts/spike.js
```

### 5. Resistencia (soak)
```bash
# Prueba corta para validar
k6 run --env AMM_PASS=<password> --env SOAK_DURATION=10m perf/scripts/soak.js

# Prueba completa
k6 run --env AMM_PASS=<password> --env SOAK_DURATION=60m perf/scripts/soak.js
```

### 6. Regresión (CI)
```bash
k6 run --env AMM_PASS=<password> perf/scripts/regression.js
```

---

## Guardar resultados

```bash
# Reporte JSON (para análisis posterior)
k6 run --out json=perf/results/2026-06-16-baseline.json \
       --env AMM_PASS=<password> perf/scripts/baseline.js

# Reporte CSV
k6 run --out csv=perf/results/2026-06-16-load.csv \
       --env AMM_PASS=<password> perf/scripts/load.js
```

---

## Certificado SSL local

La API corre con un certificado auto-firmado en desarrollo. Todos los scripts
incluyen `insecureSkipTLSVerify: true` en las options. **No usar esta opción
en pruebas contra producción.**

Si prefieres pasar el flag por línea de comandos:
```bash
k6 run --insecure-skip-tls-verify perf/scripts/baseline.js
```

---

## Orden recomendado de ejecución

```
baseline → load → regression → stress → spike → soak
```

- Nunca ejecutes stress/spike si baseline falla.
- Documenta los hallazgos en `defectos_rendimiento.md`.
- Archiva los JSON de resultados con fecha en el nombre.

---

## Notas sobre la parametrización CSV

El archivo `data/pacientes.csv` tiene 200 filas con columnas:
`documento, nombre, edad, vivo, etnia`

Los scripts usan `(__VU * N + __ITER) % 200` para asegurar que cada
VU e iteración use una fila diferente, generando documentos únicos con
el sufijo `-v{VU}i{ITER}` para evitar colisiones en la BD.

---

## Integración con GitHub Actions

```yaml
- name: Performance regression gate
  run: |
    k6 run \
      --env BASE_URL=${{ vars.PERF_BASE_URL }} \
      --env AMM_PASS=${{ secrets.AMM_PERF_PASS }} \
      perf/scripts/regression.js
```

Agrega `PERF_BASE_URL` como variable de repositorio y `AMM_PERF_PASS`
como secret en GitHub → Settings → Secrets and variables.
