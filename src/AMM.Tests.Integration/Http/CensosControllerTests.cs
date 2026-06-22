using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Application.DTOs.Censos;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

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

    [Fact]
    public async Task GetAll_SinToken_Returns401()
    {
        var client = NewClient();
        var response = await client.GetAsync("/api/censos");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Crear_ConDatosValidos_Returns201Created()
    {
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoEntornoId = 1,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = 1,
            Observacion   = "Censo de prueba"
        };

        var response = await client.PostAsync("/api/censos", JsonBody(request));

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var censo = await response.Content.ReadFromJsonAsync<CensoDto>(JsonOpts);
        censo.Should().NotBeNull();
        censo!.Id.Should().BeGreaterThan(0);
        response.Headers.Location.Should().NotBeNull("debe incluir Location header");
    }

    [Fact]
    public async Task GetById_CensoExistente_Returns200()
    {
        var client = await ClienteAutenticadoAsync();

        var response = await client.GetAsync(
            $"/api/censos/{CustomWebApplicationFactory.SeedCensoId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var censo = await response.Content.ReadFromJsonAsync<CensoDto>(JsonOpts);
        censo.Should().NotBeNull();
        censo!.Id.Should().Be(CustomWebApplicationFactory.SeedCensoId);
    }

    [Fact]
    public async Task Update_CensoExistente_Returns204NoContent()
    {
        var client = await ClienteAutenticadoAsync();
        var updateRequest = new
        {
            Id            = CustomWebApplicationFactory.SeedCensoId,
            TipoEntornoId = 1,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = 1,
            Observacion   = "Actualizado"
        };

        var response = await client.PutAsync(
            $"/api/censos/{CustomWebApplicationFactory.SeedCensoId}",
            JsonBody(updateRequest));

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
