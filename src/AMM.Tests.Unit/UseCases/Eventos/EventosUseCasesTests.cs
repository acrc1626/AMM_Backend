using AMM.Application.DTOs.Eventos;
using AMM.Application.UseCases.Eventos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Eventos;

// ═══════════════════════════════════════════════════════════════════════════════
// Tests de mapeo para los 7 EventoUseCases.
// La lógica de GetAll / GetById / GetPaged está en CatalogoUseCase<T> (base).
// Cada clase verifica el mapper específico de cada enfermedad y la paginación.
// ═══════════════════════════════════════════════════════════════════════════════

#region Escabiosis

public class EscabiosisUseCaseTests
{
    private readonly Mock<IEscabiosisRepository> _repoMock = new();
    private EscabiosisUseCase CreateSut() => new(_repoMock.Object);

    private static readonly DateTime _fecha = new(2025, 3, 10);

    private static Escabiosis BuildEntity(long id = 1L) => new()
    {
        Id            = id,
        PacienteId    = 5L,
        EventoTipoId  = 2,
        Fecha         = _fecha,
        EstadoId      = 1,
        Tratado       = true,
        CierreControl = false
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsTratadoAndCierreControl()
    {
        // Given – repositorio retorna dos registros de escabiosis
        IReadOnlyList<Escabiosis> entities = new List<Escabiosis>
            { BuildEntity(1L), BuildEntity(2L) };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When – se solicita la lista
        var result = await CreateSut().GetAllAsync();

        // Then – se mapean correctamente los campos específicos de escabiosis
        result.Should().HaveCount(2);
        result[0].Tratado.Should().BeTrue();
        result[0].CierreControl.Should().BeFalse();
        result[0].PacienteId.Should().Be(5L);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithEscabiosisFields()
    {
        // Given – existe una escabiosis con los campos específicos asignados
        var entity = BuildEntity(10L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(10L, default)).ReturnsAsync(entity);

        // When – se busca por ID
        var result = await CreateSut().GetByIdAsync(10L);

        // Then – el DTO contiene todos los campos incluyendo los de escabiosis
        result.Should().NotBeNull();
        result!.Id.Should().Be(10L);
        result.Tratado.Should().BeTrue();
        result.CierreControl.Should().BeFalse();
    }

    // ── Pruebas de base (aplican a todos los EventoUseCases) ──────────────────

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repositorio no encuentra el registro
        _repoMock.Setup(r => r.GetByIdAsync<long>(999L, default)).ReturnsAsync((Escabiosis?)null);

        // When / Then – null sin excepción
        var result = await CreateSut().GetByIdAsync(999L);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given – hay 12 registros en total, se pide página 2 con tamaño 5
        IReadOnlyList<Escabiosis> page = new List<Escabiosis> { BuildEntity(6L) };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(12);
        _repoMock.Setup(r => r.GetPagedAsync(5, 5, default)).ReturnsAsync(page);

        // When – se solicita la segunda página
        var result = await CreateSut().GetPagedAsync(page: 2, pageSize: 5);

        // Then – los metadatos de paginación son correctos
        result.Total.Should().Be(12);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalPages.Should().Be(3);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeTrue();
        result.Items.Should().HaveCount(1);
    }
}

#endregion

#region Geohelmintiasis

public class GeohelmintiasisUseCaseTests
{
    private readonly Mock<IGeohelmintiasisRepository> _repoMock = new();
    private GeohelmintiasisUseCase CreateSut() => new(_repoMock.Object);

    private static Geohelmintiasis BuildEntity(long id = 1L) => new()
    {
        Id            = id,
        PacienteId    = 5L,
        EventoTipoId  = 3,
        Fecha         = new DateTime(2025, 4, 1),
        EstadoId      = 1,
        Tratado       = false,
        CierreControl = true,
        Laboratorio   = true
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsLaboratorioField()
    {
        // Given – repositorio retorna un registro con Laboratorio = true
        IReadOnlyList<Geohelmintiasis> entities = new List<Geohelmintiasis> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When
        var result = await CreateSut().GetAllAsync();

        // Then – el campo Laboratorio exclusivo de geohelmintiasis está mapeado
        result.Should().HaveCount(1);
        result[0].Laboratorio.Should().BeTrue();
        result[0].Tratado.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithGeohelmintiasisFields()
    {
        // Given – existe una geohelmintiasis con Laboratorio asignado
        var entity = BuildEntity(7L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(7L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetByIdAsync(7L);

        // Then
        result.Should().NotBeNull();
        result!.Laboratorio.Should().BeTrue();
        result.CierreControl.Should().BeTrue();
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given – hay 8 registros, se pide página 1 con tamaño 5
        IReadOnlyList<Geohelmintiasis> page = new List<Geohelmintiasis> { BuildEntity() };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(8);
        _repoMock.Setup(r => r.GetPagedAsync(0, 5, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetPagedAsync(page: 1, pageSize: 5);

        // Then
        result.Total.Should().Be(8);
        result.TotalPages.Should().Be(2);
        result.HasPreviousPage.Should().BeFalse();
    }
}

#endregion

#region Pediculosis

public class PediculosisUseCaseTests
{
    private readonly Mock<IPediculosisRepository> _repoMock = new();
    private PediculosisUseCase CreateSut() => new(_repoMock.Object);

    private static Pediculosis BuildEntity(long id = 1L) => new()
    {
        Id            = id,
        PacienteId    = 8L,
        EventoTipoId  = 4,
        Fecha         = new DateTime(2025, 5, 20),
        EstadoId      = 1,
        Tratado       = true,
        CierreControl = null
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsPediculosisFields()
    {
        // Given – repositorio retorna un registro de pediculosis
        IReadOnlyList<Pediculosis> entities = new List<Pediculosis> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When
        var result = await CreateSut().GetAllAsync();

        // Then – Tratado mapeado; CierreControl null es válido
        result.Should().HaveCount(1);
        result[0].Tratado.Should().BeTrue();
        result[0].CierreControl.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsPediculosisDto()
    {
        // Given
        var entity = BuildEntity(3L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(3L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetByIdAsync(3L);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(3L);
        result.PacienteId.Should().Be(8L);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given
        IReadOnlyList<Pediculosis> page = new List<Pediculosis> { BuildEntity() };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(3);
        _repoMock.Setup(r => r.GetPagedAsync(0, 10, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetPagedAsync(page: 1, pageSize: 10);

        // Then
        result.Total.Should().Be(3);
        result.HasNextPage.Should().BeFalse();
    }
}

#endregion

#region Malaria

public class MalariaUseCaseTests
{
    private readonly Mock<IMalariaRepository> _repoMock = new();
    private MalariaUseCase CreateSut() => new(_repoMock.Object);

    private static Malaria BuildEntity(long id = 1L) => new()
    {
        Id           = id,
        PacienteId   = 11L,
        EventoTipoId = 5,
        Fecha        = new DateTime(2025, 6, 1),
        EstadoId     = 1,
        Gota         = true,
        Resultado    = "Positivo Plasmodium falciparum"
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsGotaAndResultado()
    {
        // Given – repositorio retorna un caso de malaria
        IReadOnlyList<Malaria> entities = new List<Malaria> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When
        var result = await CreateSut().GetAllAsync();

        // Then – campos específicos de malaria están correctamente mapeados
        result.Should().HaveCount(1);
        result[0].Gota.Should().BeTrue();
        result[0].Resultado.Should().Be("Positivo Plasmodium falciparum");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsMalariaDto()
    {
        // Given
        var entity = BuildEntity(20L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(20L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetByIdAsync(20L);

        // Then
        result.Should().NotBeNull();
        result!.Gota.Should().BeTrue();
        result.Resultado.Should().Be("Positivo Plasmodium falciparum");
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given
        IReadOnlyList<Malaria> page = new List<Malaria> { BuildEntity() };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(5);
        _repoMock.Setup(r => r.GetPagedAsync(0, 5, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetPagedAsync(page: 1, pageSize: 5);

        // Then
        result.Total.Should().Be(5);
        result.TotalPages.Should().Be(1);
    }
}

#endregion

#region Tuberculosis

public class TuberculosisUseCaseTests
{
    private readonly Mock<ITuberculosisRepository> _repoMock = new();
    private TuberculosisUseCase CreateSut() => new(_repoMock.Object);

    private static Tuberculosis BuildEntity(long id = 1L) => new()
    {
        Id           = id,
        PacienteId   = 15L,
        EventoTipoId = 6,
        Fecha        = new DateTime(2025, 7, 15),
        EstadoId     = 1,
        Sintomatico  = true,
        Resultado    = "BK positivo"
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsSintomaticoAndResultado()
    {
        // Given – repositorio retorna un caso de tuberculosis
        IReadOnlyList<Tuberculosis> entities = new List<Tuberculosis> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When
        var result = await CreateSut().GetAllAsync();

        // Then – campos exclusivos de TB están mapeados
        result.Should().HaveCount(1);
        result[0].Sintomatico.Should().BeTrue();
        result[0].Resultado.Should().Be("BK positivo");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsTuberculosisDto()
    {
        // Given
        var entity = BuildEntity(30L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(30L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetByIdAsync(30L);

        // Then
        result.Should().NotBeNull();
        result!.Sintomatico.Should().BeTrue();
        result.Resultado.Should().Be("BK positivo");
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given
        IReadOnlyList<Tuberculosis> page = new List<Tuberculosis> { BuildEntity() };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(10);
        _repoMock.Setup(r => r.GetPagedAsync(5, 5, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetPagedAsync(page: 2, pageSize: 5);

        // Then
        result.Total.Should().Be(10);
        result.TotalPages.Should().Be(2);
        result.HasPreviousPage.Should().BeTrue();
    }
}

#endregion

#region TuberculosisContacto

public class TuberculosisContactoUseCaseTests
{
    private readonly Mock<ITuberculosisContactoRepository> _repoMock = new();
    private TuberculosisContactoUseCase CreateSut() => new(_repoMock.Object);

    private static TuberculosisContacto BuildEntity(long id = 1L) => new()
    {
        Id           = id,
        PacienteId   = 20L,
        EventoTipoId = 7,
        Fecha        = new DateTime(2025, 8, 1),
        EstadoId     = 1,
        IndexId      = 99L,
        ParentescoId = 2
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsIndexIdAndParentescoId()
    {
        // Given – repositorio retorna un contacto de TB con índice y parentesco
        IReadOnlyList<TuberculosisContacto> entities = new List<TuberculosisContacto>
            { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When
        var result = await CreateSut().GetAllAsync();

        // Then – IndexId y ParentescoId están mapeados; Parentesco nav es null → null en DTO
        result.Should().HaveCount(1);
        result[0].IndexId.Should().Be(99L);
        result[0].ParentescoId.Should().Be(2);
        result[0].Parentesco.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsTuberculosisContactoDto()
    {
        // Given
        var entity = BuildEntity(50L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(50L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetByIdAsync(50L);

        // Then
        result.Should().NotBeNull();
        result!.IndexId.Should().Be(99L);
        result.ParentescoId.Should().Be(2);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given
        IReadOnlyList<TuberculosisContacto> page = new List<TuberculosisContacto> { BuildEntity() };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(6);
        _repoMock.Setup(r => r.GetPagedAsync(0, 10, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetPagedAsync(page: 1, pageSize: 10);

        // Then
        result.Total.Should().Be(6);
        result.HasNextPage.Should().BeFalse();
    }
}

#endregion

#region LeshmaniasisCutanea

public class LeshmaniasisCutaneaUseCaseTests
{
    private readonly Mock<ILeshmaniasisCutaneaRepository> _repoMock = new();
    private LeshmaniasisCutaneaUseCase CreateSut() => new(_repoMock.Object);

    private static LeshmaniasisCutanea BuildEntity(long id = 1L) => new()
    {
        Id             = id,
        PacienteId     = 25L,
        EventoTipoId   = 8,
        Fecha          = new DateTime(2025, 9, 5),
        EstadoId       = 1,
        Cicatriz       = true,
        NumeroLesiones = 3
    };

    [Fact]
    public async Task GetAllAsync_WithEvents_MapsCicatrizAndNumeroLesiones()
    {
        // Given – repositorio retorna un caso de leishmaniasis
        IReadOnlyList<LeshmaniasisCutanea> entities = new List<LeshmaniasisCutanea>
            { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        // When
        var result = await CreateSut().GetAllAsync();

        // Then – campos específicos de leishmaniasis están mapeados
        result.Should().HaveCount(1);
        result[0].Cicatriz.Should().BeTrue();
        result[0].NumeroLesiones.Should().Be(3);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsLeshmaniasisDto()
    {
        // Given
        var entity = BuildEntity(60L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(60L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetByIdAsync(60L);

        // Then
        result.Should().NotBeNull();
        result!.Cicatriz.Should().BeTrue();
        result.NumeroLesiones.Should().Be(3);
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsCorrectPaginationMetadata()
    {
        // Given
        IReadOnlyList<LeshmaniasisCutanea> page = new List<LeshmaniasisCutanea> { BuildEntity() };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(15);
        _repoMock.Setup(r => r.GetPagedAsync(10, 10, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetPagedAsync(page: 2, pageSize: 10);

        // Then
        result.Total.Should().Be(15);
        result.TotalPages.Should().Be(2);
        result.HasPreviousPage.Should().BeTrue();
        result.HasNextPage.Should().BeFalse();
    }
}

#endregion
