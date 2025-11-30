namespace AMM.Application.DTOs.Catalogos;

// Estado
public record EstadoDto(byte Id, string Nombre, string? Descripcion);
public record CrearEstadoRequest(string Nombre, string? Descripcion);

// EstadoUsuario
public record EstadoUsuarioDto(byte Id, string Nombre, string? Descripcion);
public record CrearEstadoUsuarioRequest(string Nombre, string? Descripcion);

// TipoDocumento
public record TipoDocumentoDto(byte Id, string Descripcion);
public record CrearTipoDocumentoRequest(string Descripcion);

// Sexo
public record SexoDto(byte Id, string Descripcion);
public record CrearSexoRequest(string Descripcion);

// Etnia
public record EtniaDto(byte Id, string Descripcion, string Codigo);
public record CrearEtniaRequest(string Descripcion, string Codigo);

// PuebloIndigena
public record PuebloIndigenaDto(short Id, string Descripcion);
public record CrearPuebloIndigenaRequest(string Descripcion);

// TipoEntorno
public record TipoEntornoDto(byte Id, string Descripcion);
public record CrearTipoEntornoRequest(string Descripcion);

// TipoNovedad
public record TipoNovedadDto(byte Id, string Descripcion);
public record CrearTipoNovedadRequest(string Descripcion);

// PresenciaNovedad
public record PresenciaNovedadDto(byte Id, string Descripcion);
public record CrearPresenciaNovedadRequest(string Descripcion);

// TipoSupervision
public record TipoSupervisionDto(byte Id, string Descripcion);
public record CrearTipoSupervisionRequest(string Descripcion);

// MotivoNoTratamiento
public record MotivoNoTratamientoDto(short Id, string Descripcion);
public record CrearMotivoNoTratamientoRequest(string Descripcion);

// Parentesco
public record ParentescoDto(byte Id, string Descripcion);
public record CrearParentescoRequest(string Descripcion);

// EventoTipo
public record EventoTipoDto(byte Id, string Codigo, string Nombre);
public record CrearEventoTipoRequest(string Codigo, string Nombre);

// FormaFarmaceutica
public record FormaFarmaceuticaDto(short Id, string Nombre);
public record CrearFormaFarmaceuticaRequest(string Nombre);
