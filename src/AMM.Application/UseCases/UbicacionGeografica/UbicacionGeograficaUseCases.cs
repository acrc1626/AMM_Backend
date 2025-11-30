using AMM.Application.DTOs.UbicacionGeografica;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.UbicacionGeografica;

public class DepartamentoUseCase : CatalogoUseCase<Departamento, DepartamentoDto, CrearDepartamentoRequest, short>
{
    public DepartamentoUseCase(IDepartamentoRepository repository) : base(repository) { }

    public Task<IReadOnlyList<DepartamentoDto>> GetAllDepartamentosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new DepartamentoDto(e.Id, e.CodigoDane, e.Nombre), ct);

    public Task<DepartamentoDto?> GetDepartamentoByIdAsync(short id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new DepartamentoDto(e.Id, e.CodigoDane, e.Nombre), ct);
}

public class MunicipioUseCase : CatalogoUseCase<Municipio, MunicipioDto, CrearMunicipioRequest, int>
{
    private readonly IMunicipioRepository _municipioRepository;

    public MunicipioUseCase(IMunicipioRepository repository) : base(repository) 
    {
        _municipioRepository = repository;
    }

    public Task<IReadOnlyList<MunicipioDto>> GetAllMunicipiosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new MunicipioDto(e.Id, e.DepartamentoId, e.Departamento?.Nombre ?? "", e.CodigoDaneDpto, e.Nombre), ct);

    public Task<MunicipioDto?> GetMunicipioByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new MunicipioDto(e.Id, e.DepartamentoId, e.Departamento?.Nombre ?? "", e.CodigoDaneDpto, e.Nombre), ct);

    public async Task<IReadOnlyList<MunicipioDto>> GetByDepartamentoAsync(short departamentoId, CancellationToken ct = default)
    {
        var municipios = await _municipioRepository.GetByDepartamentoIdAsync(departamentoId, ct);
        return municipios.Select(e => new MunicipioDto(e.Id, e.DepartamentoId, e.Departamento?.Nombre ?? "", e.CodigoDaneDpto, e.Nombre)).ToList();
    }
}

public class TerritorioUseCase : CatalogoUseCase<Territorio, TerritorioDto, CrearTerritorioRequest, int>
{
    private readonly ITerritorioRepository _territorioRepository;

    public TerritorioUseCase(ITerritorioRepository repository) : base(repository)
    {
        _territorioRepository = repository;
    }

    public Task<IReadOnlyList<TerritorioDto>> GetAllTerritoriosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new TerritorioDto(e.Id, e.MunicipioId, e.Municipio?.Nombre ?? "", e.Nombre), ct);

    public Task<TerritorioDto?> GetTerritorioByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new TerritorioDto(e.Id, e.MunicipioId, e.Municipio?.Nombre ?? "", e.Nombre), ct);

    public async Task<IReadOnlyList<TerritorioDto>> GetByMunicipioAsync(int municipioId, CancellationToken ct = default)
    {
        var territorios = await _territorioRepository.GetByMunicipioIdAsync(municipioId, ct);
        return territorios.Select(e => new TerritorioDto(e.Id, e.MunicipioId, e.Municipio?.Nombre ?? "", e.Nombre)).ToList();
    }
}

public class MicroterritorioUseCase : CatalogoUseCase<Microterritorio, MicroterritorioDto, CrearMicroterritorioRequest, int>
{
    private readonly IMicroterritorioRepository _microterritorioRepository;

    public MicroterritorioUseCase(IMicroterritorioRepository repository) : base(repository)
    {
        _microterritorioRepository = repository;
    }

    public Task<IReadOnlyList<MicroterritorioDto>> GetAllMicroterritoriosAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new MicroterritorioDto(e.Id, e.TerritorioId, e.Territorio?.Nombre ?? "", e.Nombre), ct);

    public Task<MicroterritorioDto?> GetMicroterritorioByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new MicroterritorioDto(e.Id, e.TerritorioId, e.Territorio?.Nombre ?? "", e.Nombre), ct);

    public async Task<IReadOnlyList<MicroterritorioDto>> GetByTerritorioAsync(int territorioId, CancellationToken ct = default)
    {
        var microterritorios = await _microterritorioRepository.GetByTerritorioIdAsync(territorioId, ct);
        return microterritorios.Select(e => new MicroterritorioDto(e.Id, e.TerritorioId, e.Territorio?.Nombre ?? "", e.Nombre)).ToList();
    }
}

public class AreaUseCase : CatalogoUseCase<Area, AreaDto, CrearAreaRequest, int>
{
    private readonly IAreaRepository _areaRepository;

    public AreaUseCase(IAreaRepository repository) : base(repository)
    {
        _areaRepository = repository;
    }

    public Task<IReadOnlyList<AreaDto>> GetAllAreasAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new AreaDto(e.Id, e.MicroterritorioId, e.Microterritorio?.Nombre ?? "", e.Nombre), ct);

    public Task<AreaDto?> GetAreaByIdAsync(int id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new AreaDto(e.Id, e.MicroterritorioId, e.Microterritorio?.Nombre ?? "", e.Nombre), ct);

    public async Task<IReadOnlyList<AreaDto>> GetByMicroterritorioAsync(int microterritorioId, CancellationToken ct = default)
    {
        var areas = await _areaRepository.GetByMicroterritorioIdAsync(microterritorioId, ct);
        return areas.Select(e => new AreaDto(e.Id, e.MicroterritorioId, e.Microterritorio?.Nombre ?? "", e.Nombre)).ToList();
    }
}

public class UbicacionUseCase : CatalogoUseCase<Ubicacion, UbicacionDto, CrearUbicacionRequest, long>
{
    public UbicacionUseCase(IUbicacionRepository repository) : base(repository) { }

    public Task<IReadOnlyList<UbicacionDto>> GetAllUbicacionesAsync(CancellationToken ct = default) =>
        GetAllAsync(e => new UbicacionDto(e.Id, e.Direccion, e.Lat, e.Lon), ct);

    public Task<UbicacionDto?> GetUbicacionByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, e => new UbicacionDto(e.Id, e.Direccion, e.Lat, e.Lon), ct);
}
