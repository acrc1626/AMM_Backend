namespace AMM.Application.DTOs.Eventos;

public record EventoDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    byte EstadoId,
    string Estado
);

public record CrearEventoRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    byte EstadoId
);

// Escabiosis
public record EventoEscabiosisDto(
    long EventoId,
    byte? Severidad,
    string? Localizacion
);

public record CrearEventoEscabiosisRequest(
    long EventoId,
    byte? Severidad,
    string? Localizacion
);

// Geohelmintiasis
public record EventoGeohelmintiasisDto(
    long EventoId,
    string? TipoPrueba,
    string? Resultado
);

public record CrearEventoGeohelmintiasisRequest(
    long EventoId,
    string? TipoPrueba,
    string? Resultado
);

// Pediculosis
public record EventoPediculosisDto(
    long EventoId,
    byte? Grado
);

public record CrearEventoPediculosisRequest(
    long EventoId,
    byte? Grado
);

// Pian
public record EventoPianDto(
    long EventoId,
    string? Estadio
);

public record CrearEventoPianRequest(
    long EventoId,
    string? Estadio
);

// Teniasis/Cisticercosis
public record EventoTeniasisCisticercosisDto(
    long EventoId,
    bool? Teniasis,
    bool? Cisticercosis
);

public record CrearEventoTeniasisCisticercosisRequest(
    long EventoId,
    bool? Teniasis,
    bool? Cisticercosis
);

// Tracoma
public record EventoTracomaDto(
    long EventoId,
    int? CriterioExclusionId,
    bool? ExaminadoTt,
    byte? Triquiasis,
    string? OpacidadCorneal
);

public record CrearEventoTracomaRequest(
    long EventoId,
    int? CriterioExclusionId,
    bool? ExaminadoTt,
    byte? Triquiasis,
    string? OpacidadCorneal
);

// Tungiasis
public record EventoTungiasisDto(
    long EventoId,
    int? NumeroLesiones,
    string? Complicaciones
);

public record CrearEventoTungiasisRequest(
    long EventoId,
    int? NumeroLesiones,
    string? Complicaciones
);

// Malaria
public record EventoMalariaDto(long EventoId, string? Especie);
public record CrearEventoMalariaRequest(long EventoId, string? Especie);

// Tuberculosis
public record EventoTuberculosisDto(long EventoId, string? TipoTuberculosis);
public record CrearEventoTuberculosisRequest(long EventoId, string? TipoTuberculosis);

// TuberculosisContacto
public record EventoTuberculosisContactoDto(long EventoId, string? Observacion);
public record CrearEventoTuberculosisContactoRequest(long EventoId, string? Observacion);

// LeshmaniasisCutanea
public record EventoLeshmaniasisCutaneaDto(long EventoId, string? TipoLesion);
public record CrearEventoLeshmaniasisCutaneaRequest(long EventoId, string? TipoLesion);
