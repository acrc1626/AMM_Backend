using AMM.Application.DTOs;
using FluentValidation;

namespace AMM.Application.Validators;

public class CrearPacienteRequestValidator : AbstractValidator<CrearPacienteRequest>
{
    public CrearPacienteRequestValidator()
    {
        RuleFor(x => x.TipoDocumentoId).GreaterThan((byte)0).WithMessage("Tipo de documento es requerido.");
        RuleFor(x => x.Documento).NotEmpty().MaximumLength(30).WithMessage("Documento es requerido (máx. 30 caracteres).");
        RuleFor(x => x.PrimerNombre).NotEmpty().MaximumLength(80);
        RuleFor(x => x.PrimerApellido).NotEmpty().MaximumLength(80);
        RuleFor(x => x.SegundoNombre).MaximumLength(80).When(x => x.SegundoNombre is not null);
        RuleFor(x => x.SegundoApellido).MaximumLength(80).When(x => x.SegundoApellido is not null);
        RuleFor(x => x.SexoId).GreaterThan((byte)0).WithMessage("Sexo es requerido.");
        RuleFor(x => x.FechaNacimiento)
            .LessThanOrEqualTo(DateTime.Today)
            .When(x => x.FechaNacimiento.HasValue)
            .WithMessage("Fecha de nacimiento no puede ser futura.");
    }
}
