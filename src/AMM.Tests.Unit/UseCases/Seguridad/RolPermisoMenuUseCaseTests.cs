using AMM.Application.DTOs.Seguridad;
using AMM.Application.UseCases.Seguridad;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Seguridad;

// ─────────────────────────────────────────────────────────────────────────────
// Tests para RolUseCase, PermisoUseCase y MenuUseCase.
// ─────────────────────────────────────────────────────────────────────────────

public class RolUseCaseTests
{
    private readonly Mock<IRolRepository> _repoMock = new();
    private RolUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllRolesAsync_WithRoles_ReturnsMappedList()
    {
        // Given – dos roles, uno con descripción y otro sin ella
        IReadOnlyList<Rol> roles = new List<Rol>
        {
            new() { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total" },
            new() { Id = 2, Nombre = "Digitador",     Descripcion = null }
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(roles);

        // When
        var result = await CreateSut().GetAllRolesAsync();

        // Then – nombre y descripción (incluida null) correctamente mapeados
        result.Should().HaveCount(2);
        result[0].Nombre.Should().Be("Administrador");
        result[0].Descripcion.Should().Be("Acceso total");
        result[1].Nombre.Should().Be("Digitador");
        result[1].Descripcion.Should().BeNull();
    }

    [Fact]
    public async Task GetRolByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default))
                 .ReturnsAsync(new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total" });

        // When
        var result = await CreateSut().GetRolByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Administrador");
        result.Descripcion.Should().Be("Acceso total");
    }

    [Fact]
    public async Task GetRolByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Rol?)null);

        // When / Then
        var result = await CreateSut().GetRolByIdAsync(999);
        result.Should().BeNull();
    }
}

public class PermisoUseCaseTests
{
    private readonly Mock<IPermisoRepository> _repoMock = new();
    private PermisoUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllPermisosAsync_WithPermisos_ReturnsMappedList()
    {
        // Given – dos permisos, uno con descripción y otro sin ella
        IReadOnlyList<Permiso> permisos = new List<Permiso>
        {
            new() { Id = 1, Codigo = "censos.crear",     Descripcion = "Crear censos" },
            new() { Id = 2, Codigo = "pacientes.editar", Descripcion = null }
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(permisos);

        // When
        var result = await CreateSut().GetAllPermisosAsync();

        // Then
        result.Should().HaveCount(2);
        result[0].Codigo.Should().Be("censos.crear");
        result[0].Descripcion.Should().Be("Crear censos");
        result[1].Codigo.Should().Be("pacientes.editar");
        result[1].Descripcion.Should().BeNull();
    }

    [Fact]
    public async Task GetPermisoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default))
                 .ReturnsAsync(new Permiso { Id = 1, Codigo = "censos.crear", Descripcion = "Crear censos" });

        // When
        var result = await CreateSut().GetPermisoByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Codigo.Should().Be("censos.crear");
        result.Descripcion.Should().Be("Crear censos");
    }

    [Fact]
    public async Task GetPermisoByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Permiso?)null);

        // When / Then
        var result = await CreateSut().GetPermisoByIdAsync(999);
        result.Should().BeNull();
    }
}

public class MenuUseCaseTests
{
    private readonly Mock<IMenuRepository> _repoMock = new();
    private MenuUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllMenusAsync_WithMenus_ReturnsMappedList()
    {
        // Given – repositorio retorna dos menús de navegación
        IReadOnlyList<Menu> menus = new List<Menu>
        {
            new() { Id = 1, Nombre = "Censos",    Ruta = "/censos" },
            new() { Id = 2, Nombre = "Pacientes", Ruta = "/pacientes" }
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(menus);

        // When
        var result = await CreateSut().GetAllMenusAsync();

        // Then
        result.Should().HaveCount(2);
        result[0].Nombre.Should().Be("Censos");
        result[0].Ruta.Should().Be("/censos");
        result[1].Nombre.Should().Be("Pacientes");
    }

    [Fact]
    public async Task GetMenuByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default))
                 .ReturnsAsync(new Menu { Id = 1, Nombre = "Censos", Ruta = "/censos" });

        // When
        var result = await CreateSut().GetMenuByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Censos");
        result.Ruta.Should().Be("/censos");
    }

    [Fact]
    public async Task GetMenuByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Menu?)null);

        // When / Then
        var result = await CreateSut().GetMenuByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByRolAsync_WithMenus_ReturnsMappedListForRole()
    {
        // Given – dos menús asociados al rol con ID 1
        IReadOnlyList<Menu> menus = new List<Menu>
        {
            new() { Id = 1, Nombre = "Censos",   Ruta = "/censos" },
            new() { Id = 3, Nombre = "Reportes", Ruta = "/reportes" }
        };
        _repoMock.Setup(r => r.GetByRolIdAsync(1, default)).ReturnsAsync(menus);

        // When
        var result = await CreateSut().GetByRolAsync(1);

        // Then
        result.Should().HaveCount(2);
        result.Select(m => m.Ruta).Should().Contain("/reportes");
    }

    [Fact]
    public async Task GetByRolAsync_WithEmptyMenus_ReturnsEmptyList()
    {
        // Given – el rol no tiene menús asignados
        _repoMock.Setup(r => r.GetByRolIdAsync(99, default))
                 .ReturnsAsync(new List<Menu>());

        // When
        var result = await CreateSut().GetByRolAsync(99);

        // Then
        result.Should().BeEmpty();
    }
}
