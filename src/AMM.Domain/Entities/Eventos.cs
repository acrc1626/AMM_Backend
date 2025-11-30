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
    public byte TipoId { get; set; }
    public DateTime FechaEvento { get; set; }

    public virtual Paciente Paciente { get; set; } = null!;
    public virtual EventoTipo Tipo { get; set; } = null!;
    public virtual Estado Estado { get; set; } = null!;
}

public class EventoEscabiosis : Evento
{
    public byte? Severidad { get; set; }
    public string? Localizacion { get; set; }
}

public class EventoGeohelmintiasis : Evento
{
    public string? TipoPrueba { get; set; }
    public string? Resultado { get; set; }
}

public class EventoPediculosis : Evento
{
    public byte? Grado { get; set; }
}

public class EventoPian : Evento
{
    public string? Estadio { get; set; }
}

public class EventoTeniasisCisticercosis : Evento
{
    public bool? Teniasis { get; set; }
    public bool? Cisticercosis { get; set; }
}

public class EventoTracoma : Evento
{
    public int? CriterioExclusionId { get; set; }
    public bool? ExaminadoTt { get; set; }
    public byte? Triquiasis { get; set; }
    public string? OpacidadCorneal { get; set; }
}

public class EventoTungiasis : Evento
{
    public int? NumeroLesiones { get; set; }
    public string? Complicaciones { get; set; }
}
