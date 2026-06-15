namespace AMM.Domain.Constants;

/// <summary>
/// Constantes para los estados de entidades (tabla ESTADO).
/// Evita magic numbers dispersos por el código.
/// </summary>
public static class EstadoId
{
    public const byte Activo = 1;
    public const byte Inactivo = 2;
    public const byte Pendiente = 3;
}

/// <summary>
/// Constantes para los estados de usuario (tabla ESTADO_USUARIO).
/// </summary>
public static class EstadoUsuarioId
{
    public const byte Activo = 1;
    public const byte Inactivo = 2;
    public const byte Bloqueado = 3;
}
