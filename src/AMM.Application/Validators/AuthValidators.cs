using AMM.Application.DTOs.Auth;
using FluentValidation;

namespace AMM.Application.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Correo).NotEmpty().EmailAddress().WithMessage("Correo electrónico inválido.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.PasswordActual).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PasswordNuevo).NotEmpty().MinimumLength(6);
        RuleFor(x => x.PasswordNuevo)
            .NotEqual(x => x.PasswordActual)
            .WithMessage("La nueva contraseña debe ser diferente a la actual.");
    }
}

public class SetPasswordRequestValidator : AbstractValidator<SetPasswordRequest>
{
    public SetPasswordRequestValidator()
    {
        RuleFor(x => x.UsuarioId).GreaterThan(0);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
