using AMM.Domain.Entities;
using AMM.Domain.Ports;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

/// <summary>
/// Repository adapter implementation for Paciente
/// Infrastructure layer - implements domain port
/// </summary>
public class PacienteRepository : IPacienteRepository
{
    private readonly AmmDbContext _context;

    public PacienteRepository(AmmDbContext context)
    {
        _context = context;
    }

    public async Task<Paciente?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Pacientes
            .Include(p => p.TipoDocumento)
            .Include(p => p.Sexo)
            .Include(p => p.PertenenciaEtnica)
            .Include(p => p.PuebloIndigena)
            .Include(p => p.Estado)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Paciente?> GetByDocumentoAsync(byte tipoDocumentoId, string documento, CancellationToken cancellationToken = default)
    {
        return await _context.Pacientes
            .FirstOrDefaultAsync(p => p.TipoDocumentoId == tipoDocumentoId && p.Documento == documento, cancellationToken);
    }

    public async Task<IReadOnlyList<Paciente>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Pacientes
            .Include(p => p.TipoDocumento)
            .Include(p => p.Sexo)
            .OrderByDescending(p => p.CreadoEn)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<Paciente> AddAsync(Paciente paciente, CancellationToken cancellationToken = default)
    {
        await _context.Pacientes.AddAsync(paciente, cancellationToken);
        return paciente;
    }

    public Task UpdateAsync(Paciente paciente, CancellationToken cancellationToken = default)
    {
        _context.Pacientes.Update(paciente);
        return Task.CompletedTask;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
