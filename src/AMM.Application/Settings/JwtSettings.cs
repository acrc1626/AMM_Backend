namespace AMM.Application.Settings;

/// <summary>
/// Configuración del JWT — se liga desde la sección "Jwt" del appsettings.
/// Vive en Application porque LoginUseCase necesita la expiración.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}
