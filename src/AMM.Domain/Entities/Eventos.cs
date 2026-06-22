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

    public virtual Paciente Paciente { get; set; } = null!;
    public virtual EventoTipo EventoTipo { get; set; } = null!;
    public virtual Estado Estado { get; set; } = null!;
}

public class EventoEscabiosis
{
    public long EventoId { get; set; }
    public byte? Severidad { get; set; }
    public string? Localizacion { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoGeohelmintiasis
{
    public long EventoId { get; set; }
    public string? TipoPrueba { get; set; }
    public string? Resultado { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoPediculosis
{
    public long EventoId { get; set; }
    public byte? Grado { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoPian
{
    public long EventoId { get; set; }
    public string? Estadio { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoTeniasisCisticercosis
{
    public long EventoId { get; set; }
    public bool? Teniasis { get; set; }
    public bool? Cisticercosis { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoTracoma
{
    public long EventoId { get; set; }
    public int? CriterioExclusionId { get; set; }
    public bool? ExaminadoTt { get; set; }
    public byte? Triquiasis { get; set; }
    public string? OpacidadCorneal { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoTungiasis
{
    public long EventoId { get; set; }
    public int? NumeroLesiones { get; set; }
    public string? Complicaciones { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoMalaria
{
    public long EventoId { get; set; }
    public string? Especie { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoTuberculosis
{
    public long EventoId { get; set; }
    public string? TipoTuberculosis { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoTuberculosisContacto
{
    public long EventoId { get; set; }
    public string? Observacion { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}

public class EventoLeshmaniasisCutanea
{
    public long EventoId { get; set; }
    public string? TipoLesion { get; set; }

    public virtual Evento Evento { get; set; } = null!;
}
