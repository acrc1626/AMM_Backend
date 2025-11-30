using AMM.Application.DTOs;
using AMM.Domain.Entities;
using AMM.Domain.Ports;

namespace AMM.Application.UseCases.Pacientes;

public class GetPacienteByIdUseCase
{
    private readonly IPacienteRepository _repository;

    public GetPacienteByIdUseCase(IPacienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<PacienteDto?> ExecuteAsync(long id, CancellationToken cancellationToken = default)
    {
        var paciente = await _repository.GetByIdAsync(id, cancellationToken);
        return paciente != null ? MapToDto(paciente) : null;
    }

    private static PacienteDto MapToDto(Paciente paciente)
    {
        var nombreCompleto = $"{paciente.PrimerNombre} {paciente.SegundoNombre} {paciente.PrimerApellido} {paciente.SegundoApellido}".Trim();
        int? edad = paciente.FechaNacimiento.HasValue 
            ? (int?)((DateTime.UtcNow - paciente.FechaNacimiento.Value).Days / 365) 
            : null;

        return new PacienteDto
        {
            Id = paciente.Id,
            TipoDocumento = paciente.TipoDocumento?.Descripcion ?? paciente.TipoDocumentoId.ToString(),
            Documento = paciente.Documento,
            NombreCompleto = nombreCompleto,
            Sexo = paciente.Sexo?.Descripcion ?? paciente.SexoId.ToString(),
            FechaNacimiento = paciente.FechaNacimiento,
            Edad = edad
        };
    }
}

public class GetAllPacientesUseCase
{
    private readonly IPacienteRepository _repository;

    public GetAllPacientesUseCase(IPacienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<PacienteDto>> ExecuteAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        var pacientes = await _repository.GetAllAsync(skip, take, cancellationToken);
        return pacientes.Select(MapToDto).ToList();
    }

    private static PacienteDto MapToDto(Paciente paciente)
    {
        var nombreCompleto = $"{paciente.PrimerNombre} {paciente.SegundoNombre} {paciente.PrimerApellido} {paciente.SegundoApellido}".Trim();
        int? edad = paciente.FechaNacimiento.HasValue 
            ? (int?)((DateTime.UtcNow - paciente.FechaNacimiento.Value).Days / 365) 
            : null;

        return new PacienteDto
        {
            Id = paciente.Id,
            TipoDocumento = paciente.TipoDocumento?.Descripcion ?? paciente.TipoDocumentoId.ToString(),
            Documento = paciente.Documento,
            NombreCompleto = nombreCompleto,
            Sexo = paciente.Sexo?.Descripcion ?? paciente.SexoId.ToString(),
            FechaNacimiento = paciente.FechaNacimiento,
            Edad = edad
        };
    }
}

public class UpdatePacienteUseCase
{
    private readonly IPacienteRepository _repository;

    public UpdatePacienteUseCase(IPacienteRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(UpdatePacienteRequest request, string usuarioActual, CancellationToken cancellationToken = default)
    {
        var paciente = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (paciente == null)
        {
            throw new KeyNotFoundException($"Paciente con ID {request.Id} no encontrado");
        }

        paciente.TipoDocumentoId = request.TipoDocumentoId;
        paciente.Documento = request.Documento;
        paciente.PrimerNombre = request.PrimerNombre;
        paciente.SegundoNombre = request.SegundoNombre;
        paciente.PrimerApellido = request.PrimerApellido;
        paciente.SegundoApellido = request.SegundoApellido;
        paciente.SexoId = request.SexoId;
        paciente.FechaNacimiento = request.FechaNacimiento;
        paciente.PertenenciaEtnicaId = request.PertenenciaEtnicaId;
        paciente.PuebloIndigenaId = request.PuebloIndigenaId;
        paciente.EstadoId = request.EstadoId;
        paciente.ModificadoEn = DateTime.UtcNow;
        paciente.ModificadoPor = usuarioActual;

        await _repository.UpdateAsync(paciente, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
