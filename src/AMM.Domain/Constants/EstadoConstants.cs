namespace AMM.Domain.Constants;

/// <summary>
/// Constantes para los estados de entidades (tabla ESTADO).
/// Valores confirmados con la BD: 1-Activo, 2-Inactivo, 3-Borrador, 4-EnProceso, 5-Finalizado.
/// </summary>
public static class EstadoId
{
    public const byte Activo     = 1;
    public const byte Inactivo   = 2;
    public const byte Borrador   = 3;
    public const byte EnProceso  = 4;
    public const byte Finalizado = 5;
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
