using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Application.DTOs.Catalogos;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas HTTP: EstadosController (hereda CatalogControllerBase → [Authorize])
// BD InMemory sin estados sembrados: GetAll → 200 lista vacía, GetById → 404
// ─────────────────────────────────────────────────────────────────────────────

public class EstadosControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public EstadosControllerTests(CustomWebApplicationFactory factory)
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

    // ── Test 1: sin token → 401 ───────────────────────────────────────────────

    [Fact]
    public async Task GetAll_SinToken_Returns401()
    {
        // Given
        var client = NewClient();

        // When
        var response = await client.GetAsync("/api/estados");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── Test 2: con token → 200 + lista (vacía en BD InMemory) ───────────────

    [Fact]
    public async Task GetAll_ConToken_Returns200()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/estados");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var estados = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<EstadoDto>>(JsonOpts);

        estados.Should().NotBeNull("la respuesta debe deserializar correctamente");
    }

    // ── Test 3: GET /api/estados/99 sin datos → 404 ───────────────────────────

    [Fact]
    public async Task GetById_IdInexistente_Returns404()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/estados/99");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
