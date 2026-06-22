using AMM.Application.DTOs.Eventos;
using AMM.Application.UseCases.Eventos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Eventos;

#region EventoEscabiosis

public class EventoEscabiosisUseCaseTests
{
    private readonly Mock<IEventoEscabiosisRepository> _repoMock = new();
    private EventoEscabiosisUseCase CreateSut() => new(_repoMock.Object);

    private static EventoEscabiosis BuildEntity(long eventoId = 1L) => new()
    {
        EventoId    = eventoId,
        Severidad   = 2,
        Localizacion = "Tronco"
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsSeveridadAndLocalizacion()
    {
        IReadOnlyList<EventoEscabiosis> entities = new List<EventoEscabiosis>
            { BuildEntity(1L), BuildEntity(2L) };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(2);
        result[0].Severidad.Should().Be(2);
        result[0].Localizacion.Should().Be("Tronco");
        result[0].EventoId.Should().Be(1L);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithEscabiosisFields()
    {
        var entity = BuildEntity(10L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(10L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(10L);

        result.Should().NotBeNull();
        result!.EventoId.Should().Be(10L);
        result.Severidad.Should().Be(2);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(999L, default))
            .ReturnsAsync((EventoEscabiosis?)null);

        var result = await CreateSut().GetByIdAsync(999L);

        result.Should().BeNull();
    }
}

#endregion

#region EventoGeohelmintiasis

public class EventoGeohelmintiasisUseCaseTests
{
    private readonly Mock<IEventoGeohelmintiasisRepository> _repoMock = new();
    private EventoGeohelmintiasisUseCase CreateSut() => new(_repoMock.Object);

    private static EventoGeohelmintiasis BuildEntity(long eventoId = 1L) => new()
    {
        EventoId  = eventoId,
        TipoPrueba = "Kato-Katz",
        Resultado  = "Positivo"
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsTipoPruebaAndResultado()
    {
        IReadOnlyList<EventoGeohelmintiasis> entities = new List<EventoGeohelmintiasis>
            { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(1);
        result[0].TipoPrueba.Should().Be("Kato-Katz");
        result[0].Resultado.Should().Be("Positivo");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithGeohelmintiasisFields()
    {
        var entity = BuildEntity(7L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(7L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(7L);

        result.Should().NotBeNull();
        result!.TipoPrueba.Should().Be("Kato-Katz");
        result.Resultado.Should().Be("Positivo");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(0L, default))
            .ReturnsAsync((EventoGeohelmintiasis?)null);

        var result = await CreateSut().GetByIdAsync(0L);

        result.Should().BeNull();
    }
}

#endregion

#region EventoPediculosis

public class EventoPediculosisUseCaseTests
{
    private readonly Mock<IEventoPediculosisRepository> _repoMock = new();
    private EventoPediculosisUseCase CreateSut() => new(_repoMock.Object);

    private static EventoPediculosis BuildEntity(long eventoId = 1L) => new()
    {
        EventoId = eventoId,
        Grado    = 3
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsGrado()
    {
        IReadOnlyList<EventoPediculosis> entities = new List<EventoPediculosis>
            { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Grado.Should().Be(3);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithGrado()
    {
        var entity = BuildEntity(3L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(3L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(3L);

        result.Should().NotBeNull();
        result!.EventoId.Should().Be(3L);
        result.Grado.Should().Be(3);
    }

    [Fact]
    public async Task GetByIdAsync_NullGrado_ReturnsDtoWithNullGrado()
    {
        var entity = new EventoPediculosis { EventoId = 5L, Grado = null };
        _repoMock.Setup(r => r.GetByIdAsync<long>(5L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(5L);

        result.Should().NotBeNull();
        result!.Grado.Should().BeNull();
    }
}

#endregion

#region EventoPian

public class EventoPianUseCaseTests
{
    private readonly Mock<IEventoPianRepository> _repoMock = new();
    private EventoPianUseCase CreateSut() => new(_repoMock.Object);

    private static EventoPian BuildEntity(long eventoId = 1L) => new()
    {
        EventoId = eventoId,
        Estadio  = "Primario"
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsEstadio()
    {
        IReadOnlyList<EventoPian> entities = new List<EventoPian> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Estadio.Should().Be("Primario");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithEstadio()
    {
        var entity = BuildEntity(20L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(20L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(20L);

        result.Should().NotBeNull();
        result!.Estadio.Should().Be("Primario");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(99L, default))
            .ReturnsAsync((EventoPian?)null);

        var result = await CreateSut().GetByIdAsync(99L);

        result.Should().BeNull();
    }
}

#endregion

#region EventoTeniasisCisticercosis

public class EventoTeniasisCisticercosisUseCaseTests
{
    private readonly Mock<IEventoTeniasisCisticercosisRepository> _repoMock = new();
    private EventoTeniasisCisticercosisUseCase CreateSut() => new(_repoMock.Object);

    private static EventoTeniasisCisticercosis BuildEntity(long eventoId = 1L) => new()
    {
        EventoId    = eventoId,
        Teniasis    = true,
        Cisticercosis = false
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsTeniasisAndCisticercosis()
    {
        IReadOnlyList<EventoTeniasisCisticercosis> entities =
            new List<EventoTeniasisCisticercosis> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(1);
        result[0].Teniasis.Should().BeTrue();
        result[0].Cisticercosis.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithBothFlags()
    {
        var entity = BuildEntity(30L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(30L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(30L);

        result.Should().NotBeNull();
        result!.Teniasis.Should().BeTrue();
        result.Cisticercosis.Should().BeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(0L, default))
            .ReturnsAsync((EventoTeniasisCisticercosis?)null);

        var result = await CreateSut().GetByIdAsync(0L);

        result.Should().BeNull();
    }
}

#endregion

#region EventoTracoma

public class EventoTracomaUseCaseTests
{
    private readonly Mock<IEventoTracomaRepository> _repoMock = new();
    private EventoTracomaUseCase CreateSut() => new(_repoMock.Object);

    private static EventoTracoma BuildEntity(long eventoId = 1L) => new()
    {
        EventoId           = eventoId,
        CriterioExclusionId = 1,
        ExaminadoTt        = true,
        Triquiasis         = 2,
        OpacidadCorneal    = "Moderada"
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsAllTracomaFields()
    {
        IReadOnlyList<EventoTracoma> entities = new List<EventoTracoma> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(1);
        result[0].ExaminadoTt.Should().BeTrue();
        result[0].Triquiasis.Should().Be(2);
        result[0].OpacidadCorneal.Should().Be("Moderada");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithTracomaFields()
    {
        var entity = BuildEntity(50L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(50L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(50L);

        result.Should().NotBeNull();
        result!.CriterioExclusionId.Should().Be(1);
        result.OpacidadCorneal.Should().Be("Moderada");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(0L, default))
            .ReturnsAsync((EventoTracoma?)null);

        var result = await CreateSut().GetByIdAsync(0L);

        result.Should().BeNull();
    }
}

#endregion

#region EventoTungiasis

public class EventoTungiasisUseCaseTests
{
    private readonly Mock<IEventoTungiasisRepository> _repoMock = new();
    private EventoTungiasisUseCase CreateSut() => new(_repoMock.Object);

    private static EventoTungiasis BuildEntity(long eventoId = 1L) => new()
    {
        EventoId       = eventoId,
        NumeroLesiones = 5,
        Complicaciones = "Infección secundaria"
    };

    [Fact]
    public async Task GetAllAsync_WithRecords_MapsNumeroLesionesAndComplicaciones()
    {
        IReadOnlyList<EventoTungiasis> entities = new List<EventoTungiasis> { BuildEntity() };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(entities);

        var result = await CreateSut().GetAllAsync();

        result.Should().HaveCount(1);
        result[0].NumeroLesiones.Should().Be(5);
        result[0].Complicaciones.Should().Be("Infección secundaria");
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDtoWithTungiasisFields()
    {
        var entity = BuildEntity(60L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(60L, default)).ReturnsAsync(entity);

        var result = await CreateSut().GetByIdAsync(60L);

        result.Should().NotBeNull();
        result!.EventoId.Should().Be(60L);
        result.NumeroLesiones.Should().Be(5);
        result.Complicaciones.Should().Be("Infección secundaria");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(0L, default))
            .ReturnsAsync((EventoTungiasis?)null);

        var result = await CreateSut().GetByIdAsync(0L);

        result.Should().BeNull();
    }
}

#endregion
