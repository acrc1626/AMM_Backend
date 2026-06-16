using AMM.Application.Interfaces;
using AMM.Domain.Constants;
using AMM.Domain.Entities;
using AMM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AMM.Tests.Integration.Infrastructure;

/// <summary>
/// Factory de pruebas HTTP que reemplaza SQL Server por EF Core InMemory y
/// siembra un usuario activo con rol para ejercitar el flujo login → JWT → endpoint.
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = Guid.NewGuid().ToString();

    public const string TestUserEmail    = "test@ins.gov.co";
    public const string TestUserPassword = "Test123!";
    public const string TestUserName     = "Usuario de Pruebas";
    public const string TestRolNombre    = "Administrador";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Evita que el DevSeeder de Program.cs corra (requiere IsDevelopment())
        builder.UseEnvironment("Testing");

        // ── 1. Override JWT + CORS config ────────────────────────────────────────
        builder.ConfigureAppConfiguration((_, config) =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"]               = "clave-secreta-de-pruebas-para-tests-ok!",
                ["Jwt:Issuer"]            = "amm-tests",
                ["Jwt:Audience"]          = "amm-tests-client",
                ["Jwt:ExpirationMinutes"] = "60",
                ["AllowedOrigins:0"]      = "http://localhost:4200"
            }));

        // ── 2. Reemplaza DbContext SQL Server por InMemory ────────────────────────
        //
        // AddDbContext siempre inyecta UseApplicationServiceProvider internamente,
        // lo que hace que EF Core busque IDatabaseProvider en el contenedor de la
        // aplicación. Si tanto SqlServer como InMemory están registrados ahí, EF
        // lanza "Multiple providers configured". La solución es registrar las
        // DbContextOptions directamente (sin AddDbContext) para que EF administre
        // su propio service provider interno con solo InMemory.
        builder.ConfigureTestServices(services =>
        {
            var toRemove = services
                .Where(d =>
                    d.ServiceType == typeof(DbContextOptions<AmmDbContext>) ||
                    d.ServiceType == typeof(AmmDbContext))
                .ToList();

            foreach (var d in toRemove)
                services.Remove(d);

            // Opciones sin UseApplicationServiceProvider → EF gestiona su SP interno
            services.AddScoped(_ =>
                new DbContextOptionsBuilder<AmmDbContext>()
                    .UseInMemoryDatabase(_dbName)
                    .ConfigureWarnings(w =>
                        w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                    .Options);

            services.AddScoped<AmmDbContext>();
        });
    }

    // ── 3. Siembra DESPUÉS de que el host esté completamente construido ──────────
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = base.CreateHost(builder);

        using var scope = host.Services.CreateScope();
        var ctx         = scope.ServiceProvider.GetRequiredService<AmmDbContext>();
        var hasher      = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        ctx.Database.EnsureCreated();
        SeedTestData(ctx, hasher);

        return host;
    }

    public const string SeedPacienteDocumento   = "DOC-SEED-001";
    public const byte   SeedPacienteTipoDocId   = 1;
    public const long   SeedPacienteId          = 100L;
    public const long   SeedCensoId             = 1L;
    public const int    SeedDeleteUserId        = 2;

    private static void SeedTestData(AmmDbContext ctx, IPasswordHasher hasher)
    {
        if (ctx.Roles.Any()) return; // idempotente

        // ── Catálogos mínimos requeridos por Include en los repositorios ──────────
        ctx.Estados.Add(new Estado          { Id = EstadoId.Activo, Nombre = "Activo",    Descripcion = "Activo" });
        ctx.TipoDocumentos.Add(new TipoDocumento { Id = 1,           Descripcion = "Cédula de Ciudadanía" });
        ctx.Sexos.Add(new Sexo              { Id = 1,               Descripcion = "Masculino" });
        ctx.TipoEntornos.Add(new TipoEntorno{ Id = 1,               Descripcion = "Rural" });

        // ── Catálogos adicionales para pruebas GetById ────────────────────────────
        ctx.Etnias.Add(new Etnia                  { Id = 1,  Descripcion = "Sin pertenencia étnica", Codigo = "SP" });
        ctx.PueblosIndigenas.Add(new PuebloIndigena { Id = 1, Descripcion = "Pueblo de prueba" });
        ctx.EventoTipos.Add(new EventoTipo         { Id = 1,  Codigo = "EVT01", Nombre = "Evento tipo prueba" });
        ctx.FormasFarmaceuticas.Add(new FormaFarmaceutica { Id = 1, Nombre = "Tableta" });

        // ── Geografía en cadena para pruebas GetById ──────────────────────────────
        ctx.Departamentos.Add(new Departamento    { Id = 1, CodigoDane = "05",    Nombre = "Antioquia" });
        ctx.Municipios.Add(new Municipio          { Id = 1, DepartamentoId = 1,   CodigoDaneDpto = "05001", Nombre = "Medellín" });
        ctx.Territorios.Add(new Territorio        { Id = 1, MunicipioId = 1,      Nombre = "Territorio 1" });
        ctx.Microterritorios.Add(new Microterritorio { Id = 1, TerritorioId = 1,  Nombre = "Micro 1" });
        ctx.Areas.Add(new Area                    { Id = 1, MicroterritorioId = 1, Nombre = "Área 1" });
        ctx.Ubicaciones.Add(new Ubicacion         { Id = 1, Direccion = "Calle 1 # 2-3" });

        var rolAdmin = new Rol
        {
            Id          = 1,
            Nombre      = TestRolNombre,
            Descripcion = "Acceso total al sistema"
        };
        ctx.Roles.Add(rolAdmin);

        ctx.Usuarios.Add(new Usuario
        {
            Id              = 1,
            Correo          = TestUserEmail,
            NombreCompleto  = TestUserName,
            PasswordHash    = hasher.Hash(TestUserPassword),
            EstadoUsuarioId = EstadoUsuarioId.Activo,
            CreadoEn        = DateTime.UtcNow,
            CreadoPor       = "seed",
            Roles           = new List<Rol> { rolAdmin }
        });

        ctx.Usuarios.Add(new Usuario
        {
            Id              = SeedDeleteUserId,
            Correo          = "delete-test@ins.gov.co",
            NombreCompleto  = "Usuario Para Borrar",
            EstadoUsuarioId = EstadoUsuarioId.Activo,
            CreadoEn        = DateTime.UtcNow,
            CreadoPor       = "seed"
        });

        ctx.Pacientes.Add(new Paciente
        {
            Id              = SeedPacienteId,
            TipoDocumentoId = SeedPacienteTipoDocId,
            Documento       = SeedPacienteDocumento,
            PrimerNombre    = "Paciente",
            PrimerApellido  = "Prueba",
            SexoId          = 1,
            EstadoId        = EstadoId.Activo,
            CreadoEn        = DateTime.UtcNow,
            CreadoPor       = "seed"
        });

        ctx.Censos.Add(new Censo
        {
            Id            = SeedCensoId,
            TipoEntornoId = 1,
            PacienteId    = SeedPacienteId,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = EstadoId.Activo,
            CreadoEn      = DateTime.UtcNow,
            CreadoPor     = "seed"
        });

        ctx.SaveChanges();
    }
}
