using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("USUARIO");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.AzureAdObjectId).HasColumnName("azure_ad_object_id");
        builder.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(256).IsUnicode(false);
        builder.Property(e => e.NombreCompleto).HasColumnName("nombre_completo").HasMaxLength(200);
        builder.Property(e => e.EntidadId).HasColumnName("entidad_id");
        builder.Property(e => e.EstadoUsuarioId).HasColumnName("estado_usuario_id");

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);

        builder.HasOne(d => d.EstadoUsuario).WithMany().HasForeignKey(d => d.EstadoUsuarioId).OnDelete(DeleteBehavior.ClientSetNull);

        builder.HasMany(d => d.Roles).WithMany(p => p.Usuarios)
            .UsingEntity<Dictionary<string, object>>(
                "USUARIO_ROL",
                l => l.HasOne<Rol>().WithMany().HasForeignKey("rol_id").OnDelete(DeleteBehavior.ClientSetNull),
                r => r.HasOne<Usuario>().WithMany().HasForeignKey("usuario_id").OnDelete(DeleteBehavior.ClientSetNull),
                j =>
                {
                    j.HasKey("usuario_id", "rol_id");
                    j.ToTable("USUARIO_ROL");
                    j.IndexerProperty<int>("usuario_id").HasColumnName("usuario_id");
                    j.IndexerProperty<int>("rol_id").HasColumnName("rol_id");
                });
    }
}

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("ROL");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(80).IsUnicode(false);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255);

        builder.HasMany(d => d.Menus).WithMany(p => p.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "ROL_MENU",
                l => l.HasOne<Menu>().WithMany().HasForeignKey("menu_id").OnDelete(DeleteBehavior.ClientSetNull),
                r => r.HasOne<Rol>().WithMany().HasForeignKey("rol_id").OnDelete(DeleteBehavior.ClientSetNull),
                j =>
                {
                    j.HasKey("rol_id", "menu_id");
                    j.ToTable("ROL_MENU");
                    j.IndexerProperty<int>("rol_id").HasColumnName("rol_id");
                    j.IndexerProperty<int>("menu_id").HasColumnName("menu_id");
                });

        builder.HasMany(d => d.Permisos).WithMany(p => p.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "ROL_PERMISO",
                l => l.HasOne<Permiso>().WithMany().HasForeignKey("permiso_id").OnDelete(DeleteBehavior.ClientSetNull),
                r => r.HasOne<Rol>().WithMany().HasForeignKey("rol_id").OnDelete(DeleteBehavior.ClientSetNull),
                j =>
                {
                    j.HasKey("rol_id", "permiso_id");
                    j.ToTable("ROL_PERMISO");
                    j.IndexerProperty<int>("rol_id").HasColumnName("rol_id");
                    j.IndexerProperty<int>("permiso_id").HasColumnName("permiso_id");
                });
    }
}

public class PermisoConfiguration : IEntityTypeConfiguration<Permiso>
{
    public void Configure(EntityTypeBuilder<Permiso> builder)
    {
        builder.ToTable("PERMISO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Codigo).HasColumnName("codigo").HasMaxLength(100).IsUnicode(false);
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
    }
}

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.ToTable("MENU");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(80).IsUnicode(false);
        builder.Property(e => e.Ruta).HasColumnName("ruta").HasMaxLength(200).IsUnicode(false);
    }
}
