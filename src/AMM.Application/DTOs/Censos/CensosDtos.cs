namespace AMM.Application.DTOs.Censos;

// ─── DTOs de request para creación (HU-001, HU-002, HU-003) ─────────────────

/// <summary>Enfermedad registrada a una persona durante el censo.</summary>
public record PersonaEnfermedadRequest(
    byte EnfermedadId,
    byte EstadoId
);

/// <summary>
/// Persona asociada al censo.
/// ParentescoId solo aplica en censos tipo Hogar (RN-006: ignorado en Educativo e Institucional).
/// Si PacienteId tiene valor se usa directamente.
/// Si PacienteId es null, se busca por TipoDocumentoId + Documento:
///   - existe  → se usa su Id
///   - no existe → se crea el paciente con los campos opcionales y se usa el nuevo Id
/// </summary>
public record CensoPersonaRequest(
    long?   PacienteId,
    byte?   ParentescoId,
    string? Comunidad,
    string? GrupoPoblacionEspecial,
    IReadOnlyList<PersonaEnfermedadRequest> Enfermedades,
    // Identificación del paciente (requerida cuando PacienteId = 0)
    byte?     TipoDocumentoId = null,
    string?   Documento       = null,
    // Datos para creación (requeridos cuando el paciente tampoco existe en BD)
    string?   PrimerNombre    = null,
    string?   SegundoNombre   = null,
    string?   PrimerApellido  = null,
    string?   SegundoApellido = null,
    byte?     SexoId          = null,
    DateTime? FechaNacimiento = null
);

/// <summary>
/// Request unificado para POST /api/Censos.
/// Aplica a los tres tipos de entorno: Hogar (HU-001), Educativo (HU-002) e Institucional (HU-003).
/// JefeHogar: requerido en Hogar (RN-001), debe ser null en Educativo e Institucional (RN-005).
/// Personas: lista de personas adicionales; acepta cualquier tamaño (RN-007, RN-010).
/// </summary>
public record CrearCensoRequest(
    byte     TipoEntornoId,
    long?    UbicacionId,
    short?   DepartamentoId,
    int?     MunicipioId,
    DateTime Fecha,
    string?  Observacion,
    byte     EstadoId,
    CensoPersonaRequest?                   JefeHogar              = null,
    IReadOnlyList<CensoPersonaRequest>?    Personas               = null,
    string?                                Direccion               = null,
    string?                                ObjetoInterventor       = null,
    string?                                Territorio              = null,
    string?                                Microterritorio         = null,
    string?                                Area                    = null,
    string?                                Geolocalizacion         = null,
    int?                                   TotalMiembros           = null,
    int?                                   VisitantesTemporalesCol = null,
    int?                                   VisitantesTemporalesMig = null
);

/// <summary>Respuesta resumida devuelta tras crear un censo (HU-001, HU-002, HU-003).</summary>
public record CensoResumenDto(
    long     Id,
    byte     TipoEntornoId,
    string   TipoEntorno,
    DateTime Fecha,
    byte     EstadoId,
    string   Estado,
    int      CantidadPersonas
);

// ─── DTOs de respuesta para consulta (HU-004) ────────────────────────────────

/// <summary>Enfermedad de una persona del censo con descripción resuelta.</summary>
public record PersonaEnfermedadDetalleDto(
    long   Id,
    byte   EnfermedadId,
    string Enfermedad,
    byte   EstadoId,
    string Estado
);

/// <summary>Persona del censo con parentesco, estado y enfermedades resueltos.</summary>
public record CensoPersonaDetalleDto(
    long      Id,
    long      PacienteId,
    string?   PrimerNombre,
    string?   SegundoNombre,
    string?   PrimerApellido,
    string?   SegundoApellido,
    string?   Documento,
    byte      TipoDocumentoId,
    string?   TipoDocumento,
    byte      SexoId,
    string?   Sexo,
    DateTime? FechaNacimiento,
    byte?     ParentescoId,
    string?   Parentesco,
    bool      EsJefeHogar,
    string?   Comunidad,
    string?   GrupoPoblacionEspecial,
    byte      EstadoPersonaId,
    string    EstadoPersona,
    IReadOnlyList<PersonaEnfermedadDetalleDto> Enfermedades
);

/// <summary>
/// Respuesta completa para GET /api/Censos/{id} (HU-004).
/// Incluye caracterización del censo, lista de personas y conteo.
/// </summary>
public record CensoDetalleDto(
    long     Id,
    byte     TipoEntornoId,
    string   TipoEntorno,
    long?    UbicacionId,
    string?  UbicacionDireccion,
    short?   DepartamentoId,
    string?  DepartamentoNombre,
    int?     MunicipioId,
    string?  MunicipioNombre,
    DateTime Fecha,
    string?  Observacion,
    string?  Direccion,
    string?  ObjetoInterventor,
    string?  Territorio,
    string?  Microterritorio,
    string?  Area,
    string?  Geolocalizacion,
    int?     TotalMiembros,
    int?     VisitantesTemporalesCol,
    int?     VisitantesTemporalesMig,
    byte     EstadoId,
    string   Estado,
    int      CantidadPersonas,
    IReadOnlyList<CensoPersonaDetalleDto> Personas
);

// ─── DTO de actualización (HU-005) ───────────────────────────────────────────

/// <summary>
/// Request para PUT /api/Censos/{id} (HU-005).
/// TipoEntornoId no es editable tras la creación: no se incluye en este DTO.
/// Las personas asociadas nunca se modifican desde este endpoint (RN-011, RN-012).
/// </summary>
public record UpdateCensoRequest(
    long     Id,
    long?    UbicacionId,
    short?   DepartamentoId,
    int?     MunicipioId,
    DateTime Fecha,
    string?  Observacion,
    byte     EstadoId,
    string?  Direccion               = null,
    string?  ObjetoInterventor       = null,
    string?  Territorio              = null,
    string?  Microterritorio         = null,
    string?  Area                    = null,
    string?  Geolocalizacion         = null,
    int?     TotalMiembros           = null,
    int?     VisitantesTemporalesCol = null,
    int?     VisitantesTemporalesMig = null
);

// ─── DTOs de lista / resumen (GET all) ───────────────────────────────────────

/// <summary>Proyección liviana del censo para listados.</summary>
public record CensoDto(
    long     Id,
    byte     TipoEntornoId,
    string   TipoEntorno,
    long?    UbicacionId,
    string?  UbicacionDireccion,
    short?   DepartamentoId,
    string?  DepartamentoNombre,
    int?     MunicipioId,
    string?  MunicipioNombre,
    DateTime Fecha,
    string?  Observacion,
    byte     EstadoId,
    string   Estado
);

// ─── DTOs de CensoNovedad ────────────────────────────────────────────────────

public record CensoNovedadDto(
    long    Id,
    long    CensoId,
    long?   PacienteId,
    string? PacienteNombre,
    byte    TipoNovedadId,
    string  TipoNovedad,
    byte?   PresenciaNovedadId,
    string? PresenciaNovedad,
    long?   TratamientoId,
    string? Observacion,
    DateTime Fecha,
    byte    EstadoId,
    string  Estado
);

public record CrearCensoNovedadRequest(
    long     CensoId,
    long?    PacienteId,
    byte     TipoNovedadId,
    byte?    PresenciaNovedadId,
    long?    TratamientoId,
    string?  Observacion,
    DateTime Fecha,
    byte     EstadoId
);

public record UpdateCensoNovedadRequest(
    long     Id,
    long     CensoId,
    long?    PacienteId,
    byte     TipoNovedadId,
    byte?    PresenciaNovedadId,
    long?    TratamientoId,
    string?  Observacion,
    DateTime Fecha,
    byte     EstadoId
);

// ─── DEF-AMM-004: PATCH /api/Censos/{id} ─────────────────────────────────────

/// <summary>
/// Request para PATCH /api/Censos/{id} (DEF-AMM-004).
/// Agrega personas, tratamientos y novedad censal a un censo ya creado.
/// No modifica la caracterización del entorno guardada en el POST inicial.
/// </summary>
public record AgregarPersonasYTratamientosRequest(
    IReadOnlyList<CensoPersonaRequest>? Personas      = null,
    TratamientosCensoRequest?           Tratamientos  = null,
    NovedadCensalRequest?               NovedadCensal = null
);

/// <summary>Tratamientos colectivos del census (tracoma, teniasis) y criterios de exclusión.</summary>
public record TratamientosCensoRequest(
    bool    Tracoma,
    bool    Teniasis,
    bool    RechazaTratamiento  = false,
    bool    EnfermedadRenal     = false,
    bool    EnfermedadCardiaca  = false,
    bool    Polimedicacion      = false,
    bool    Alergias            = false,
    bool    Embarazo            = false,
    bool    Lactancia           = false,
    bool    MenorEdad           = false,
    bool    EnfermedadHepatica  = false,
    bool    TrastornosGastricos = false,
    string? Observaciones       = null
);

/// <summary>Novedad censal y presencia capturadas en el último step del wizard.</summary>
public record NovedadCensalRequest(
    string  Presencia,
    string  TipoNovedad,
    string? FechaFallecimiento  = null,
    string? OtraNovedadDetalle  = null,
    string? Observaciones       = null
);
