using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas HTTP: controladores de ubicación geográfica.
// Todos heredan CatalogControllerBase ([Authorize]).
// GetAll usa base AsNoTracking().ToListAsync() sin Include,
// por lo que devuelve 200 + [] en BD vacía.
// ─────────────────────────────────────────────────────────────────────────────

public class UbicacionGeograficaControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public UbicacionGeograficaControllerTests(CustomWebApplicationFactory factory)
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

    // ── Departamentos ─────────────────────────────────────────────────────────

    [Fact]
    public async Task Departamentos_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/departamentos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Municipios ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Municipios_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/municipios");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Territorios ───────────────────────────────────────────────────────────

    [Fact]
    public async Task Territorios_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/territorios");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Microterritorios ──────────────────────────────────────────────────────

    [Fact]
    public async Task Microterritorios_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/microterritorios");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Areas ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Areas_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/areas");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Ubicaciones ───────────────────────────────────────────────────────────

    [Fact]
    public async Task Ubicaciones_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/ubicaciones");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── GetById: registro existente (Id=1 sembrado) → 200 ────────────────────

    [Theory]
    [InlineData("/api/departamentos/1")]
    [InlineData("/api/municipios/1")]
    [InlineData("/api/territorios/1")]
    [InlineData("/api/microterritorios/1")]
    [InlineData("/api/areas/1")]
    [InlineData("/api/ubicaciones/1")]
    public async Task GetById_ExistingId_ReturnsOk(string url)
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync(url);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK,
            $"el registro con Id=1 fue sembrado en {url}");
    }

    // ── GetById: Id inexistente → 404 ────────────────────────────────────────

    [Theory]
    [InlineData("/api/departamentos/9999")]
    [InlineData("/api/municipios/9999")]
    [InlineData("/api/territorios/9999")]
    [InlineData("/api/microterritorios/9999")]
    [InlineData("/api/areas/9999")]
    [InlineData("/api/ubicaciones/9999")]
    public async Task GetById_NonExistentId_ReturnsNotFound(string url)
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync(url);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound,
            $"no existe ningún registro con ese Id en {url}");
    }
}
