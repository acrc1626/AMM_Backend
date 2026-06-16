using System.Text;
using AMM.Application;
using AMM.Infrastructure;
using AMM.Application.Settings;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ──────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(AMM.Application.Validators.LoginRequestValidator).Assembly);

// ── OpenAPI / Swagger ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AMM Backend API",
        Version = "v1",
        Description = "API para el sistema de vigilancia epidemiológica AMM — INS Colombia",
        Contact = new OpenApiContact
        {
            Name = "INS Colombia",
            Email = "soporte@ins.gov.co"
        }
    });

    // Soporte para Bearer token en Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT con el prefijo 'Bearer <token>'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection(JwtSettings.SectionName);
builder.Services.Configure<JwtSettings>(jwtSection);

// Validar que la sección existe al arrancar (rápido-fail)
if (jwtSection.Get<JwtSettings>() is null)
    throw new InvalidOperationException("Falta la sección 'Jwt' en appsettings.json.");

// Registrar sin parámetros fijos: la configuración se resuelve desde IOptions<JwtSettings>
// en tiempo de ejecución, lo que permite que las pruebas inyecten sus propios valores.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

builder.Services
    .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<Microsoft.Extensions.Options.IOptions<JwtSettings>>((bearerOpts, jwtOpts) =>
    {
        var s = jwtOpts.Value;
        bearerOpts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidIssuer              = s.Issuer,
            ValidateAudience         = true,
            ValidAudience            = s.Audience,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey         = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(s.Key)),
            ClockSkew                = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// ── ProblemDetails (RFC 9457) ─────────────────────────────────────────────────
builder.Services.AddProblemDetails();

// ── Application & Infrastructure layers ──────────────────────────────────────
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins(
                builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                ?? ["http://localhost:4200"])
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Middlewares ───────────────────────────────────────────────────────────────
app.UseExceptionHandler();
app.UseStatusCodePages();

// Swagger solo en Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "AMM Backend API v1");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "AMM API - Documentación";
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

app.UseAuthentication(); // ← PRIMERO autenticación
app.UseAuthorization();  // ← DESPUÉS autorización (sin duplicado)

app.MapControllers();

app.Run();

// Exposes Program to WebApplicationFactory<Program> in test projects
public partial class Program { }
