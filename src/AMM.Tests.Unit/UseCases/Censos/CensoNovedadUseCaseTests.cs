using AMM.Application.DTOs.Censos;
using AMM.Application.UseCases.Censos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Censos;

public class CensoNovedadUseCaseTests
{
    private readonly Mock<ICensoNovedadRepository> _repoMock = new();
    private CensoNovedadUseCase CreateSut() => new(_repoMock.Object);

    private static readonly DateTime _fecha = new(2025, 7, 10);

    private static CensoNovedad BuildEntity(long id = 1L) => new()
    {
        Id                 = id,
        CensoId            = 20L,
        PacienteId         = 42L,
        TipoNovedadId      = 1,
        PresenciaNovedadId = 2,
        Observacion        = "Observación test",
        Fecha              = _fecha,
        EstadoId           = 1
    };

    private static CrearCensoNovedadRequest BuildValidRequest() => new(
        CensoId:            20L,
        PacienteId:         42L,
        TipoNovedadId:      1,
        PresenciaNovedadId: 2,
        TratamientoId:      null,
        Observacion:        "Observación test",
        Fecha:              _fecha,
        EstadoId:           1
    );

    // ─── Test 1: Listar novedades ─────────────────────────────────────────────

    [Fact]
    public async Task GetAllCensoNovedadesAsync_WithItems_ReturnsMappedList()
    {
        // Given – el repositorio tiene dos novedades registradas
        IReadOnlyList<CensoNovedad> items = new List<CensoNovedad>
            { BuildEntity(1L), BuildEntity(2L) };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(items);

        // When – se solicita la lista completa
        var result = await CreateSut().GetAllCensoNovedadesAsync();

        // Then – se obtienen las dos novedades con sus campos
        result.Should().HaveCount(2);
        result[0].CensoId.Should().Be(20L);
        result[0].TipoNovedadId.Should().Be(1);
    }

    // ─── Test 2: Obtener por ID (encontrado) ─────────────────────────────────

    [Fact]
    public async Task GetCensoNovedadByIdAsync_ExistingId_ReturnsMappedDto()
    {
        // Given – existe una novedad con ese ID
        var entity = BuildEntity(5L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(5L, default)).ReturnsAsync(entity);

        // When
        var result = await CreateSut().GetCensoNovedadByIdAsync(5L);

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(5L);
        result.PacienteId.Should().Be(42L);
    }

    // ─── Test 3: Obtener por ID (no encontrado) ───────────────────────────────

    [Fact]
    public async Task GetCensoNovedadByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repositorio no encuentra la novedad
        _repoMock.Setup(r => r.GetByIdAsync<long>(999L, default))
                 .ReturnsAsync((CensoNovedad?)null);

        // When / Then
        var result = await CreateSut().GetCensoNovedadByIdAsync(999L);
        result.Should().BeNull();
    }

    // ─── Test 4: Obtener por censo ────────────────────────────────────────────

    [Fact]
    public async Task GetByCensoAsync_WithExistingCenso_ReturnsNovedadesForCenso()
    {
        // Given – hay dos novedades asociadas al censo 20
        IReadOnlyList<CensoNovedad> novedades = new List<CensoNovedad>
            { BuildEntity(1L), BuildEntity(2L) };
        _repoMock.Setup(r => r.GetByCensoIdAsync(20L, default)).ReturnsAsync(novedades);

        // When – se filtra por censoId
        var result = await CreateSut().GetByCensoAsync(20L);

        // Then
        result.Should().HaveCount(2);
        result.All(n => n.CensoId == 20L).Should().BeTrue();
    }

    // ─── Test 5: Obtener por paciente ─────────────────────────────────────────

    [Fact]
    public async Task GetByPacienteAsync_WithExistingPaciente_ReturnsNovedadesForPaciente()
    {
        // Given – hay una novedad asociada al paciente 42
        IReadOnlyList<CensoNovedad> novedades = new List<CensoNovedad> { BuildEntity(1L) };
        _repoMock.Setup(r => r.GetByPacienteIdAsync(42L, default)).ReturnsAsync(novedades);

        // When
        var result = await CreateSut().GetByPacienteAsync(42L);

        // Then
        result.Should().HaveCount(1);
        result[0].PacienteId.Should().Be(42L);
    }

    // ─── Test 6: Crear novedad válida ─────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidRequest_CreatesCensoNovedadAndReturnsDto()
    {
        // Given – el repo persiste y recarga la entidad
        var saved = BuildEntity(10L);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<CensoNovedad>(), default)).ReturnsAsync(saved);
        _repoMock.Setup(r => r.GetByIdAsync<long>(10L, default)).ReturnsAsync(saved);

        // When
        var result = await CreateSut().CreateAsync(BuildValidRequest(), "admin@ins.gov.co");

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(10L);
        result.CensoId.Should().Be(20L);
        result.TipoNovedadId.Should().Be(1);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 7: Actualizar novedad existente ─────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ExistingNovedad_UpdatesFieldsAndSaves()
    {
        // Given – existe una novedad con datos originales
        var entity = BuildEntity(7L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(7L, default)).ReturnsAsync(entity);

        var updateRequest = new UpdateCensoNovedadRequest(
            Id:                 7L,
            CensoId:            20L,
            PacienteId:         42L,
            TipoNovedadId:      3,
            PresenciaNovedadId: 1,
            TratamientoId:      null,
            Observacion:        "Observación actualizada",
            Fecha:              _fecha,
            EstadoId:           2
        );

        // When
        await CreateSut().UpdateAsync(updateRequest, "editor@ins.gov.co");

        // Then – el TipoNovedadId fue actualizado y se persistió
        entity.TipoNovedadId.Should().Be(3);
        entity.Observacion.Should().Be("Observación actualizada");
        entity.EstadoId.Should().Be(2);
        entity.ModificadoPor.Should().Be("editor@ins.gov.co");
        _repoMock.Verify(r => r.UpdateAsync(entity, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 8: Actualizar novedad inexistente ───────────────────────────────

    [Fact]
    public async Task UpdateAsync_NonExistentNovedad_ThrowsKeyNotFoundException()
    {
        // Given – el repositorio no encuentra la novedad
        _repoMock.Setup(r => r.GetByIdAsync<long>(999L, default))
                 .ReturnsAsync((CensoNovedad?)null);

        var request = new UpdateCensoNovedadRequest(
            999L, 1L, null, 1, null, null, null, _fecha, 1);

        // When / Then
        await CreateSut()
            .Invoking(s => s.UpdateAsync(request, "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");
    }
}
