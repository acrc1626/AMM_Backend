using AMM.Domain.Entities;

namespace AMM.Domain.Ports;

/// <summary>
/// Repository port for Paciente aggregate
/// </summary>
public interface IPacienteRepository
{
    Task<Paciente?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<Paciente?> GetByDocumentoAsync(byte tipoDocumentoId, string documento, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Paciente>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<Paciente> AddAsync(Paciente paciente, CancellationToken cancellationToken = default);
    Task UpdateAsync(Paciente paciente, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
