using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class CatalogConfiguration :
    IEntityTypeConfiguration<Estado>,
    IEntityTypeConfiguration<EstadoUsuario>,
    IEntityTypeConfiguration<TipoDocumento>,
    IEntityTypeConfiguration<Sexo>,
    IEntityTypeConfiguration<Etnia>,
    IEntityTypeConfiguration<PuebloIndigena>,
    IEntityTypeConfiguration<EtniaPuebloIndigena>,
    IEntityTypeConfiguration<TipoEntorno>,
    IEntityTypeConfiguration<TipoNovedad>,
    IEntityTypeConfiguration<PresenciaNovedad>,
    IEntityTypeConfiguration<TipoSupervision>,
    IEntityTypeConfiguration<MotivoNoTratamiento>,
    IEntityTypeConfiguration<Parentesco>,
    IEntityTypeConfiguration<EventoTipo>,
    IEntityTypeConfiguration<FormaFarmaceutica>,
    IEntityTypeConfiguration<EstadoPersona>,
    IEntityTypeConfiguration<Enfermedad>
{
    public void Configure(EntityTypeBuilder<Estado> builder)
    {
        builder.ToTable("ESTADO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(40).IsUnicode(false);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
    }

    public void Configure(EntityTypeBuilder<EstadoUsuario> builder)
    {
        builder.ToTable("ESTADO_USUARIO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(30).IsUnicode(false);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
    }

    public void Configure(EntityTypeBuilder<TipoDocumento> builder)
    {
        builder.ToTable("TIPO_DOCUMENTO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(10).IsUnicode(false);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(60).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<Sexo> builder)
    {
        builder.ToTable("SEXO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(30).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<Etnia> builder)
    {
        builder.ToTable("ETNIA");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(60).IsUnicode(false);
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(5).IsUnicode(false).HasDefaultValue("");

        builder.HasMany(e => e.PueblosIndigenas)
               .WithMany(p => p.Etnias)
               .UsingEntity<EtniaPuebloIndigena>(
                   j => j.HasOne(x => x.PuebloIndigena).WithMany().HasForeignKey(x => x.PuebloIndigenaId).OnDelete(DeleteBehavior.ClientSetNull),
                   j => j.HasOne(x => x.Etnia).WithMany().HasForeignKey(x => x.EtniaId).OnDelete(DeleteBehavior.ClientSetNull)
               );
    }

    public void Configure(EntityTypeBuilder<PuebloIndigena> builder)
    {
        builder.ToTable("PUEBLO_INDIGENA");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(100).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<EtniaPuebloIndigena> builder)
    {
        builder.ToTable("ETNIA_PUEBLO_INDIGENA");
        builder.HasKey(e => new { e.EtniaId, e.PuebloIndigenaId });
        builder.Property(e => e.EtniaId).HasColumnName("etnia_id");
        builder.Property(e => e.PuebloIndigenaId).HasColumnName("pueblo_indigena_id");
    }

    public void Configure(EntityTypeBuilder<TipoEntorno> builder)
    {
        builder.ToTable("TIPO_ENTORNO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<TipoNovedad> builder)
    {
        builder.ToTable("TIPO_NOVEDAD");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(80).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<PresenciaNovedad> builder)
    {
        builder.ToTable("PRESENCIA_NOVEDAD");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(50).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<TipoSupervision> builder)
    {
        builder.ToTable("TIPO_SUPERVISION");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(60).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<MotivoNoTratamiento> builder)
    {
        builder.ToTable("MOTIVO_NO_TRATAMIENTO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(120).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<Parentesco> builder)
    {
        builder.ToTable("PARENTESCO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(60).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<EventoTipo> builder)
    {
        builder.ToTable("EVENTO_TIPO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(20).IsUnicode(false);
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(120).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<FormaFarmaceutica> builder)
    {
        builder.ToTable("FORMA_FARMACEUTICA");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(80).IsUnicode(false);
    }

    public void Configure(EntityTypeBuilder<EstadoPersona> builder)
    {
        builder.ToTable("ESTADO_PERSONA");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(40).IsUnicode(false);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(200);
    }

    public void Configure(EntityTypeBuilder<Enfermedad> builder)
    {
        builder.ToTable("ENFERMEDAD");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(30).IsUnicode(false);
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(false);
    }
}
