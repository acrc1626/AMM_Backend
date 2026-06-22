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

    private static readonly DateTime _fecha = new(2025, 6, 15);

    private static Censo BuildSavedCenso(long id = 1L) => new()
    {
        Id            = id,
        TipoEntornoId = 1,
        Fecha         = _fecha,
        EstadoId      = 1
    };

    [Fact]
    public async Task GetAllCensosAsync_WithExistingCensos_ReturnsCorrectList()
    {
        IReadOnlyList<Censo> censos = new List<Censo>
        {
            BuildSavedCenso(id: 1L),
            BuildSavedCenso(id: 2L)
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(censos);

        var result = await CreateSut().GetAllCensosAsync();

        result.Should().HaveCount(2);
        result.Select(c => c.Id).Should().BeEquivalentTo(new[] { 1L, 2L });
    }

    [Fact]
    public async Task GetCensoByIdAsync_NonExistentId_ReturnsNull()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(999L, default))
                 .ReturnsAsync((Censo?)null);

        var result = await CreateSut().GetCensoByIdAsync(999L);

        result.Should().BeNull();
    }

    /// <summary>
    /// HU-005 / RN-011, RN-012:
    /// UpdateAsync solo modifica campos del censo — TipoEntornoId permanece intacto.
    /// </summary>
    [Fact]
    public async Task UpdateAsync_ExistingCenso_UpdatesEditableFieldsAndPreservesTipoEntorno()
    {
        var entity = BuildSavedCenso(id: 5L); // TipoEntornoId = 1
        _repoMock.Setup(r => r.GetByIdAsync<long>(5L, default)).ReturnsAsync(entity);

        var request = new UpdateCensoRequest(
            Id:             5L,
            UbicacionId:    null,
            DepartamentoId: null,
            MunicipioId:    null,
            Fecha:          _fecha.AddDays(7),
            Observacion:    "Observación actualizada",
            EstadoId:       2
        );

        await CreateSut().UpdateAsync(request, "editor@ins.gov.co");

        // Campos editables actualizados
        entity.Observacion.Should().Be("Observación actualizada");
        entity.EstadoId.Should().Be(2);
        entity.ModificadoPor.Should().Be("editor@ins.gov.co");

        // TipoEntornoId no debe cambiar (RN-007 implícito en UpdateAsync)
        entity.TipoEntornoId.Should().Be(1);

        _repoMock.Verify(r => r.UpdateAsync(entity, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentCenso_ThrowsKeyNotFoundException()
    {
        _repoMock.Setup(r => r.GetByIdAsync<long>(404L, default)).ReturnsAsync((Censo?)null);

        var request = new UpdateCensoRequest(404L, null, null, null, _fecha, null, 1);

        await CreateSut()
            .Invoking(s => s.UpdateAsync(request, "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*404*");
    }

    [Fact]
    public async Task GetAllCensosPagedAsync_ReturnsPagedResult()
    {
        IReadOnlyList<Censo> page = new List<Censo> { BuildSavedCenso(6L) };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(20);
        _repoMock.Setup(r => r.GetPagedAsync(5, 5, default)).ReturnsAsync(page);

        var result = await CreateSut().GetAllCensosPagedAsync(page: 2, pageSize: 5);

        result.Total.Should().Be(20);
        result.Page.Should().Be(2);
        result.TotalPages.Should().Be(4);
        result.Items.Should().HaveCount(1);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_DelegatesDeleteToRepositoryAndSaves()
    {
        _repoMock.Setup(r => r.DeleteAsync<long>(It.IsAny<long>(), default))
                 .Returns(Task.CompletedTask);

        await CreateSut().DeleteAsync(1L);

        _repoMock.Verify(r => r.DeleteAsync<long>(1L, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}
