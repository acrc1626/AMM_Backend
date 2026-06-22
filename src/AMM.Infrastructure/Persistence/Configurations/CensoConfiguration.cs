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
        builder.Property(e => e.UbicacionId).HasColumnName("ubicacion_id");
        builder.Property(e => e.DepartamentoId).HasColumnName("departamento_id");
        builder.Property(e => e.MunicipioId).HasColumnName("municipio_id");
        builder.Property(e => e.Fecha).HasColumnName("fecha").HasColumnType("date");
        builder.Property(e => e.Observacion).HasColumnName("observacion").HasMaxLength(500);
        builder.Property(e => e.Direccion).HasColumnName("direccion").HasMaxLength(300);
        builder.Property(e => e.ObjetoInterventor).HasColumnName("objeto_interventor").HasMaxLength(200).IsUnicode(false);
        builder.Property(e => e.Territorio).HasColumnName("territorio").HasMaxLength(200).IsUnicode(false);
        builder.Property(e => e.Microterritorio).HasColumnName("microterritorio").HasMaxLength(200).IsUnicode(false);
        builder.Property(e => e.Area).HasColumnName("area").HasMaxLength(200).IsUnicode(false);
        builder.Property(e => e.Geolocalizacion).HasColumnName("geolocalizacion").HasMaxLength(100).IsUnicode(false);
        builder.Property(e => e.TotalMiembros).HasColumnName("total_miembros").HasColumnType("int");
        builder.Property(e => e.VisitantesTemporalesCol).HasColumnName("visitantes_temporales_col").HasColumnType("int");
        builder.Property(e => e.VisitantesTemporalesMig).HasColumnName("visitantes_temporales_mig").HasColumnType("int");

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.TipoEntorno).WithMany().HasForeignKey(d => d.TipoEntornoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Ubicacion).WithMany().HasForeignKey(d => d.UbicacionId);
        builder.HasOne(d => d.Departamento).WithMany().HasForeignKey(d => d.DepartamentoId);
        builder.HasOne(d => d.Municipio).WithMany().HasForeignKey(d => d.MunicipioId);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class CensoPersonaConfiguration : IEntityTypeConfiguration<CensoPersona>
{
    public void Configure(EntityTypeBuilder<CensoPersona> builder)
    {
        builder.ToTable("CENSO_PERSONA");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.CensoId).HasColumnName("censo_id");
        builder.Property(e => e.PacienteId).HasColumnName("paciente_id");
        builder.Property(e => e.ParentescoId).HasColumnName("parentesco_id");
        builder.Property(e => e.EsJefeHogar).HasColumnName("es_jefe_hogar").HasDefaultValue(false);
        builder.Property(e => e.Comunidad).HasColumnName("comunidad").HasMaxLength(200);
        builder.Property(e => e.GrupoPoblacionEspecial).HasColumnName("grupo_poblacion_especial").HasMaxLength(200);
        builder.Property(e => e.EstadoPersonaId).HasColumnName("estado_persona_id");

        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);

        // Relationships
        builder.HasOne(d => d.Censo).WithMany(p => p.Personas).HasForeignKey(d => d.CensoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Parentesco).WithMany().HasForeignKey(d => d.ParentescoId);
        builder.HasOne(d => d.EstadoPersona).WithMany().HasForeignKey(d => d.EstadoPersonaId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class PersonaEnfermedadConfiguration : IEntityTypeConfiguration<PersonaEnfermedad>
{
    public void Configure(EntityTypeBuilder<PersonaEnfermedad> builder)
    {
        builder.ToTable("PERSONA_ENFERMEDAD");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.CensoPersonaId).HasColumnName("censo_persona_id");
        builder.Property(e => e.EnfermedadId).HasColumnName("enfermedad_id");
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");

        // Relationships
        builder.HasOne(d => d.CensoPersona).WithMany(p => p.Enfermedades).HasForeignKey(d => d.CensoPersonaId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Enfermedad).WithMany().HasForeignKey(d => d.EnfermedadId).OnDelete(DeleteBehavior.ClientSetNull);
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
