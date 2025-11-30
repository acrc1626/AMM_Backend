namespace AMM.Application.DTOs.UbicacionGeografica;

// Departamento
public record DepartamentoDto(short Id, string CodigoDane, string Nombre);
public record CrearDepartamentoRequest(string CodigoDane, string Nombre);

// Municipio
public record MunicipioDto(int Id, short DepartamentoId, string DepartamentoNombre, string CodigoDaneDpto, string Nombre);
public record CrearMunicipioRequest(short DepartamentoId, string CodigoDaneDpto, string Nombre);

// Territorio
public record TerritorioDto(int Id, int MunicipioId, string MunicipioNombre, string Nombre);
public record CrearTerritorioRequest(int MunicipioId, string Nombre);

// Microterritorio
public record MicroterritorioDto(int Id, int TerritorioId, string TerritorioNombre, string Nombre);
public record CrearMicroterritorioRequest(int TerritorioId, string Nombre);

// Area
public record AreaDto(int Id, int MicroterritorioId, string MicroterritorioNombre, string Nombre);
public record CrearAreaRequest(int MicroterritorioId, string Nombre);

// Ubicacion
public record UbicacionDto(long Id, string? Direccion, double? Lat, double? Lon);
public record CrearUbicacionRequest(string? Direccion, double? Lat, double? Lon);
