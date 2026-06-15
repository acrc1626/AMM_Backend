using AMM.Application.DTOs.UbicacionGeografica;
using AMM.Application.UseCases.UbicacionGeografica;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.UbicacionGeografica;

// ═══════════════════════════════════════════════════════════════════════════════
// Tests para los 6 UbicacionGeografica UseCases.
// Los mappers con nav-props opcionales se prueban en ambas ramas (null / no-null)
// para maximizar el branch coverage en los operadores ?.  y  ?? "".
// ═══════════════════════════════════════════════════════════════════════════════

#region Departamento

public class DepartamentoUseCaseTests
{
    private readonly Mock<IDepartamentoRepository> _repoMock = new();
    private DepartamentoUseCase CreateSut() => new(_repoMock.Object);

    private static Departamento Build(short id = 1) => new()
        { Id = id, CodigoDane = "05", Nombre = "Antioquia" };

    [Fact]
    public async Task GetAllDepartamentosAsync_WithItems_ReturnsMappedList()
    {
        // Given – repositorio retorna dos departamentos
        IReadOnlyList<Departamento> items = new List<Departamento> { Build(1), Build(5) };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllDepartamentosAsync();

        // Then – código DANE y nombre correctamente mapeados
        result.Should().HaveCount(2);
        result[0].CodigoDane.Should().Be("05");
        result[0].Nombre.Should().Be("Antioquia");
    }

    [Fact]
    public async Task GetDepartamentoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<short>((short)1, default)).ReturnsAsync(Build(1));

        // When
        var result = await CreateSut().GetDepartamentoByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.CodigoDane.Should().Be("05");
        result.Nombre.Should().Be("Antioquia");
    }

    [Fact]
    public async Task GetDepartamentoByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repositorio no encuentra el departamento
        _repoMock.Setup(r => r.GetByIdAsync<short>(It.IsAny<short>(), default))
                 .ReturnsAsync((Departamento?)null);

        // When / Then – null sin excepción (rama entity == null en la base)
        var result = await CreateSut().GetDepartamentoByIdAsync(99);
        result.Should().BeNull();
    }
}

#endregion

#region Municipio

public class MunicipioUseCaseTests
{
    private readonly Mock<IMunicipioRepository> _repoMock = new();
    private MunicipioUseCase CreateSut() => new(_repoMock.Object);

    private static Municipio BuildWithNav(int id = 1) => new()
    {
        Id              = id,
        DepartamentoId  = 5,
        CodigoDaneDpto  = "05001",
        Nombre          = "Medellín",
        Departamento    = new Departamento { Id = 5, Nombre = "Antioquia" }
    };

    private static Municipio BuildWithoutNav(int id = 1) => new()
        { Id = id, DepartamentoId = 5, CodigoDaneDpto = "05001", Nombre = "Medellín" };

    [Fact]
    public async Task GetAllMunicipiosAsync_WithDepartamentoNavProp_MapsDepartamentoNombre()
    {
        // Given – nav prop cargada → DepartamentoNombre proviene de Departamento.Nombre
        IReadOnlyList<Municipio> items = new List<Municipio> { BuildWithNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllMunicipiosAsync();

        // Then – rama NOT-NULL del operador ?.
        result.Should().HaveCount(1);
        result[0].DepartamentoNombre.Should().Be("Antioquia");
        result[0].Nombre.Should().Be("Medellín");
    }

    [Fact]
    public async Task GetAllMunicipiosAsync_WithoutDepartamentoNavProp_FallsBackToEmpty()
    {
        // Given – nav prop null → debe caer al fallback ?? ""
        IReadOnlyList<Municipio> items = new List<Municipio> { BuildWithoutNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllMunicipiosAsync();

        // Then – rama NULL del operador ?. → DepartamentoNombre es cadena vacía
        result[0].DepartamentoNombre.Should().Be("");
    }

    [Fact]
    public async Task GetMunicipioByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default)).ReturnsAsync(BuildWithNav());

        // When
        var result = await CreateSut().GetMunicipioByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Nombre.Should().Be("Medellín");
        result.DepartamentoNombre.Should().Be("Antioquia");
    }

    [Fact]
    public async Task GetMunicipioByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Municipio?)null);

        // When / Then
        var result = await CreateSut().GetMunicipioByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByDepartamentoAsync_WithMunicipios_ReturnsMappedList()
    {
        // Given – dos municipios del departamento 5
        IReadOnlyList<Municipio> items = new List<Municipio> { BuildWithNav(1), BuildWithNav(2) };
        _repoMock.Setup(r => r.GetByDepartamentoIdAsync((short)5, default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetByDepartamentoAsync(5);

        // Then
        result.Should().HaveCount(2);
        result.Should().OnlyContain(m => m.DepartamentoNombre == "Antioquia");
    }
}

#endregion

#region Territorio

public class TerritorioUseCaseTests
{
    private readonly Mock<ITerritorioRepository> _repoMock = new();
    private TerritorioUseCase CreateSut() => new(_repoMock.Object);

    private static Territorio BuildWithNav(int id = 1) => new()
    {
        Id          = id,
        MunicipioId = 5001,
        Nombre      = "Zona Norte",
        Municipio   = new Municipio { Id = 5001, Nombre = "Medellín" }
    };

    private static Territorio BuildWithoutNav(int id = 1) => new()
        { Id = id, MunicipioId = 5001, Nombre = "Zona Norte" };

    [Fact]
    public async Task GetAllTerritoriosAsync_WithMunicipioNavProp_MapsMunicipioNombre()
    {
        // Given – nav prop cargada
        IReadOnlyList<Territorio> items = new List<Territorio> { BuildWithNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllTerritoriosAsync();

        // Then – rama NOT-NULL del operador ?.
        result.Should().HaveCount(1);
        result[0].MunicipioNombre.Should().Be("Medellín");
        result[0].Nombre.Should().Be("Zona Norte");
    }

    [Fact]
    public async Task GetAllTerritoriosAsync_WithoutMunicipioNavProp_FallsBackToEmpty()
    {
        // Given – nav prop null
        IReadOnlyList<Territorio> items = new List<Territorio> { BuildWithoutNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllTerritoriosAsync();

        // Then – rama NULL → MunicipioNombre vacío
        result[0].MunicipioNombre.Should().Be("");
    }

    [Fact]
    public async Task GetTerritorioByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default)).ReturnsAsync(BuildWithNav());

        // When
        var result = await CreateSut().GetTerritorioByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Nombre.Should().Be("Zona Norte");
        result.MunicipioNombre.Should().Be("Medellín");
    }

    [Fact]
    public async Task GetTerritorioByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Territorio?)null);

        // When / Then
        var result = await CreateSut().GetTerritorioByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByMunicipioAsync_WithTerritorios_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<Territorio> items = new List<Territorio> { BuildWithNav(1), BuildWithNav(2) };
        _repoMock.Setup(r => r.GetByMunicipioIdAsync(5001, default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetByMunicipioAsync(5001);

        // Then
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.MunicipioNombre == "Medellín");
    }
}

#endregion

#region Microterritorio

public class MicroterritorioUseCaseTests
{
    private readonly Mock<IMicroterritorioRepository> _repoMock = new();
    private MicroterritorioUseCase CreateSut() => new(_repoMock.Object);

    private static Microterritorio BuildWithNav(int id = 1) => new()
    {
        Id           = id,
        TerritorioId = 10,
        Nombre       = "Barrio Centro",
        Territorio   = new Territorio { Id = 10, Nombre = "Zona Norte" }
    };

    private static Microterritorio BuildWithoutNav(int id = 1) => new()
        { Id = id, TerritorioId = 10, Nombre = "Barrio Centro" };

    [Fact]
    public async Task GetAllMicroterritoriosAsync_WithTerritorioNavProp_MapsTerritorioNombre()
    {
        // Given
        IReadOnlyList<Microterritorio> items = new List<Microterritorio> { BuildWithNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllMicroterritoriosAsync();

        // Then – rama NOT-NULL
        result.Should().HaveCount(1);
        result[0].TerritorioNombre.Should().Be("Zona Norte");
        result[0].Nombre.Should().Be("Barrio Centro");
    }

    [Fact]
    public async Task GetAllMicroterritoriosAsync_WithoutTerritorioNavProp_FallsBackToEmpty()
    {
        // Given
        IReadOnlyList<Microterritorio> items = new List<Microterritorio> { BuildWithoutNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllMicroterritoriosAsync();

        // Then – rama NULL → TerritorioNombre vacío
        result[0].TerritorioNombre.Should().Be("");
    }

    [Fact]
    public async Task GetMicroterritorioByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default)).ReturnsAsync(BuildWithNav());

        // When
        var result = await CreateSut().GetMicroterritorioByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Nombre.Should().Be("Barrio Centro");
        result.TerritorioNombre.Should().Be("Zona Norte");
    }

    [Fact]
    public async Task GetMicroterritorioByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Microterritorio?)null);

        // When / Then
        var result = await CreateSut().GetMicroterritorioByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByTerritorioAsync_WithMicroterritorios_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<Microterritorio> items = new List<Microterritorio>
            { BuildWithNav(1), BuildWithNav(2) };
        _repoMock.Setup(r => r.GetByTerritorioIdAsync(10, default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetByTerritorioAsync(10);

        // Then
        result.Should().HaveCount(2);
        result.Should().OnlyContain(m => m.TerritorioNombre == "Zona Norte");
    }
}

#endregion

#region Area

public class AreaUseCaseTests
{
    private readonly Mock<IAreaRepository> _repoMock = new();
    private AreaUseCase CreateSut() => new(_repoMock.Object);

    private static Area BuildWithNav(int id = 1) => new()
    {
        Id                = id,
        MicroterritorioId = 10,
        Nombre            = "Sector 1",
        Microterritorio   = new Microterritorio { Id = 10, Nombre = "Barrio Centro" }
    };

    private static Area BuildWithoutNav(int id = 1) => new()
        { Id = id, MicroterritorioId = 10, Nombre = "Sector 1" };

    [Fact]
    public async Task GetAllAreasAsync_WithMicroterritorioNavProp_MapsMicroterritorioNombre()
    {
        // Given
        IReadOnlyList<Area> items = new List<Area> { BuildWithNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllAreasAsync();

        // Then – rama NOT-NULL
        result.Should().HaveCount(1);
        result[0].MicroterritorioNombre.Should().Be("Barrio Centro");
        result[0].Nombre.Should().Be("Sector 1");
    }

    [Fact]
    public async Task GetAllAreasAsync_WithoutMicroterritorioNavProp_FallsBackToEmpty()
    {
        // Given
        IReadOnlyList<Area> items = new List<Area> { BuildWithoutNav() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllAreasAsync();

        // Then – rama NULL → MicroterritorioNombre vacío
        result[0].MicroterritorioNombre.Should().Be("");
    }

    [Fact]
    public async Task GetAreaByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(1, default)).ReturnsAsync(BuildWithNav());

        // When
        var result = await CreateSut().GetAreaByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Nombre.Should().Be("Sector 1");
        result.MicroterritorioNombre.Should().Be("Barrio Centro");
    }

    [Fact]
    public async Task GetAreaByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<int>(It.IsAny<int>(), default))
                 .ReturnsAsync((Area?)null);

        // When / Then
        var result = await CreateSut().GetAreaByIdAsync(999);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByMicroterritorioAsync_WithAreas_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<Area> items = new List<Area> { BuildWithNav(1), BuildWithNav(2) };
        _repoMock.Setup(r => r.GetByMicroterritorioIdAsync(10, default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetByMicroterritorioAsync(10);

        // Then
        result.Should().HaveCount(2);
        result.Should().OnlyContain(a => a.MicroterritorioNombre == "Barrio Centro");
    }
}

#endregion

#region Ubicacion

public class UbicacionUseCaseTests
{
    private readonly Mock<IUbicacionRepository> _repoMock = new();
    private UbicacionUseCase CreateSut() => new(_repoMock.Object);

    private static Ubicacion Build(long id = 1L) => new()
        { Id = id, Direccion = "Cra 10 # 20-30", Lat = 6.25, Lon = -75.56 };

    [Fact]
    public async Task GetAllUbicacionesAsync_WithItems_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<Ubicacion> items = new List<Ubicacion> { Build(1L), Build(2L) };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllUbicacionesAsync();

        // Then
        result.Should().HaveCount(2);
        result[0].Direccion.Should().Be("Cra 10 # 20-30");
        result[0].Lat.Should().Be(6.25);
        result[0].Lon.Should().Be(-75.56);
    }

    [Fact]
    public async Task GetUbicacionByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<long>(1L, default)).ReturnsAsync(Build(1L));

        // When
        var result = await CreateSut().GetUbicacionByIdAsync(1L);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(1L);
        result.Direccion.Should().Be("Cra 10 # 20-30");
    }

    [Fact]
    public async Task GetUbicacionByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<long>(It.IsAny<long>(), default))
                 .ReturnsAsync((Ubicacion?)null);

        // When / Then
        var result = await CreateSut().GetUbicacionByIdAsync(999L);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllUbicacionesAsync_WithNullDireccion_MapsNullDireccion()
    {
        // Given – Direccion puede ser null
        IReadOnlyList<Ubicacion> items = new List<Ubicacion>
            { new() { Id = 1L, Direccion = null, Lat = null, Lon = null } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllUbicacionesAsync();

        // Then – campos opcionales se mapean como null
        result[0].Direccion.Should().BeNull();
        result[0].Lat.Should().BeNull();
        result[0].Lon.Should().BeNull();
    }
}

#endregion
