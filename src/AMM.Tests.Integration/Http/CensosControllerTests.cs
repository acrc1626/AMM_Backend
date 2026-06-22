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
    public async Task Crear_CensoHogar_ConJefeHogar_Returns201()
    {
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoEntornoId = 2,   // Hogar
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = (byte)1,
            Observacion   = "Hogar de prueba",
            JefeHogar     = new
            {
                PacienteId             = CustomWebApplicationFactory.SeedPacienteId,
                ParentescoId           = (byte?)null,
                Comunidad              = (string?)null,
                GrupoPoblacionEspecial = (string?)null,
                Enfermedades           = Array.Empty<object>()
            },
            Personas = Array.Empty<object>()
        };
        var response = await client.PostAsync("/api/censos", JsonBody(request));
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Crear_CensoHogar_SinJefeHogar_Returns400()
    {
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoEntornoId = 2,   // Hogar requiere JefeHogar
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = (byte)1
        };
        var response = await client.PostAsync("/api/censos", JsonBody(request));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Crear_CensoEducativo_ConJefeHogar_Returns400()
    {
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            TipoEntornoId = 1,   // Educativo NO admite JefeHogar
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = (byte)1,
            JefeHogar     = new
            {
                PacienteId             = CustomWebApplicationFactory.SeedPacienteId,
                ParentescoId           = (byte?)null,
                Comunidad              = (string?)null,
                GrupoPoblacionEspecial = (string?)null,
                Enfermedades           = Array.Empty<object>()
            }
        };
        var response = await client.PostAsync("/api/censos", JsonBody(request));
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Patch_ConTratamientosYNovedad_Returns204()
    {
        // Given — crear un censo nuevo para patchear
        var client = await ClienteAutenticadoAsync();
        var createReq = new
        {
            TipoEntornoId = 1,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = (byte)1,
            Observacion   = "Para PATCH con tratamientos"
        };
        var created = await client.PostAsync("/api/censos", JsonBody(createReq));
        created.StatusCode.Should().Be(HttpStatusCode.Created);
        var censo = await created.Content
            .ReadFromJsonAsync<AMM.Application.DTOs.Censos.CensoResumenDto>(JsonOpts);

        var patchRequest = new
        {
            Personas = Array.Empty<object>(),
            Tratamientos = new
            {
                Tracoma           = true,
                Teniasis          = true,
                RechazaTratamiento = false,
                EnfermedadRenal   = false,
                EnfermedadCardiaca = false,
                Polimedicacion    = false,
                Alergias          = false,
                Embarazo          = false,
                Lactancia         = false,
                MenorEdad         = false,
                EnfermedadHepatica = false,
                TrastornosGastricos = false,
                Observaciones     = "Test de cobertura"
            },
            NovedadCensal = new
            {
                Presencia             = "presente",
                TipoNovedad           = "fallecimiento",
                FechaFallecimiento    = "2026-01-15",
                OtraNovedadDetalle    = (string?)null,
                Observaciones         = "Observación novedad"
            }
        };
        var response = await client.PatchAsync(
            $"/api/censos/{censo!.Id}", JsonBody(patchRequest));
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
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

    [Fact]
    public async Task GetAll_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/censos");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAllPaged_ConToken_Returns200()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/censos/paged?page=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_CensoNoExistente_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var response = await client.GetAsync("/api/censos/99999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Patch_CensoExistente_SinPersonas_Returns204()
    {
        // Given — crear un censo nuevo para no afectar el estado de SeedCensoId
        var client = await ClienteAutenticadoAsync();
        var createRequest = new
        {
            TipoEntornoId = 1,
            Fecha         = DateTime.UtcNow.Date,
            EstadoId      = 1,
            Observacion   = "Censo para PATCH"
        };
        var created = await client.PostAsync("/api/censos", JsonBody(createRequest));
        created.StatusCode.Should().Be(HttpStatusCode.Created);
        var censo = await created.Content
            .ReadFromJsonAsync<AMM.Application.DTOs.Censos.CensoResumenDto>(JsonOpts);

        var patchRequest = new
        {
            Personas      = (object?)null,
            Tratamientos  = (object?)null,
            NovedadCensal = (object?)null
        };
        var response = await client.PatchAsync(
            $"/api/censos/{censo!.Id}",
            JsonBody(patchRequest));
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Patch_CensoNoExistente_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var request = new
        {
            Personas      = (object?)null,
            Tratamientos  = (object?)null,
            NovedadCensal = (object?)null
        };
        var response = await client.PatchAsync("/api/censos/99999", JsonBody(request));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_CensoNoExistente_Returns404()
    {
        var client = await ClienteAutenticadoAsync();
        var updateRequest = new
        {
            Id          = 99999L,
            Fecha       = DateTime.UtcNow.Date,
            EstadoId    = 1,
            Observacion = "No existe"
        };
        var response = await client.PutAsync("/api/censos/99999", JsonBody(updateRequest));
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
