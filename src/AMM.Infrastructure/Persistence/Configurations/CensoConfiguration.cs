using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class CensoConfiguration : IEntityTypeConfiguration<Censo>
{
    public void Configure(EntityTypeBuilder<Censo> builder)
    {
        builder.ToTable("CENSO");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.TipoEntornoId).HasColumnName("tipo_entorno_id");
        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");
        builder.Property(e => e.UbicacionId).HasColumnName("ubicacion_id");
        builder.Property(e => e.DepartamentoId).HasColumnName("departamento_id");
        builder.Property(e => e.MunicipioId).HasColumnName("municipio_id");
        builder.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");
        builder.Property(e => e.Observacion).HasColumnName("observacion").HasMaxLength(500);

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.TipoEntorno).WithMany().HasForeignKey(d => d.TipoEntornoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId);
        builder.HasOne(d => d.Ubicacion).WithMany().HasForeignKey(d => d.UbicacionId);
        builder.HasOne(d => d.Departamento).WithMany().HasForeignKey(d => d.DepartamentoId);
        builder.HasOne(d => d.Municipio).WithMany().HasForeignKey(d => d.MunicipioId);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class CensoNovedadConfiguration : IEntityTypeConfiguration<CensoNovedad>
{
    public void Configure(EntityTypeBuilder<CensoNovedad> builder)
    {
        builder.ToTable("CENSO_NOVEDAD");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.CensoId).HasColumnName("censo_id");
        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");
        builder.Property(e => e.TipoNovedadId).HasColumnName("tipo_novedad_id");
        builder.Property(e => e.PresenciaNovedadId).HasColumnName("presencia_novedad_id");
        builder.Property(e => e.TratamientoId).HasColumnName("tratamiento_id");
        builder.Property(e => e.Observacion).HasColumnName("observacion").HasMaxLength(500);
        builder.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.Censo).WithMany(p => p.Novedades).HasForeignKey(d => d.CensoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId);
        builder.HasOne(d => d.TipoNovedad).WithMany().HasForeignKey(d => d.TipoNovedadId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.PresenciaNovedad).WithMany().HasForeignKey(d => d.PresenciaNovedadId);
        builder.HasOne(d => d.Tratamiento).WithMany().HasForeignKey(d => d.TratamientoId);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}
