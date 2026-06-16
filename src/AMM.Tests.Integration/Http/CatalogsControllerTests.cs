using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas HTTP: controladores de catálogos simples.
// Todos heredan CatalogControllerBase ([Authorize]).
// La BD InMemory puede estar vacía para GetAll — la respuesta es 200 + [].
// ─────────────────────────────────────────────────────────────────────────────

public class CatalogsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public CatalogsControllerTests(CustomWebApplicationFactory factory)
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

    // ── TiposDocumento ────────────────────────────────────────────────────────

    [Fact]
    public async Task TiposDocumento_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/tiposdocumento");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Sexos ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Sexos_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/sexos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── Etnias ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Etnias_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/etnias");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── PueblosIndígenas ──────────────────────────────────────────────────────

    [Fact]
    public async Task PueblosIndigenas_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/pueblosindigenas");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── TiposEntorno ──────────────────────────────────────────────────────────

    [Fact]
    public async Task TiposEntorno_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/tiposentorno");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── EventoTipos ───────────────────────────────────────────────────────────

    [Fact]
    public async Task EventoTipos_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/eventotipos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── FormasFarmacéuticas ───────────────────────────────────────────────────

    [Fact]
    public async Task FormasFarmaceuticas_GetAll_WithToken_ReturnsOk()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/formasfarmaceuticas");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    // ── GetById: registro existente (Id=1 sembrado) → 200 ────────────────────
    // byte: 1 → no excede 255; short: 1 válido en todos los controllers.

    [Theory]
    [InlineData("/api/tiposdocumento/1")]
    [InlineData("/api/sexos/1")]
    [InlineData("/api/etnias/1")]
    [InlineData("/api/pueblosindigenas/1")]
    [InlineData("/api/tiposentorno/1")]
    [InlineData("/api/eventotipos/1")]
    [InlineData("/api/formasfarmaceuticas/1")]
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
    // byte max=255, usamos 99; short/int usan 9999.

    [Theory]
    [InlineData("/api/tiposdocumento/99")]
    [InlineData("/api/sexos/99")]
    [InlineData("/api/etnias/99")]
    [InlineData("/api/pueblosindigenas/9999")]
    [InlineData("/api/tiposentorno/99")]
    [InlineData("/api/eventotipos/99")]
    [InlineData("/api/formasfarmaceuticas/9999")]
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
