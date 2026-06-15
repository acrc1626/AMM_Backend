using AMM.Application.DTOs.Auth;
using AMM.Application.Interfaces;
using AMM.Application.UseCases.Auth;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Auth;

public class SetPasswordUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repoMock   = new();
    private readonly Mock<IPasswordHasher>    _hasherMock = new();

    private SetPasswordUseCase CreateSut() =>
        new(_repoMock.Object, _hasherMock.Object);

    [Fact]
    public async Task ExecuteAsync_ValidRequest_HashesAndSavesPassword()
    {
        // Arrange
        var usuario = new Usuario { Id = 5, Correo = "test@ins.gov.co", NombreCompleto = "Test" };
        _repoMock.Setup(r => r.GetByIdAsync(5, default)).ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Hash("NewPass123")).Returns("salt.hashed");

        var request = new SetPasswordRequest(5, "NewPass123");

        // Act
        await CreateSut().ExecuteAsync(request, "admin@ins.gov.co");

        // Assert
        usuario.PasswordHash.Should().Be("salt.hashed");
        usuario.ModificadoPor.Should().Be("admin@ins.gov.co");
        _repoMock.Verify(r => r.UpdateAsync(usuario, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_UserNotFound_ThrowsKeyNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(99, default)).ReturnsAsync((Usuario?)null);

        // Act & Assert
        await CreateSut()
            .Invoking(s => s.ExecuteAsync(new SetPasswordRequest(99, "pass"), "admin"))
            .Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task ChangeAsync_CorrectCurrentPassword_UpdatesHash()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id           = 1,
            Correo       = "user@ins.gov.co",
            NombreCompleto = "User",
            PasswordHash = "salt.oldhash"
        };
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Verify("OldPass", "salt.oldhash")).Returns(true);
        _hasherMock.Setup(h => h.Hash("NewPass123")).Returns("salt.newhash");

        var request = new ChangePasswordRequest("OldPass", "NewPass123");

        // Act
        await CreateSut().ChangeAsync(1, request);

        // Assert
        usuario.PasswordHash.Should().Be("salt.newhash");
    }

    [Fact]
    public async Task ChangeAsync_WrongCurrentPassword_ThrowsUnauthorized()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id           = 1,
            NombreCompleto = "User",
            Correo       = "user@ins.gov.co",
            PasswordHash = "salt.oldhash"
        };
        _repoMock.Setup(r => r.GetByIdAsync(1, default)).ReturnsAsync(usuario);
        _hasherMock.Setup(h => h.Verify("WrongPass", "salt.oldhash")).Returns(false);

        // Act & Assert
        await CreateSut()
            .Invoking(s => s.ChangeAsync(1, new ChangePasswordRequest("WrongPass", "NewPass")))
            .Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("*contraseña actual*");
    }
}
