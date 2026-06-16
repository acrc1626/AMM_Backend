using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Application.DTOs.Seguridad;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas HTTP: UsuariosController, MenusController, PermisosController.
// Todos heredan CatalogControllerBase ([Authorize]).
// La factory siembra un usuario con Id=1 (test user) y Id=2 (para soft-delete).
// ─────────────────────────────────────────────────────────────────────────────

public class SeguridadControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public SeguridadControllerTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    private HttpClient NewClient() => _factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static StringContent JsonBody(object payload) =>
        new(JsonSerializer.Serialize(payload, JsonOpts),
            System.Text.Encoding.UTF8, "application/json");

    private async Task<HttpClient> ClienteAutenticadoAsync()
    {
        var client = NewClient();
        var login = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo   = CustomWebApplicationFactory.TestUserEmail,
                           Password = CustomWebApplicationFactory.TestUserPassword }));
        login.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await login.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", body!.Token);
        return client;
    }

    // ── UsuariosController ────────────────────────────────────────────────────

    [Fact]
    public async Task Usuarios_GetAll_WithToken_Returns200()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/usuarios");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<IReadOnlyList<UsuarioDto>>(JsonOpts);
        list.Should().NotBeNull().And.NotBeEmpty("el usuario sembrado debe aparecer en la lista");
    }

    [Fact]
    public async Task Usuarios_GetById_ExistingId_Returns200()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/usuarios/1");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var dto = await response.Content.ReadFromJsonAsync<UsuarioDto>(JsonOpts);
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(1);
        dto.Correo.Should().Be(CustomWebApplicationFactory.TestUserEmail);
    }

    [Fact]
    public async Task Usuarios_Create_Returns201Created()
    {
        // Given
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            Correo          = "nuevo@ins.gov.co",
            NombreCompleto  = "Usuario Nuevo de Prueba",
            EstadoUsuarioId = (byte)1,
            AzureAdObjectId = (Guid?)null
        };

        // When
        var response = await client.PostAsync("/api/usuarios", JsonBody(request));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var dto = await response.Content.ReadFromJsonAsync<UsuarioDto>(JsonOpts);
        dto.Should().NotBeNull();
        dto!.Id.Should().BeGreaterThan(0);
        dto.Correo.Should().Be("nuevo@ins.gov.co");
        response.Headers.Location.Should().NotBeNull("debe incluir Location header");
    }

    [Fact]
    public async Task Usuarios_Delete_SoftDelete_Returns204()
    {
        // Given — usuario sembrado con Id=SeedDeleteUserId, distinto del usuario de login
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.DeleteAsync(
            $"/api/usuarios/{CustomWebApplicationFactory.SeedDeleteUserId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    // ── MenusController ───────────────────────────────────────────────────────

    [Fact]
    public async Task Menus_GetAll_WithToken_Returns200()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/menus");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Menus_GetByRol_WithToken_Returns200()
    {
        // Given — Rol con Id=1 está sembrado; puede no tener menús pero devuelve 200 + []
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/menus/rol/1");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── PermisosController ────────────────────────────────────────────────────

    [Fact]
    public async Task Permisos_GetAll_WithToken_Returns200()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/permisos");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
