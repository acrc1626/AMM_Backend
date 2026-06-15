using AMM.Application.DTOs.Auth;
using AMM.Application.Validators;
using FluentAssertions;
using FluentValidation;

namespace AMM.Tests.Unit.Validators;

public class LoginRequestValidatorTests
{
    private readonly IValidator<LoginRequest> _v = new LoginRequestValidator();

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Given – correo con formato válido y contraseña con longitud suficiente
        var req = new LoginRequest("admin@ins.gov.co", "Pass123");
        // When / Then
        _v.Validate(req).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("",               "Pass123", "Correo")]    // correo vacío
    [InlineData("notanemail",     "Pass123", "Correo")]    // formato inválido
    [InlineData("user@ins.gov.co","",        "Password")]  // contraseña vacía
    [InlineData("user@ins.gov.co","ab",      "Password")]  // contraseña < 6 chars
    public void Validate_InvalidInput_FailsOnExpectedProperty(
        string correo, string password, string expectedProperty)
    {
        // Given – campo inválido en la petición de login
        var req = new LoginRequest(correo, password);
        // When
        var result = _v.Validate(req);
        // Then – falla y el error apunta a la propiedad esperada
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedProperty);
    }
}

public class ChangePasswordRequestValidatorTests
{
    private readonly IValidator<ChangePasswordRequest> _v = new ChangePasswordRequestValidator();

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Given – contraseñas distintas con longitud mínima
        var req = new ChangePasswordRequest("OldPass123", "NewPass456");
        _v.Validate(req).IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_SamePasswords_FailsOnPasswordNuevo()
    {
        // Given – la nueva contraseña es idéntica a la actual (regla de negocio)
        var req = new ChangePasswordRequest("Igual123", "Igual123");
        // When / Then
        var result = _v.Validate(req);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "PasswordNuevo");
    }

    [Theory]
    [InlineData("",         "NewPass1", "PasswordActual")]  // actual vacía
    [InlineData("OldPass1", "",         "PasswordNuevo")]   // nueva vacía
    [InlineData("OldPass1", "ab",       "PasswordNuevo")]   // nueva < 6 chars
    public void Validate_InvalidInput_FailsOnExpectedProperty(
        string actual, string nueva, string expectedProperty)
    {
        // Given – campo inválido en la petición de cambio de contraseña
        var result = _v.Validate(new ChangePasswordRequest(actual, nueva));
        // When / Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedProperty);
    }
}

public class SetPasswordRequestValidatorTests
{
    private readonly IValidator<SetPasswordRequest> _v = new SetPasswordRequestValidator();

    [Fact]
    public void Validate_ValidRequest_IsValid()
    {
        // Given – ID positivo y contraseña con longitud mínima
        _v.Validate(new SetPasswordRequest(1, "Pass123")).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0, "Pass123", "UsuarioId")]  // ID cero no es válido
    [InlineData(1, "",        "Password")]   // contraseña vacía
    [InlineData(1, "abc",     "Password")]   // contraseña < 6 chars
    public void Validate_InvalidInput_FailsOnExpectedProperty(
        int usuarioId, string password, string expectedProperty)
    {
        // Given – campo inválido en la petición de establecer contraseña
        var result = _v.Validate(new SetPasswordRequest(usuarioId, password));
        // When / Then
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == expectedProperty);
    }
}
