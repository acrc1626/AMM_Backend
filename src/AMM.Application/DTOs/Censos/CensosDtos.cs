namespace AMM.Application.DTOs.Censos;

public record CensoDto(
    long Id, 
    byte TipoEntornoId, 
    string TipoEntorno, 
    long? PacienteId, 
    string? PacienteNombre, 
    long? UbicacionId, 
    string? UbicacionDireccion, 
    short? DepartamentoId, 
    string? DepartamentoNombre, 
    int? MunicipioId, 
    string? MunicipioNombre, 
    DateTime Fecha, 
    string? Observacion, 
    byte EstadoId,
    string Estado
);

public record CrearCensoRequest(
    byte TipoEntornoId, 
    long? PacienteId, 
    long? UbicacionId, 
    short? DepartamentoId, 
    int? MunicipioId, 
    DateTime Fecha, 
    string? Observacion, 
    byte EstadoId
);

public record UpdateCensoRequest(
    long Id,
    byte TipoEntornoId, 
    long? PacienteId, 
    long? UbicacionId, 
    short? DepartamentoId, 
    int? MunicipioId, 
    DateTime Fecha, 
    string? Observacion, 
    byte EstadoId
);

public record CensoNovedadDto(
    long Id, 
    long CensoId, 
    long? PacienteId, 
    string? PacienteNombre, 
    byte TipoNovedadId, 
    string TipoNovedad, 
    byte? PresenciaNovedadId, 
    string? PresenciaNovedad, 
    long? TratamientoId, 
    string? Observacion, 
    DateTime Fecha, 
    byte EstadoId,
    string Estado
);

public record CrearCensoNovedadRequest(
    long CensoId, 
    long? PacienteId, 
    byte TipoNovedadId, 
    byte? PresenciaNovedadId, 
    long? TratamientoId, 
    string? Observacion, 
    DateTime Fecha, 
    byte EstadoId
);

public record UpdateCensoNovedadRequest(
    long Id,
    long CensoId, 
    long? PacienteId, 
    byte TipoNovedadId, 
    byte? PresenciaNovedadId, 
    long? TratamientoId, 
    string? Observacion, 
    DateTime Fecha, 
    byte EstadoId
);
