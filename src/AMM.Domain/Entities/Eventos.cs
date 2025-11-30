using System;
using AMM.Domain.Common;

namespace AMM.Domain.Entities;

public class EventoTipo
{
    public byte Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nombre { get; set; } = null!;
}

public class Evento : AuditableEntity
{
    public long Id { get; set; }
    public long PacienteId { get; set; }
    public byte EventoTipoId { get; set; }
    public DateTime Fecha { get; set; }
    public string? Observacion { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;
    public virtual EventoTipo EventoTipo { get; set; } = null!;
    public virtual Estado Estado { get; set; } = null!;
}

public class Escabiosis : Evento
{
    public bool? Tratado { get; set; }
    public bool? CierreControl { get; set; }
}

public class Geohelmintiasis : Evento
{
    public bool? Tratado { get; set; }
    public bool? CierreControl { get; set; }
    public bool? Laboratorio { get; set; }
}

public class Pediculosis : Evento
{
    public bool? Tratado { get; set; }
    public bool? CierreControl { get; set; }
}

public class Malaria : Evento
{
    public bool? Gota { get; set; }
    public string? Resultado { get; set; }
}

public class Tuberculosis : Evento
{
    public bool? Sintomatico { get; set; }
    public string? Resultado { get; set; }
}

public class TuberculosisContacto : Evento
{
    public long? IndexId { get; set; }
    public byte? ParentescoId { get; set; }

    public virtual Paciente? Index { get; set; }
    public virtual Parentesco? Parentesco { get; set; }
}

public class LeshmaniasisCutanea : Evento
{
    public bool? Cicatriz { get; set; }
    public int? NumeroLesiones { get; set; }
}
