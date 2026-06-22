# Resumen Ejecutivo — Estrategia de Pruebas  
## Sistema AMM — Vigilancia Epidemiológica Colombia  
**Fecha:** Junio 2026 | **Plataforma:** .NET 9 | **Repositorio:** `AMM_Backend`

---

## 1. Arquitectura del Proyecto

El backend sigue una **Clean Architecture** estricta dividida en cuatro capas con dependencias unidireccionales hacia el dominio:

```
┌────────────────────────────────────────────────────┐
│  AMM.Api          → Controllers, Program, Swagger   │
├────────────────────────────────────────────────────┤
│  AMM.Application  → Use Cases, DTOs, Validators    │
├────────────────────────────────────────────────────┤
│  AMM.Infrastructure → EF Core, JWT, Repositories   │
├────────────────────────────────────────────────────┤
│  AMM.Domain       → Entities, Ports, Constants     │
└────────────────────────────────────────────────────┘
```

| Capa             | Responsabilidad principal                                        |
|------------------|------------------------------------------------------------------|
| **Domain**       | Entidades de negocio, interfaces de repositorios (ports), constantes |
| **Application**  | Casos de uso, DTOs, validaciones con FluentValidation            |
| **Infrastructure**| Persistencia (EF Core + SQL Server), JWT service, PasswordHasher |
| **Api**          | Controllers REST, configuración de autenticación JWT, Swagger    |

**Tecnologías principales:**

| Componente       | Tecnología                                            |
|------------------|-------------------------------------------------------|
| Framework        | ASP.NET Core 9                                        |
| ORM              | Entity Framework Core 9 + SQL Server                  |
| Autenticación    | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Validación       | FluentValidation 11 con auto-validación en pipeline   |
| Documentación    | Swagger / Swashbuckle                                 |
| CI/CD            | GitHub Actions (`.github/workflows/ci.yml`)           |

---

## 2. Estrategia Global de Pruebas

La suite completa abarca **tres niveles** que se complementan siguiendo la pirámide de testing:

```
          /\
         /  \   Pruebas de Carga (k6)
        /────\  6 scripts · SLOs definidos
       /      \
      /────────\  Pruebas de Integración
     /          \  117 tests · HTTP + EF InMemory
    /────────────\
   /              \  Pruebas Unitarias
  /                \  206 tests · Mocks · Validators
 /──────────────────\
```

**Patrón transversal: AAA (Arrange · Act · Assert)**  
Todas las pruebas —unitarias, de integración y de carga— están estructuradas con el patrón AAA (también expresado como **Given / When / Then** en algunos archivos de integración):

```
// Arrange  →  preparar datos, configurar mocks/contexto
// Act      →  invocar el componente bajo prueba
// Assert   →  verificar el resultado con FluentAssertions o check()
```

---

## 3. Pruebas Unitarias

### Alcance

| Categoría            | Archivos de test                                  | Tests |
|----------------------|---------------------------------------------------|-------|
| Use Cases — Auth     | `LoginUseCaseTests`, `SetPasswordUseCaseTests`    |  ~28  |
| Use Cases — Pacientes| `PacienteUseCasesTests`, `CrearPacienteUseCaseTests` | ~30 |
| Use Cases — Censos   | `CensoUseCaseTests`, `CensoNovedadUseCaseTests`   |  ~20  |
| Use Cases — Eventos  | `EventosUseCasesTests`                            |  ~22  |
| Use Cases — Catalogos| `CatalogoUseCasesTests`                           |  ~15  |
| Use Cases — Seguridad| `UsuarioUseCaseTests`, `RolPermisoMenuUseCaseTests`|  ~18 |
| Use Cases — Ubicación| `UbicacionGeograficaUseCaseTests`                 |  ~12  |
| Validadores          | `AuthValidatorsTests`, `PacienteValidatorsTests`, `SeguridadValidatorsTests` | ~35 |
| Dominio              | `DomainEntityTests`                               |  ~10  |
| Infraestructura      | `PasswordHasherTests`                             |   ~6  |
| **TOTAL**            | **16 archivos**                                   | **206**|

### Estrategia de aislamiento

Las pruebas unitarias usan **dobles de prueba (test doubles)** para aislar completamente la lógica de negocio de sus dependencias de infraestructura:

```csharp
// Arrange — inyección de dependencias via Moq
private readonly Mock<IUsuarioRepository> _usuarioRepoMock = new();
private readonly Mock<IJwtService>        _jwtServiceMock  = new();
private readonly Mock<IPasswordHasher>    _hasherMock      = new();

private LoginUseCase CreateSut() =>
    new(_usuarioRepoMock.Object, _jwtServiceMock.Object, _hasherMock.Object);

[Fact]
public async Task ExecuteAsync_CorrectCredentials_ReturnsTokenAndRoles()
{
    // Arrange
    var usuario = BuildActiveUser();
    _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("admin@ins.gov.co", default))
                    .ReturnsAsync(usuario);
    _hasherMock.Setup(h => h.Verify("Pass123!", "salt.hash")).Returns(true);

    // Act
    var result = await CreateSut().ExecuteAsync(new LoginRequest("admin@ins.gov.co", "Pass123!"));

    // Assert
    result.Token.Should().NotBeNullOrEmpty();
    result.Roles.Should().Contain("Administrador");
}
```

### Bibliotecas utilizadas

| Librería                  | Versión  | Rol en las pruebas                            |
|---------------------------|----------|-----------------------------------------------|
| `xunit`                   | 2.9.2    | Framework de pruebas, `[Fact]`, `[Theory]`    |
| `xunit.runner.visualstudio`| 2.8.2   | Integración con Visual Studio / dotnet test   |
| `Moq`                     | 4.20.72  | Mock de interfaces (repositorios, servicios)  |
| `FluentAssertions`        | 6.12.2   | Aserciones legibles: `.Should().Be()`, `.Contain()` |
| `FluentValidation`        | 11.4.0   | Validación de DTOs y pruebas de validadores   |
| `coverlet.collector`      | 6.0.2    | Instrumentación de cobertura (XPlat)          |
| `Microsoft.NET.Test.Sdk`  | 17.12.0  | SDK de ejecución de pruebas .NET              |

---

## 4. Pruebas de Integración

### Alcance

| Categoría            | Archivos de test                                        | Tests |
|----------------------|---------------------------------------------------------|-------|
| HTTP — Auth          | `AuthControllerTests`                                   |   9   |
| HTTP — Pacientes     | `PacientesControllerTests`                              |   8   |
| HTTP — Censos        | `CensosControllerTests`                                 |   5   |
| HTTP — Eventos       | `EventosControllerTests` (7 controllers × 3 tests + CensoNovedades) | 26 |
| HTTP — Catálogos     | `CatalogsControllerTests`, `EstadosControllerTests`     |  12   |
| HTTP — Seguridad     | `SeguridadControllerTests`, `UsuariosControllerTests`, `RolesControllerTests` | 14 |
| HTTP — Ubicación     | `UbicacionGeograficaControllerTests`                    |  12   |
| Repositorios (EF)    | `RolRepositoryTests`, `PacienteRepositoryTests`         |  10   |
| Use Cases con Mocks  | `RolUseCaseMockTests`, `CrearPacienteUseCaseMockTests`  |   7   |
| **TOTAL**            | **14 archivos**                                         | **117**|

### Estrategia: WebApplicationFactory + EF Core InMemory

Las pruebas HTTP levantan **el host completo de la aplicación** en memoria, sin servidores externos:

```
Test → HttpClient → [Pipeline ASP.NET Core completo] → Controller → UseCase → Repository → EF InMemory
```

**`CustomWebApplicationFactory`** es el núcleo de la infraestructura de integración:

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");           // ← evita que DevSeeder altere el seed

        builder.ConfigureAppConfiguration((_, cfg) =>
            cfg.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"]    = "clave-secreta-de-pruebas-para-tests-ok!",
                ["Jwt:Issuer"] = "amm-tests",
                // ...
            }));

        builder.ConfigureTestServices(services =>
        {
            // Reemplaza SQL Server por EF Core InMemory
            services.AddScoped(_ =>
                new DbContextOptionsBuilder<AmmDbContext>()
                    .UseInMemoryDatabase(_dbName)
                    .Options);
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);
        SeedTestData(/* usuario, rol, paciente, censo */);  // siembra datos deterministas
        return host;
    }
}
```

**Flujo estándar en pruebas HTTP (patrón AAA):**

```csharp
[Fact]
public async Task Login_ValidCredentials_Returns200WithTokenAndRole()
{
    // Arrange
    var client = NewClient();

    // Act
    var response = await client.PostAsync("/api/auth/login",
        JsonBody(new { Correo = "test@ins.gov.co", Password = "Test123!" }));

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    var body = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
    body!.Token.Should().NotBeNullOrEmpty();
    body.Roles.Should().Contain("Administrador");
}
```

### Bibliotecas utilizadas

| Librería                           | Versión | Rol en las pruebas                               |
|------------------------------------|---------|--------------------------------------------------|
| `Microsoft.AspNetCore.Mvc.Testing` | 9.0.0   | `WebApplicationFactory<Program>` — host en memoria |
| `Microsoft.EntityFrameworkCore.InMemory` | 9.0.0 | Base de datos en memoria para tests           |
| `xunit`                            | 2.9.2   | Framework de ejecución                           |
| `Moq`                              | 4.20.72 | Mocks en tests de use cases dentro de integración|
| `FluentAssertions`                 | 6.12.2  | Aserciones sobre `HttpStatusCode`, DTOs          |
| `coverlet.collector`               | 6.0.2   | Recolección de cobertura                         |

### Incidentes resueltos durante la implementación

| Incidente | Causa raíz | Solución |
|-----------|-----------|----------|
| Login devolvía 401 en todos los tests | `DevSeeder` insertaba un `Rol "Administrador"` al iniciar el host, activando el guard `if (ctx.Roles.Any()) return` en `SeedTestData` → el usuario de prueba nunca se sembraba | `builder.UseEnvironment("Testing")` impide que `DevSeeder` corra |
| Endpoints `/me`, `change-password`, `set-password` devolvían 404 | Commit `350e7e1` los eliminó del `AuthController` sin actualizar los tests | Restaurados los 3 endpoints y `SetPasswordUseCase` al controller |
| Cobertura reportada al 29% en CI | 10 archivos XML acumulados de runs anteriores + tests fallando | Tests corregidos + limpieza de `TestResults/` en cada run |

---

## 5. Cobertura de Código

### Resultado final

| Métrica             | Valor    | Umbral mínimo |
|---------------------|----------|---------------|
| **Line coverage**   | **83.9%**| ≥ 80%         |
| Branch coverage     | 75.2%    | —             |
| Method coverage     | 83.9%    | —             |

### Cobertura por capa (Infrastructure excluida de medición)

| Ensamblado        | Cobertura | Observación                                     |
|-------------------|-----------|-------------------------------------------------|
| `AMM.Application` | 90.9%     | Use Cases, DTOs y Validators casi completamente cubiertos |
| `AMM.Domain`      | 90.0%     | Entidades y constantes con cobertura alta        |
| `AMM.Api`         | 58.5%     | Controllers de eventos epidemiológicos cubiertos tras agregar `EventosControllerTests` |

> `AMM.Infrastructure` está excluida explícitamente de la medición con el filtro
> `[AMM.Infrastructure]*` en Coverlet, dado que contiene código de persistencia y
> migraciones que requieren una base de datos real para ser ejercitados.

### Herramientas de cobertura

| Herramienta             | Rol                                                 |
|-------------------------|-----------------------------------------------------|
| `coverlet.collector`    | Instrumenta los binarios durante `dotnet test`      |
| `ReportGenerator`       | Combina XMLs Cobertura y genera reporte HTML + texto|
| GitHub Actions          | Ejecuta el umbral del 80% y archiva el reporte como artifact |

### Pipeline CI (GitHub Actions)

```yaml
# Ejecuta pruebas con recolección de cobertura
dotnet test AMM.Tests.Unit --collect:"XPlat Code Coverage"
dotnet test AMM.Tests.Integration --collect:"XPlat Code Coverage"

# Genera reporte combinado
reportgenerator -reports:./TestResults/**/coverage.cobertura.xml \
                -targetdir:./coverage-report -reporttypes:TextSummary

# Verifica umbral
COVERAGE=$(grep "Line coverage" ./coverage-report/Summary.txt | grep -oE '[0-9]+\.[0-9]+')
awk -v cov="$COVERAGE" 'BEGIN { exit (cov >= 80) ? 0 : 1 }'
```

---

## 6. Pruebas de Carga (k6)

### Suite de scripts

| Script           | Tipo            | VUs máx | Duración | Propósito                                      |
|------------------|-----------------|---------|----------|------------------------------------------------|
| `baseline.js`    | Smoke test      | 1       | 2 min    | Validar que todos los endpoints responden antes de cargar |
| `load.js`        | Carga normal    | 40      | ~9 min   | Simular concurrencia cotidiana en producción   |
| `stress.js`      | Estrés          | 200     | ~22 min  | Encontrar el punto de quiebre del sistema      |
| `spike.js`       | Pico súbito     | 300     | ~15 min  | Simular alerta epidemiológica — burst repentino |
| `soak.js`        | Resistencia     | 15      | 60 min   | Detectar memory leaks y degradación progresiva |
| `regression.js`  | Gate CI         | 20      | 5 min    | Bloquear merges con regresiones de rendimiento |

### SLOs definidos

| Métrica                  | Umbral normal | Umbral estrés/pico |
|--------------------------|---------------|---------------------|
| Latencia p95             | < 300 ms      | < 1 000 ms          |
| Latencia p99             | < 800 ms      | < 3 000 ms          |
| Tasa de error HTTP       | < 1%          | < 10–15%            |
| Checks exitosos          | > 99%         | > 80–85%            |
| Recuperación post-pico   | —             | > 90%               |

### Flujo común en todos los scripts (patrón AAA)

```javascript
// setup() — UNA vez antes de los VUs
export function setup() {
  // Arrange
  const payload = JSON.stringify({ correo: 'qa@amm.local', password: PASS });
  // Act
  const res = http.post(`${BASE_URL}/api/auth/login`, payload, PARAMS);
  // Assert
  check(res, { 'login 200': (r) => r.status === 200 });
  return { token: res.json('token') };
}

// default() — por cada VU × iteración
export default function (data) {
  // Arrange — fila CSV única por VU e iteración
  const fila     = pacientes[(__VU * N + __ITER) % pacientes.length];
  const docUnico = `${fila.documento}-v${__VU}i${__ITER}`;
  // Act
  const res = http.post(`${BASE_URL}/api/pacientes`, JSON.stringify({ ... }), authParams(data.token));
  // Assert
  check(res, { '[crear] 201': (r) => r.status === 201 });
}
```

### Parametrización con CSV

```
perf/data/pacientes.csv  →  200 filas
Columnas: documento, nombre, edad, vivo, etnia
```

Cada VU selecciona una fila mediante `(__VU * N + __ITER) % 200` para garantizar
que no haya colisiones de documentos entre usuarios virtuales concurrentes.
El `documento` final se construye como `CC-PERF-NNN-v{VU}i{ITER}`.

### Herramientas de carga

| Herramienta        | Versión | Rol                                                  |
|--------------------|---------|------------------------------------------------------|
| `k6` (Grafana Labs)| 2.0.0   | Motor de ejecución de pruebas de carga               |
| `papaparse`        | 5.1.1   | Parseo del CSV vía `SharedArray` en k6               |
| `k6/metrics`       | —       | `Counter`, `Rate`, `Trend` para métricas personalizadas |

---

## 7. Resumen de Totales

| Nivel de prueba       | Cantidad | Estado    | Cobertura / SLO        |
|-----------------------|----------|-----------|------------------------|
| Pruebas unitarias     | **206**  | ✅ Pasan  | —                      |
| Pruebas de integración| **117**  | ✅ Pasan  | —                      |
| **Total automatizadas**| **323** | ✅ Pasan  | **83.9% line coverage**|
| Pruebas de carga      | 6 scripts| ✅ Listas | p95 < 300 ms · p99 < 800 ms |
| Umbral CI             | ≥ 80%    | ✅ Cumple | 83.9% (3.9 pp de margen)|

---

## 8. Lecciones Aprendidas

| Tema | Descripción |
|------|-------------|
| **Aislamiento de entorno en tests** | El entorno de prueba debe diferenciarse explícitamente del de desarrollo. `UseEnvironment("Testing")` impidió que el `DevSeeder` contaminara la BD InMemory. |
| **Acumulación de artefactos** | Los archivos XML de cobertura se acumulan entre runs locales. Limpiar `TestResults/` antes de cada medición es obligatorio para obtener cifras reales. |
| **Cobertura ≠ calidad** | Los controllers de eventos tenían 0% de cobertura aunque el código existía y compilaba. Los tests de integración HTTP son necesarios para cubrir el binding de rutas y la serialización JSON. |
| **Coherencia código–tests** | Al eliminar endpoints del controller sin actualizar los tests, se rompe el CI silenciosamente en la fase de cobertura (los tests fallidos producen 0 líneas instrumentadas). |
| **Parametrización de documentos únicos** | En pruebas de carga con múltiples VUs creando entidades con campos únicos, el sufijo `v{VU}i{ITER}` garantiza no-colisión sin necesidad de limpiar la BD entre runs. |
