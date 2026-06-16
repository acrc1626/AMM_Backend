using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

public class EventosControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public EventosControllerTests(CustomWebApplicationFactory factory) => _factory = factory;

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

    // ── Escabiosis ────────────────────────────────────────────────────────────

    [Fact]
    public async Task Escabiosis_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/escabiosis"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Escabiosis_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/escabiosis");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Escabiosis_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/escabiosis/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Geohelmintiasis ───────────────────────────────────────────────────────

    [Fact]
    public async Task Geohelmintiasis_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/geohelmintiasis"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Geohelmintiasis_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/geohelmintiasis");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Geohelmintiasis_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/geohelmintiasis/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Pediculosis ───────────────────────────────────────────────────────────

    [Fact]
    public async Task Pediculosis_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/pediculosis"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Pediculosis_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/pediculosis");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Pediculosis_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/pediculosis/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Malaria ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Malaria_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/malaria"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Malaria_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/malaria");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Malaria_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/malaria/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── Tuberculosis ──────────────────────────────────────────────────────────

    [Fact]
    public async Task Tuberculosis_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/tuberculosis"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task Tuberculosis_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/tuberculosis");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Tuberculosis_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/tuberculosis/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── TuberculosisContacto ──────────────────────────────────────────────────

    [Fact]
    public async Task TuberculosisContacto_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/tuberculosiscontacto"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task TuberculosisContacto_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/tuberculosiscontacto");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task TuberculosisContacto_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/tuberculosiscontacto/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── LeshmaniasisCutanea ───────────────────────────────────────────────────

    [Fact]
    public async Task LeshmaniasisCutanea_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/leshmaniasiscutanea"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task LeshmaniasisCutanea_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/leshmaniasiscutanea");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LeshmaniasisCutanea_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/leshmaniasiscutanea/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    // ── CensoNovedades ────────────────────────────────────────────────────────

    [Fact]
    public async Task CensoNovedades_SinToken_Returns401() =>
        (await NewClient().GetAsync("/api/censonovedades"))
            .StatusCode.Should().Be(HttpStatusCode.Unauthorized);

    [Fact]
    public async Task CensoNovedades_GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/censonovedades");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CensoNovedades_GetById_NoExiste_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/censonovedades/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CensoNovedades_GetByCenso_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync(
            $"/api/censonovedades/censo/{CustomWebApplicationFactory.SeedCensoId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CensoNovedades_GetByPaciente_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync(
            $"/api/censonovedades/paciente/{CustomWebApplicationFactory.SeedPacienteId}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
