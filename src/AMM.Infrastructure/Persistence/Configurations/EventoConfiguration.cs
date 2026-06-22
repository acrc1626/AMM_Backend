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
        builder.Property(e => e.EventoTipoId).HasColumnName("tipo_id");
        builder.Property(e => e.Fecha).HasColumnName("fecha_evento").HasColumnType("date");

        // Auditable
        builder.Property(e => e.CreadoEn).HasColumnName("creado_en").HasColumnType("datetime2(0)").HasDefaultValueSql("sysutcdatetime()");
        builder.Property(e => e.CreadoPor).HasColumnName("creado_por").HasMaxLength(100);
        builder.Property(e => e.ModificadoEn).HasColumnName("modificado_en").HasColumnType("datetime2(0)");
        builder.Property(e => e.ModificadoPor).HasColumnName("modificado_por").HasMaxLength(100);
        builder.Property(e => e.EstadoId).HasColumnName("estado_id").HasDefaultValue((byte)1);

        // Relationships
        builder.HasOne(d => d.Paciente).WithMany().HasForeignKey(d => d.PacienteId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.EventoTipo).WithMany().HasForeignKey(d => d.EventoTipoId).OnDelete(DeleteBehavior.ClientSetNull);
        builder.HasOne(d => d.Estado).WithMany().HasForeignKey(d => d.EstadoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoEscabiosisConfiguration : IEntityTypeConfiguration<EventoEscabiosis>
{
    public void Configure(EntityTypeBuilder<EventoEscabiosis> builder)
    {
        builder.ToTable("EVENTO_ESCABIOSIS");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.Severidad).HasColumnName("severidad");
        builder.Property(e => e.Localizacion).HasColumnName("localizacion").HasMaxLength(200);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoGeohelmintiasisConfiguration : IEntityTypeConfiguration<EventoGeohelmintiasis>
{
    public void Configure(EntityTypeBuilder<EventoGeohelmintiasis> builder)
    {
        builder.ToTable("EVENTO_GEOHELMINTIASIS");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.TipoPrueba).HasColumnName("tipo_prueba").HasMaxLength(80).IsUnicode(false);
        builder.Property(e => e.Resultado).HasColumnName("resultado").HasMaxLength(80).IsUnicode(false);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoPediculosisConfiguration : IEntityTypeConfiguration<EventoPediculosis>
{
    public void Configure(EntityTypeBuilder<EventoPediculosis> builder)
    {
        builder.ToTable("EVENTO_PEDICULOSIS");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.Grado).HasColumnName("grado");

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoPianConfiguration : IEntityTypeConfiguration<EventoPian>
{
    public void Configure(EntityTypeBuilder<EventoPian> builder)
    {
        builder.ToTable("EVENTO_PIAN");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.Estadio).HasColumnName("estadio").HasMaxLength(50).IsUnicode(false);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoTeniasisCisticercosisConfiguration : IEntityTypeConfiguration<EventoTeniasisCisticercosis>
{
    public void Configure(EntityTypeBuilder<EventoTeniasisCisticercosis> builder)
    {
        builder.ToTable("EVENTO_TENIASIS_CISTICERCOSIS");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.Teniasis).HasColumnName("teniasis");
        builder.Property(e => e.Cisticercosis).HasColumnName("cisticercosis");

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoTracomaConfiguration : IEntityTypeConfiguration<EventoTracoma>
{
    public void Configure(EntityTypeBuilder<EventoTracoma> builder)
    {
        builder.ToTable("EVENTO_TRACOMA");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.CriterioExclusionId).HasColumnName("criterio_exclusion_id");
        builder.Property(e => e.ExaminadoTt).HasColumnName("examinado_tt");
        builder.Property(e => e.Triquiasis).HasColumnName("triquiasis");
        builder.Property(e => e.OpacidadCorneal).HasColumnName("opacidad_corneal").HasMaxLength(50).IsUnicode(false);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoTungiasisConfiguration : IEntityTypeConfiguration<EventoTungiasis>
{
    public void Configure(EntityTypeBuilder<EventoTungiasis> builder)
    {
        builder.ToTable("EVENTO_TUNGIASIS");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.NumeroLesiones).HasColumnName("numero_lesiones");
        builder.Property(e => e.Complicaciones).HasColumnName("complicaciones").HasMaxLength(200);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoMalariaConfiguration : IEntityTypeConfiguration<EventoMalaria>
{
    public void Configure(EntityTypeBuilder<EventoMalaria> builder)
    {
        builder.ToTable("EVENTO_MALARIA");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.Especie).HasColumnName("especie").HasMaxLength(100);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoTuberculosisConfiguration : IEntityTypeConfiguration<EventoTuberculosis>
{
    public void Configure(EntityTypeBuilder<EventoTuberculosis> builder)
    {
        builder.ToTable("EVENTO_TUBERCULOSIS");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.TipoTuberculosis).HasColumnName("tipo_tuberculosis").HasMaxLength(100);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoTuberculosisContactoConfiguration : IEntityTypeConfiguration<EventoTuberculosisContacto>
{
    public void Configure(EntityTypeBuilder<EventoTuberculosisContacto> builder)
    {
        builder.ToTable("EVENTO_TUBERCULOSIS_CONTACTO");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.Observacion).HasColumnName("observacion").HasMaxLength(500);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}

public class EventoLeshmaniasisCutaneaConfiguration : IEntityTypeConfiguration<EventoLeshmaniasisCutanea>
{
    public void Configure(EntityTypeBuilder<EventoLeshmaniasisCutanea> builder)
    {
        builder.ToTable("EVENTO_LEISHMANIASIS_CUTANEA");

        builder.HasKey(e => e.EventoId);
        builder.Property(e => e.EventoId).HasColumnName("evento_id");
        builder.Property(e => e.TipoLesion).HasColumnName("tipo_lesion").HasMaxLength(100);

        builder.HasOne(d => d.Evento).WithMany().HasForeignKey(d => d.EventoId).OnDelete(DeleteBehavior.ClientSetNull);
    }
}
