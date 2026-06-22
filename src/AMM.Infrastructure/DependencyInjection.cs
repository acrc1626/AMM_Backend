using AMM.Application.Interfaces;
using AMM.Application.Settings;
using AMM.Infrastructure.Interceptors;
using AMM.Infrastructure.Persistence;
using AMM.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AMM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<AuditInterceptor>();

        services.AddDbContext<AmmDbContext>((sp, options) =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AmmDbContext).Assembly.FullName))
            .AddInterceptors(sp.GetRequiredService<AuditInterceptor>()));

        // Security services
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordHasher, Security.PasswordHasher>();


        // Register repositories (adapters)
        services.AddScoped<AMM.Domain.Ports.IPacienteRepository, Repositories.PacienteRepository>();
        
        // Catalog repositories
        services.AddScoped<AMM.Domain.Ports.Repositories.IEstadoRepository, Repositories.EstadoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEstadoUsuarioRepository, Repositories.EstadoUsuarioRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITipoDocumentoRepository, Repositories.TipoDocumentoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ISexoRepository, Repositories.SexoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEtniaRepository, Repositories.EtniaRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IPuebloIndigenaRepository, Repositories.PuebloIndigenaRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITipoEntornoRepository, Repositories.TipoEntornoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITipoNovedadRepository, Repositories.TipoNovedadRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IPresenciaNovedadRepository, Repositories.PresenciaNovedadRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITipoSupervisionRepository, Repositories.TipoSupervisionRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IMotivoNoTratamientoRepository, Repositories.MotivoNoTratamientoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IParentescoRepository, Repositories.ParentescoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoTipoRepository, Repositories.EventoTipoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IFormaFarmaceuticaRepository, Repositories.FormaFarmaceuticaRepository>();

        // Geographic repositories
        services.AddScoped<AMM.Domain.Ports.Repositories.IDepartamentoRepository, Repositories.DepartamentoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IMunicipioRepository, Repositories.MunicipioRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITerritorioRepository, Repositories.TerritorioRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IMicroterritorioRepository, Repositories.MicroterritorioRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IAreaRepository, Repositories.AreaRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IUbicacionRepository, Repositories.UbicacionRepository>();

        // Security repositories
        services.AddScoped<AMM.Domain.Ports.Repositories.IUsuarioRepository, Repositories.UsuarioRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IRolRepository, Repositories.RolRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IPermisoRepository, Repositories.PermisoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IMenuRepository, Repositories.MenuRepository>();

        // Censo repositories
        services.AddScoped<AMM.Domain.Ports.Repositories.ICensoRepository, Repositories.CensoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ICensoNovedadRepository, Repositories.CensoNovedadRepository>();

        // Eventos repositories
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoRepository, Repositories.EventoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoEscabiosisRepository, Repositories.EventoEscabiosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoGeohelmintiasisRepository, Repositories.EventoGeohelmintiasisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoPediculosisRepository, Repositories.EventoPediculosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoPianRepository, Repositories.EventoPianRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoTeniasisCisticercosisRepository, Repositories.EventoTeniasisCisticercosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoTracomaRepository, Repositories.EventoTracomaRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoTungiasisRepository, Repositories.EventoTungiasisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoMalariaRepository, Repositories.EventoMalariaRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoTuberculosisRepository, Repositories.EventoTuberculosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoTuberculosisContactoRepository, Repositories.EventoTuberculosisContactoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IEventoLeshmaniasisCutaneaRepository, Repositories.EventoLeshmaniasisCutaneaRepository>();

        return services;
    }
}
