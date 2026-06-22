using AMM.Application.UseCases.Pacientes;
using Microsoft.Extensions.DependencyInjection;

namespace AMM.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Auth use cases
        services.AddScoped<UseCases.Auth.LoginUseCase>();
        services.AddScoped<UseCases.Auth.SetPasswordUseCase>();

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
        services.AddScoped<UseCases.Catalogos.ParentescoUseCase>();

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
        services.AddScoped<UseCases.Censos.CrearCensoUseCase>();
        services.AddScoped<UseCases.Censos.ConsultarCensoUseCase>();

        // Eventos use cases
        services.AddScoped<UseCases.Eventos.EventoUseCase>();
        services.AddScoped<UseCases.Eventos.EventoEscabiosisUseCase>();
        services.AddScoped<UseCases.Eventos.EventoGeohelmintiasisUseCase>();
        services.AddScoped<UseCases.Eventos.EventoPediculosisUseCase>();
        services.AddScoped<UseCases.Eventos.EventoPianUseCase>();
        services.AddScoped<UseCases.Eventos.EventoTeniasisCisticercosisUseCase>();
        services.AddScoped<UseCases.Eventos.EventoTracomaUseCase>();
        services.AddScoped<UseCases.Eventos.EventoTungiasisUseCase>();
        services.AddScoped<UseCases.Eventos.EventoMalariaUseCase>();
        services.AddScoped<UseCases.Eventos.EventoTuberculosisUseCase>();
        services.AddScoped<UseCases.Eventos.EventoTuberculosisContactoUseCase>();
        services.AddScoped<UseCases.Eventos.EventoLeshmaniasisCutaneaUseCase>();

        return services;
    }
}
