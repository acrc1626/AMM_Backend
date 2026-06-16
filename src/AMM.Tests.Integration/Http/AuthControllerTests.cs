using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AMM.Application.DTOs.Auth;
using AMM.Tests.Integration.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AMM.Tests.Integration.Http;

// ─────────────────────────────────────────────────────────────────────────────
// Pruebas de sistema HTTP: AuthController
// Flujo cubierto: POST /api/auth/login → token → GET /api/auth/me
// La fábrica comparte una BD InMemory sembrada con un usuario activo.
// ─────────────────────────────────────────────────────────────────────────────

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonOpts =
        new(JsonSerializerDefaults.Web);

    public AuthControllerTests(CustomWebApplicationFactory factory)
        => _factory = factory;

    private HttpClient NewClient() => _factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static StringContent JsonBody(object payload) =>
        new(JsonSerializer.Serialize(payload, JsonOpts),
            System.Text.Encoding.UTF8, "application/json");

    private async Task<string> ObtenerTokenAsync()
    {
        var client   = NewClient();
        var response = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo = CustomWebApplicationFactory.TestUserEmail,
                           Password = CustomWebApplicationFactory.TestUserPassword }));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        return body!.Token;
    }

    // ── Test 1: credenciales válidas → 200 + token + rol ─────────────────────

    [Fact]
    public async Task Login_ValidCredentials_Returns200WithTokenAndRole()
    {
        // Given
        var client = NewClient();

        // When
        var response = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo  = CustomWebApplicationFactory.TestUserEmail,
                           Password = CustomWebApplicationFactory.TestUserPassword }));

        // Then – respuesta exitosa
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<LoginResponse>(JsonOpts);
        body.Should().NotBeNull();
        body!.Token.Should().NotBeNullOrEmpty("debe contener un JWT firmado");
        body.Correo.Should().Be(CustomWebApplicationFactory.TestUserEmail);
        body.NombreCompleto.Should().Be(CustomWebApplicationFactory.TestUserName);
        body.Roles.Should().Contain(CustomWebApplicationFactory.TestRolNombre);
        body.Expira.Should().BeAfter(DateTime.UtcNow);
    }

    // ── Test 2: contraseña incorrecta → 401 ──────────────────────────────────

    [Fact]
    public async Task Login_WrongPassword_Returns401()
    {
        // Given
        var client = NewClient();

        // When
        var response = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo  = CustomWebApplicationFactory.TestUserEmail,
                           Password = "WrongPassword!" }));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── Test 3: correo inexistente → 401 ─────────────────────────────────────

    [Fact]
    public async Task Login_UnknownEmail_Returns401()
    {
        // Given
        var client = NewClient();

        // When
        var response = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo  = "noexiste@ins.gov.co",
                           Password = "Test123!" }));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── Test 4: campos vacíos → 400 (FluentValidation) ───────────────────────

    [Fact]
    public async Task Login_EmptyFields_Returns400()
    {
        // Given
        var client = NewClient();

        // When – body sin Correo ni Password
        var response = await client.PostAsync("/api/auth/login",
            JsonBody(new { Correo = "", Password = "" }));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── Test 5: GET /api/auth/me con token válido → 200 + email + rol ────────

    [Fact]
    public async Task Me_WithValidToken_Returns200WithEmailAndRole()
    {
        // Given – obtenemos un token real via login
        var token  = await ObtenerTokenAsync();
        var client = NewClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // When
        var response = await client.GetAsync("/api/auth/me");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content
            .ReadFromJsonAsync<JsonElement>(JsonOpts);

        body.GetProperty("email").GetString()
            .Should().Be(CustomWebApplicationFactory.TestUserEmail);

        body.GetProperty("roles").EnumerateArray()
            .Select(e => e.GetString())
            .Should().Contain(CustomWebApplicationFactory.TestRolNombre);
    }

    // ── Test 6: GET /api/auth/me sin token → 401 ─────────────────────────────

    [Fact]
    public async Task Me_WithoutToken_Returns401()
    {
        // Given
        var client = NewClient();

        // When
        var response = await client.GetAsync("/api/auth/me");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    // ── Test 7: PUT /api/auth/change-password con token válido → 204 ──────────

    [Fact]
    public async Task ChangePassword_WithValidToken_Returns204NoContent()
    {
        // Given – token del usuario sembrado + contraseña temporal
        const string nuevaPassword = "NewTest456!";
        var token  = await ObtenerTokenAsync();
        var client = NewClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // When
        var response = await client.PutAsync("/api/auth/change-password",
            JsonBody(new { PasswordActual = CustomWebApplicationFactory.TestUserPassword,
                           PasswordNuevo  = nuevaPassword }));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Restore – el JWT sigue válido; revierte para no romper otros tests
        await client.PutAsync("/api/auth/change-password",
            JsonBody(new { PasswordActual = nuevaPassword,
                           PasswordNuevo  = CustomWebApplicationFactory.TestUserPassword }));
    }

    // ── Test 8: POST /api/auth/set-password como Administrador → 204 ──────────

    [Fact]
    public async Task SetPassword_AsAdministrator_Returns204NoContent()
    {
        // Given – usuario sembrado tiene rol Administrador
        var token  = await ObtenerTokenAsync();
        var client = NewClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // When – asigna contraseña al segundo usuario sembrado (Id = 2, sin hash aún)
        var response = await client.PostAsync("/api/auth/set-password",
            JsonBody(new { UsuarioId = CustomWebApplicationFactory.SeedDeleteUserId,
                           Password  = "Admin123!" }));

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    // ── Test 9: GET /api/auth/me con token válido → 200 con todos los datos ───

    [Fact]
    public async Task Me_WithValidToken_Returns200WithFullUserData()
    {
        // Given
        var token  = await ObtenerTokenAsync();
        var client = NewClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // When
        var response = await client.GetAsync("/api/auth/me");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOpts);

        body.GetProperty("email").GetString()
            .Should().Be(CustomWebApplicationFactory.TestUserEmail);
        body.GetProperty("roles").EnumerateArray()
            .Select(e => e.GetString())
            .Should().Contain(CustomWebApplicationFactory.TestRolNombre);
        body.GetProperty("sub").GetString()
            .Should().NotBeNullOrEmpty("debe contener el identificador numérico del usuario");
    }
}
