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
// Pruebas HTTP: UsuariosController  (route: api/usuarios)
// Todos los endpoints requieren [Authorize] vía CatalogControllerBase.
// Datos de partida: usuario Id=1 (TestUser/Administrador) y Id=2 (SeedDeleteUserId).
// ─────────────────────────────────────────────────────────────────────────────

public class UsuariosControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public UsuariosControllerTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    private HttpClient NewClient() => _factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static StringContent JsonBody(object payload) =>
        new(JsonSerializer.Serialize(payload, JsonOpts),
            System.Text.Encoding.UTF8, "application/json");

    private async Task<HttpClient> ClienteAutenticadoAsync()
    {
        var client = NewClient();
        var login  = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo   = CustomWebApplicationFactory.TestUserEmail,
                           Password = CustomWebApplicationFactory.TestUserPassword }));
        login.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await login.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", body!.Token);
        return client;
    }

    // ── Test 1: GET /api/usuarios con token → 200 + lista no vacía ───────────

    [Fact]
    public async Task GetAll_WithValidToken_Returns200WithList()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/usuarios");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<UsuarioDto>>(JsonOpts);

        list.Should().NotBeNull()
            .And.NotBeEmpty("el usuario sembrado debe aparecer en la lista");
        list!.Should().Contain(u => u.Correo == CustomWebApplicationFactory.TestUserEmail);
    }

    // ── Test 2: GET /api/usuarios/{id} existente → 200 + datos del usuario ───

    [Fact]
    public async Task GetById_ExistingId_Returns200WithDto()
    {
        // Given – usuario sembrado con Id = 1
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/usuarios/1");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<UsuarioDto>(JsonOpts);
        dto.Should().NotBeNull();
        dto!.Id.Should().Be(1);
        dto.Correo.Should().Be(CustomWebApplicationFactory.TestUserEmail);
        dto.NombreCompleto.Should().Be(CustomWebApplicationFactory.TestUserName);
    }

    // ── Test 3: GET /api/usuarios/{id} inexistente → 404 ─────────────────────

    [Fact]
    public async Task GetById_NonExistingId_Returns404()
    {
        // Given – Id 9999 no existe en la BD sembrada
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/usuarios/9999");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Test 4: POST /api/usuarios con datos válidos → 201 + Location header ─

    [Fact]
    public async Task Create_WithValidData_Returns201WithLocationHeader()
    {
        // Given
        var client  = await ClienteAutenticadoAsync();
        var request = new
        {
            Correo          = "nuevo-usuario@ins.gov.co",
            NombreCompleto  = "Usuario Creado en Test",
            EstadoUsuarioId = (byte)1,
            AzureAdObjectId = (Guid?)null
        };

        // When
        var response = await client.PostAsync("/api/usuarios", JsonBody(request));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var dto = await response.Content.ReadFromJsonAsync<UsuarioDto>(JsonOpts);
        dto.Should().NotBeNull();
        dto!.Id.Should().BeGreaterThan(0, "la BD debe asignar un Id válido");
        dto.Correo.Should().Be("nuevo-usuario@ins.gov.co");
        dto.NombreCompleto.Should().Be("Usuario Creado en Test");

        response.Headers.Location.Should()
            .NotBeNull("el 201 debe incluir Location apuntando al nuevo recurso");
    }

    // ── Test 5: DELETE /api/usuarios/{id} → 204 NoContent (soft-delete) ──────

    [Fact]
    public async Task Delete_ExistingId_Returns204NoContent()
    {
        // Given – usuario sembrado Id=2, distinto del usuario de login para no
        //         invalidar el token del resto de tests en esta clase
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.DeleteAsync(
            $"/api/usuarios/{CustomWebApplicationFactory.SeedDeleteUserId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
