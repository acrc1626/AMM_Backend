using AMM.Application.DTOs;
using AMM.Application.UseCases.Pacientes;
using AMM.Domain.Entities;
using AMM.Domain.Ports;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Pacientes;

public class CrearPacienteUseCaseTests
{
    private readonly Mock<IPacienteRepository> _repoMock = new();

    private CrearPacienteUseCase CreateSut() => new(_repoMock.Object);

    // ─── Helpers ─────────────────────────────────────────────────────────────

    private static CrearPacienteRequest BuildValidRequest() => new()
    {
        TipoDocumentoId = 1,
        Documento       = "1234567890",
        PrimerNombre    = "Juan",
        PrimerApellido  = "Perez",
        SexoId          = 1,
        FechaNacimiento = new DateTime(1990, 5, 15)
    };

    private static Paciente BuildSavedPaciente(CrearPacienteRequest req) => new()
    {
        Id              = 100,
        TipoDocumentoId = req.TipoDocumentoId,
        Documento       = req.Documento,
        PrimerNombre    = req.PrimerNombre,
        PrimerApellido  = req.PrimerApellido,
        SexoId          = req.SexoId,
        FechaNacimiento = req.FechaNacimiento,
        EstadoId        = AMM.Domain.Constants.EstadoId.Activo
    };

    // ─── Test 1: Paciente válido ──────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_ValidRequest_RegistersPatientAndReturnsDto()
    {
        // Given – no existe paciente con ese documento; el repo persiste y devuelve la entidad
        var request = BuildValidRequest();
        var saved   = BuildSavedPaciente(request);

        _repoMock.Setup(r => r.GetByDocumentoAsync(request.TipoDocumentoId, request.Documento, default))
                 .ReturnsAsync((Paciente?)null);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Paciente>(), default))
                 .ReturnsAsync(saved);

        // When – se ejecuta el caso de uso
        var result = await CreateSut().ExecuteAsync(request, "admin@ins.gov.co");

        // Then – el DTO contiene los datos del paciente y se persistió exactamente una vez
        result.Should().NotBeNull();
        result.Id.Should().Be(100);
        result.Documento.Should().Be("1234567890");
        result.NombreCompleto.Should().Contain("Juan").And.Contain("Perez");
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 2: Documento duplicado ─────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_DuplicateDocument_ThrowsInvalidOperationException()
    {
        // Given – ya existe un paciente registrado con el mismo tipo y número de documento
        var request  = BuildValidRequest();
        var existing = BuildSavedPaciente(request);

        _repoMock.Setup(r => r.GetByDocumentoAsync(request.TipoDocumentoId, request.Documento, default))
                 .ReturnsAsync(existing);

        // When – se intenta registrar el mismo paciente nuevamente
        // Then – lanza InvalidOperationException con el número de documento en el mensaje
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(request, "admin"))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"*{request.Documento}*");

        // Then – nunca debe intentar persistir un duplicado
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Paciente>(), default), Times.Never);
    }

    // ─── Test 3: Request nulo ─────────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_NullRequest_ThrowsArgumentNullException()
    {
        // Given – se pasa null como request (defensa en frontera del caso de uso)
        // When – se llama ExecuteAsync con null
        // Then – lanza ArgumentNullException antes de consultar el repositorio
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(null!, "admin"))
            .Should().ThrowAsync<ArgumentNullException>();

        _repoMock.Verify(r => r.GetByDocumentoAsync(
            It.IsAny<byte>(), It.IsAny<string>(), default), Times.Never);
    }

    // ─── Test 4: Repositorio falla ────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_RepositoryThrowsOnAdd_PropagatesException()
    {
        // Given – no existe duplicado pero el repositorio falla al persistir
        var request = BuildValidRequest();

        _repoMock.Setup(r => r.GetByDocumentoAsync(request.TipoDocumentoId, request.Documento, default))
                 .ReturnsAsync((Paciente?)null);
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Paciente>(), default))
                 .ThrowsAsync(new InvalidOperationException("Connection lost"));

        // When – se ejecuta el caso de uso
        // Then – la excepción del repositorio se propaga sin ser capturada
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(request, "admin"))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Connection lost");
    }
}
