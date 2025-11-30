using AMM.Application.UseCases.Pacientes;
using Microsoft.Extensions.DependencyInjection;

namespace AMM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register use cases
        services.AddScoped<CrearPacienteUseCase>();
        services.AddScoped<GetPacienteByIdUseCase>();
        services.AddScoped<GetAllPacientesUseCase>();
        services.AddScoped<UpdatePacienteUseCase>();
        
        // Catalog use cases
        services.AddScoped<UseCases.Catalogos.EstadoUseCase>();
        services.AddScoped<UseCases.Catalogos.TipoDocumentoUseCase>();
        services.AddScoped<UseCases.Catalogos.SexoUseCase>();
        services.AddScoped<UseCases.Catalogos.EtniaUseCase>();
        services.AddScoped<UseCases.Catalogos.PuebloIndigenaUseCase>();
        services.AddScoped<UseCases.Catalogos.TipoEntornoUseCase>();
        services.AddScoped<UseCases.Catalogos.EventoTipoUseCase>();
        services.AddScoped<UseCases.Catalogos.FormaFarmaceuticaUseCase>();

        // Geographic use cases
        services.AddScoped<UseCases.UbicacionGeografica.DepartamentoUseCase>();
        services.AddScoped<UseCases.UbicacionGeografica.MunicipioUseCase>();
        services.AddScoped<UseCases.UbicacionGeografica.TerritorioUseCase>();
        services.AddScoped<UseCases.UbicacionGeografica.MicroterritorioUseCase>();
        services.AddScoped<UseCases.UbicacionGeografica.AreaUseCase>();
        services.AddScoped<UseCases.UbicacionGeografica.UbicacionUseCase>();

        // Security use cases
        services.AddScoped<UseCases.Seguridad.UsuarioUseCase>();
        services.AddScoped<UseCases.Seguridad.RolUseCase>();
        services.AddScoped<UseCases.Seguridad.PermisoUseCase>();
        services.AddScoped<UseCases.Seguridad.MenuUseCase>();

        // Censo use cases
        services.AddScoped<UseCases.Censos.CensoUseCase>();
        services.AddScoped<UseCases.Censos.CensoNovedadUseCase>();

        // Eventos use cases
        services.AddScoped<UseCases.Eventos.EscabiosisUseCase>();
        services.AddScoped<UseCases.Eventos.GeohelmintiasisUseCase>();
        services.AddScoped<UseCases.Eventos.PediculosisUseCase>();
        services.AddScoped<UseCases.Eventos.MalariaUseCase>();
        services.AddScoped<UseCases.Eventos.TuberculosisUseCase>();
        services.AddScoped<UseCases.Eventos.TuberculosisContactoUseCase>();
        services.AddScoped<UseCases.Eventos.LeshmaniasisCutaneaUseCase>();

        return services;
    }
}
