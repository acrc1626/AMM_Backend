using AMM.Application.DTOs.Censos;
using AMM.Application.UseCases.Catalogos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Censos;

public class CensoUseCase : CatalogoUseCase<Censo, CensoDto, CrearCensoRequest, long>
{
    private readonly ICensoRepository _censoRepository;

    public CensoUseCase(ICensoRepository repository) : base(repository) 
    {
        _censoRepository = repository;
    }

    public Task<IReadOnlyList<CensoDto>> GetAllCensosAsync(CancellationToken ct = default) =>
        GetAllAsync(MapToDto, ct);

    public Task<CensoDto?> GetCensoByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, MapToDto, ct);

    public async Task<IReadOnlyList<CensoDto>> GetByPacienteAsync(long pacienteId, CancellationToken ct = default)
    {
        var censos = await _censoRepository.GetByPacienteIdAsync(pacienteId, ct);
        return censos.Select(MapToDto).ToList();
    }

    public async Task<CensoDto> CreateAsync(CrearCensoRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = new Censo
        {
            TipoEntornoId = request.TipoEntornoId,
            PacienteId = request.PacienteId,
            UbicacionId = request.UbicacionId,
            DepartamentoId = request.DepartamentoId,
            MunicipioId = request.MunicipioId,
            Fecha = request.Fecha,
            Observacion = request.Observacion,
            EstadoId = request.EstadoId,
            CreadoEn = DateTime.UtcNow,
            CreadoPor = usuarioActual
        };

        var created = await _censoRepository.AddAsync(entity, ct);
        await _censoRepository.SaveChangesAsync(ct);
        
        var result = await _censoRepository.GetByIdAsync(created.Id, ct);
        return MapToDto(result!);
    }

    public async Task UpdateAsync(UpdateCensoRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = await _censoRepository.GetByIdAsync(request.Id, ct);
        if (entity == null) throw new KeyNotFoundException($"Censo con ID {request.Id} no encontrado");

        entity.TipoEntornoId = request.TipoEntornoId;
        entity.PacienteId = request.PacienteId;
        entity.UbicacionId = request.UbicacionId;
        entity.DepartamentoId = request.DepartamentoId;
        entity.MunicipioId = request.MunicipioId;
        entity.Fecha = request.Fecha;
        entity.Observacion = request.Observacion;
        entity.EstadoId = request.EstadoId;
        entity.ModificadoEn = DateTime.UtcNow;
        entity.ModificadoPor = usuarioActual;

        await _censoRepository.UpdateAsync(entity, ct);
        await _censoRepository.SaveChangesAsync(ct);
    }

    private static CensoDto MapToDto(Censo e) => new CensoDto(
        e.Id, 
        e.TipoEntornoId, 
        e.TipoEntorno?.Descripcion ?? "", 
        e.PacienteId, 
        e.Paciente != null ? $"{e.Paciente.PrimerNombre} {e.Paciente.PrimerApellido}" : null, 
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

public class CensoNovedadUseCase : CatalogoUseCase<CensoNovedad, CensoNovedadDto, CrearCensoNovedadRequest, long>
{
    private readonly ICensoNovedadRepository _censoNovedadRepository;

    public CensoNovedadUseCase(ICensoNovedadRepository repository) : base(repository) 
    {
        _censoNovedadRepository = repository;
    }

    public Task<IReadOnlyList<CensoNovedadDto>> GetAllCensoNovedadesAsync(CancellationToken ct = default) =>
        GetAllAsync(MapToDto, ct);

    public Task<CensoNovedadDto?> GetCensoNovedadByIdAsync(long id, CancellationToken ct = default) =>
        GetByIdAsync(id, MapToDto, ct);

    public async Task<IReadOnlyList<CensoNovedadDto>> GetByCensoAsync(long censoId, CancellationToken ct = default)
    {
        var novedades = await _censoNovedadRepository.GetByCensoIdAsync(censoId, ct);
        return novedades.Select(MapToDto).ToList();
    }

    public async Task<IReadOnlyList<CensoNovedadDto>> GetByPacienteAsync(long pacienteId, CancellationToken ct = default)
    {
        var novedades = await _censoNovedadRepository.GetByPacienteIdAsync(pacienteId, ct);
        return novedades.Select(MapToDto).ToList();
    }

    public async Task<CensoNovedadDto> CreateAsync(CrearCensoNovedadRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = new CensoNovedad
        {
            CensoId = request.CensoId,
            PacienteId = request.PacienteId,
            TipoNovedadId = request.TipoNovedadId,
            PresenciaNovedadId = request.PresenciaNovedadId,
            TratamientoId = request.TratamientoId,
            Observacion = request.Observacion,
            Fecha = request.Fecha,
            EstadoId = request.EstadoId,
            CreadoEn = DateTime.UtcNow,
            CreadoPor = usuarioActual
        };

        var created = await _censoNovedadRepository.AddAsync(entity, ct);
        await _censoNovedadRepository.SaveChangesAsync(ct);
        
        var result = await _censoNovedadRepository.GetByIdAsync(created.Id, ct);
        return MapToDto(result!);
    }

    public async Task UpdateAsync(UpdateCensoNovedadRequest request, string usuarioActual, CancellationToken ct = default)
    {
        var entity = await _censoNovedadRepository.GetByIdAsync(request.Id, ct);
        if (entity == null) throw new KeyNotFoundException($"CensoNovedad con ID {request.Id} no encontrado");

        entity.CensoId = request.CensoId;
        entity.PacienteId = request.PacienteId;
        entity.TipoNovedadId = request.TipoNovedadId;
        entity.PresenciaNovedadId = request.PresenciaNovedadId;
        entity.TratamientoId = request.TratamientoId;
        entity.Observacion = request.Observacion;
        entity.Fecha = request.Fecha;
        entity.EstadoId = request.EstadoId;
        entity.ModificadoEn = DateTime.UtcNow;
        entity.ModificadoPor = usuarioActual;

        await _censoNovedadRepository.UpdateAsync(entity, ct);
        await _censoNovedadRepository.SaveChangesAsync(ct);
    }

    private static CensoNovedadDto MapToDto(CensoNovedad e) => new CensoNovedadDto(
        e.Id, 
        e.CensoId, 
        e.PacienteId, 
        e.Paciente != null ? $"{e.Paciente.PrimerNombre} {e.Paciente.PrimerApellido}" : null, 
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
