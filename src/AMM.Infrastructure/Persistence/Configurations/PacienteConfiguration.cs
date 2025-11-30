using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AMM.Infrastructure.Persistence.Configurations;

public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
{
    public void Configure(EntityTypeBuilder<Paciente> builder)
    {
        builder.ToTable("PACIENTE");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.TipoDocumentoId).HasColumnName("tipo_documento_id");
        builder.Property(e => e.Documento).HasColumnName("documento").HasMaxLength(30).IsUnicode(false);
        builder.Property(e => e.PrimerNombre).HasColumnName("primer_nombre").HasMaxLength(80);
        builder.Property(e => e.SegundoNombre).HasColumnName("segundo_nombre").HasMaxLength(80);
        builder.Property(e => e.PrimerApellido).HasColumnName("primer_apellido").HasMaxLength(80);
        builder.Property(e => e.SegundoApellido).HasColumnName("segundo_apellido").HasMaxLength(80);
        builder.Property(e => e.SexoId).HasColumnName("sexo_id");
        builder.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento").HasColumnType("date");
        builder.Property(e => e.PertenenciaEtnicaId).HasColumnName("pertenencia_etnica_id");
        builder.Property(e => e.PuebloIndigenaId).HasColumnName("pueblo_indigena_id");
        
        // Auditable fields
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.TipoDocumento).WithMany().HasForeignKey(d => d.TipoDocumentoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Sexo).WithMany().HasForeignKey(d => d.SexoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.PertenenciaEtnica).WithMany().HasForeignKey(d => d.PertenenciaEtnicaId);
        builder.HasOne(d => d.PuebloIndigena).WithMany().HasForeignKey(d => d.PuebloIndigenaId);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}
