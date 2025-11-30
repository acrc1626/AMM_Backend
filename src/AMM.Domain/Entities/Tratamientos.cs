using System;
using System.Collections.Generic;
using AMM.Domain.Common;

namespace AMM.Domain.Entities;

public class FormaFarmaceutica
{
    public short Id { get; set; }
    public string Nombre { get; set; } = null!;
}

public class Medicamento
{
    public int Id { get; set; }
    public string NombreGenerico { get; set; } = null!;
    public short FormaFarmaceuticaId { get; set; }
    public string? Concentracion { get; set; }
    public string? CodigoCum { get; set; }

    public virtual FormaFarmaceutica FormaFarmaceutica { get; set; } = null!;
}

public class Tratamiento : AuditableEntity
{
    public long Id { get; set; }
    public long EventoId { get; set; }
    public int? SupervisadoPorId { get; set; }
    public bool RequiereTratamiento { get; set; }
    public short? MotivoNoTratamientoId { get; set; }
    public string? Observacion { get; set; }
    public DateTime Fecha { get; set; }

    public virtual Evento Evento { get; set; } = null!;
    public virtual Usuario? SupervisadoPor { get; set; }
    public virtual MotivoNoTratamiento? MotivoNoTratamiento { get; set; }
    public virtual Estado Estado { get; set; } = null!;
    
    public virtual ICollection<TratamientoDetalle> Detalles { get; set; } = new List<TratamientoDetalle>();
}

public class TratamientoDetalle
{
    public long Id { get; set; }
    public long TratamientoId { get; set; }
    public int MedicamentoId { get; set; }
    public string? Dosis { get; set; }
    public string? Via { get; set; }
    public short? FormaFarmaceuticaId { get; set; }
    public byte? SupervisionId { get; set; }

    public virtual Tratamiento Tratamiento { get; set; } = null!;
    public virtual Medicamento Medicamento { get; set; } = null!;
    public virtual FormaFarmaceutica? FormaFarmaceutica { get; set; }
    public virtual TipoSupervision? Supervision { get; set; }
}

public class Prm
{
    public long Id { get; set; }
    public long TratamientoId { get; set; }
    public long PacienteId { get; set; }
    public string? Descripcion { get; set; }
    public byte? Severidad { get; set; }
    public DateTime CreadoEn { get; set; }

    public virtual Tratamiento Tratamiento { get; set; } = null!;
    public virtual Paciente Paciente { get; set; } = null!;
}

public class Rafa
{
    public long Id { get; set; }
    public long TratamientoId { get; set; }
    public long PacienteId { get; set; }
    public DateTime FechaEvento { get; set; }
    public byte? Severidad { get; set; }
    public string? Sintomas { get; set; }
    public string? EnlaceReporteInvima { get; set; }
    public DateTime CreadoEn { get; set; }

    public virtual Tratamiento Tratamiento { get; set; } = null!;
    public virtual Paciente Paciente { get; set; } = null!;
}
