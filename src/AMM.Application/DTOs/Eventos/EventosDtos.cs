namespace AMM.Application.DTOs.Eventos;

// Base Evento DTO
public record EventoDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado
);

public record CrearEventoRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId
);

// Escabiosis
public record EscabiosisDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    bool? Tratado,
    bool? CierreControl
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearEscabiosisRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    bool? Tratado,
    bool? CierreControl
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);

// Geohelmintiasis
public record GeohelmintiasisDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    bool? Tratado,
    bool? CierreControl,
    bool? Laboratorio
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearGeohelmintiasisRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    bool? Tratado,
    bool? CierreControl,
    bool? Laboratorio
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);

// Pediculosis
public record PediculosisDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    bool? Tratado,
    bool? CierreControl
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearPediculosisRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    bool? Tratado,
    bool? CierreControl
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);

// Malaria
public record MalariaDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    bool? Gota,
    string? Resultado
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearMalariaRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    bool? Gota,
    string? Resultado
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);

// Tuberculosis
public record TuberculosisDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    bool? Sintomatico,
    string? Resultado
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearTuberculosisRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    bool? Sintomatico,
    string? Resultado
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);

// TuberculosisContacto
public record TuberculosisContactoDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    long? IndexId,
    byte? ParentescoId,
    string? Parentesco
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearTuberculosisContactoRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    long? IndexId,
    byte? ParentescoId
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);

// LeshmaniasisCutanea
public record LeshmaniasisCutaneaDto(
    long Id,
    byte EventoTipoId,
    string EventoTipo,
    long PacienteId,
    string PacienteNombre,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    string Estado,
    bool? Cicatriz,
    int? NumeroLesiones
) : EventoDto(Id, EventoTipoId, EventoTipo, PacienteId, PacienteNombre, Fecha, Observacion, EstadoId, Estado);

public record CrearLeshmaniasisCutaneaRequest(
    byte EventoTipoId,
    long PacienteId,
    DateTime Fecha,
    string? Observacion,
    byte EstadoId,
    bool? Cicatriz,
    int? NumeroLesiones
) : CrearEventoRequest(EventoTipoId, PacienteId, Fecha, Observacion, EstadoId);
