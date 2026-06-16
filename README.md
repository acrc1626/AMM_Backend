# AMM Backend

[![CI](https://github.com/acrc1626/AMM_Backend/actions/workflows/ci.yml/badge.svg)](https://github.com/acrc1626/AMM_Backend/actions/workflows/ci.yml)

**AMM — Sistema de Administración de Medicamentos**

API REST para la gestión y vigilancia epidemiológica en Colombia. Permite el registro y seguimiento de pacientes, eventos epidemiológicos, censos, tratamientos y ubicaciones geográficas, con un módulo de seguridad basado en roles y permisos.

---

## Stack tecnológico

| Capa | Tecnología |
|---|---|
| Lenguaje / Framework | C# · .NET 9 |
| Arquitectura | Clean Architecture |
| ORM | Entity Framework Core 9 |
| Base de datos | Azure SQL Server |
| Autenticación | JWT Bearer (HS256) |
| Validaciones | FluentValidation 11 |
| Documentación API | Swagger / OpenAPI |
| Cobertura | Coverlet + ReportGenerator |
| CI/CD | GitHub Actions |

---

## Estructura del proyecto

```
AMM_Backend/
└── src/
    ├── AMM.Domain/              # Entidades, puertos (interfaces) y constantes de negocio
    │   ├── Entities/
    │   ├── Ports/
    │   └── Constants/
    │
    ├── AMM.Application/         # Casos de uso, DTOs, validadores y contratos de servicios
    │   ├── UseCases/
    │   ├── DTOs/
    │   ├── Validators/
    │   └── Interfaces/
    │
    ├── AMM.Infrastructure/      # EF Core, repositorios, servicios de seguridad y migraciones
    │   ├── Persistence/
    │   ├── Repositories/
    │   ├── Security/
    │   └── Migrations/
    │
    ├── AMM.Api/                 # Controllers HTTP, Program.cs y configuración de la app
    │   └── Controllers/
    │
    ├── AMM.Tests.Unit/          # Pruebas unitarias (xUnit + Mocks)
    └── AMM.Tests.Integration/   # Pruebas de integración (EF InMemory + CustomWebApplicationFactory)
```

---

## Ejecutar localmente

### Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (local) o cadena de conexión a Azure SQL

### 1. Configurar appsettings.json

Edita `src/AMM.Api/appsettings.json` con tus valores:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<servidor>;Initial Catalog=amm;User ID=<usuario>;Password=<clave>;Encrypt=True;"
  },
  "Jwt": {
    "Key": "<clave-de-al-menos-32-caracteres>",
    "Issuer": "amm-backend",
    "Audience": "amm-frontend",
    "ExpirationMinutes": 60
  },
  "AllowedOrigins": [
    "http://localhost:4200"
  ]
}
```

### 2. Aplicar migraciones

```bash
cd src
dotnet ef database update --project AMM.Infrastructure --startup-project AMM.Api
```

### 3. Ejecutar la API

```bash
cd src
dotnet run --project AMM.Api
```

La API queda disponible en `https://localhost:7xxx` y Swagger en `/swagger`.

---

## Ejecutar las pruebas

### Todas las pruebas

```bash
cd src
dotnet test AMM_Backend.sln
```

### Con reporte de cobertura (excluyendo Infrastructure)

```bash
# Pruebas unitarias
dotnet test AMM.Tests.Unit \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults/unit \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Exclude="[AMM.Infrastructure]*"

# Pruebas de integración
dotnet test AMM.Tests.Integration \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults/integration \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Exclude="[AMM.Infrastructure]*"

# Generar reporte HTML combinado
reportgenerator \
  "-reports:./TestResults/**/coverage.cobertura.xml" \
  "-targetdir:./coverage-report" \
  "-reporttypes:Html;TextSummary"
```

El reporte HTML queda en `src/coverage-report/index.html`.

---

## Cobertura actual

| Capa | Cobertura de líneas |
|---|:---:|
| **Global** | **81.6%** |
| Domain | 95.5% |
| Application | 91.5% |
| Api | 65.6% |
| Infrastructure | excluida |

> Total de pruebas: **297** (206 unitarias + 91 de integración)

---

## CI/CD

El pipeline de GitHub Actions se activa en cada `push` o `pull request` hacia `main` y ejecuta los siguientes pasos:

1. **Restore** — restaura dependencias NuGet
2. **Build** — compila en modo `Release`
3. **Test (Unit)** — ejecuta las 206 pruebas unitarias con cobertura
4. **Test (Integration)** — ejecuta las 91 pruebas de integración con EF InMemory
5. **Coverage Report** — combina los XMLs y genera reporte con ReportGenerator
6. **Quality Gate** — falla el pipeline si la cobertura global es **< 80%**
7. **Upload Artifact** — publica el reporte HTML como artefacto del workflow (30 días de retención)

La rama `main` tiene **Branch Protection** activada: ningún PR puede mergearse si el pipeline falla o si la cobertura cae por debajo del umbral.
