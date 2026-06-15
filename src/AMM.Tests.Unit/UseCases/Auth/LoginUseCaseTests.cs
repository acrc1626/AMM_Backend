using AMM.Application.DTOs.Auth;
using AMM.Application.Interfaces;
using AMM.Application.UseCases.Auth;
using AMM.Domain.Constants;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Auth;

public class LoginUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepoMock = new();
    private readonly Mock<IJwtService>        _jwtServiceMock  = new();
    private readonly Mock<IPasswordHasher>    _hasherMock      = new();

    private LoginUseCase CreateSut() =>
        new(_usuarioRepoMock.Object, _jwtServiceMock.Object, _hasherMock.Object);

    // ─── Helpers ────────────────────────────────────────────────────────────────
    private static Usuario BuildActiveUser(byte estadoUsuarioId = 1) => new()
    {
        Id              = 1,
        Correo          = "admin@ins.gov.co",
        NombreCompleto  = "Administrador INS",
        PasswordHash    = "salt.hash",
        EstadoUsuarioId = estadoUsuarioId,
        Roles           = new List<Rol> { new() { Id = 1, Nombre = "Administrador" } }
    };

    // ─── Tests ──────────────────────────────────────────────────────────────────

    [Fact]
    public async Task ExecuteAsync_CorrectCredentials_ReturnsTokenAndRoles()
    {
        // Arrange
        var usuario = BuildActiveUser();
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("admin@ins.gov.co", default))
                        .ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Verify("Pass123!", "salt.hash")).Returns(true);
        _jwtServiceMock.Setup(j => j.GenerateToken(usuario, It.IsAny<IEnumerable<string>>()))
                       .Returns(("eyJhbGci.payload.sig", DateTime.UtcNow.AddHours(1)));

        var request = new LoginRequest("admin@ins.gov.co", "Pass123!");

        // Act
        var result = await CreateSut().ExecuteAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Token.Should().Be("eyJhbGci.payload.sig");
        result.Correo.Should().Be("admin@ins.gov.co");
        result.Roles.Should().ContainSingle(r => r == "Administrador");
        result.Expira.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task ExecuteAsync_UserNotFound_ThrowsUnauthorized()
    {
        // Arrange
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync(It.IsAny<string>(), default))
                        .ReturnsAsync((Usuario?)null);

        // Act & Assert
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(new LoginRequest("noexiste@ins.gov.co", "pass")))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Correo o contraseña incorrectos*");
    }

    [Fact]
    public async Task ExecuteAsync_NoPasswordHash_ThrowsUnauthorized()
    {
        // Arrange — usuario sin contraseña asignada
        var usuario = BuildActiveUser();
        usuario.PasswordHash = null;
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync(It.IsAny<string>(), default))
                        .ReturnsAsync(usuario);

        // Act & Assert
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(new LoginRequest("admin@ins.gov.co", "pass")))
            .Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task ExecuteAsync_WrongPassword_ThrowsUnauthorized()
    {
        // Arrange
        var usuario = BuildActiveUser();
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("admin@ins.gov.co", default))
                        .ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Verify("wrongpass", "salt.hash")).Returns(false);

        // Act & Assert
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(new LoginRequest("admin@ins.gov.co", "wrongpass")))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*Correo o contraseña incorrectos*");
    }

    [Fact]
    public async Task ExecuteAsync_InactiveUser_ThrowsUnauthorized()
    {
        // Arrange
        var usuario = BuildActiveUser(estadoUsuarioId: EstadoUsuarioId.Inactivo);
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("admin@ins.gov.co", default))
                        .ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        // Act & Assert
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(new LoginRequest("admin@ins.gov.co", "Pass123!")))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*inactivo*");
    }

    [Fact]
    public async Task ExecuteAsync_Success_CallsJwtGenerateOnlyOnce()
    {
        // Arrange
        var usuario = BuildActiveUser();
        _usuarioRepoMock.Setup(r => r.GetByCorreoAsync("admin@ins.gov.co", default))
                        .ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _jwtServiceMock.Setup(j => j.GenerateToken(It.IsAny<Usuario>(), It.IsAny<IEnumerable<string>>()))
                       .Returns(("token", DateTime.UtcNow.AddHours(1)));

        // Act
        await CreateSut().ExecuteAsync(new LoginRequest("admin@ins.gov.co", "Pass123!"));

        // Assert
        _jwtServiceMock.Verify(
            j => j.GenerateToken(It.IsAny<Usuario>(), It.IsAny<IEnumerable<string>>()),
            Times.Once);
    }
}
