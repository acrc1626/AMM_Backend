using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Application.DTOs.Censos;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas HTTP: CensosController (hereda CatalogControllerBase → [Authorize])
// La factory siembra un Paciente con Id=100 que se usa como PacienteId.
// InMemory no impone FKs, por lo que TipoEntornoId puede ser cualquier byte.
// ─────────────────────────────────────────────────────────────────────────────

public class CensosControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public CensosControllerTests(CustomWebApplicationFactory factory)
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
        var response = await client.GetAsync("/api/censos");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── Test 2: POST con PacienteId válido → 201 Created ─────────────────────

    [Fact]
    public async Task Crear_ConPacienteId_Returns201Created()
    {
        // Given
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoEntornoId = 1,
            PacienteId    = CustomWebApplicationFactory.SeedPacienteId,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = 1,
            Observacion   = "Censo de prueba"
        };

        // When
        var response = await client.PostAsync("/api/censos", JsonBody(request));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var censo = await response.Content
            .ReadFromJsonAsync<CensoDto>(JsonOpts);

        censo.Should().NotBeNull();
        censo!.Id.Should().BeGreaterThan(0);
        censo.PacienteId.Should().Be(CustomWebApplicationFactory.SeedPacienteId);

        response.Headers.Location.Should().NotBeNull("debe incluir Location header");
    }

    // ── Test 3: GET /api/censos/paciente/{id} → 200 ───────────────────────────

    [Fact]
    public async Task GetByPaciente_Returns200ConLista()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync(
            $"/api/censos/paciente/{CustomWebApplicationFactory.SeedPacienteId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var censos = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<CensoDto>>(JsonOpts);

        censos.Should().NotBeNull("la respuesta debe deserializar correctamente");
    }

    // ── Test 4: GET /api/censos/{id} existente → 200 + dto correcto ──────────

    [Fact]
    public async Task GetById_CensoExistente_Returns200()
    {
        // Given — el censo sembrado tiene Id = SeedCensoId
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync(
            $"/api/censos/{CustomWebApplicationFactory.SeedCensoId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var censo = await response.Content
            .ReadFromJsonAsync<CensoDto>(JsonOpts);

        censo.Should().NotBeNull();
        censo!.Id.Should().Be(CustomWebApplicationFactory.SeedCensoId);
        censo.PacienteId.Should().Be(CustomWebApplicationFactory.SeedPacienteId);
    }

    // ── Test 5: PUT /api/censos/{id} → 204 NoContent ─────────────────────────

    [Fact]
    public async Task Update_CensoExistente_Returns204NoContent()
    {
        // Given
        var client = await ClienteAutenticadoAsync();
        var updateRequest = new
        {
            Id            = CustomWebApplicationFactory.SeedCensoId,
            TipoEntornoId = 1,
            PacienteId    = CustomWebApplicationFactory.SeedPacienteId,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = 1,
            Observacion   = "Actualizado"
        };

        // When
        var response = await client.PutAsync(
            $"/api/censos/{CustomWebApplicationFactory.SeedCensoId}",
            JsonBody(updateRequest));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
