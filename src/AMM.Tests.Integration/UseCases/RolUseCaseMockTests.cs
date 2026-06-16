using AMM.Application.DTOs.Seguridad;
using AMM.Application.UseCases.Seguridad;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Integration.UseCases;

// ─────────────────────────────────────────────────────────────────────────────
// Parte B — RolUseCase con IRolRepository mockeado.
// Verifica que el mapper (Rol → RolDto) funciona correctamente
// ante distintas combinaciones de Descripcion (null / no null).
// ─────────────────────────────────────────────────────────────────────────────

public class RolUseCaseMockTests
{
    private readonly Mock<IRolRepository> _repoMock = new();
    private RolUseCase CreateSut() => new(_repoMock.Object);

    // ─── Test 1: GetAllRolesAsync mapea Id, Nombre y Descripcion ─────────────

    [Fact]
    public async Task GetAllRolesAsync_ReturnsDtosMappedFromRepoEntities()
    {
        // Given – el repositorio devuelve dos roles: uno con descripción, otro sin ella
        IReadOnlyList<Rol> roles = new List<Rol>
        {
            new() { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total" },
            new() { Id = 2, Nombre = "Digitador",     Descripcion = null }
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(roles);

        // When – se ejecuta el caso de uso
        var result = await CreateSut().GetAllRolesAsync();

        // Then – los DTOs reflejan exactamente los campos de la entidad
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1);
        result[0].Nombre.Should().Be("Administrador");
        result[0].Descripcion.Should().Be("Acceso total");

        result[1].Id.Should().Be(2);
        result[1].Nombre.Should().Be("Digitador");
        result[1].Descripcion.Should().BeNull("la descripcion nula en la entidad se preserva en el DTO");
    }

    // ─── Test 2: GetAllRolesAsync con lista vacía → lista vacía ──────────────

    [Fact]
    public async Task GetAllRolesAsync_EmptyRepo_ReturnsEmptyList()
    {
        // Given – repositorio sin roles
        _repoMock.Setup(r => r.GetAllAsync(default))
                 .ReturnsAsync(new List<Rol>());

        // When
        var result = await CreateSut().GetAllRolesAsync();

        // Then
        result.Should().BeEmpty();
    }

    // ─── Test 3: GetRolByIdAsync encontrado → DTO correcto ───────────────────

    [Fact]
    public async Task GetRolByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given – el repo devuelve un rol con Id=3
        _repoMock.Setup(r => r.GetByIdAsync<int>(3, default))
                 .ReturnsAsync(new Rol { Id = 3, Nombre = "Supervisor", Descripcion = "Revisión" });

        // When
        var result = await CreateSut().GetRolByIdAsync(3);

        // Then – DTO con todos los campos mapeados
        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
        result.Nombre.Should().Be("Supervisor");
        result.Descripcion.Should().Be("Revisión");
    }

    // ─── Test 4: GetRolByIdAsync no encontrado → null ────────────────────────

    [Fact]
    public async Task GetRolByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repo no encuentra ningún rol con ese ID
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Rol?)null);

        // When / Then
        var result = await CreateSut().GetRolByIdAsync(999);
        result.Should().BeNull();
    }
}
