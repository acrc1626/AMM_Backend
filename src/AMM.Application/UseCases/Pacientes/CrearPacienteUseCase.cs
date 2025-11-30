using AMM.Application.DTOs;
using AMM.Domain.Entities;
using AMM.Domain.Ports;

namespace AMM.Application.UseCases.Pacientes;

/// <summary>
/// Use case: Create a new patient
/// Orchestrates domain logic and repository operations
/// </summary>
public class CrearPacienteUseCase
{
    private readonly IPacienteRepository _pacienteRepository;

    public CrearPacienteUseCase(IPacienteRepository pacienteRepository)
    {
        _pacienteRepository = pacienteRepository;
    }

    public async Task<PacienteDto> ExecuteAsync(CrearPacienteRequest request, string usuarioActual, CancellationToken cancellationToken = default)
    {
        // Business rule: Check if patient already exists
        var existente = await _pacienteRepository.GetByDocumentoAsync(
            request.TipoDocumentoId, 
            request.Documento, 
            cancellationToken);

        if (existente != null)
        {
            throw new InvalidOperationException($"Ya existe un paciente con el documento {request.Documento}");
        }

        // Create entity
        var paciente = new Paciente
        {
            TipoDocumentoId = request.TipoDocumentoId,
            Documento = request.Documento,
            PrimerNombre = request.PrimerNombre,
            SegundoNombre = request.SegundoNombre,
            PrimerApellido = request.PrimerApellido,
            SegundoApellido = request.SegundoApellido,
            SexoId = request.SexoId,
            FechaNacimiento = request.FechaNacimiento,
            PertenenciaEtnicaId = request.PertenenciaEtnicaId,
            PuebloIndigenaId = request.PuebloIndigenaId,
            CreadoEn = DateTime.UtcNow,
            CreadoPor = usuarioActual,
            EstadoId = 1 // Active
        };

        var resultado = await _pacienteRepository.AddAsync(paciente, cancellationToken);
        await _pacienteRepository.SaveChangesAsync(cancellationToken);

        // Map to DTO
        return MapToDto(resultado);
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
            TipoDocumento = paciente.TipoDocumentoId.ToString(), // TODO: Load from navigation property
            Documento = paciente.Documento,
            NombreCompleto = nombreCompleto,
            Sexo = paciente.SexoId.ToString(), // TODO: Load from navigation property
            FechaNacimiento = paciente.FechaNacimiento,
            Edad = edad
        };
    }
}
