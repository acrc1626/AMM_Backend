namespace AMM.Domain.Constants;

/// <summary>
/// Identificadores para la tabla ESTADO_PERSONA.
/// Los valores deben coincidir exactamente con los registros en la BD.
/// </summary>
public static class EstadoPersonaId
{
    public const byte Pendiente  = 1;
    public const byte EnProceso  = 2;
    public const byte Completada = 3;
}
