using System.Reflection;
using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Persistence;

public class AmmDbContext : DbContext
{
    public AmmDbContext(DbContextOptions<AmmDbContext> options) : base(options)
    {
    }

    // Catálogos
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<EstadoUsuario> EstadoUsuarios => Set<EstadoUsuario>();
    public DbSet<EstadoPersona> EstadoPersonas => Set<EstadoPersona>();
    public DbSet<Etnia> Etnias => Set<Etnia>();
    public DbSet<EtniaPuebloIndigena> EtniaPueblosIndigenas => Set<EtniaPuebloIndigena>();
    public DbSet<Enfermedad> Enfermedades => Set<Enfermedad>();
    public DbSet<EventoTipo> EventoTipos => Set<EventoTipo>();
    public DbSet<FormaFarmaceutica> FormasFarmaceuticas => Set<FormaFarmaceutica>();
    public DbSet<MotivoNoTratamiento> MotivosNoTratamiento => Set<MotivoNoTratamiento>();
    public DbSet<Parentesco> Parentescos => Set<Parentesco>();
    public DbSet<PresenciaNovedad> PresenciaNovedades => Set<PresenciaNovedad>();
    public DbSet<PuebloIndigena> PueblosIndigenas => Set<PuebloIndigena>();
    public DbSet<Sexo> Sexos => Set<Sexo>();
    public DbSet<TipoDocumento> TipoDocumentos => Set<TipoDocumento>();
    public DbSet<TipoEntorno> TipoEntornos => Set<TipoEntorno>();
    public DbSet<TipoNovedad> TipoNovedades => Set<TipoNovedad>();
    public DbSet<TipoSupervision> TipoSupervisiones => Set<TipoSupervision>();

    // Geografía
    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Microterritorio> Microterritorios => Set<Microterritorio>();
    public DbSet<Municipio> Municipios => Set<Municipio>();
    public DbSet<Territorio> Territorios => Set<Territorio>();
    public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();

    // Pacientes
    public DbSet<Paciente> Pacientes => Set<Paciente>();

    // Censos
    public DbSet<Censo> Censos => Set<Censo>();
    public DbSet<CensoNovedad> CensoNovedades => Set<CensoNovedad>();
    public DbSet<CensoPersona> CensoPersonas => Set<CensoPersona>();
    public DbSet<PersonaEnfermedad> PersonaEnfermedades => Set<PersonaEnfermedad>();

    // Eventos
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<EventoEscabiosis> EventoEscabiosis => Set<EventoEscabiosis>();
    public DbSet<EventoGeohelmintiasis> EventoGeohelmintiasis => Set<EventoGeohelmintiasis>();
    public DbSet<EventoPediculosis> EventoPediculosis => Set<EventoPediculosis>();
    public DbSet<EventoPian> EventoPian => Set<EventoPian>();
    public DbSet<EventoTeniasisCisticercosis> EventoTeniasisCisticercosis => Set<EventoTeniasisCisticercosis>();
    public DbSet<EventoTracoma> EventoTracoma => Set<EventoTracoma>();
    public DbSet<EventoTungiasis> EventoTungiasis => Set<EventoTungiasis>();
    public DbSet<EventoMalaria> EventoMalaria => Set<EventoMalaria>();
    public DbSet<EventoTuberculosis> EventoTuberculosis => Set<EventoTuberculosis>();
    public DbSet<EventoTuberculosisContacto> EventoTuberculosisContacto => Set<EventoTuberculosisContacto>();
    public DbSet<EventoLeshmaniasisCutanea> EventoLeshmaniasisCutanea => Set<EventoLeshmaniasisCutanea>();

    // Tratamientos
    public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
    public DbSet<Prm> Prms => Set<Prm>();
    public DbSet<Rafa> Rafas => Set<Rafa>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<TratamientoDetalle> TratamientoDetalles => Set<TratamientoDetalle>();

    // Seguridad
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("USUARIO");

            entity.Ignore(e => e.EstadoId);

            entity.Property(e => e.AzureAdObjectId).HasColumnName("azure_ad_object_id");
            entity.Property(e => e.Correo).HasColumnName("correo");
            entity.Property(e => e.NombreCompleto).HasColumnName("nombre_completo");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.EntidadId).HasColumnName("entidad_id");
            entity.Property(e => e.EstadoUsuarioId).HasColumnName("estado_usuario_id").IsRequired();
        });
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
