using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        builder.ToTable("DEPARTAMENTO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.CodigoDane).HasColumnName("codigoDANE").HasMaxLength(5).IsUnicode(false);
        builder.Property(e => e.Nombre).HasColumnName("departamento").HasMaxLength(100).IsUnicode(false);
    }
}

public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
{
    public void Configure(EntityTypeBuilder<Municipio> builder)
    {
        builder.ToTable("MUNICIPIO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.DepartamentoId).HasColumnName("departamento_id");
        builder.Property(e => e.CodigoDaneDpto).HasColumnName("codigoDANE_Dpto").HasMaxLength(10).IsUnicode(false);
        builder.Property(e => e.Nombre).HasColumnName("municipio").HasMaxLength(120).IsUnicode(false);

        builder.HasOne(d => d.Departamento).WithMany().HasForeignKey(d => d.DepartamentoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class TerritorioConfiguration : IEntityTypeConfiguration<Territorio>
{
    public void Configure(EntityTypeBuilder<Territorio> builder)
    {
        builder.ToTable("TERRITORIO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.MunicipioId).HasColumnName("municipio_id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(120).IsUnicode(false);

        builder.HasOne(d => d.Municipio).WithMany().HasForeignKey(d => d.MunicipioId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class MicroterritorioConfiguration : IEntityTypeConfiguration<Microterritorio>
{
    public void Configure(EntityTypeBuilder<Microterritorio> builder)
    {
        builder.ToTable("MICROTERRITORIO");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.TerritorioId).HasColumnName("territorio_id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(120).IsUnicode(false);

        builder.HasOne(d => d.Territorio).WithMany().HasForeignKey(d => d.TerritorioId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("AREA");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.MicroterritorioId).HasColumnName("microterritorio_id");
        builder.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(120).IsUnicode(false);

        builder.HasOne(d => d.Microterritorio).WithMany().HasForeignKey(d => d.MicroterritorioId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class UbicacionConfiguration : IEntityTypeConfiguration<Ubicacion>
{
    public void Configure(EntityTypeBuilder<Ubicacion> builder)
    {
        builder.ToTable("UBICACION");
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Direccion).HasColumnName("direccion").HasMaxLength(200);
        builder.Property(e => e.Lat).HasColumnName("lat");
        builder.Property(e => e.Lon).HasColumnName("lon");
        // builder.Property(e => e.Geo).HasColumnName("geo"); // Geography type requires NetTopologySuite, skipping for now or mapping as object/ignored if not used yet.
        builder.Ignore(e => e.Geo); 
    }
}
