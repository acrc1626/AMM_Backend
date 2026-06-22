using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Application.DTOs.Censos;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

/// <summary>
/// Verifica RN-011: después de un PUT sobre un censo Hogar, el Jefe de Hogar
/// original sigue presente con EsJefeHogar = true. La regla se cierra con
/// evidencia HTTP, no solo por diseño del DTO.
/// </summary>
public class CensosRn011Tests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions _jsonOpts = new(JsonSerializerDefaults.Web);

    /// <summary>Inicializa la clase con la factory de pruebas HTTP.</summary>
    public CensosRn011Tests(CustomWebApplicationFactory factory)
        => this._factory = factory;

    private HttpClient NewClient() => this._factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    private static StringContent JsonBody(object payload) =>
        new(JsonSerializer.Serialize(payload, _jsonOpts),
            System.Text.Encoding.UTF8, "application/json");

    private async Task<HttpClient> ClienteAutenticadoAsync()
    {
        var client = NewClient();
        var login  = await client.PostAsync("/api/auth/login", JsonBody(new
        {
            Correo   = CustomWebApplicationFactory.TestUserEmail,
            Password = CustomWebApplicationFactory.TestUserPassword
        }));
        var body = await login.Content.ReadFromJsonAsync<LoginResponse>(_jsonOpts);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", body!.Token);
        return client;
    }

    /// <summary>
    /// RN-011 — evidencia HTTP:
    /// Después de PUT /api/Censos/{id}, el censo Hogar conserva exactamente
    /// el mismo Jefe de Hogar con EsJefeHogar = true.
    /// </summary>
    [Fact]
    public async Task Put_CensoHogar_ConservaJefeDeHogar_RN011()
    {
        // Given — censo Hogar sembrado con Jefe de Hogar (PacienteId = SeedJefeHogarPacienteId)
        var client = await this.ClienteAutenticadoAsync();

        var updateRequest = new
        {
            Id             = CustomWebApplicationFactory.SeedCensoHogarId,
            UbicacionId    = (long?)null,
            DepartamentoId = (short?)null,
            MunicipioId    = (int?)null,
            Fecha          = DateTime.UtcNow.Date.AddDays(1),
            Observacion    = "Actualizado — verificación RN-011",
            EstadoId       = 1
        };

        // When — se actualizan campos editables del censo
        var putResponse = await client.PutAsync(
            $"/api/censos/{CustomWebApplicationFactory.SeedCensoHogarId}",
            JsonBody(updateRequest));

        putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent,
            "el PUT debe responder 204 NoContent");

        // Then — se consulta el detalle completo del censo
        var getResponse = await client.GetAsync(
            $"/api/censos/{CustomWebApplicationFactory.SeedCensoHogarId}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK,
            "el GET debe encontrar el censo después del PUT");

        var censo = await getResponse.Content.ReadFromJsonAsync<CensoDetalleDto>(_jsonOpts);
        censo.Should().NotBeNull();

        // Assert 1 — existe exactamente un Jefe de Hogar (RN-011)
        var jefes = censo!.Personas.Where(p => p.EsJefeHogar).ToList();

        jefes.Should().HaveCount(1,
            "RN-011: debe conservarse exactamente un Jefe de Hogar tras el PUT");

        // Assert 2 — es el mismo paciente sembrado (identidad no cambia)
        jefes[0].PacienteId.Should().Be(
            CustomWebApplicationFactory.SeedJefeHogarPacienteId,
            "RN-011: el Jefe de Hogar no debe cambiar de identidad tras el PUT");

        // Assert 3 — los campos del censo sí se actualizaron (control de regresión)
        censo.Observacion.Should().Be("Actualizado — verificación RN-011",
            "el PUT debe haber persistido los cambios del censo");
    }
}
