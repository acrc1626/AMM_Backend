using AMM.Domain.Common;
using AMM.Domain.Entities;
using FluentAssertions;

namespace AMM.Tests.Unit.Domain;

// ─────────────────────────────────────────────────────────────────────────────
// Tests de entidades de dominio puras (POCO).
// Ninguno necesita mocks — verifican que las propiedades se almacenan bien
// y que las invariantes de AuditableEntity están disponibles.
// ─────────────────────────────────────────────────────────────────────────────

public class CatalogoEntidadesTests
{
    [Fact]
    public void Estado_SetProperties_ValuesAreStored()
    {
        // Given / When
        var e = new Estado { Id = 1, Nombre = "Activo", Descripcion = "Estado activo del sistema" };
        // Then
        e.Id.Should().Be(1);
        e.Nombre.Should().Be("Activo");
        e.Descripcion.Should().Be("Estado activo del sistema");
    }

    [Fact]
    public void EstadoUsuario_SetProperties_ValuesAreStored()
    {
        var e = new EstadoUsuario { Id = 2, Nombre = "Bloqueado", Descripcion = null };
        e.Id.Should().Be(2);
        e.Nombre.Should().Be("Bloqueado");
        e.Descripcion.Should().BeNull();
    }

    [Fact]
    public void TipoDocumento_SetProperties_ValuesAreStored()
    {
        var e = new TipoDocumento { Id = 1, Descripcion = "Cédula de ciudadanía" };
        e.Id.Should().Be(1);
        e.Descripcion.Should().Be("Cédula de ciudadanía");
    }

    [Fact]
    public void Sexo_SetProperties_ValuesAreStored()
    {
        var e = new Sexo { Id = 1, Descripcion = "Masculino" };
        e.Id.Should().Be(1);
        e.Descripcion.Should().Be("Masculino");
    }

    [Fact]
    public void Etnia_SetProperties_ValuesAreStored()
    {
        var e = new Etnia { Id = 3, Descripcion = "Afrocolombiano", Codigo = "AFC" };
        e.Id.Should().Be(3);
        e.Descripcion.Should().Be("Afrocolombiano");
        e.Codigo.Should().Be("AFC");
    }

    [Fact]
    public void PuebloIndigena_SetProperties_ValuesAreStored()
    {
        var e = new PuebloIndigena { Id = 100, Descripcion = "Wayuu" };
        e.Id.Should().Be(100);
        e.Descripcion.Should().Be("Wayuu");
    }

    [Fact]
    public void TipoEntorno_SetProperties_ValuesAreStored()
    {
        var e = new TipoEntorno { Id = 1, Descripcion = "Urbano" };
        e.Id.Should().Be(1);
        e.Descripcion.Should().Be("Urbano");
    }

    [Fact]
    public void TipoNovedad_SetProperties_ValuesAreStored()
    {
        var e = new TipoNovedad { Id = 2, Descripcion = "Reinfección" };
        e.Id.Should().Be(2);
        e.Descripcion.Should().Be("Reinfección");
    }

    [Fact]
    public void PresenciaNovedad_SetProperties_ValuesAreStored()
    {
        var e = new PresenciaNovedad { Id = 1, Descripcion = "Positivo" };
        e.Id.Should().Be(1);
        e.Descripcion.Should().Be("Positivo");
    }

    [Fact]
    public void TipoSupervision_SetProperties_ValuesAreStored()
    {
        var e = new TipoSupervision { Id = 1, Descripcion = "Directamente observado" };
        e.Id.Should().Be(1);
        e.Descripcion.Should().Be("Directamente observado");
    }

    [Fact]
    public void MotivoNoTratamiento_SetProperties_ValuesAreStored()
    {
        var e = new MotivoNoTratamiento { Id = 10, Descripcion = "Rechazo del paciente" };
        e.Id.Should().Be(10);
        e.Descripcion.Should().Be("Rechazo del paciente");
    }

    [Fact]
    public void Parentesco_SetProperties_ValuesAreStored()
    {
        var e = new Parentesco { Id = 3, Descripcion = "Hijo/a" };
        e.Id.Should().Be(3);
        e.Descripcion.Should().Be("Hijo/a");
    }

    [Fact]
    public void EventoTipo_SetProperties_ValuesAreStored()
    {
        var e = new EventoTipo { Id = 5, Codigo = "TB", Nombre = "Tuberculosis" };
        e.Id.Should().Be(5);
        e.Codigo.Should().Be("TB");
        e.Nombre.Should().Be("Tuberculosis");
    }
}

public class UbicacionGeograficaEntidadesTests
{
    [Fact]
    public void Departamento_SetProperties_ValuesAreStored()
    {
        var d = new Departamento { Id = 5, CodigoDane = "05", Nombre = "Antioquia" };
        d.Id.Should().Be(5);
        d.CodigoDane.Should().Be("05");
        d.Nombre.Should().Be("Antioquia");
    }

    [Fact]
    public void Municipio_SetPropertiesAndNavigation_ValuesAreStored()
    {
        var depto = new Departamento { Id = 5, Nombre = "Antioquia" };
        var m = new Municipio
        {
            Id = 5001, DepartamentoId = 5,
            CodigoDaneDpto = "05", Nombre = "Medellín",
            Departamento = depto
        };
        m.Id.Should().Be(5001);
        m.Nombre.Should().Be("Medellín");
        m.Departamento.Nombre.Should().Be("Antioquia");
    }

    [Fact]
    public void Territorio_SetPropertiesAndNavigation_ValuesAreStored()
    {
        var municipio = new Municipio { Id = 5001, Nombre = "Medellín" };
        var t = new Territorio { Id = 1, MunicipioId = 5001, Nombre = "Zona Norte", Municipio = municipio };
        t.Id.Should().Be(1);
        t.Nombre.Should().Be("Zona Norte");
        t.Municipio.Nombre.Should().Be("Medellín");
    }

    [Fact]
    public void Microterritorio_SetPropertiesAndNavigation_ValuesAreStored()
    {
        var territorio = new Territorio { Id = 1, Nombre = "Zona Norte" };
        var m = new Microterritorio { Id = 10, TerritorioId = 1, Nombre = "Barrio Centro", Territorio = territorio };
        m.Id.Should().Be(10);
        m.Nombre.Should().Be("Barrio Centro");
        m.Territorio.Nombre.Should().Be("Zona Norte");
    }

    [Fact]
    public void Area_SetPropertiesAndNavigation_ValuesAreStored()
    {
        var micro = new Microterritorio { Id = 10, Nombre = "Barrio Centro" };
        var a = new Area { Id = 100, MicroterritorioId = 10, Nombre = "Sector 1", Microterritorio = micro };
        a.Id.Should().Be(100);
        a.Nombre.Should().Be("Sector 1");
        a.Microterritorio.Nombre.Should().Be("Barrio Centro");
    }

    [Fact]
    public void Ubicacion_SetAllProperties_ValuesAreStored()
    {
        var u = new Ubicacion { Id = 1L, Direccion = "Cra 10 # 20-30", Lat = 6.25, Lon = -75.56, Geo = null };
        u.Id.Should().Be(1L);
        u.Direccion.Should().Be("Cra 10 # 20-30");
        u.Lat.Should().Be(6.25);
        u.Lon.Should().Be(-75.56);
        u.Geo.Should().BeNull();
    }
}

public class TratamientoEntidadesTests
{
    private static readonly DateTime _now = new(2025, 6, 1, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void FormaFarmaceutica_SetProperties_ValuesAreStored()
    {
        var f = new FormaFarmaceutica { Id = 1, Nombre = "Tableta" };
        f.Id.Should().Be(1);
        f.Nombre.Should().Be("Tableta");
    }

    [Fact]
    public void Medicamento_SetAllProperties_ValuesAreStored()
    {
        var forma = new FormaFarmaceutica { Id = 1, Nombre = "Tableta" };
        var m = new Medicamento
        {
            Id = 1, NombreGenerico = "Rifampicina",
            FormaFarmaceuticaId = 1, Concentracion = "300 mg",
            CodigoCum = "123456", FormaFarmaceutica = forma
        };
        m.NombreGenerico.Should().Be("Rifampicina");
        m.Concentracion.Should().Be("300 mg");
        m.FormaFarmaceutica.Nombre.Should().Be("Tableta");
    }

    [Fact]
    public void Tratamiento_SetAllProperties_AuditFieldsIncluded()
    {
        var t = new Tratamiento
        {
            Id                   = 1L,
            EventoId             = 10L,
            RequiereTratamiento  = true,
            MotivoNoTratamientoId = null,
            Observacion          = "Sin contraindicaciones",
            Fecha                = _now,
            CreadoEn             = _now,
            CreadoPor            = "admin",
            EstadoId             = 1
        };
        t.Id.Should().Be(1L);
        t.RequiereTratamiento.Should().BeTrue();
        t.Observacion.Should().Be("Sin contraindicaciones");
        t.CreadoPor.Should().Be("admin");
        t.Detalles.Should().BeEmpty();
    }

    [Fact]
    public void TratamientoDetalle_SetAllProperties_ValuesAreStored()
    {
        var td = new TratamientoDetalle
        {
            Id                 = 1L,
            TratamientoId      = 10L,
            MedicamentoId      = 5,
            Dosis              = "1 tableta diaria",
            Via                = "Oral",
            FormaFarmaceuticaId = 1,
            SupervisionId      = 1
        };
        td.Dosis.Should().Be("1 tableta diaria");
        td.Via.Should().Be("Oral");
    }

    [Fact]
    public void Prm_SetAllProperties_ValuesAreStored()
    {
        var prm = new Prm
        {
            Id             = 1L,
            TratamientoId  = 5L,
            PacienteId     = 10L,
            Descripcion    = "Reacción adversa leve",
            Severidad      = 2,
            CreadoEn       = _now
        };
        prm.Descripcion.Should().Be("Reacción adversa leve");
        prm.Severidad.Should().Be(2);
        prm.CreadoEn.Should().Be(_now);
    }

    [Fact]
    public void Rafa_SetAllProperties_ValuesAreStored()
    {
        var rafa = new Rafa
        {
            Id                   = 1L,
            TratamientoId        = 5L,
            PacienteId           = 10L,
            FechaEvento          = _now,
            Severidad            = 3,
            Sintomas             = "Ictericia, náuseas",
            EnlaceReporteInvima  = "https://invima.gov.co/rafa/123",
            CreadoEn             = _now
        };
        rafa.Sintomas.Should().Be("Ictericia, náuseas");
        rafa.Severidad.Should().Be(3);
        rafa.EnlaceReporteInvima.Should().Contain("invima");
    }
}

public class CensoNovedadEntityTests
{
    [Fact]
    public void CensoNovedad_SetAllProperties_AuditFieldsIncluded()
    {
        // Given / When – entidad con todos los campos asignados
        var cn = new CensoNovedad
        {
            Id                 = 5L,
            CensoId            = 10L,
            PacienteId         = 42L,
            TipoNovedadId      = 2,
            PresenciaNovedadId = 1,
            TratamientoId      = 100L,
            Observacion        = "Observación de novedad",
            Fecha              = new DateTime(2025, 4, 1),
            CreadoEn           = DateTime.UtcNow,
            CreadoPor          = "admin@ins.gov.co",
            EstadoId           = 1
        };

        // Then
        cn.Id.Should().Be(5L);
        cn.CensoId.Should().Be(10L);
        cn.PacienteId.Should().Be(42L);
        cn.TipoNovedadId.Should().Be(2);
        cn.PresenciaNovedadId.Should().Be(1);
        cn.Observacion.Should().Be("Observación de novedad");
        cn.CreadoPor.Should().Be("admin@ins.gov.co");
    }

    [Fact]
    public void CensoNovedad_WithNullOptionalFields_IsValid()
    {
        // Given – campos opcionales sin asignar
        var cn = new CensoNovedad
        {
            Id            = 1L,
            CensoId       = 2L,
            TipoNovedadId = 1,
            Fecha         = new DateTime(2025, 1, 15),
            EstadoId      = 1
        };

        // Then – campos opcionales son null por defecto
        cn.PacienteId.Should().BeNull();
        cn.PresenciaNovedadId.Should().BeNull();
        cn.TratamientoId.Should().BeNull();
        cn.Observacion.Should().BeNull();
        cn.Paciente.Should().BeNull();
        cn.PresenciaNovedad.Should().BeNull();
    }

    [Fact]
    public void CensoNovedad_WithNavigationProperties_ReturnsCorrectReferences()
    {
        // Given – se asignan propiedades de navegación
        var novedad = new TipoNovedad { Id = 1, Descripcion = "Desparasitación" };
        var presencia = new PresenciaNovedad { Id = 1, Descripcion = "Positivo" };

        var cn = new CensoNovedad
        {
            Id            = 1L,
            CensoId       = 2L,
            TipoNovedadId = 1,
            TipoNovedad   = novedad,
            PresenciaNovedad = presencia,
            Fecha         = DateTime.Today,
            EstadoId      = 1
        };

        // Then – las nav props devuelven las instancias asignadas
        cn.TipoNovedad.Descripcion.Should().Be("Desparasitación");
        cn.PresenciaNovedad!.Descripcion.Should().Be("Positivo");
    }
}

public class SeguridadEntidadesTests
{
    [Fact]
    public void Menu_SetProperties_ValuesAreStored()
    {
        var m = new Menu { Id = 1, Nombre = "Censos", Ruta = "/censos" };
        m.Id.Should().Be(1);
        m.Nombre.Should().Be("Censos");
        m.Ruta.Should().Be("/censos");
        m.Roles.Should().BeEmpty();
    }

    [Fact]
    public void Permiso_SetProperties_ValuesAreStored()
    {
        var p = new Permiso { Id = 1, Codigo = "censos.crear", Descripcion = "Crear censos" };
        p.Id.Should().Be(1);
        p.Codigo.Should().Be("censos.crear");
        p.Descripcion.Should().Be("Crear censos");
        p.Roles.Should().BeEmpty();
    }
}

public class AuditableEntityTests
{
    [Fact]
    public void AuditableEntity_AllAuditFields_CanBeSetAndRead()
    {
        // Given – entidad auditable con todos sus campos de auditoría
        var fecha = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var paciente = new Paciente
        {
            Id             = 1L,
            Documento      = "123",
            PrimerNombre   = "Juan",
            PrimerApellido = "Pérez",
            SexoId         = 1,
            TipoDocumentoId = 1,
            CreadoEn       = fecha,
            CreadoPor      = "admin",
            ModificadoEn   = fecha.AddDays(1),
            ModificadoPor  = "editor",
            EstadoId       = 1
        };

        // Then – todos los campos de auditoría son accesibles
        paciente.CreadoEn.Should().Be(fecha);
        paciente.CreadoPor.Should().Be("admin");
        paciente.ModificadoEn.Should().Be(fecha.AddDays(1));
        paciente.ModificadoPor.Should().Be("editor");
        paciente.EstadoId.Should().Be(1);
    }
}
