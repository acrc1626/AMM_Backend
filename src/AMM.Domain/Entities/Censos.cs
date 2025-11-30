using System;
using System.Collections.Generic;
using AMM.Domain.Common;

namespace AMM.Domain.Entities;

public class Censo : AuditableEntity
{
    public long Id { get; set; }
    public byte TipoEntornoId { get; set; }
    public long? PacienteId { get; set; }
    public long? UbicacionId { get; set; }
    public short? DepartamentoId { get; set; }
    public int? MunicipioId { get; set; }
    public DateTime Fecha { get; set; }
    public string? Observacion { get; set; }

    public virtual TipoEntorno TipoEntorno { get; set; } = null!;
    public virtual Paciente? Paciente { get; set; }
    public virtual Ubicacion? Ubicacion { get; set; }
    public virtual Departamento? Departamento { get; set; }
    public virtual Municipio? Municipio { get; set; }
    public virtual Estado Estado { get; set; } = null!;
    
    public virtual ICollection<CensoNovedad> Novedades { get; set; } = new List<CensoNovedad>();
}

public class CensoNovedad : AuditableEntity
{
    public long Id { get; set; }
    public long CensoId { get; set; }
    public long? PacienteId { get; set; }
    public byte TipoNovedadId { get; set; }
    public byte? PresenciaNovedadId { get; set; }
    public long? TratamientoId { get; set; }
    public string? Observacion { get; set; }
    public DateTime Fecha { get; set; }

    public virtual Censo Censo { get; set; } = null!;
    public virtual Paciente? Paciente { get; set; }
    public virtual TipoNovedad TipoNovedad { get; set; } = null!;
    public virtual PresenciaNovedad? PresenciaNovedad { get; set; }
    public virtual Tratamiento? Tratamiento { get; set; }
    public virtual Estado Estado { get; set; } = null!;
}
