using AMM.Application;
using AMM.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// OpenAPI / Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AMM Backend API",
        Version = "v1",
        Description = "API para el sistema AMM",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "INS Colombia",
            Email = "soporte@ins.gov.co"
        }
    });

    // Include XML comments if available
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddProblemDetails(); // RFC 9457

// Layer registration
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler(); // Uses ProblemDetails middleware to return RFC 9457 responses
app.UseStatusCodePages();

// Enable Swagger in all environments (change if needed for production)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "AMM Backend API v1");
    options.RoutePrefix = "swagger";
    options.DocumentTitle = "AMM API - Documentación";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
