using AMM.Infrastructure.Security;
using FluentAssertions;

namespace AMM.Tests.Unit.Infrastructure.Security;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void Hash_ReturnsNonEmptyString()
    {
        var hash = _hasher.Hash("myPassword123");
        hash.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Hash_ContainsDotSeparator()
    {
        // Formato esperado: "base64Salt.base64Hash"
        var hash = _hasher.Hash("password");
        hash.Should().Contain(".");
    }

    [Fact]
    public void Hash_SamePasswordProducesDifferentHashes()
    {
        // Cada llamada genera un salt diferente → hashes distintos
        var hash1 = _hasher.Hash("myPassword");
        var hash2 = _hasher.Hash("myPassword");
        hash1.Should().NotBe(hash2);
    }

    [Fact]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        var hash = _hasher.Hash("SecurePass!99");
        _hasher.Verify("SecurePass!99", hash).Should().BeTrue();
    }

    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        var hash = _hasher.Hash("correctPassword");
        _hasher.Verify("wrongPassword", hash).Should().BeFalse();
    }

    [Fact]
    public void Verify_EmptyPassword_ReturnsFalse()
    {
        var hash = _hasher.Hash("realPassword");
        _hasher.Verify("", hash).Should().BeFalse();
    }

    [Fact]
    public void Verify_InvalidHashFormat_ReturnsFalse()
    {
        _hasher.Verify("password", "notavalidhash").Should().BeFalse();
    }

    [Fact]
    public void Verify_CorruptBase64_ReturnsFalse()
    {
        _hasher.Verify("password", "!!!invalid!!!.!!!invalid!!!").Should().BeFalse();
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("P@ssw0rd!2025")]
    [InlineData("contraseñaConEñes")]
    [InlineData("   espacios   ")]
    public void Verify_RoundTrip_AlwaysSucceeds(string password)
    {
        var hash = _hasher.Hash(password);
        _hasher.Verify(password, hash).Should().BeTrue();
    }
}
