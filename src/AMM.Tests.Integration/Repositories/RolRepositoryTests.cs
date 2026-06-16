using AMM.Domain.Entities;
using AMM.Infrastructure.Persistence;
using AMM.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AMM.Tests.Integration.Repositories;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas de integración: RolRepository contra EF Core InMemory.
// Cada test recibe su propia base de datos (Guid único) para aislamiento total.
// NO se registra AuditInterceptor — AmmDbContext se instancia directamente.
// ─────────────────────────────────────────────────────────────────────────────

public class RolRepositoryTests
{
    // ─── Factory ──────────────────────────────────────────────────────────────

    private static AmmDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AmmDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    // ─── Test 1: persistencia y recuperación por ID ───────────────────────────

    [Fact]
    public async Task AddAsync_ValidRol_PersistsAndIsRecoveredByGetById()
    {
        // Given – contexto limpio + rol nuevo con nombre y descripción
        await using var ctx = CreateContext();
        var repo = new RolRepository(ctx);
        var rol = new Rol
        {
            Id          = 1,
            Nombre      = "Administrador",
            Descripcion = "Acceso total al sistema"
        };

        // When – se persiste a través del repositorio
        await repo.AddAsync(rol);
        await repo.SaveChangesAsync();

        // Then – GetByIdAsync recupera exactamente la misma entidad
        var result = await repo.GetByIdAsync<int>(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Administrador");
        result.Descripcion.Should().Be("Acceso total al sistema");
    }

    // ─── Test 2: listado con dos roles ────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_WithTwoSeededRoles_ReturnsBoth()
    {
        // Given – dos roles sembrados directamente en el contexto
        await using var ctx = CreateContext();
        ctx.Roles.AddRange(
            new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total" },
            new Rol { Id = 2, Nombre = "Digitador",     Descripcion = "Solo registro" });
        await ctx.SaveChangesAsync();

        var repo = new RolRepository(ctx);

        // When – se consulta la lista completa
        var result = await repo.GetAllAsync();

        // Then – ambos roles están presentes con sus nombres correctos
        result.Should().HaveCount(2);
        result.Select(r => r.Nombre).Should().BeEquivalentTo(["Administrador", "Digitador"]);
    }

    // ─── Test 3: GetByIdAsync con ID inexistente retorna null ─────────────────

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – base vacía
        await using var ctx = CreateContext();
        var repo = new RolRepository(ctx);

        // When – se busca un ID que no existe
        var result = await repo.GetByIdAsync<int>(999);

        // Then – retorna null sin lanzar excepción
        result.Should().BeNull();
    }
}
