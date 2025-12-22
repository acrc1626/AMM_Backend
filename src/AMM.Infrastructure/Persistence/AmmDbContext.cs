using System.Reflection;
using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Persistence;

public class AmmDbContext : DbContext
{
    public AmmDbContext(DbContextOptions<AmmDbContext> options) : base(options)
    {
    }

    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Censo> Censos => Set<Censo>();
    public DbSet<CensoNovedad> CensoNovedades => Set<CensoNovedad>();
    public DbSet<Departamento> Departamentos => Set<Departamento>();
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<EstadoUsuario> EstadoUsuarios => Set<EstadoUsuario>();
    public DbSet<Etnia> Etnias => Set<Etnia>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Escabiosis> Escabiosis => Set<Escabiosis>();
    public DbSet<Geohelmintiasis> Geohelmintiasis => Set<Geohelmintiasis>();
    public DbSet<Pediculosis> Pediculosis => Set<Pediculosis>();
    public DbSet<Malaria> Malaria => Set<Malaria>();
    public DbSet<Tuberculosis> Tuberculosis => Set<Tuberculosis>();
    public DbSet<TuberculosisContacto> TuberculosisContacto => Set<TuberculosisContacto>();
    public DbSet<LeshmaniasisCutanea> LeshmaniasisCutanea => Set<LeshmaniasisCutanea>();
    public DbSet<EventoTipo> EventoTipos => Set<EventoTipo>();
    public DbSet<FormaFarmaceutica> FormasFarmaceuticas => Set<FormaFarmaceutica>();
    public DbSet<Medicamento> Medicamentos => Set<Medicamento>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<Microterritorio> Microterritorios => Set<Microterritorio>();
    public DbSet<MotivoNoTratamiento> MotivosNoTratamiento => Set<MotivoNoTratamiento>();
    public DbSet<Municipio> Municipios => Set<Municipio>();
    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Parentesco> Parentescos => Set<Parentesco>();
    public DbSet<Permiso> Permisos => Set<Permiso>();
    public DbSet<PresenciaNovedad> PresenciaNovedades => Set<PresenciaNovedad>();
    public DbSet<Prm> Prms => Set<Prm>();
    public DbSet<PuebloIndigena> PueblosIndigenas => Set<PuebloIndigena>();
    public DbSet<Rafa> Rafas => Set<Rafa>();
    public DbSet<Rol> Roles => Set<Rol>();
    public DbSet<Sexo> Sexos => Set<Sexo>();
    public DbSet<Territorio> Territorios => Set<Territorio>();
    public DbSet<TipoDocumento> TipoDocumentos => Set<TipoDocumento>();
    public DbSet<TipoEntorno> TipoEntornos => Set<TipoEntorno>();
    public DbSet<TipoNovedad> TipoNovedades => Set<TipoNovedad>();
    public DbSet<TipoSupervision> TipoSupervisiones => Set<TipoSupervision>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<TratamientoDetalle> TratamientoDetalles => Set<TratamientoDetalle>();
    public DbSet<Ubicacion> Ubicaciones => Set<Ubicacion>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("USUARIO");

            // Ignorar EstadoId (de AuditableEntity) para que no lo mapee
            entity.Ignore(e => e.EstadoId);

            // Mapear EstadoUsuarioId a estado_usuario_id
            entity.Property(e => e.EstadoUsuarioId)
                  .HasColumnName("estado_usuario_id")
                  .IsRequired();
        });
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
