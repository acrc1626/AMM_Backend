namespace AMM.Application.Interfaces;

/// <summary>
/// Puerto de aplicación para hash y verificación de contraseñas.
/// La implementación (PBKDF2) vive en Infrastructure.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}
