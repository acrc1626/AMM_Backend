using AMM.Application.DTOs.Common;
using AMM.Application.DTOs.Eventos;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Eventos;

public class EventoUseCase : CatalogoUseCase<Evento, EventoDto, CrearEventoRequest, long>
{
    public EventoUseCase(IEventoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "",
            e.PacienteId, GetPacienteNombre(e.Paciente), e.Fecha, e.EstadoId, e.Estado?.Nombre ?? ""), ct);

    public Task<EventoDto?> GetByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new EventoDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "",
            e.PacienteId, GetPacienteNombre(e.Paciente), e.Fecha, e.EstadoId, e.Estado?.Nombre ?? ""), ct);

    public Task<PagedResult<EventoDto>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default) =>
        GetAllPagedAsync(e => new EventoDto(e.Id, e.EventoTipoId, e.EventoTipo?.Nombre ?? "",
            e.PacienteId, GetPacienteNombre(e.Paciente), e.Fecha, e.EstadoId, e.Estado?.Nombre ?? ""), page, pageSize, ct);

    private static string GetPacienteNombre(Paciente? p) =>
        p != null ? $"{p.PrimerNombre} {p.PrimerApellido}" : "";
}

public class EventoEscabiosisUseCase : CatalogoUseCase<EventoEscabiosis, EventoEscabiosisDto, CrearEventoEscabiosisRequest, long>
{
    public EventoEscabiosisUseCase(IEventoEscabiosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoEscabiosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoEscabiosisDto(e.EventoId, e.Severidad, e.Localizacion), ct);

    public Task<EventoEscabiosisDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoEscabiosisDto(e.EventoId, e.Severidad, e.Localizacion), ct);
}

public class EventoGeohelmintiasisUseCase : CatalogoUseCase<EventoGeohelmintiasis, EventoGeohelmintiasisDto, CrearEventoGeohelmintiasisRequest, long>
{
    public EventoGeohelmintiasisUseCase(IEventoGeohelmintiasisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoGeohelmintiasisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoGeohelmintiasisDto(e.EventoId, e.TipoPrueba, e.Resultado), ct);

    public Task<EventoGeohelmintiasisDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoGeohelmintiasisDto(e.EventoId, e.TipoPrueba, e.Resultado), ct);
}

public class EventoPediculosisUseCase : CatalogoUseCase<EventoPediculosis, EventoPediculosisDto, CrearEventoPediculosisRequest, long>
{
    public EventoPediculosisUseCase(IEventoPediculosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoPediculosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoPediculosisDto(e.EventoId, e.Grado), ct);

    public Task<EventoPediculosisDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoPediculosisDto(e.EventoId, e.Grado), ct);
}

public class EventoPianUseCase : CatalogoUseCase<EventoPian, EventoPianDto, CrearEventoPianRequest, long>
{
    public EventoPianUseCase(IEventoPianRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoPianDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoPianDto(e.EventoId, e.Estadio), ct);

    public Task<EventoPianDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoPianDto(e.EventoId, e.Estadio), ct);
}

public class EventoTeniasisCisticercosisUseCase : CatalogoUseCase<EventoTeniasisCisticercosis, EventoTeniasisCisticercosisDto, CrearEventoTeniasisCisticercosisRequest, long>
{
    public EventoTeniasisCisticercosisUseCase(IEventoTeniasisCisticercosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoTeniasisCisticercosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoTeniasisCisticercosisDto(e.EventoId, e.Teniasis, e.Cisticercosis), ct);

    public Task<EventoTeniasisCisticercosisDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoTeniasisCisticercosisDto(e.EventoId, e.Teniasis, e.Cisticercosis), ct);
}

public class EventoTracomaUseCase : CatalogoUseCase<EventoTracoma, EventoTracomaDto, CrearEventoTracomaRequest, long>
{
    public EventoTracomaUseCase(IEventoTracomaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoTracomaDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoTracomaDto(e.EventoId, e.CriterioExclusionId, e.ExaminadoTt, e.Triquiasis, e.OpacidadCorneal), ct);

    public Task<EventoTracomaDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoTracomaDto(e.EventoId, e.CriterioExclusionId, e.ExaminadoTt, e.Triquiasis, e.OpacidadCorneal), ct);
}

public class EventoTungiasisUseCase : CatalogoUseCase<EventoTungiasis, EventoTungiasisDto, CrearEventoTungiasisRequest, long>
{
    public EventoTungiasisUseCase(IEventoTungiasisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoTungiasisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoTungiasisDto(e.EventoId, e.NumeroLesiones, e.Complicaciones), ct);

    public Task<EventoTungiasisDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoTungiasisDto(e.EventoId, e.NumeroLesiones, e.Complicaciones), ct);
}

public class EventoMalariaUseCase : CatalogoUseCase<EventoMalaria, EventoMalariaDto, CrearEventoMalariaRequest, long>
{
    public EventoMalariaUseCase(IEventoMalariaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoMalariaDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoMalariaDto(e.EventoId, e.Especie), ct);

    public Task<EventoMalariaDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoMalariaDto(e.EventoId, e.Especie), ct);
}

public class EventoTuberculosisUseCase : CatalogoUseCase<EventoTuberculosis, EventoTuberculosisDto, CrearEventoTuberculosisRequest, long>
{
    public EventoTuberculosisUseCase(IEventoTuberculosisRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoTuberculosisDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoTuberculosisDto(e.EventoId, e.TipoTuberculosis), ct);

    public Task<EventoTuberculosisDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoTuberculosisDto(e.EventoId, e.TipoTuberculosis), ct);
}

public class EventoTuberculosisContactoUseCase : CatalogoUseCase<EventoTuberculosisContacto, EventoTuberculosisContactoDto, CrearEventoTuberculosisContactoRequest, long>
{
    public EventoTuberculosisContactoUseCase(IEventoTuberculosisContactoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoTuberculosisContactoDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoTuberculosisContactoDto(e.EventoId, e.Observacion), ct);

    public Task<EventoTuberculosisContactoDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoTuberculosisContactoDto(e.EventoId, e.Observacion), ct);
}

public class EventoLeshmaniasisCutaneaUseCase : CatalogoUseCase<EventoLeshmaniasisCutanea, EventoLeshmaniasisCutaneaDto, CrearEventoLeshmaniasisCutaneaRequest, long>
{
    public EventoLeshmaniasisCutaneaUseCase(IEventoLeshmaniasisCutaneaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoLeshmaniasisCutaneaDto>> GetAllAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoLeshmaniasisCutaneaDto(e.EventoId, e.TipoLesion), ct);

    public Task<EventoLeshmaniasisCutaneaDto?> GetByIdAsync(long eventoId, CancellationToken ct = default) =>
        GetByIdAsync(eventoId, e => new EventoLeshmaniasisCutaneaDto(e.EventoId, e.TipoLesion), ct);
}
