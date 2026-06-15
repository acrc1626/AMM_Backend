using AMM.Application.DTOs;
using AMM.Application.UseCases.Pacientes;
using AMM.Domain.Entities;
using AMM.Domain.Ports;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Pacientes;

public class GetPacienteByIdUseCaseTests
{
    private readonly Mock<IPacienteRepository> _repoMock = new();
    private GetPacienteByIdUseCase CreateSut() => new(_repoMock.Object);

    private static Paciente BuildPaciente(bool withNavProps = false) => new()
    {
        Id              = 10L,
        TipoDocumentoId = 1,
        Documento       = "1234567890",
        PrimerNombre    = "Ana",
        SegundoNombre   = "María",
        PrimerApellido  = "López",
        SegundoApellido = "Ríos",
        SexoId          = 2,
        FechaNacimiento = new DateTime(1990, 3, 15),
        EstadoId        = 1,
        TipoDocumento   = withNavProps ? new TipoDocumento { Id = 1, Descripcion = "Cédula" }   : null!,
        Sexo            = withNavProps ? new Sexo           { Id = 2, Descripcion = "Femenino" } : null!
    };

    // ─── Test 1: Paciente encontrado con nav props ────────────────────────────

    [Fact]
    public async Task ExecuteAsync_ExistingId_ReturnsDtoWithNavProps()
    {
        // Given – el paciente tiene TipoDocumento y Sexo cargados como nav props
        var paciente = BuildPaciente(withNavProps: true);
        _repoMock.Setup(r => r.GetByIdAsync(10L, default)).ReturnsAsync(paciente);

        // When
        var result = await CreateSut().ExecuteAsync(10L);

        // Then – el DTO usa las descripciones de las nav props
        result.Should().NotBeNull();
        result!.Id.Should().Be(10L);
        result.TipoDocumento.Should().Be("Cédula");
        result.Sexo.Should().Be("Femenino");
        result.NombreCompleto.Should().Contain("Ana").And.Contain("López");
        result.Edad.Should().BeGreaterThan(0);
    }

    // ─── Test 2: Paciente encontrado sin nav props (fallback a ToString) ──────

    [Fact]
    public async Task ExecuteAsync_ExistingIdWithoutNavProps_UsesFallbackToString()
    {
        // Given – TipoDocumento y Sexo no están cargados (null nav props)
        var paciente = BuildPaciente(withNavProps: false);
        _repoMock.Setup(r => r.GetByIdAsync(10L, default)).ReturnsAsync(paciente);

        // When
        var result = await CreateSut().ExecuteAsync(10L);

        // Then – el DTO usa el ToString() del ID como fallback
        result.Should().NotBeNull();
        result!.TipoDocumento.Should().Be("1");  // TipoDocumentoId.ToString()
        result.Sexo.Should().Be("2");              // SexoId.ToString()
    }

    // ─── Test 3: Sin fecha de nacimiento → edad nula ──────────────────────────

    [Fact]
    public async Task ExecuteAsync_PacienteWithoutFechaNacimiento_ReturnsNullEdad()
    {
        // Given – paciente sin fecha de nacimiento registrada
        var paciente = BuildPaciente();
        paciente.FechaNacimiento = null;
        _repoMock.Setup(r => r.GetByIdAsync(10L, default)).ReturnsAsync(paciente);

        // When
        var result = await CreateSut().ExecuteAsync(10L);

        // Then – Edad debe ser null cuando no hay fecha de nacimiento
        result!.Edad.Should().BeNull();
    }

    // ─── Test 4: No encontrado ────────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repositorio no encuentra ningún paciente con ese ID
        _repoMock.Setup(r => r.GetByIdAsync(999L, default)).ReturnsAsync((Paciente?)null);

        // When / Then
        var result = await CreateSut().ExecuteAsync(999L);
        result.Should().BeNull();
    }
}

public class GetAllPacientesUseCaseTests
{
    private readonly Mock<IPacienteRepository> _repoMock = new();
    private GetAllPacientesUseCase CreateSut() => new(_repoMock.Object);

    [Fact]
    public async Task ExecuteAsync_WithPacientes_ReturnsMappedList()
    {
        // Given – el repositorio retorna dos pacientes con fechas de nacimiento
        IReadOnlyList<Paciente> pacientes = new List<Paciente>
        {
            new()
            {
                Id = 1L, TipoDocumentoId = 1, Documento = "111",
                PrimerNombre = "Luis", PrimerApellido = "Mora", SexoId = 1,
                FechaNacimiento = new DateTime(1985, 6, 10), EstadoId = 1
            },
            new()
            {
                Id = 2L, TipoDocumentoId = 1, Documento = "222",
                PrimerNombre = "Clara", PrimerApellido = "Ruiz", SexoId = 2,
                FechaNacimiento = null, EstadoId = 1
            }
        };
        _repoMock.Setup(r => r.GetAllAsync(0, 50, default)).ReturnsAsync(pacientes);

        // When – se solicita la lista con skip=0, take=50
        var result = await CreateSut().ExecuteAsync(0, 50);

        // Then – se obtienen los dos pacientes con edad calculada correctamente
        result.Should().HaveCount(2);
        result[0].NombreCompleto.Should().Contain("Luis");
        result[0].Edad.Should().BeGreaterThan(0);  // FechaNacimiento set
        result[1].Edad.Should().BeNull();           // FechaNacimiento null
    }

    [Fact]
    public async Task ExecuteAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Given – no hay pacientes registrados
        _repoMock.Setup(r => r.GetAllAsync(0, 10, default))
                 .ReturnsAsync(new List<Paciente>());

        // When / Then
        var result = await CreateSut().ExecuteAsync(0, 10);
        result.Should().BeEmpty();
    }
}

public class UpdatePacienteUseCaseTests
{
    private readonly Mock<IPacienteRepository> _repoMock = new();
    private UpdatePacienteUseCase CreateSut() => new(_repoMock.Object);

    private static UpdatePacienteRequest BuildUpdateRequest(long id = 1L) => new()
    {
        Id              = id,
        TipoDocumentoId = 2,
        Documento       = "9876543210",
        PrimerNombre    = "Carlos",
        PrimerApellido  = "Mendoza",
        SexoId          = 1,
        FechaNacimiento = new DateTime(1988, 11, 5),
        EstadoId        = 1
    };

    // ─── Test 1: Actualizar paciente existente ────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_ExistingPaciente_UpdatesAllFieldsAndSaves()
    {
        // Given – el repositorio encuentra al paciente
        var paciente = new Paciente
        {
            Id = 1L, TipoDocumentoId = 1, Documento = "1111",
            PrimerNombre = "Antiguo", PrimerApellido = "Nombre", SexoId = 2, EstadoId = 1
        };
        _repoMock.Setup(r => r.GetByIdAsync(1L, default)).ReturnsAsync(paciente);

        // When
        await CreateSut().ExecuteAsync(BuildUpdateRequest(1L), "editor@ins.gov.co");

        // Then – los campos se actualizaron en la entidad en memoria
        paciente.PrimerNombre.Should().Be("Carlos");
        paciente.Documento.Should().Be("9876543210");
        paciente.TipoDocumentoId.Should().Be(2);
        paciente.ModificadoPor.Should().Be("editor@ins.gov.co");
        _repoMock.Verify(r => r.UpdateAsync(paciente, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 2: Paciente no encontrado ──────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_NonExistentPaciente_ThrowsKeyNotFoundException()
    {
        // Given – el repositorio no encuentra al paciente
        _repoMock.Setup(r => r.GetByIdAsync(99L, default)).ReturnsAsync((Paciente?)null);

        // When / Then
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(BuildUpdateRequest(99L), "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*99*");

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Paciente>(), default), Times.Never);
    }
}
