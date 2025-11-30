using System;
using AMM.Domain.Common;

namespace AMM.Domain.Entities;

public class Paciente : AuditableEntity
{
    public long Id { get; set; }
    public byte TipoDocumentoId { get; set; }
    public string Documento { get; set; } = null!;
    public string PrimerNombre { get; set; } = null!;
    public string? SegundoNombre { get; set; }
    public string PrimerApellido { get; set; } = null!;
    public string? SegundoApellido { get; set; }
    public byte SexoId { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public byte? PertenenciaEtnicaId { get; set; }
    public short? PuebloIndigenaId { get; set; }

    // Navigation properties
    public virtual TipoDocumento TipoDocumento { get; set; } = null!;
    public virtual Sexo Sexo { get; set; } = null!;
    public virtual Etnia? PertenenciaEtnica { get; set; }
    public virtual PuebloIndigena? PuebloIndigena { get; set; }
    public virtual Estado Estado { get; set; } = null!;
}
