using AMM.Application.DTOs.Censos;
using AMM.Domain.Constants;
using AMM.Domain.Entities;
using AMM.Domain.Ports;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Censos;

/// <summary>
/// Orquesta la creación de censos para los tres tipos de entorno:
/// Hogar (HU-001), Educativo (HU-002) e Institucional (HU-003).
/// Persiste el grafo completo (Censo + CensoPersonas + PersonaEnfermedades)
/// en una única transacción.
/// También expone AgregarPersonasYTratamientosAsync para el PATCH del wizard (DEF-AMM-004).
/// </summary>
public sealed class CrearCensoUseCase
{
    private readonly ICensoRepository       _censoRepository;
    private readonly ICensoNovedadRepository _censoNovedadRepository;
    private readonly IPacienteRepository    _pacienteRepository;

    /// <summary>Inicializa el caso de uso con los repositorios requeridos.</summary>
    public CrearCensoUseCase(
        ICensoRepository        censoRepository,
        ICensoNovedadRepository censoNovedadRepository,
        IPacienteRepository     pacienteRepository)
    {
        this._censoRepository        = censoRepository;
        this._censoNovedadRepository = censoNovedadRepository;
        this._pacienteRepository     = pacienteRepository;
    }

    /// <summary>
    /// Crea un censo del tipo indicado por <see cref="CrearCensoRequest.TipoEntornoId"/>.
    /// Aplica las reglas de negocio correspondientes antes de persistir:
    /// <list type="bullet">
    ///   <item>RN-001: censos Hogar requieren exactamente un Jefe de Hogar.</item>
    ///   <item>RN-005: censos Educativo e Institucional no admiten Jefe de Hogar.</item>
    ///   <item>RN-006: ParentescoId se ignora en Educativo e Institucional.</item>
    ///   <item>RN-007 / RN-010: Personas acepta cualquier cantidad (carga manual o masiva).</item>
    /// </list>
    /// </summary>
    public async Task<CensoResumenDto> CrearAsync(
        CrearCensoRequest request,
        string usuarioActual,
        CancellationToken ct = default)
    {
        this.ValidarSegunTipoEntorno(request);

        // Resolver IDs de pacientes (busca por documento → crea si no existe)
        var jefeResuelto = request.JefeHogar is not null
            ? request.JefeHogar with
              { PacienteId = await this.ResolverPacienteIdAsync(request.JefeHogar, usuarioActual, ct) }
            : null;

        var personasResueltas = new List<CensoPersonaRequest>();
        foreach (var p in request.Personas ?? [])
            personasResueltas.Add(p with
                { PacienteId = await this.ResolverPacienteIdAsync(p, usuarioActual, ct) });

        ValidarSinDuplicados(jefeResuelto, personasResueltas);

        var requestResuelto = request with { JefeHogar = jefeResuelto, Personas = personasResueltas };
        var personas = this.BuildPersonas(requestResuelto, usuarioActual);

        var censo = new Censo
        {
            TipoEntornoId           = request.TipoEntornoId,
            UbicacionId             = request.UbicacionId,
            DepartamentoId          = request.DepartamentoId,
            MunicipioId             = request.MunicipioId,
            Fecha                   = request.Fecha,
            Observacion             = request.Observacion,
            Direccion               = request.Direccion,
            ObjetoInterventor       = request.ObjetoInterventor,
            Territorio              = request.Territorio,
            Microterritorio         = request.Microterritorio,
            Area                    = request.Area,
            Geolocalizacion         = request.Geolocalizacion,
            TotalMiembros           = request.TotalMiembros,
            VisitantesTemporalesCol = request.VisitantesTemporalesCol,
            VisitantesTemporalesMig = request.VisitantesTemporalesMig,
            EstadoId                = EstadoId.Borrador,
            CreadoEn                = DateTime.UtcNow,
            CreadoPor               = usuarioActual,
            Personas                = personas
        };

        await this._censoRepository.AddAsync(censo, ct);
        await this._censoRepository.SaveChangesAsync(ct);

        return new CensoResumenDto(
            censo.Id,
            censo.TipoEntornoId,
            censo.TipoEntorno?.Descripcion ?? "",
            censo.Fecha,
            censo.EstadoId,
            censo.Estado?.Nombre ?? "",
            personas.Count);
    }

    // ─── Validaciones por tipo de entorno ───────────────────────────────────

    private void ValidarSegunTipoEntorno(CrearCensoRequest request)
    {
        if (request.TipoEntornoId == TipoEntornoId.Hogar)
        {
            this.ValidarCensoHogar(request);
        }
        else
        {
            this.ValidarCensoSinJefe(request);
        }
    }

    /// <summary>RN-001: el censo Hogar requiere exactamente un Jefe de Hogar.</summary>
    private void ValidarCensoHogar(CrearCensoRequest request)
    {
        if (request.JefeHogar is null)
            throw new ArgumentException(
                "Un censo tipo Hogar requiere un Jefe de Hogar (RN-001).");

        ValidarIdentificacionPersona(request.JefeHogar, "Jefe de Hogar");

        foreach (var persona in request.Personas ?? [])
            ValidarIdentificacionPersona(persona, "persona del censo");
    }

    /// <summary>RN-005: censos Educativo e Institucional no admiten Jefe de Hogar.</summary>
    private void ValidarCensoSinJefe(CrearCensoRequest request)
    {
        if (request.JefeHogar is not null)
            throw new ArgumentException(
                "Un censo de tipo Educativo o Institucional no admite Jefe de Hogar (RN-005).");

        foreach (var persona in request.Personas ?? [])
            ValidarIdentificacionPersona(persona, "persona del censo");
    }

    private static void ValidarIdentificacionPersona(CensoPersonaRequest req, string contexto)
    {
        if (req.PacienteId.HasValue) return;

        if (req.TipoDocumentoId is null || string.IsNullOrWhiteSpace(req.Documento))
            throw new ArgumentException(
                $"Se requiere TipoDocumentoId y Documento para identificar a la {contexto} cuando PacienteId no se proporciona.");
    }

    private static void ValidarSinDuplicados(
        CensoPersonaRequest? jefe,
        IEnumerable<CensoPersonaRequest> personas)
    {
        var ids = new List<long>();
        if (jefe is not null) ids.Add(jefe.PacienteId!.Value);
        ids.AddRange(personas.Select(p => p.PacienteId!.Value));

        if (ids.Count != ids.Distinct().Count())
            throw new ArgumentException(
                "No se puede registrar el mismo paciente más de una vez en el mismo censo.");
    }

    private async Task<long> ResolverPacienteIdAsync(
        CensoPersonaRequest req,
        string usuarioActual,
        CancellationToken ct)
    {
        if (req.PacienteId is > 0)
            return req.PacienteId.Value;

        var existente = await this._pacienteRepository
            .GetByDocumentoAsync(req.TipoDocumentoId!.Value, req.Documento!.Trim(), ct);

        if (existente is not null)
            return existente.Id;

        var nuevo = new Paciente
        {
            TipoDocumentoId = req.TipoDocumentoId!.Value,
            Documento       = req.Documento!.Trim(),
            PrimerNombre    = req.PrimerNombre    ?? throw new ArgumentException("PrimerNombre es requerido para crear un nuevo paciente."),
            SegundoNombre   = req.SegundoNombre,
            PrimerApellido  = req.PrimerApellido  ?? throw new ArgumentException("PrimerApellido es requerido para crear un nuevo paciente."),
            SegundoApellido = req.SegundoApellido,
            SexoId          = req.SexoId          ?? throw new ArgumentException("SexoId es requerido para crear un nuevo paciente."),
            FechaNacimiento = req.FechaNacimiento,
            EstadoId        = EstadoId.Activo,
            CreadoEn        = DateTime.UtcNow,
            CreadoPor       = usuarioActual
        };

        await this._pacienteRepository.AddAsync(nuevo, ct);
        await this._pacienteRepository.SaveChangesAsync(ct);

        return nuevo.Id;
    }

    // ─── Construcción del grafo ──────────────────────────────────────────────

    private List<CensoPersona> BuildPersonas(CrearCensoRequest request, string usuarioActual)
    {
        var result = new List<CensoPersona>();

        if (request.JefeHogar is not null)
            result.Add(this.BuildCensoPersona(request.JefeHogar, esJefe: true,
                aplicaParentesco: request.TipoEntornoId == TipoEntornoId.Hogar,
                usuarioActual));

        foreach (var persona in request.Personas ?? [])
            result.Add(this.BuildCensoPersona(persona, esJefe: false,
                aplicaParentesco: request.TipoEntornoId == TipoEntornoId.Hogar,
                usuarioActual));

        return result;
    }

    private CensoPersona BuildCensoPersona(
        CensoPersonaRequest req,
        bool   esJefe,
        bool   aplicaParentesco,
        string usuarioActual) => new()
    {
        PacienteId              = req.PacienteId!.Value,
        ParentescoId            = aplicaParentesco ? req.ParentescoId : null,
        EsJefeHogar             = esJefe,
        Comunidad               = req.Comunidad,
        GrupoPoblacionEspecial  = req.GrupoPoblacionEspecial,
        EstadoPersonaId         = EstadoPersonaId.Pendiente,
        CreadoEn                = DateTime.UtcNow,
        CreadoPor               = usuarioActual,
        Enfermedades            = req.Enfermedades
            .Select(e => new PersonaEnfermedad
            {
                EnfermedadId = e.EnfermedadId,
                EstadoId     = e.EstadoId,
                CreadoEn     = DateTime.UtcNow
            })
            .ToList()
    };

    // ─── PATCH: Agregar personas y tratamientos (DEF-AMM-004) ────────────────

    /// <summary>
    /// Agrega personas, registra tratamientos como observación y persiste la
    /// novedad censal en un censo ya creado. No toca la caracterización inicial.
    /// </summary>
    public async Task AgregarPersonasYTratamientosAsync(
        long id,
        AgregarPersonasYTratamientosRequest request,
        string usuarioActual,
        CancellationToken ct = default)
    {
        var censo = await this._censoRepository.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Censo {id} no encontrado.");

        var nuevasPersonas = (request.Personas ?? [])
            .Select(p =>
            {
                var persona = this.BuildCensoPersona(
                    p,
                    esJefe: false,
                    aplicaParentesco: censo.TipoEntornoId == TipoEntornoId.Hogar,
                    usuarioActual);
                persona.CensoId = id;
                return persona;
            })
            .ToList();

        if (nuevasPersonas.Count > 0)
            await this._censoRepository.AgregarPersonasAsync(nuevasPersonas, ct);

        // Siempre marcar el censo como Finalizado al completar el wizard
        censo.EstadoId      = EstadoId.Finalizado;
        censo.ModificadoEn  = DateTime.UtcNow;
        censo.ModificadoPor = usuarioActual;

        if (request.Tratamientos is not null)
        {
            var textoTratamientos = BuildTextoTratamientos(request.Tratamientos);
            censo.Observacion = string.IsNullOrWhiteSpace(censo.Observacion)
                ? textoTratamientos
                : $"{censo.Observacion}\n{textoTratamientos}";
        }

        await this._censoRepository.UpdateAsync(censo, ct);

        if (request.NovedadCensal is not null)
        {
            await this._censoNovedadRepository.AddAsync(new CensoNovedad
            {
                CensoId            = id,
                TipoNovedadId      = MapTipoNovedad(request.NovedadCensal.TipoNovedad),
                PresenciaNovedadId = MapPresencia(request.NovedadCensal.Presencia),
                Observacion        = BuildTextoNovedad(request.NovedadCensal),
                Fecha              = DateTime.UtcNow,
                EstadoId           = EstadoId.Activo,
                CreadoEn           = DateTime.UtcNow,
                CreadoPor          = usuarioActual
            }, ct);
        }

        await this._censoRepository.SaveChangesAsync(ct);
    }

    private static string BuildTextoTratamientos(TratamientosCensoRequest t)
    {
        var items = new List<string>();
        if (t.Tracoma)              items.Add("Tracoma");
        if (t.Teniasis)             items.Add("Teniasis");

        var exclusiones = new List<string>();
        if (t.RechazaTratamiento)  exclusiones.Add("Rechaza tratamiento");
        if (t.EnfermedadRenal)     exclusiones.Add("Enfermedad Renal");
        if (t.EnfermedadCardiaca)  exclusiones.Add("Enfermedad Cardíaca");
        if (t.Polimedicacion)      exclusiones.Add("Polimedicación");
        if (t.Alergias)            exclusiones.Add("Alergias");
        if (t.Embarazo)            exclusiones.Add("Embarazo");
        if (t.Lactancia)           exclusiones.Add("Lactancia");
        if (t.MenorEdad)           exclusiones.Add("Menor de edad (<5)");
        if (t.EnfermedadHepatica)  exclusiones.Add("Enfermedad Hepática");
        if (t.TrastornosGastricos) exclusiones.Add("Trastornos Gástricos");

        var partes = new List<string>();
        if (items.Count > 0)      partes.Add($"Tratamientos: {string.Join(", ", items)}");
        if (exclusiones.Count > 0) partes.Add($"Exclusiones: {string.Join(", ", exclusiones)}");
        if (!string.IsNullOrWhiteSpace(t.Observaciones)) partes.Add(t.Observaciones);

        return string.Join(" | ", partes);
    }

    private static string BuildTextoNovedad(NovedadCensalRequest n)
    {
        var partes = new List<string> { $"Presencia: {n.Presencia}", $"Novedad: {n.TipoNovedad}" };
        if (!string.IsNullOrWhiteSpace(n.FechaFallecimiento))  partes.Add($"Fallecimiento: {n.FechaFallecimiento}");
        if (!string.IsNullOrWhiteSpace(n.OtraNovedadDetalle))  partes.Add($"Detalle: {n.OtraNovedadDetalle}");
        if (!string.IsNullOrWhiteSpace(n.Observaciones))       partes.Add(n.Observaciones);
        return string.Join(" | ", partes);
    }

    // TipoNovedadId y PresenciaNovedadId: IDs provisionales hasta tener los valores reales del catálogo.
    private static byte MapTipoNovedad(string tipo) => tipo switch
    {
        "fallecimiento"       => 2,
        "cambio-residencia"   => 3,
        "nacimiento"          => 4,
        "migracion"           => 5,
        "nueva-vivienda"      => 6,
        "cambio-composicion"  => 7,
        "otro"                => 8,
        _                     => 1   // "ninguna" o desconocido
    };

    private static byte? MapPresencia(string presencia) => presencia switch
    {
        "presente"         => 1,
        "ausente"          => 2,
        "ausente-temporal" => 3,
        _                  => null
    };
}
