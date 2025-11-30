using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class EventoConfiguration : IEntityTypeConfiguration<Evento>
{
    public void Configure(EntityTypeBuilder<Evento> builder)
    {
        builder.ToTable("EVENTO");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");
        builder.Property(e => e.EventoTipoId).HasColumnName("evento_tipo_id");
        builder.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");
        builder.Property(e => e.Observacion).HasColumnName("observacion").HasMaxLength(500);

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId);
        builder.HasOne(d => d.EventoTipo).WithMany().HasForeignKey(d => d.EventoTipoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EscabiosisConfiguration : IEntityTypeConfiguration<Escabiosis>
{
    public void Configure(EntityTypeBuilder<Escabiosis> builder)
    {
        builder.ToTable("ESCABIOSIS");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.Tratado).HasColumnName("tratado");
        builder.Property(e => e.CierreControl).HasColumnName("cierre_control");
    }
}

public class GeohelmintiasisConfiguration : IEntityTypeConfiguration<Geohelmintiasis>
{
    public void Configure(EntityTypeBuilder<Geohelmintiasis> builder)
    {
        builder.ToTable("GEOHELMINTIASIS");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.Tratado).HasColumnName("tratado");
        builder.Property(e => e.CierreControl).HasColumnName("cierre_control");
        builder.Property(e => e.Laboratorio).HasColumnName("laboratorio");
    }
}

public class PediculosisConfiguration : IEntityTypeConfiguration<Pediculosis>
{
    public void Configure(EntityTypeBuilder<Pediculosis> builder)
    {
        builder.ToTable("PEDICULOSIS");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.Tratado).HasColumnName("tratado");
        builder.Property(e => e.CierreControl).HasColumnName("cierre_control");
    }
}

public class MalariaConfiguration : IEntityTypeConfiguration<Malaria>
{
    public void Configure(EntityTypeBuilder<Malaria> builder)
    {
        builder.ToTable("MALARIA");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.Gota).HasColumnName("gota");
        builder.Property(e => e.Resultado).HasColumnName("resultado").HasMaxLength(100);
    }
}

public class TuberculosisConfiguration : IEntityTypeConfiguration<Tuberculosis>
{
    public void Configure(EntityTypeBuilder<Tuberculosis> builder)
    {
        builder.ToTable("TUBERCULOSIS");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.Sintomatico).HasColumnName("sintomatico");
        builder.Property(e => e.Resultado).HasColumnName("resultado").HasMaxLength(100);
    }
}

public class TuberculosisContactoConfiguration : IEntityTypeConfiguration<TuberculosisContacto>
{
    public void Configure(EntityTypeBuilder<TuberculosisContacto> builder)
    {
        builder.ToTable("TUBERCULOSIS_CONTACTO");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.IndexId).HasColumnName("index_id");
        builder.Property(e => e.ParentescoId).HasColumnName("parentesco_id");

        builder.HasOne(e => e.Index).WithMany().HasForeignKey(e => e.IndexId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(e => e.Parentesco).WithMany().HasForeignKey(e => e.ParentescoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class LeshmaniasisCutaneaConfiguration : IEntityTypeConfiguration<LeshmaniasisCutanea>
{
    public void Configure(EntityTypeBuilder<LeshmaniasisCutanea> builder)
    {
        builder.ToTable("LESHMANIASIS_CUTANEA");
        builder.Property(e => e.Id).HasColumnName("evento_id");
        builder.Property(e => e.Cicatriz).HasColumnName("cicatriz");
        builder.Property(e => e.NumeroLesiones).HasColumnName("numero_lesiones");
    }
}
