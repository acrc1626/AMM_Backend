using System.Security.Cryptography;
using AMM.Application.Interfaces;

namespace AMM.Infrastructure.Security;

/// <summary>
/// Implementación de IPasswordHasher usando PBKDF2 + SHA256.
/// Sin dependencias externas — usa System.Security.Cryptography nativo de .NET.
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verify(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt, storedHash;
        try
        {
            salt = Convert.FromBase64String(parts[0]);
            storedHash = Convert.FromBase64String(parts[1]);
        }
        catch (FormatException)
        {
            return false;
        }

        var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

        return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
    }
}
