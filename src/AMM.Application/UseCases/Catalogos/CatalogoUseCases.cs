using AMM.Application.DTOs.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Catalogos;

// Estado Use Cases
public class EstadoUseCase : CatalogoUseCase<Estado, EstadoDto, CrearEstadoRequest, byte>
{
    public EstadoUseCase(IEstadoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EstadoDto>> GetAllEstadosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EstadoDto(e.Id, e.Nombre, e.Descripcion), ct);

    public Task<EstadoDto?> GetEstadoByIdAsync(byte id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new EstadoDto(e.Id, e.Nombre, e.Descripcion), ct);

    public Task<EstadoDto> CreateEstadoAsync(CrearEstadoRequest request, CancellationToken ct = default) =>
        CreateAsync(request, 
            r => new Estado { Nombre = r.Nombre, Descripcion = r.Descripcion },
            e => new EstadoDto(e.Id, e.Nombre, e.Descripcion), 
            ct);
}

// TipoDocumento Use Cases
public class TipoDocumentoUseCase : CatalogoUseCase<TipoDocumento, TipoDocumentoDto, CrearTipoDocumentoRequest, byte>
{
    public TipoDocumentoUseCase(ITipoDocumentoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<TipoDocumentoDto>> GetAllTiposDocumentoAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new TipoDocumentoDto(e.Id, e.Descripcion), ct);

    public Task<TipoDocumentoDto?> GetTipoDocumentoByIdAsync(byte id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new TipoDocumentoDto(e.Id, e.Descripcion), ct);
}

// Sexo Use Cases
public class SexoUseCase : CatalogoUseCase<Sexo, SexoDto, CrearSexoRequest, byte>
{
    public SexoUseCase(ISexoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<SexoDto>> GetAllSexosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new SexoDto(e.Id, e.Descripcion), ct);

    public Task<SexoDto?> GetSexoByIdAsync(byte id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new SexoDto(e.Id, e.Descripcion), ct);
}

// Etnia Use Cases
public class EtniaUseCase : CatalogoUseCase<Etnia, EtniaDto, CrearEtniaRequest, byte>
{
    public EtniaUseCase(IEtniaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EtniaDto>> GetAllEtniasAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EtniaDto(e.Id, e.Descripcion, e.Codigo), ct);

    public Task<EtniaDto?> GetEtniaByIdAsync(byte id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new EtniaDto(e.Id, e.Descripcion, e.Codigo), ct);
}

// PuebloIndigena Use Cases
public class PuebloIndigenaUseCase : CatalogoUseCase<PuebloIndigena, PuebloIndigenaDto, CrearPuebloIndigenaRequest, short>
{
    public PuebloIndigenaUseCase(IPuebloIndigenaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<PuebloIndigenaDto>> GetAllPueblosIndigenasAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new PuebloIndigenaDto(e.Id, e.Descripcion), ct);

    public Task<PuebloIndigenaDto?> GetPuebloIndigenaByIdAsync(short id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new PuebloIndigenaDto(e.Id, e.Descripcion), ct);
}

// TipoEntorno Use Cases
public class TipoEntornoUseCase : CatalogoUseCase<TipoEntorno, TipoEntornoDto, CrearTipoEntornoRequest, byte>
{
    public TipoEntornoUseCase(ITipoEntornoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<TipoEntornoDto>> GetAllTiposEntornoAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new TipoEntornoDto(e.Id, e.Descripcion), ct);

    public Task<TipoEntornoDto?> GetTipoEntornoByIdAsync(byte id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new TipoEntornoDto(e.Id, e.Descripcion), ct);
}

// EventoTipo Use Cases
public class EventoTipoUseCase : CatalogoUseCase<EventoTipo, EventoTipoDto, CrearEventoTipoRequest, byte>
{
    public EventoTipoUseCase(IEventoTipoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<EventoTipoDto>> GetAllEventoTiposAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new EventoTipoDto(e.Id, e.Codigo, e.Nombre), ct);

    public Task<EventoTipoDto?> GetEventoTipoByIdAsync(byte id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new EventoTipoDto(e.Id, e.Codigo, e.Nombre), ct);
}

// FormaFarmaceutica Use Cases
public class FormaFarmaceuticaUseCase : CatalogoUseCase<FormaFarmaceutica, FormaFarmaceuticaDto, CrearFormaFarmaceuticaRequest, short>
{
    public FormaFarmaceuticaUseCase(IFormaFarmaceuticaRepository repository) : base(repository) { }

    public Task<IReadOnlyList<FormaFarmaceuticaDto>> GetAllFormasFarmaceuticasAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new FormaFarmaceuticaDto(e.Id, e.Nombre), ct);

    public Task<FormaFarmaceuticaDto?> GetFormaFarmaceuticaByIdAsync(short id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new FormaFarmaceuticaDto(e.Id, e.Nombre), ct);
}
