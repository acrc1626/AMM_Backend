using AMM.Application.DTOs.Eventos;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Eventos;

// Base use case para eventos con lógica común
public abstract class EventoBaseUseCase<TEntity, TDto, TCreateRequest, TId> : CatalogoUseCase<TEntity, TDto, TCreateRequest, TId>
    where TEntity : Evento
    where TDto : EventoDto
    where TCreateRequest : CrearEventoRequest
{
    protected EventoBaseUseCase(ICatalogRepository<TEntity> repository) : base(repository) { }

    protected static string GetPacienteNombre(Paciente? paciente) =>
        paciente != null ? $"{paciente.PrimerNombre} {paciente.PrimerApellido}" : "";
}

// Escabiosis
public class EscabiosisUseCase : EventoBaseUseCase<Escabiosis, EscabiosisDto, CrearEscabiosisRequest, long>
{
    public EscabiosisUseCase(IEscabiosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EscabiosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EscabiosisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Tratado, e.CierreControl), ct);

    public Task<EscabiosisDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new EscabiosisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Tratado, e.CierreControl), ct);
}

// Geohelmintiasis
public class GeohelmintiasisUseCase : EventoBaseUseCase<Geohelmintiasis, GeohelmintiasisDto, CrearGeohelmintiasisRequest, long>
{
    public GeohelmintiasisUseCase(IGeohelmintiasisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<GeohelmintiasisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new GeohelmintiasisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Tratado, e.CierreControl, e.Laboratorio), ct);

    public Task<GeohelmintiasisDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new GeohelmintiasisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Tratado, e.CierreControl, e.Laboratorio), ct);
}

// Pediculosis
public class PediculosisUseCase : EventoBaseUseCase<Pediculosis, PediculosisDto, CrearPediculosisRequest, long>
{
    public PediculosisUseCase(IPediculosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<PediculosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new PediculosisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Tratado, e.CierreControl), ct);

    public Task<PediculosisDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new PediculosisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Tratado, e.CierreControl), ct);
}

// Malaria
public class MalariaUseCase : EventoBaseUseCase<Malaria, MalariaDto, CrearMalariaRequest, long>
{
    public MalariaUseCase(IMalariaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<MalariaDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new MalariaDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Gota, e.Resultado), ct);

    public Task<MalariaDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new MalariaDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Gota, e.Resultado), ct);
}

// Tuberculosis
public class TuberculosisUseCase : EventoBaseUseCase<Tuberculosis, TuberculosisDto, CrearTuberculosisRequest, long>
{
    public TuberculosisUseCase(ITuberculosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<TuberculosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new TuberculosisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Sintomatico, e.Resultado), ct);

    public Task<TuberculosisDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new TuberculosisDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Sintomatico, e.Resultado), ct);
}

// TuberculosisContacto
public class TuberculosisContactoUseCase : EventoBaseUseCase<TuberculosisContacto, TuberculosisContactoDto, CrearTuberculosisContactoRequest, long>
{
    public TuberculosisContactoUseCase(ITuberculosisContactoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<TuberculosisContactoDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new TuberculosisContactoDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.IndexId, e.ParentescoId, e.Parentesco?.Descripcion), ct);

    public Task<TuberculosisContactoDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new TuberculosisContactoDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.IndexId, e.ParentescoId, e.Parentesco?.Descripcion), ct);
}

// LeshmaniasisCutanea
public class LeshmaniasisCutaneaUseCase : EventoBaseUseCase<LeshmaniasisCutanea, LeshmaniasisCutaneaDto, CrearLeshmaniasisCutaneaRequest, long>
{
    public LeshmaniasisCutaneaUseCase(ILeshmaniasisCutaneaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<LeshmaniasisCutaneaDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new LeshmaniasisCutaneaDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Cicatriz, e.NumeroLesiones), ct);

    public Task<LeshmaniasisCutaneaDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new LeshmaniasisCutaneaDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "", e.PacienteId, 
            GetPacienteNombre(e.Paciente), e.Fecha, e.Observacion, e.EstadoId, e.Estado?.Nombre ?? "", 
            e.Cicatriz, e.NumeroLesiones), ct);
}
