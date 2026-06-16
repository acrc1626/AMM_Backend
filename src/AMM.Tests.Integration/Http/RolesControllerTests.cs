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
// Pruebas de sistema HTTP: RolesController
// Flujo: login → token → GET /api/roles (protegido con [Authorize])
// La BD InMemory contiene el rol "Administrador" sembrado por la factory.
// ─────────────────────────────────────────────────────────────────────────────

public class RolesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public RolesControllerTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    private HttpClient NewClient() => _factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static StringContent JsonBody(object payload) =>
        new(JsonSerializer.Serialize(payload, JsonOpts),
            System.Text.Encoding.UTF8, "application/json");

    private async Task<HttpClient> ClienteAutenticadoAsync()
    {
        var client = NewClient();

        var loginResponse = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo  = CustomWebApplicationFactory.TestUserEmail,
                           Password = CustomWebApplicationFactory.TestUserPassword }));

        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>(JsonOpts);

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", body!.Token);

        return client;
    }

    // ── Test 1: sin token → 401 ───────────────────────────────────────────────

    [Fact]
    public async Task GetRoles_WithoutToken_Returns401()
    {
        // Given
        var client = NewClient();

        // When
        var response = await client.GetAsync("/api/roles");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── Test 2: con token válido → 200 + lista con "Administrador" ────────────

    [Fact]
    public async Task GetRoles_WithValidToken_Returns200AndContainsAdministrador()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/roles");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var roles = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<RolDto>>(JsonOpts);

        roles.Should().NotBeNullOrEmpty();
        roles!.Should().Contain(r => r.Nombre == CustomWebApplicationFactory.TestRolNombre,
            "la BD de prueba tiene sembrado el rol Administrador");
    }

    // ── Test 3: GET /api/roles/1 → 200 + dto correcto ─────────────────────────

    [Fact]
    public async Task GetRolById_ExistingId_Returns200WithCorrectDto()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/roles/1");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var rol = await response.Content
            .ReadFromJsonAsync<RolDto>(JsonOpts);

        rol.Should().NotBeNull();
        rol!.Id.Should().Be(1);
        rol.Nombre.Should().Be(CustomWebApplicationFactory.TestRolNombre);
    }

    // ── Test 4: GET /api/roles/9999 → 404 ────────────────────────────────────

    [Fact]
    public async Task GetRolById_NonExistentId_Returns404()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/roles/9999");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
