using AMM.Application.DTOs.Seguridad;
using AMM.Application.UseCases.Seguridad;
using AMM.Domain.Constants;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using FluentAssertions;
using Moq;

namespace AMM.Tests.Unit.UseCases.Seguridad;

public class UsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repoMock = new();

    private UsuarioUseCase CreateSut() => new(_repoMock.Object);

    // ─── Helpers ──────────────────────────────────────────────────────────────

    private static CrearUsuarioRequest BuildValidRequest() => new(
        Correo:          "nuevo@ins.gov.co",
        NombreCompleto:  "María García",
        EstadoUsuarioId: EstadoUsuarioId.Activo,
        AzureAdObjectId: null
    );

    private static Usuario BuildSavedUsuario(int id = 10) => new()
    {
        Id              = id,
        Correo          = "nuevo@ins.gov.co",
        NombreCompleto  = "María García",
        EstadoUsuarioId = EstadoUsuarioId.Activo,
        EstadoId        = EstadoId.Activo,
        CreadoEn        = DateTime.UtcNow
    };

    // ─── Test 1: Crear usuario válido ─────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidRequest_CreatesUserAndReturnsDto()
    {
        // Given – no existe conflicto; el repo persiste y recarga la entidad
        var request = BuildValidRequest();
        var saved   = BuildSavedUsuario(id: 10);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<Usuario>(), default))
                 .ReturnsAsync(saved);
        _repoMock.Setup(r => r.GetByIdAsync<int>(10, default))
                 .ReturnsAsync(saved);

        // When – se ejecuta el caso de uso
        var result = await CreateSut().CreateAsync(request, "admin@ins.gov.co");

        // Then – el DTO contiene los datos del usuario y se persistió exactamente una vez
        result.Should().NotBeNull();
        result.Id.Should().Be(10);
        result.Correo.Should().Be("nuevo@ins.gov.co");
        result.NombreCompleto.Should().Be("María García");
        result.EstadoUsuarioId.Should().Be(EstadoUsuarioId.Activo);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 2: Soft-delete ──────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_ExistingUser_SetsInactiveAndDoesNotDeletePhysically()
    {
        // Given – el usuario existe en estado activo
        var usuario = BuildSavedUsuario(id: 5);
        usuario.EstadoId = EstadoId.Activo;

        _repoMock.Setup(r => r.GetByIdAsync<int>(5, default))
                 .ReturnsAsync(usuario);

        // When – se solicita eliminar el usuario
        await CreateSut().DeleteAsync(5, "admin@ins.gov.co");

        // Then – el estado cambia a Inactivo en la entidad en memoria
        usuario.EstadoId.Should().Be(EstadoId.Inactivo);
        usuario.ModificadoPor.Should().Be("admin@ins.gov.co");

        // Then – se llama UpdateAsync (soft-delete), nunca DeleteAsync físico
        _repoMock.Verify(r => r.UpdateAsync(usuario, default), Times.Once);
        _repoMock.Verify(r => r.DeleteAsync<int>(It.IsAny<int>(), default), Times.Never);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 3: Delete usuario no encontrado ────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(99)]
    [InlineData(-1)]
    public async Task DeleteAsync_NonExistentUser_ThrowsKeyNotFoundException(int userId)
    {
        // Given – el repositorio no encuentra ningún usuario con ese ID
        _repoMock.Setup(r => r.GetByIdAsync<int>(userId, default))
                 .ReturnsAsync((Usuario?)null);

        // When – se intenta eliminar un usuario inexistente
        // Then – lanza KeyNotFoundException con el ID en el mensaje
        await CreateSut()
            .Invoking(s => s.DeleteAsync(userId, "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{userId}*");

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>(), default), Times.Never);
    }

    [Fact]
    public async Task GetUsuarioByIdAsync_NonExistentId_ReturnsNull()
    {
        // Given – el repositorio no encuentra ningún usuario con ese ID
        _repoMock.Setup(r => r.GetByIdAsync<int>(404, default))
                 .ReturnsAsync((Usuario?)null);

        // When – se busca por un ID que no existe
        var result = await CreateSut().GetUsuarioByIdAsync(404);

        // Then – se retorna null sin lanzar excepción
        result.Should().BeNull();
    }

    // ─── Test 4: Listar usuarios ──────────────────────────────────────────────

    [Fact]
    public async Task GetAllUsuariosAsync_WithExistingUsers_ReturnsCompleteList()
    {
        // Given – el repositorio contiene tres usuarios registrados
        IReadOnlyList<Usuario> usuarios = new List<Usuario>
        {
            new() { Id = 1, Correo = "u1@ins.gov.co", NombreCompleto = "User Uno",  EstadoUsuarioId = EstadoUsuarioId.Activo,   CreadoEn = DateTime.UtcNow },
            new() { Id = 2, Correo = "u2@ins.gov.co", NombreCompleto = "User Dos",  EstadoUsuarioId = EstadoUsuarioId.Inactivo,  CreadoEn = DateTime.UtcNow },
            new() { Id = 3, Correo = "u3@ins.gov.co", NombreCompleto = "User Tres", EstadoUsuarioId = EstadoUsuarioId.Bloqueado, CreadoEn = DateTime.UtcNow }
        };
        _repoMock.Setup(r => r.GetAllAsync(default)).ReturnsAsync(usuarios);

        // When – se solicita la lista completa
        var result = await CreateSut().GetAllUsuariosAsync();

        // Then – se obtienen los tres usuarios con sus IDs y correos correctos
        result.Should().HaveCount(3);
        result.Select(u => u.Id).Should().BeEquivalentTo(new[] { 1, 2, 3 });
        result.Select(u => u.Correo).Should().Contain("u2@ins.gov.co");
    }

    // ─── Test 5: UpdateAsync exitoso ──────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ExistingUser_UpdatesAllFieldsAndSaves()
    {
        // Given – el usuario existe con datos originales
        var usuario = BuildSavedUsuario(id: 7);
        _repoMock.Setup(r => r.GetByIdAsync<int>(7, default)).ReturnsAsync(usuario);

        var request = new UpdateUsuarioRequest(
            Id:              7,
            Correo:          "actualizado@ins.gov.co",
            NombreCompleto:  "Nombre Actualizado",
            EstadoUsuarioId: EstadoUsuarioId.Inactivo,
            AzureAdObjectId: null,
            EntidadId:       5
        );

        // When
        await CreateSut().UpdateAsync(request, "admin@ins.gov.co");

        // Then – los campos de la entidad se actualizaron
        usuario.Correo.Should().Be("actualizado@ins.gov.co");
        usuario.NombreCompleto.Should().Be("Nombre Actualizado");
        usuario.EstadoUsuarioId.Should().Be(EstadoUsuarioId.Inactivo);
        usuario.EntidadId.Should().Be(5);
        usuario.ModificadoPor.Should().Be("admin@ins.gov.co");
        _repoMock.Verify(r => r.UpdateAsync(usuario, default), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    // ─── Test 6: UpdateAsync no encontrado ───────────────────────────────────

    [Fact]
    public async Task UpdateAsync_NonExistentUser_ThrowsKeyNotFoundException()
    {
        // Given – el repositorio no encuentra al usuario
        _repoMock.Setup(r => r.GetByIdAsync<int>(999, default)).ReturnsAsync((Usuario?)null);

        var request = new UpdateUsuarioRequest(999, "x@y.co", "X", 1, null, null);

        // When / Then
        await CreateSut()
            .Invoking(s => s.UpdateAsync(request, "admin"))
            .Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("*999*");

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>(), default), Times.Never);
    }

    // ─── Test 7: GetByCorreoAsync encontrado ─────────────────────────────────

    [Fact]
    public async Task GetByCorreoAsync_ExistingCorreo_ReturnsDto()
    {
        // Given – existe un usuario con ese correo
        var usuario = BuildSavedUsuario(id: 3);
        _repoMock.Setup(r => r.GetByCorreoAsync("nuevo@ins.gov.co", default))
                 .ReturnsAsync(usuario);

        // When
        var result = await CreateSut().GetByCorreoAsync("nuevo@ins.gov.co");

        // Then
        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
        result.Correo.Should().Be("nuevo@ins.gov.co");
    }

    // ─── Test 8: GetByCorreoAsync no encontrado ───────────────────────────────

    [Fact]
    public async Task GetByCorreoAsync_NotFound_ReturnsNull()
    {
        // Given – no existe usuario con ese correo
        _repoMock.Setup(r => r.GetByCorreoAsync("noexiste@ins.gov.co", default))
                 .ReturnsAsync((Usuario?)null);

        // When / Then
        var result = await CreateSut().GetByCorreoAsync("noexiste@ins.gov.co");
        result.Should().BeNull();
    }

    // ─── Test 9: GetAllUsuariosPagedAsync ────────────────────────────────────

    [Fact]
    public async Task GetAllUsuariosPagedAsync_ReturnsCorrectPagedResult()
    {
        // Given – 15 usuarios en total, se solicita la segunda página de 5
        IReadOnlyList<Usuario> page = new List<Usuario>
        {
            new() { Id = 6, Correo = "u6@ins.gov.co", NombreCompleto = "User Seis",  EstadoUsuarioId = 1, CreadoEn = DateTime.UtcNow },
            new() { Id = 7, Correo = "u7@ins.gov.co", NombreCompleto = "User Siete", EstadoUsuarioId = 1, CreadoEn = DateTime.UtcNow }
        };
        _repoMock.Setup(r => r.GetCountAsync(default)).ReturnsAsync(15);
        _repoMock.Setup(r => r.GetPagedAsync(5, 5, default)).ReturnsAsync(page);

        // When
        var result = await CreateSut().GetAllUsuariosPagedAsync(page: 2, pageSize: 5);

        // Then – metadatos de paginación y contenido correctos
        result.Total.Should().Be(15);
        result.Page.Should().Be(2);
        result.PageSize.Should().Be(5);
        result.TotalPages.Should().Be(3);
        result.Items.Should().HaveCount(2);
        result.Items[0].Correo.Should().Be("u6@ins.gov.co");
    }
}
