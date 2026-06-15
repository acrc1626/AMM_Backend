using AMM.Application.DTOs.Seguridad;
using FluentValidation;

namespace AMM.Application.Validators;

public class CrearUsuarioRequestValidator : AbstractValidator<CrearUsuarioRequest>
{
    public CrearUsuarioRequestValidator()
    {
        RuleFor(x => x.Correo).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.NombreCompleto).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EstadoUsuarioId).GreaterThan((byte)0);
    }
}

public class UpdateUsuarioRequestValidator : AbstractValidator<UpdateUsuarioRequest>
{
    public UpdateUsuarioRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Correo).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.NombreCompleto).NotEmpty().MaximumLength(200);
        RuleFor(x => x.EstadoUsuarioId).GreaterThan((byte)0);
    }
}
