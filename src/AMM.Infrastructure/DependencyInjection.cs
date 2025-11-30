using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AMM.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AmmDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AmmDbContext).Assembly.FullName)));

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
        services.AddScoped<AMM.Domain.Ports.Repositories.IEscabiosisRepository, Repositories.EscabiosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IGeohelmintiasisRepository, Repositories.GeohelmintiasisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IPediculosisRepository, Repositories.PediculosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.IMalariaRepository, Repositories.MalariaRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITuberculosisRepository, Repositories.TuberculosisRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ITuberculosisContactoRepository, Repositories.TuberculosisContactoRepository>();
        services.AddScoped<AMM.Domain.Ports.Repositories.ILeshmaniasisCutaneaRepository, Repositories.LeshmaniasisCutaneaRepository>();

        return services;
    }
}
