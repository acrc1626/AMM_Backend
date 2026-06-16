using AMM.Domain.Constants;
using AMM.Domain.Entities;
using AMM.Infrastructure.Persistence;
using AMM.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AMM.Tests.Integration.Repositories;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas de integración: PacienteRepository contra EF Core InMemory.
// InMemory no aplica HasDefaultValueSql, por lo tanto CreadoEn se fija
// manualmente en cada entidad de prueba.
// InMemory no valida FK → no es necesario sembrar TipoDocumento / Sexo.
// ─────────────────────────────────────────────────────────────────────────────

public class PacienteRepositoryTests
{
    // ─── Factory ──────────────────────────────────────────────────────────────

    private static AmmDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AmmDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    // ─── Builder ──────────────────────────────────────────────────────────────

    private static Paciente BuildPaciente(long id = 1, string documento = "10200300") => new()
    {
        Id              = id,
        TipoDocumentoId = 1,
        Documento       = documento,
        PrimerNombre    = "Laura",
        PrimerApellido  = "Martínez",
        SexoId          = 2,
        EstadoId        = EstadoId.Activo,
        CreadoEn        = DateTime.UtcNow,   // InMemory no aplica sysutcdatetime()
        CreadoPor       = "test-integration"
    };

    // ─── Test 1: AddAsync persiste los campos requeridos ─────────────────────

    [Fact]
    public async Task AddAsync_ValidPaciente_PersistsAllRequiredFields()
    {
        // Given – paciente nuevo con los campos mínimos requeridos
        await using var ctx = CreateContext();
        var repo = new PacienteRepository(ctx);
        var paciente = BuildPaciente(id: 1, documento: "10200300");

        // When – se persiste vía el repositorio (AddAsync + SaveChangesAsync)
        await repo.AddAsync(paciente);
        await repo.SaveChangesAsync();

        // Then – el contexto contiene exactamente un paciente con los datos correctos
        var saved = await ctx.Pacientes.FindAsync(1L);

        saved.Should().NotBeNull();
        saved!.Documento.Should().Be("10200300");
        saved.PrimerNombre.Should().Be("Laura");
        saved.PrimerApellido.Should().Be("Martínez");
        saved.TipoDocumentoId.Should().Be(1);
        saved.EstadoId.Should().Be(EstadoId.Activo);
        saved.CreadoPor.Should().Be("test-integration");
    }

    // ─── Test 2: GetByDocumentoAsync → true (documento ya existe) ─────────────

    [Fact]
    public async Task GetByDocumentoAsync_ExistingDocumento_ReturnsPaciente()
    {
        // Given – un paciente ya registrado con tipoDocId=1, doc="99887766"
        await using var ctx = CreateContext();
        ctx.Pacientes.Add(BuildPaciente(id: 2, documento: "99887766"));
        await ctx.SaveChangesAsync();

        var repo = new PacienteRepository(ctx);

        // When – se busca el mismo tipo de documento y número
        var result = await repo.GetByDocumentoAsync(tipoDocumentoId: 1, documento: "99887766");

        // Then – retorna la entidad (no null); el documento duplicado será detectado
        result.Should().NotBeNull();
        result!.Documento.Should().Be("99887766");
        result.Id.Should().Be(2);
    }

    // ─── Test 3: GetByDocumentoAsync → false (documento no existe) ────────────

    [Fact]
    public async Task GetByDocumentoAsync_NonExistingDocumento_ReturnsNull()
    {
        // Given – base vacía (nueva db aislada por Guid)
        await using var ctx = CreateContext();
        var repo = new PacienteRepository(ctx);

        // When – se busca un documento que nunca se registró
        var result = await repo.GetByDocumentoAsync(tipoDocumentoId: 1, documento: "00000000");

        // Then – retorna null (no existe el paciente)
        result.Should().BeNull();
    }

    // ─── Test 4: AddAsync + GetByDocumentoAsync verifica unicidad de documento ─

    [Fact]
    public async Task GetByDocumentoAsync_AfterAdd_DetectsExistingDocument()
    {
        // Given – repositorio vacío
        await using var ctx = CreateContext();
        var repo = new PacienteRepository(ctx);

        // When – se registra un paciente
        await repo.AddAsync(BuildPaciente(id: 5, documento: "55566677"));
        await repo.SaveChangesAsync();

        // Then – la misma combinación tipoDoc+documento es encontrada (no null)
        var duplicate = await repo.GetByDocumentoAsync(1, "55566677");
        duplicate.Should().NotBeNull("el Use Case debe detectar el duplicado vía este mismo método");

        // And – un documento distinto aún no existe
        var otro = await repo.GetByDocumentoAsync(1, "11111111");
        otro.Should().BeNull();
    }
}
