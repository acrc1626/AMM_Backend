namespace AMM.Application.DTOs.Seguridad;

// Usuario
public record UsuarioDto(int Id, string Correo, string NombreCompleto, byte EstadoUsuarioId, DateTime? ModificadoEn);
public record CrearUsuarioRequest(string Correo, string NombreCompleto, byte EstadoUsuarioId, Guid? AzureAdObjectId);
public record UpdateUsuarioRequest(int Id, string Correo, string NombreCompleto, byte EstadoUsuarioId, Guid? AzureAdObjectId, int? EntidadId);

// Rol
public record RolDto(int Id, string Nombre, string? Descripcion);
public record CrearRolRequest(string Nombre, string? Descripcion);

// Permiso
public record PermisoDto(int Id, string Codigo, string? Descripcion);
public record CrearPermisoRequest(string Codigo, string? Descripcion);

// Menu
public record MenuDto(int Id, string Nombre, string Ruta);
public record CrearMenuRequest(string Nombre, string Ruta);
