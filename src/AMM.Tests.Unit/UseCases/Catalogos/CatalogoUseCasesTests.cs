using AMM.Application.DTOs.Catalogos;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Catalogos;

// ─────────────────────────────────────────────────────────────────────────────
// Tests para los Use Cases de catálogos simples.
// EstadoUseCase cubre además los métodos base CatalogoUseCase.CreateAsync
// y CatalogoUseCase.DeleteAsync, que son los que faltaban al 64.2%.
// Los demás use cases verifican que el mapper específico es correcto.
// ─────────────────────────────────────────────────────────────────────────────

public class EstadoUseCaseTests
{
    private readonly Mock<IEstadoRepository> _repoMock = new();
    private EstadoUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllEstadosAsync_WithItems_ReturnsMappedList()
    {
        // Given – repositorio retorna dos estados
        IReadOnlyList<Estado> estados = new List<Estado>
        {
            new() { Id = 1, Nombre = "Activo",   Descripcion = "Activo" },
            new() { Id = 2, Nombre = "Inactivo",  Descripcion = null }
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(estados);

        // When
        var result = await CreateSut().GetAllEstadosAsync();

        // Then – nombre e Id correctamente mapeados
        result.Should().HaveCount(2);
        result[0].Nombre.Should().Be("Activo");
        result[1].Descripcion.Should().BeNull();
    }

    [Fact]
    public async Task GetEstadoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given – repositorio encuentra el estado con Id = 1
        _repoMock.Setup(r => r.GetByIdAsync<byte>((byte)1, default))
                 .ReturnsAsync(new Estado { Id = 1, Nombre = "Activo" });

        // When
        var result = await CreateSut().GetEstadoByIdAsync(1);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Nombre.Should().Be("Activo");
    }

    [Fact]
    public async Task GetEstadoByIdAsync_NotFound_ReturnsNull()
    {
        // Given – no existe estado con ese Id
        _repoMock.Setup(r => r.GetByIdAsync<byte>(It.IsAny<byte>(), default))
                 .ReturnsAsync((Estado?)null);

        // When / Then
        var result = await CreateSut().GetEstadoByIdAsync(99);
        result.Should().BeNull();
    }

    // ─── Cubre CatalogoUseCase.CreateAsync (base) ────────────────────────────

    [Fact]
    public async Task CreateEstadoAsync_ValidRequest_CreatesViaBaseFactoryAndReturnsDto()
    {
        // Given – el repo persiste el estado recién creado
        var created = new Estado { Id = 1, Nombre = "Pendiente", Descripcion = "En revisión" };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Estado>(), default)).ReturnsAsync(created);

        // When
        var result = await CreateSut()
            .CreateEstadoAsync(new CrearEstadoRequest("Pendiente", "En revisión"));

        // Then – la fábrica creó la entidad, se persistió y se mapeó al DTO
        result.Should().NotBeNull();
        result.Nombre.Should().Be("Pendiente");
        result.Descripcion.Should().Be("En revisión");
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Cubre CatalogoUseCase.DeleteAsync (base) ────────────────────────────

    [Fact]
    public async Task DeleteAsync_ExistingId_DelegatesDeleteToRepositoryAndSaves()
    {
        // Given – el repo procesa la eliminación sin error
        _repoMock.Setup(r => r.DeleteAsync<byte>(It.IsAny<byte>(), default))
                 .Returns(Task.CompletedTask);

        // When
        await CreateSut().DeleteAsync((byte)1);

        // Then – se llamó DeleteAsync y luego SaveChanges, exactamente una vez cada uno
        _repoMock.Verify(r => r.DeleteAsync<byte>((byte)1, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}

public class TipoDocumentoUseCaseTests
{
    private readonly Mock<ITipoDocumentoRepository> _repoMock = new();
    private TipoDocumentoUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllTiposDocumentoAsync_WithItems_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<TipoDocumento> items = new List<TipoDocumento>
            { new() { Id = 1, Descripcion = "Cédula" }, new() { Id = 2, Descripcion = "Pasaporte" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllTiposDocumentoAsync();

        // Then
        result.Should().HaveCount(2);
        result[0].Descripcion.Should().Be("Cédula");
    }

    [Fact]
    public async Task GetTipoDocumentoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<byte>((byte)1, default))
                 .ReturnsAsync(new TipoDocumento { Id = 1, Descripcion = "Cédula" });

        // When / Then
        var result = await CreateSut().GetTipoDocumentoByIdAsync(1);
        result!.Descripcion.Should().Be("Cédula");
    }
}

public class SexoUseCaseTests
{
    private readonly Mock<ISexoRepository> _repoMock = new();
    private SexoUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllSexosAsync_WithItems_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<Sexo> items = new List<Sexo>
            { new() { Id = 1, Descripcion = "Masculino" }, new() { Id = 2, Descripcion = "Femenino" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllSexosAsync();

        // Then
        result.Should().HaveCount(2);
        result[1].Descripcion.Should().Be("Femenino");
    }

    [Fact]
    public async Task GetSexoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<byte>((byte)2, default))
                 .ReturnsAsync(new Sexo { Id = 2, Descripcion = "Femenino" });

        // When / Then
        var result = await CreateSut().GetSexoByIdAsync(2);
        result!.Descripcion.Should().Be("Femenino");
    }
}

public class EtniaUseCaseTests
{
    private readonly Mock<IEtniaRepository> _repoMock = new();
    private EtniaUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllEtniasAsync_WithItems_MapsCodigoField()
    {
        // Given
        IReadOnlyList<Etnia> items = new List<Etnia>
            { new() { Id = 1, Descripcion = "Indígena", Codigo = "IND" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllEtniasAsync();

        // Then
        result[0].Codigo.Should().Be("IND");
    }

    [Fact]
    public async Task GetEtniaByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<byte>((byte)1, default))
                 .ReturnsAsync(new Etnia { Id = 1, Descripcion = "Indígena", Codigo = "IND" });

        // When / Then
        var result = await CreateSut().GetEtniaByIdAsync(1);
        result!.Codigo.Should().Be("IND");
    }
}

public class PuebloIndigenaUseCaseTests
{
    private readonly Mock<IPuebloIndigenaRepository> _repoMock = new();
    private PuebloIndigenaUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllPueblosIndigenasAsync_WithItems_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<PuebloIndigena> items = new List<PuebloIndigena>
            { new() { Id = 1, Descripcion = "Wayuu" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllPueblosIndigenasAsync();

        // Then
        result[0].Descripcion.Should().Be("Wayuu");
    }

    [Fact]
    public async Task GetPuebloIndigenaByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<short>((short)1, default))
                 .ReturnsAsync(new PuebloIndigena { Id = 1, Descripcion = "Wayuu" });

        // When / Then
        var result = await CreateSut().GetPuebloIndigenaByIdAsync(1);
        result!.Descripcion.Should().Be("Wayuu");
    }
}

public class TipoEntornoUseCaseTests
{
    private readonly Mock<ITipoEntornoRepository> _repoMock = new();
    private TipoEntornoUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllTiposEntornoAsync_WithItems_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<TipoEntorno> items = new List<TipoEntorno>
            { new() { Id = 1, Descripcion = "Urbano" }, new() { Id = 2, Descripcion = "Rural" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllTiposEntornoAsync();

        // Then
        result.Should().HaveCount(2);
        result[1].Descripcion.Should().Be("Rural");
    }

    [Fact]
    public async Task GetTipoEntornoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<byte>((byte)1, default))
                 .ReturnsAsync(new TipoEntorno { Id = 1, Descripcion = "Urbano" });

        // When / Then
        var result = await CreateSut().GetTipoEntornoByIdAsync(1);
        result!.Descripcion.Should().Be("Urbano");
    }
}

public class EventoTipoUseCaseTests
{
    private readonly Mock<IEventoTipoRepository> _repoMock = new();
    private EventoTipoUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllEventoTiposAsync_WithItems_MapsCodigo()
    {
        // Given
        IReadOnlyList<EventoTipo> items = new List<EventoTipo>
            { new() { Id = 1, Codigo = "ESC", Nombre = "Escabiosis" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllEventoTiposAsync();

        // Then
        result[0].Codigo.Should().Be("ESC");
        result[0].Nombre.Should().Be("Escabiosis");
    }

    [Fact]
    public async Task GetEventoTipoByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<byte>((byte)1, default))
                 .ReturnsAsync(new EventoTipo { Id = 1, Codigo = "ESC", Nombre = "Escabiosis" });

        // When / Then
        var result = await CreateSut().GetEventoTipoByIdAsync(1);
        result!.Codigo.Should().Be("ESC");
    }
}

public class FormaFarmaceuticaUseCaseTests
{
    private readonly Mock<IFormaFarmaceuticaRepository> _repoMock = new();
    private FormaFarmaceuticaUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task GetAllFormasFarmaceuticasAsync_WithItems_ReturnsMappedList()
    {
        // Given
        IReadOnlyList<FormaFarmaceutica> items = new List<FormaFarmaceutica>
            { new() { Id = 1, Nombre = "Tableta" }, new() { Id = 2, Nombre = "Jarabe" } };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When
        var result = await CreateSut().GetAllFormasFarmaceuticasAsync();

        // Then
        result.Should().HaveCount(2);
        result[0].Nombre.Should().Be("Tableta");
    }

    [Fact]
    public async Task GetFormaFarmaceuticaByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given
        _repoMock.Setup(r => r.GetByIdAsync<short>((short)1, default))
                 .ReturnsAsync(new FormaFarmaceutica { Id = 1, Nombre = "Tableta" });

        // When / Then
        var result = await CreateSut().GetFormaFarmaceuticaByIdAsync(1);
        result!.Nombre.Should().Be("Tableta");
    }
}
