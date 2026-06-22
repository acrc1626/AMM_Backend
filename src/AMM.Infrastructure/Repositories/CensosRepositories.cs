using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AMM.Infrastructure.Repositories;

/// <summary>Repositorio de censos con soporte para consulta de detalle profundo.</summary>
public class CensoRepository : CatalogRepository<Censo>, ICensoRepository
{
    /// <summary>Inicializa el repositorio con el contexto de base de datos.</summary>
    public CensoRepository(AmmDbContext context) : base(context) { }

    /// <inheritdoc/>
    public override async Task<IReadOnlyList<Censo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this._dbSet
            .Include(c => c.TipoEntorno)
            .Include(c => c.Ubicacion)
            .Include(c => c.Departamento)
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<Censo?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        if (id is not long longId) return null;

        return await this._dbSet
            .Include(c => c.TipoEntorno)
            .Include(c => c.Ubicacion)
            .Include(c => c.Departamento)
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .FirstOrDefaultAsync(c => c.Id == longId, cancellationToken);
    }

    /// <summary>
    /// Retorna el censo con todos sus datos anidados cargados mediante eager loading:
    /// personas, parentescos, estados de persona y enfermedades por persona.
    /// </summary>
    public async Task<Censo?> GetDetalleAsync(long id, CancellationToken ct = default)
    {
        return await this._dbSet
            .Include(c => c.TipoEntorno)
            .Include(c => c.Ubicacion)
            .Include(c => c.Departamento)
            .Include(c => c.Municipio)
            .Include(c => c.Estado)
            .Include(c => c.Personas).ThenInclude(p => p.Paciente).ThenInclude(pac => pac.TipoDocumento)
            .Include(c => c.Personas).ThenInclude(p => p.Paciente).ThenInclude(pac => pac.Sexo)
            .Include(c => c.Personas).ThenInclude(p => p.Parentesco)
            .Include(c => c.Personas).ThenInclude(p => p.EstadoPersona)
            .Include(c => c.Personas).ThenInclude(p => p.Enfermedades).ThenInclude(e => e.Enfermedad)
            .Include(c => c.Personas).ThenInclude(p => p.Enfermedades).ThenInclude(e => e.Estado)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    /// <inheritdoc/>
    public async Task AgregarPersonasAsync(IEnumerable<CensoPersona> personas, CancellationToken ct = default)
    {
        await this._context.Set<CensoPersona>().AddRangeAsync(personas, ct);
    }
}

/// <summary>Repositorio de novedades de censo.</summary>
public class CensoNovedadRepository : CatalogRepository<CensoNovedad>, ICensoNovedadRepository
{
    /// <summary>Inicializa el repositorio con el contexto de base de datos.</summary>
    public CensoNovedadRepository(AmmDbContext context) : base(context) { }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CensoNovedad>> GetByCensoIdAsync(long censoId, CancellationToken cancellationToken = default)
    {
        return await this._dbSet
            .Where(cn => cn.CensoId == censoId)
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<CensoNovedad>> GetByPacienteIdAsync(long pacienteId, CancellationToken cancellationToken = default)
    {
        return await this._dbSet
            .Where(cn => cn.PacienteId == pacienteId)
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<IReadOnlyList<CensoNovedad>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await this._dbSet
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override async Task<CensoNovedad?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
    {
        if (id is not long longId) return null;

        return await this._dbSet
            .Include(cn => cn.TipoNovedad)
            .Include(cn => cn.PresenciaNovedad)
            .Include(cn => cn.Paciente)
            .Include(cn => cn.Estado)
            .FirstOrDefaultAsync(cn => cn.Id == longId, cancellationToken);
    }
}
