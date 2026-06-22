using System;
using System.Collections.Generic;
using AMM.Domain.Common;

namespace AMM.Domain.Entities;

public class Censo : AuditableEntity
{
    public long Id { get; set; }
    public byte TipoEntornoId { get; set; }
    public long? UbicacionId { get; set; }
    public short? DepartamentoId { get; set; }
    public int? MunicipioId { get; set; }
    public DateTime Fecha { get; set; }
    public string? Observacion { get; set; }
    public string? Direccion { get; set; }
    public string? ObjetoInterventor { get; set; }
    public string? Territorio { get; set; }
    public string? Microterritorio { get; set; }
    public string? Area { get; set; }
    public string? Geolocalizacion { get; set; }
    public int? TotalMiembros { get; set; }
    public int? VisitantesTemporalesCol { get; set; }
    public int? VisitantesTemporalesMig { get; set; }

    public virtual TipoEntorno TipoEntorno { get; set; } = null!;
    public virtual Ubicacion? Ubicacion { get; set; }
    public virtual Departamento? Departamento { get; set; }
    public virtual Municipio? Municipio { get; set; }
    public virtual Estado Estado { get; set; } = null!;

    public virtual ICollection<CensoNovedad> Novedades { get; set; } = new List<CensoNovedad>();
    public virtual ICollection<CensoPersona> Personas { get; set; } = new List<CensoPersona>();
}

public class CensoPersona
{
    public long Id { get; set; }
    public long CensoId { get; set; }
    public long PacienteId { get; set; }
    public byte? ParentescoId { get; set; }
    public bool EsJefeHogar { get; set; }
    public string? Comunidad { get; set; }
    public string? GrupoPoblacionEspecial { get; set; }
    public byte EstadoPersonaId { get; set; }
    public DateTime CreadoEn { get; set; }
    public string? CreadoPor { get; set; }
    public DateTime? ModificadoEn { get; set; }
    public string? ModificadoPor { get; set; }

    public virtual Censo Censo { get; set; } = null!;
    public virtual Paciente Paciente { get; set; } = null!;
    public virtual Parentesco? Parentesco { get; set; }
    public virtual EstadoPersona EstadoPersona { get; set; } = null!;

    public virtual ICollection<PersonaEnfermedad> Enfermedades { get; set; } = new List<PersonaEnfermedad>();
}

public class PersonaEnfermedad
{
    public long Id { get; set; }
    public long CensoPersonaId { get; set; }
    public byte EnfermedadId { get; set; }
    public byte EstadoId { get; set; }
    public DateTime CreadoEn { get; set; }

    public virtual CensoPersona CensoPersona { get; set; } = null!;
    public virtual Enfermedad Enfermedad { get; set; } = null!;
    public virtual Estado Estado { get; set; } = null!;
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
