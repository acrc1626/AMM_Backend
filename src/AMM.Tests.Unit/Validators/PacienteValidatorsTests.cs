using AMM.Application.DTOs;
using AMM.Application.Validators;
using FluentAssertions;
using FluentValidation;

namespace AMM.Tests.Unit.Validators;

public class CrearPacienteRequestValidatorTests
{
    private readonly IValidator<CrearPacienteRequest> _v = new CrearPacienteRequestValidator();

    private static CrearPacienteRequest BuildValid() => new()
    {
        TipoDocumentoId = 1,
        Documento       = "1234567890",
        PrimerNombre    = "María",
        PrimerApellido  = "García",
        SexoId          = 1,
        FechaNacimiento = new DateTime(1990, 1, 1)
    };

    // ─── Caso válido ──────────────────────────────────────────────────────────

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Given – todos los campos requeridos correctamente llenados
        _v.Validate(BuildValid()).IsValid.Should().BeTrue();
    }

    // ─── TipoDocumento ────────────────────────────────────────────────────────

    [Fact]
    public void Validate_TipoDocumentoIdZero_Fails()
    {
        // Given – tipo de documento no asignado (0 = valor por defecto de byte)
        var result = _v.Validate(BuildValid() with { TipoDocumentoId = 0 });
        // When / Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "TipoDocumentoId");
    }

    // ─── Documento ────────────────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyDocumento_Fails()
    {
        // Given – número de documento vacío
        var result = _v.Validate(BuildValid() with { Documento = "" });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Documento");
    }

    [Fact]
    public void Validate_DocumentoExceedsMaxLength_Fails()
    {
        // Given – documento supera el límite de 30 caracteres
        var result = _v.Validate(BuildValid() with { Documento = new string('9', 31) });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Documento");
    }

    // ─── Nombres y apellidos ─────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyPrimerNombre_Fails()
    {
        var result = _v.Validate(BuildValid() with { PrimerNombre = "" });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PrimerNombre");
    }

    [Fact]
    public void Validate_EmptyPrimerApellido_Fails()
    {
        var result = _v.Validate(BuildValid() with { PrimerApellido = "" });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PrimerApellido");
    }

    [Fact]
    public void Validate_SegundoNombreExceedsMaxLength_Fails()
    {
        // Given – SegundoNombre supera 80 chars; la regla When(not null) debe activarse
        var result = _v.Validate(BuildValid() with { SegundoNombre = new string('A', 81) });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SegundoNombre");
    }

    // ─── Sexo ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Validate_SexoIdZero_Fails()
    {
        // Given – sexo no asignado (0 = valor por defecto de byte)
        var result = _v.Validate(BuildValid() with { SexoId = 0 });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "SexoId");
    }

    // ─── Fecha de nacimiento ──────────────────────────────────────────────────

    [Fact]
    public void Validate_FutureFechaNacimiento_Fails()
    {
        // Given – fecha de nacimiento posterior a hoy (imposible biológicamente)
        var result = _v.Validate(BuildValid() with { FechaNacimiento = DateTime.Today.AddDays(1) });
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "FechaNacimiento");
    }

    [Fact]
    public void Validate_NullFechaNacimiento_IsValid()
    {
        // Given – fecha no informada; la regla solo aplica When(HasValue)
        _v.Validate(BuildValid() with { FechaNacimiento = null }).IsValid.Should().BeTrue();
    }
}
