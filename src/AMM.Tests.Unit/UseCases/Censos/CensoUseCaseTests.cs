using AMM.Application.DTOs.Censos;
using AMM.Application.UseCases.Censos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Censos;

public class CensoUseCaseTests
{
    private readonly Mock<ICensoRepository> _repoMock = new();

    private CensoUseCase CreateSut() => new(_repoMock.Object);

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private static readonly DateTime _fecha = new(2025, 6, 15);

    private static CrearCensoRequest BuildValidRequest(long? pacienteId = 42L) => new(
        TipoEntornoId:  1,
        PacienteId:     pacienteId,
        UbicacionId:    null,
        DepartamentoId: null,
        MunicipioId:    null,
        Fecha:          _fecha,
        Observacion:    "Observación de prueba",
        EstadoId:       1
    );

    private static Censo BuildSavedCenso(long id = 1L) => new()
    {
        Id            = id,
        TipoEntornoId = 1,
        PacienteId    = 42L,
        Fecha         = _fecha,
        EstadoId      = 1
    };

    // ─── Test 1: Crear censo válido ───────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidRequest_CreatesCensoAndReturnsDto()
    {
        // Given – request con paciente asignado; el repo persiste y recarga la entidad completa
        var request = BuildValidRequest();
        var saved   = BuildSavedCenso(id: 10L);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Censo>(), default))
                 .ReturnsAsync(saved);
        _repoMock.Setup(r => r.GetByIdAsync<long>(10L, default))
                 .ReturnsAsync(saved);

        // When – se ejecuta el caso de uso
        var result = await CreateSut().CreateAsync(request, "admin@ins.gov.co");

        // Then – el DTO refleja la entidad persistida y se llamó SaveChanges una vez
        result.Should().NotBeNull();
        result.Id.Should().Be(10L);
        result.PacienteId.Should().Be(42L);
        result.Fecha.Should().Be(_fecha);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 2: Censo sin paciente ───────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_NullPacienteId_ThrowsArgumentException()
    {
        // Given – request en que PacienteId no fue especificado
        var request = BuildValidRequest(pacienteId: null);

        // When – se intenta crear el censo sin paciente asociado
        // Then – lanza ArgumentException con mención al campo paciente
        await CreateSut()
            .Invoking(s => s.CreateAsync(request, "admin"))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*paciente*");

        // Then – nunca debe intentar persistir
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Censo>(), default), Times.Never);
    }

    // ─── Test 3: Listar censos ─────────────────────────────────────────────────

    [Fact]
    public async Task GetAllCensosAsync_WithExistingCensos_ReturnsCorrectList()
    {
        // Given – el repositorio contiene dos censos registrados
        IReadOnlyList<Censo> censos = new List<Censo>
        {
            BuildSavedCenso(id: 1L),
            BuildSavedCenso(id: 2L)
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(censos);

        // When – se solicita la lista completa de censos
        var result = await CreateSut().GetAllCensosAsync();

        // Then – se obtienen exactamente dos registros con los IDs esperados
        result.Should().HaveCount(2);
        result.Select(c => c.Id).Should().BeEquivalentTo(new[] { 1L, 2L });
    }

    // ─── Test 4: Censo inexistente ─────────────────────────────────────────────

    [Fact]
    public async Task GetCensoByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repositorio no encuentra ningún censo con el ID solicitado
        _repoMock.Setup(r => r.GetByIdAsync<long>(999L, default))
                 .ReturnsAsync((Censo?)null);

        // When – se busca por un ID que no existe
        var result = await CreateSut().GetCensoByIdAsync(999L);

        // Then – se retorna null sin lanzar excepción
        result.Should().BeNull();
    }

    // ─── Test 5: UpdateAsync exitoso ──────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ExistingCenso_UpdatesAllFieldsAndSaves()
    {
        // Given – existe un censo con datos originales
        var entity = BuildSavedCenso(id: 5L);
        _repoMock.Setup(r => r.GetByIdAsync<long>(5L, default)).ReturnsAsync(entity);

        var request = new UpdateCensoRequest(
            Id:             5L,
            TipoEntornoId:  2,
            PacienteId:     99L,
            UbicacionId:    null,
            DepartamentoId: null,
            MunicipioId:    null,
            Fecha:          _fecha.AddDays(7),
            Observacion:    "Observación actualizada",
            EstadoId:       2
        );

        // When
        await CreateSut().UpdateAsync(request, "editor@ins.gov.co");

        // Then – los campos se actualizaron en la entidad en memoria
        entity.TipoEntornoId.Should().Be(2);
        entity.PacienteId.Should().Be(99L);
        entity.Observacion.Should().Be("Observación actualizada");
        entity.ModificadoPor.Should().Be("editor@ins.gov.co");
        _repoMock.Verify(r => r.UpdateAsync(entity, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 6: UpdateAsync no encontrado ───────────────────────────────────

    [Fact]
    public async Task UpdateAsync_NonExistentCenso_ThrowsKeyNotFoundException()
    {
        // Given – el repositorio no encuentra el censo con ese ID
        _repoMock.Setup(r => r.GetByIdAsync<long>(404L, default)).ReturnsAsync((Censo?)null);

        var request = new UpdateCensoRequest(404L, 1, null, null, null, null, _fecha, null, 1);

        // When / Then
        await CreateSut()
            .Invoking(s => s.UpdateAsync(request, "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*404*");
    }

    // ─── Test 7: GetByPacienteAsync ───────────────────────────────────────────

    [Fact]
    public async Task GetByPacienteAsync_WithExistingPaciente_ReturnsCensoList()
    {
        // Given – hay dos censos asociados al paciente 42
        IReadOnlyList<Censo> censos = new List<Censo>
            { BuildSavedCenso(1L), BuildSavedCenso(2L) };
        _repoMock.Setup(r => r.GetByPacienteIdAsync(42L, default)).ReturnsAsync(censos);

        // When
        var result = await CreateSut().GetByPacienteAsync(42L);

        // Then
        result.Should().HaveCount(2);
        result.Should().OnlyContain(c => c.PacienteId == 42L);
    }

    // ─── Test 8: GetAllCensosPagedAsync ───────────────────────────────────────

    [Fact]
    public async Task GetAllCensosPagedAsync_ReturnsPagedResult()
    {
        // Given – hay 20 censos en total, se pide la segunda página de 5
        IReadOnlyList<Censo> page = new List<Censo> { BuildSavedCenso(6L) };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(20);
        _repoMock.Setup(r => r.GetPagedAsync(5, 5, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetAllCensosPagedAsync(page: 2, pageSize: 5);

        // Then
        result.Total.Should().Be(20);
        result.Page.Should().Be(2);
        result.TotalPages.Should().Be(4);
        result.Items.Should().HaveCount(1);
    }

    // ─── Test 9: DeleteAsync (base CatalogoUseCase) ───────────────────────────

    [Fact]
    public async Task DeleteAsync_ExistingId_DelegatesDeleteToRepositoryAndSaves()
    {
        // Given – el repositorio procesa la eliminación sin error
        _repoMock.Setup(r => r.DeleteAsync<long>(It.IsAny<long>(), default))
                 .Returns(Task.CompletedTask);

        // When
        await CreateSut().DeleteAsync(1L);

        // Then – DeleteAsync y SaveChanges del repo base se llaman exactamente una vez
        _repoMock.Verify(r => r.DeleteAsync<long>(1L, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}
