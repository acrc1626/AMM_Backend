using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs;
using AMM.Application.DTOs.Auth;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas HTTP: PacientesController
// La factory siembra un paciente (TipoDocumentoId=1, Documento="DOC-SEED-001")
// para verificar que la detección de duplicados devuelve 400 (no 409).
// ─────────────────────────────────────────────────────────────────────────────

public class PacientesControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public PacientesControllerTests(CustomWebApplicationFactory factory)
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

    // ── Test 1: POST con datos válidos → 201 Created ──────────────────────────

    [Fact]
    public async Task Crear_ConDatosValidos_Returns201Created()
    {
        // Given
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoDocumentoId = 1,
            Documento       = "TEST-NEW-DOC-001",
            PrimerNombre    = "Juan",
            PrimerApellido  = "García",
            SexoId          = 1
        };

        // When
        var response = await client.PostAsync("/api/pacientes", JsonBody(request));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var paciente = await response.Content
            .ReadFromJsonAsync<PacienteDto>(JsonOpts);

        paciente.Should().NotBeNull();
        paciente!.Id.Should().BeGreaterThan(0);
        paciente.Documento.Should().Be("TEST-NEW-DOC-001");

        response.Headers.Location.Should().NotBeNull("debe incluir Location header");
    }

    // ── Test 2: POST con documento duplicado → 400 BadRequest ─────────────────
    // PacientesController captura InvalidOperationException y devuelve 400,
    // NO 409 — el comportamiento del controlador es explícito en el catch block.

    [Fact]
    public async Task Crear_ConDocumentoDuplicado_Returns400BadRequest()
    {
        // Given — el documento "DOC-SEED-001" (TipoDocumentoId=1) ya existe en la BD
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoDocumentoId = CustomWebApplicationFactory.SeedPacienteTipoDocId,
            Documento       = CustomWebApplicationFactory.SeedPacienteDocumento,
            PrimerNombre    = "Otro",
            PrimerApellido  = "Paciente",
            SexoId          = 1
        };

        // When
        var response = await client.PostAsync("/api/pacientes", JsonBody(request));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── Test 3: GET /api/pacientes con token → 200 + lista ───────────────────

    [Fact]
    public async Task GetAll_ConToken_Returns200ConLista()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync("/api/pacientes");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var pacientes = await response.Content
            .ReadFromJsonAsync<IReadOnlyList<PacienteDto>>(JsonOpts);

        pacientes.Should().NotBeNull();
        pacientes!.Should().NotBeEmpty("la factory siembra al menos un paciente");
    }

    // ── Test 4: GET /api/pacientes/{id} existente → 200 + dto correcto ───────

    [Fact]
    public async Task GetById_PacienteExistente_Returns200()
    {
        // Given
        var client = await ClienteAutenticadoAsync();

        // When
        var response = await client.GetAsync(
            $"/api/pacientes/{CustomWebApplicationFactory.SeedPacienteId}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var paciente = await response.Content
            .ReadFromJsonAsync<PacienteDto>(JsonOpts);

        paciente.Should().NotBeNull();
        paciente!.Id.Should().Be(CustomWebApplicationFactory.SeedPacienteId);
        paciente.Documento.Should().Be(CustomWebApplicationFactory.SeedPacienteDocumento);
    }

    // ── Test 5: PUT /api/pacientes/{id} → 204 NoContent ──────────────────────

    [Fact]
    public async Task Update_PacienteExistente_Returns204NoContent()
    {
        // Given
        var client = await ClienteAutenticadoAsync();
        var updateRequest = new
        {
            Id              = CustomWebApplicationFactory.SeedPacienteId,
            TipoDocumentoId = CustomWebApplicationFactory.SeedPacienteTipoDocId,
            Documento       = CustomWebApplicationFactory.SeedPacienteDocumento,
            PrimerNombre    = "Paciente",
            PrimerApellido  = "Actualizado",
            SexoId          = 1,
            EstadoId        = 1
        };

        // When
        var response = await client.PutAsync(
            $"/api/pacientes/{CustomWebApplicationFactory.SeedPacienteId}",
            JsonBody(updateRequest));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
