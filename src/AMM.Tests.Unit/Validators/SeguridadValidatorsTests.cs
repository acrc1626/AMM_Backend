using AMM.Application.DTOs.Seguridad;
using AMM.Application.Validators;
using FluentAssertions;
using FluentValidation;

namespace AMM.Tests.Unit.Validators;

public class CrearUsuarioRequestValidatorTests
{
    private readonly IValidator<CrearUsuarioRequest> _v = new CrearUsuarioRequestValidator();

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Given – todos los campos con valores válidos
        _v.Validate(new CrearUsuarioRequest("user@ins.gov.co", "Nombre Completo", 1, null))
          .IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("",               "Nombre Completo", 1, "Correo")]
    [InlineData("notanemail",     "Nombre Completo", 1, "Correo")]
    [InlineData("user@ins.gov.co","",                1, "NombreCompleto")]
    [InlineData("user@ins.gov.co","Nombre Completo", 0, "EstadoUsuarioId")]
    public void Validate_InvalidInput_FailsOnExpectedProperty(
        string correo, string nombre, int estado, string expectedProperty)
    {
        // Given – campo inválido en la petición de creación de usuario
        var result = _v.Validate(new CrearUsuarioRequest(correo, nombre, (byte)estado, null));
        // When / Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedProperty);
    }
}

public class UpdateUsuarioRequestValidatorTests
{
    private readonly IValidator<UpdateUsuarioRequest> _v = new UpdateUsuarioRequestValidator();

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Given – todos los campos con valores válidos incluyendo Id positivo
        _v.Validate(new UpdateUsuarioRequest(1, "user@ins.gov.co", "Nombre", 1, null, null))
          .IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, "user@ins.gov.co", "Nombre", 1, "Id")]
    [InlineData(1, "notanemail",       "Nombre", 1, "Correo")]
    [InlineData(1, "user@ins.gov.co", "",        1, "NombreCompleto")]
    public void Validate_InvalidInput_FailsOnExpectedProperty(
        int id, string correo, string nombre, int estado, string expectedProperty)
    {
        // Given – campo inválido en la petición de actualización de usuario
        var result = _v.Validate(new UpdateUsuarioRequest(id, correo, nombre, (byte)estado, null, null));
        // When / Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedProperty);
    }
}
