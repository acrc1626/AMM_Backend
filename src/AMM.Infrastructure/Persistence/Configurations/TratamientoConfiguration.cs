using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class TratamientoConfiguration : IEntityTypeConfiguration<Tratamiento>
{
    public void Configure(EntityTypeBuilder<Tratamiento> builder)
    {
        builder.ToTable("TRATAMIENTO");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.SupervisadoPorId).HasColumnName("supervisado_por_id");
        builder.Property(e => e.RequiereTratamiento).HasColumnName("requiere_tratamiento").HasDefaultValue(true);
        builder.Property(e => e.MotivoNoTratamientoId).HasColumnName("motivo_no_tratamiento_id");
        builder.Property(e => e.Observacion).HasColumnName("observacion").HasMaxLength(500);
        builder.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.SupervisadoPor).WithMany().HasForeignKey(d => d.SupervisadoPorId);
        builder.HasOne(d => d.MotivoNoTratamiento).WithMany().HasForeignKey(d => d.MotivoNoTratamientoId);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class TratamientoDetalleConfiguration : IEntityTypeConfiguration<TratamientoDetalle>
{
    public void Configure(EntityTypeBuilder<TratamientoDetalle> builder)
    {
        builder.ToTable("TRATAMIENTO_DETALLE");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.TratamientoId).HasColumnName("tratamiento_id");
        builder.Property(e => e.MedicamentoId).HasColumnName("medicamento_id");
        builder.Property(e => e.Dosis).HasColumnName("dosis").HasMaxLength(40).IsUnicode(false);
        builder.Property(e => e.Via).HasColumnName("via").HasMaxLength(40).IsUnicode(false);
        builder.Property(e => e.FormaFarmaceuticaId).HasColumnName("forma_farmaceutica_id");
        builder.Property(e => e.SupervisionId).HasColumnName("supervision_id");

        // Relationships
        builder.HasOne(d => d.Tratamiento).WithMany(p => p.Detalles).HasForeignKey(d => d.TratamientoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Medicamento).WithMany().HasForeignKey(d => d.MedicamentoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.FormaFarmaceutica).WithMany().HasForeignKey(d => d.FormaFarmaceuticaId);
        builder.HasOne(d => d.Supervision).WithMany().HasForeignKey(d => d.SupervisionId);
    }
}

public class PrmConfiguration : IEntityTypeConfiguration<Prm>
{
    public void Configure(EntityTypeBuilder<Prm> builder)
    {
        builder.ToTable("PRM");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.TratamientoId).HasColumnName("tratamiento_id");
        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");
        builder.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(500);
        builder.Property(e => e.Severidad).HasColumnName("severidad");
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");

        // Relationships
        builder.HasOne(d => d.Tratamiento).WithMany().HasForeignKey(d => d.TratamientoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class RafaConfiguration : IEntityTypeConfiguration<Rafa>
{
    public void Configure(EntityTypeBuilder<Rafa> builder)
    {
        builder.ToTable("RAFA");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.TratamientoId).HasColumnName("tratamiento_id");
        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");
        builder.Property(e => e.FechaEvento).HasColumnName("fecha_evento").HasColumnType("date");
        builder.Property(e => e.Severidad).HasColumnName("severidad");
        builder.Property(e => e.Sintomas).HasColumnName("sintomas").HasMaxLength(500);
        builder.Property(e => e.EnlaceReporteInvima).HasColumnName("enlace_reporte_invima").HasMaxLength(300).IsUnicode(false);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");

        // Relationships
        builder.HasOne(d => d.Tratamiento).WithMany().HasForeignKey(d => d.TratamientoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class MedicamentoConfiguration : IEntityTypeConfiguration<Medicamento>
{
    public void Configure(EntityTypeBuilder<Medicamento> builder)
    {
        builder.ToTable("MEDICAMENTO");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.NombreGenerico).HasColumnName("nombre_generico").HasMaxLength(150);
        builder.Property(e => e.FormaFarmaceuticaId).HasColumnName("forma_farmaceutica_id");
        builder.Property(e => e.Concentracion).HasColumnName("concentracion").HasMaxLength(60).IsUnicode(false);
        builder.Property(e => e.CodigoCum).HasColumnName("codigo_cum").HasMaxLength(50).IsUnicode(false);

        builder.HasOne(d => d.FormaFarmaceutica).WithMany().HasForeignKey(d => d.FormaFarmaceuticaId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}
