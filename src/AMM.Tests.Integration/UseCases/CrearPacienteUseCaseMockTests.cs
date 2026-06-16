using AMM.Application.DTOs;
using AMM.Application.UseCases.Pacientes;
using AMM.Domain.Entities;
using AMM.Domain.Ports;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Integration.UseCases;

// ─────────────────────────────────────────────────────────────────────────────
// Parte B — CrearPacienteUseCase con IPacienteRepository mockeado.
// Verifica la lógica de detección de duplicados: el Use Case delega
// la verificación de existencia en GetByDocumentoAsync, y solo llama
// a AddAsync cuando el documento es nuevo.
// ─────────────────────────────────────────────────────────────────────────────

public class CrearPacienteUseCaseMockTests
{
    private readonly Mock<IPacienteRepository> _repoMock = new();
    private CrearPacienteUseCase CreateSut() => new(_repoMock.Object);

    // ─── Builder ──────────────────────────────────────────────────────────────

    private static CrearPacienteRequest BuildRequest(
        byte tipoDocumentoId = 1,
        string documento = "10200300") => new()
    {
        TipoDocumentoId = tipoDocumentoId,
        Documento       = documento,
        PrimerNombre    = "Juan",
        PrimerApellido  = "Pérez",
        SexoId          = 1
    };

    // ─── Test 1: documento ya existe → lanza InvalidOperationException ────────

    [Fact]
    public async Task ExecuteAsync_ExistingDocumento_ThrowsAndNeverCallsAddAsync()
    {
        // Given – GetByDocumentoAsync retorna un paciente ya registrado
        var existente = new Paciente
        {
            Id = 99, TipoDocumentoId = 1, Documento = "10200300",
            PrimerNombre = "Ana", PrimerApellido = "López",
            SexoId = 2, CreadoEn = DateTime.UtcNow
        };
        _repoMock.Setup(r => r.GetByDocumentoAsync(1, "10200300", default))
                 .ReturnsAsync(existente);

        // When – se intenta crear un paciente con el mismo documento
        var act = () => CreateSut().ExecuteAsync(BuildRequest(tipoDocumentoId: 1, documento: "10200300"),
                                                 "tester");

        // Then – se lanza la excepción de duplicado
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*10200300*");

        // And – el repositorio nunca intentó persistir la entidad
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Paciente>(), default), Times.Never);
        _repoMock.Verify(r => r.SaveChangesAsync(default),               Times.Never);
    }

    // ─── Test 2: documento nuevo → llama AddAsync exactamente una vez ─────────

    [Fact]
    public async Task ExecuteAsync_NewDocumento_CallsAddAsyncOnceAndReturnsDto()
    {
        // Given – GetByDocumentoAsync no encuentra ningún paciente existente
        _repoMock.Setup(r => r.GetByDocumentoAsync(1, "99887766", default))
                 .ReturnsAsync((Paciente?)null);

        // And – AddAsync devuelve la entidad con Id asignado
        var saved = new Paciente
        {
            Id = 10, TipoDocumentoId = 1, Documento = "99887766",
            PrimerNombre = "Juan", PrimerApellido = "Pérez",
            SexoId = 1, CreadoEn = DateTime.UtcNow
        };
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Paciente>(), default))
                 .ReturnsAsync(saved);
        _repoMock.Setup(r => r.SaveChangesAsync(default)).ReturnsAsync(1);

        // When – se crea el paciente con un documento nuevo
        var result = await CreateSut().ExecuteAsync(
            BuildRequest(tipoDocumentoId: 1, documento: "99887766"), "tester");

        // Then – se retorna un DTO con el Id generado
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Documento.Should().Be("99887766");

        // And – AddAsync fue invocado exactamente una vez con una entidad con ese documento
        _repoMock.Verify(
            r => r.AddAsync(It.Is<Paciente>(p => p.Documento == "99887766"), default),
            Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 3: request null → ArgumentNullException antes de tocar el repo ──

    [Fact]
    public async Task ExecuteAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Given / When
        var act = () => CreateSut().ExecuteAsync(null!, "tester");

        // Then – falla temprano, el repositorio no se invoca
        await act.Should().ThrowAsync<ArgumentNullException>();

        _repoMock.Verify(r => r.GetByDocumentoAsync(
            It.IsAny<byte>(), It.IsAny<string>(), default), Times.Never);
    }
}
