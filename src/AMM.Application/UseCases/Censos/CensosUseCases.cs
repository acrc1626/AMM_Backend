using AMM.Application.DTOs.Censos;
using AMM.Application.DTOs.Common;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Censos;

/// <summary>
/// Caso de uso para operaciones CRUD simples sobre censos:
/// listado, consulta por ID (liviana), paginación y edición (HU-005).
/// La creación con el wizard completo está en <see cref="CrearCensoUseCase"/>.
/// La consulta con detalle profundo está en <see cref="ConsultarCensoUseCase"/>.
/// </summary>
public class CensoUseCase : CatalogoUseCase<Censo, CensoDto, CrearCensoRequest, long>
{
    private readonly ICensoRepository _censoRepository;

    /// <summary>Inicializa el caso de uso con el repositorio de censos.</summary>
    public CensoUseCase(ICensoRepository repository) : base(repository)
    {
        this._censoRepository = repository;
    }

    /// <summary>Retorna todos los censos como proyección liviana.</summary>
    public Task<IReadOnlyList<CensoDto>> GetAllCensosAsync(CancellationToken ct = default) =>
        GetAllAsync(MapToDto, ct);

    /// <summary>Retorna censos paginados como proyección liviana.</summary>
    public Task<PagedResult<CensoDto>> GetAllCensosPagedAsync(int page, int pageSize, CancellationToken ct = default) =>
        GetAllPagedAsync(MapToDto, page, pageSize, ct);

    /// <summary>Retorna la proyección liviana del censo por ID, o null si no existe.</summary>
    public Task<CensoDto?> GetCensoByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, MapToDto, ct);

    /// <summary>
    /// Actualiza los campos editables del censo (HU-005).
    /// <para>
    /// RN-011: no elimina ni modifica el Jefe de Hogar del censo.
    /// RN-012: no altera la integridad de las personas asociadas (CensoPersonas / PersonaEnfermedades).
    /// Ambas reglas se garantizan por diseño: este método solo escribe sobre la entidad CENSO.
    /// </para>
    /// TipoEntornoId no es editable después de la creación y no forma parte del request.
    /// </summary>
    public async Task UpdateAsync(UpdateCensoRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = await this._censoRepository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"Censo con ID {request.Id} no encontrado.");

        // Solo se actualizan campos editables. TipoEntornoId se conserva intacto.
        entity.UbicacionId              = request.UbicacionId;
        entity.DepartamentoId           = request.DepartamentoId;
        entity.MunicipioId              = request.MunicipioId;
        entity.Fecha                    = request.Fecha;
        entity.Observacion              = request.Observacion;
        entity.EstadoId                 = request.EstadoId;
        entity.Direccion                = request.Direccion;
        entity.ObjetoInterventor        = request.ObjetoInterventor;
        entity.Territorio               = request.Territorio;
        entity.Microterritorio          = request.Microterritorio;
        entity.Area                     = request.Area;
        entity.Geolocalizacion          = request.Geolocalizacion;
        entity.TotalMiembros            = request.TotalMiembros;
        entity.VisitantesTemporalesCol  = request.VisitantesTemporalesCol;
        entity.VisitantesTemporalesMig  = request.VisitantesTemporalesMig;
        entity.ModificadoEn             = DateTime.UtcNow;
        entity.ModificadoPor            = usuarioActual;

        await this._censoRepository.UpdateAsync(entity, ct);
        await this._censoRepository.SaveChangesAsync(ct);
    }

    // ─── Mapeo liviano ───────────────────────────────────────────────────────

    private static CensoDto MapToDto(Censo e) => new(
        e.Id,
        e.TipoEntornoId,
        e.TipoEntorno?.Descripcion ?? "",
        e.UbicacionId,
        e.Ubicacion?.Direccion,
        e.DepartamentoId,
        e.Departamento?.Nombre,
        e.MunicipioId,
        e.Municipio?.Nombre,
        e.Fecha,
        e.Observacion,
        e.EstadoId,
        e.Estado?.Nombre ?? ""
    );
}

/// <summary>Caso de uso para operaciones CRUD sobre novedades de censo.</summary>
public class CensoNovedadUseCase : CatalogoUseCase<CensoNovedad, CensoNovedadDto, CrearCensoNovedadRequest, long>
{
    private readonly ICensoNovedadRepository _censoNovedadRepository;

    /// <summary>Inicializa el caso de uso con el repositorio de novedades.</summary>
    public CensoNovedadUseCase(ICensoNovedadRepository repository) : base(repository)
    {
        this._censoNovedadRepository = repository;
    }

    /// <summary>Retorna todas las novedades de censo.</summary>
    public Task<IReadOnlyList<CensoNovedadDto>> GetAllCensoNovedadesAsync(CancellationToken ct = default) =>
        GetAllAsync(MapToDto, ct);

    /// <summary>Retorna la novedad por ID, o null si no existe.</summary>
    public Task<CensoNovedadDto?> GetCensoNovedadByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, MapToDto, ct);

    /// <summary>Retorna las novedades asociadas a un censo.</summary>
    public async Task<IReadOnlyList<CensoNovedadDto>> GetByCensoAsync(long censoId, CancellationToken ct = default)
    {
        var novedades = await this._censoNovedadRepository.GetByCensoIdAsync(censoId, ct);
        return novedades.Select(MapToDto).ToList();
    }

    /// <summary>Retorna las novedades asociadas a un paciente.</summary>
    public async Task<IReadOnlyList<CensoNovedadDto>> GetByPacienteAsync(long pacienteId, CancellationToken ct = default)
    {
        var novedades = await this._censoNovedadRepository.GetByPacienteIdAsync(pacienteId, ct);
        return novedades.Select(MapToDto).ToList();
    }

    /// <summary>Crea una novedad de censo.</summary>
    public async Task<CensoNovedadDto> CreateAsync(CrearCensoNovedadRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = new CensoNovedad
        {
            CensoId            = request.CensoId,
            PacienteId         = request.PacienteId,
            TipoNovedadId      = request.TipoNovedadId,
            PresenciaNovedadId = request.PresenciaNovedadId,
            TratamientoId      = request.TratamientoId,
            Observacion        = request.Observacion,
            Fecha              = request.Fecha,
            EstadoId           = request.EstadoId,
            CreadoEn           = DateTime.UtcNow,
            CreadoPor          = usuarioActual
        };

        var created = await this._censoNovedadRepository.AddAsync(entity, ct);
        await this._censoNovedadRepository.SaveChangesAsync(ct);

        var result = await this._censoNovedadRepository.GetByIdAsync(created.Id, ct);
        return MapToDto(result!);
    }

    /// <summary>Actualiza una novedad de censo.</summary>
    public async Task UpdateAsync(UpdateCensoNovedadRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = await this._censoNovedadRepository.GetByIdAsync(request.Id, ct)
            ?? throw new KeyNotFoundException($"CensoNovedad con ID {request.Id} no encontrado.");

        entity.CensoId            = request.CensoId;
        entity.PacienteId         = request.PacienteId;
        entity.TipoNovedadId      = request.TipoNovedadId;
        entity.PresenciaNovedadId = request.PresenciaNovedadId;
        entity.TratamientoId      = request.TratamientoId;
        entity.Observacion        = request.Observacion;
        entity.Fecha              = request.Fecha;
        entity.EstadoId           = request.EstadoId;
        entity.ModificadoEn       = DateTime.UtcNow;
        entity.ModificadoPor      = usuarioActual;

        await this._censoNovedadRepository.UpdateAsync(entity, ct);
        await this._censoNovedadRepository.SaveChangesAsync(ct);
    }

    private static CensoNovedadDto MapToDto(CensoNovedad e) => new(
        e.Id,
        e.CensoId,
        e.PacienteId,
        e.Paciente is not null ? $"{e.Paciente.PrimerNombre} {e.Paciente.PrimerApellido}" : null,
        e.TipoNovedadId,
        e.TipoNovedad?.Descripcion ?? "",
        e.PresenciaNovedadId,
        e.PresenciaNovedad?.Descripcion,
        e.TratamientoId,
        e.Observacion,
        e.Fecha,
        e.EstadoId,
        e.Estado?.Nombre ?? ""
    );
}
