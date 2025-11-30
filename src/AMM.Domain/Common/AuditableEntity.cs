using System;

namespace AMM.Domain.Common;

public abstract class AuditableEntity
{
    public DateTime CreadoEn { get; set; }
    public string? CreadoPor { get; set; }
    public DateTime? ModificadoEn { get; set; }
    public string? ModificadoPor { get; set; }
    public byte EstadoId { get; set; }
}
