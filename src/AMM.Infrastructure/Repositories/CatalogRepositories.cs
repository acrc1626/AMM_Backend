using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;
using AMM.Infrastructure.Persistence;

namespace AMM.Infrastructure.Repositories;

// Concrete implementations for each catalog entity
public class EstadoRepository : CatalogRepository<Estado>, IEstadoRepository
{
    public EstadoRepository(AmmDbContext context) : base(context) { }
}

public class EstadoUsuarioRepository : CatalogRepository<EstadoUsuario>, IEstadoUsuarioRepository
{
    public EstadoUsuarioRepository(AmmDbContext context) : base(context) { }
}

public class TipoDocumentoRepository : CatalogRepository<TipoDocumento>, ITipoDocumentoRepository
{
    public TipoDocumentoRepository(AmmDbContext context) : base(context) { }
}

public class SexoRepository : CatalogRepository<Sexo>, ISexoRepository
{
    public SexoRepository(AmmDbContext context) : base(context) { }
}

public class EtniaRepository : CatalogRepository<Etnia>, IEtniaRepository
{
    public EtniaRepository(AmmDbContext context) : base(context) { }
}

public class PuebloIndigenaRepository : CatalogRepository<PuebloIndigena>, IPuebloIndigenaRepository
{
    public PuebloIndigenaRepository(AmmDbContext context) : base(context) { }
}

public class TipoEntornoRepository : CatalogRepository<TipoEntorno>, ITipoEntornoRepository
{
    public TipoEntornoRepository(AmmDbContext context) : base(context) { }
}

public class TipoNovedadRepository : CatalogRepository<TipoNovedad>, ITipoNovedadRepository
{
    public TipoNovedadRepository(AmmDbContext context) : base(context) { }
}

public class PresenciaNovedadRepository : CatalogRepository<PresenciaNovedad>, IPresenciaNovedadRepository
{
    public PresenciaNovedadRepository(AmmDbContext context) : base(context) { }
}

public class TipoSupervisionRepository : CatalogRepository<TipoSupervision>, ITipoSupervisionRepository
{
    public TipoSupervisionRepository(AmmDbContext context) : base(context) { }
}

public class MotivoNoTratamientoRepository : CatalogRepository<MotivoNoTratamiento>, IMotivoNoTratamientoRepository
{
    public MotivoNoTratamientoRepository(AmmDbContext context) : base(context) { }
}

public class ParentescoRepository : CatalogRepository<Parentesco>, IParentescoRepository
{
    public ParentescoRepository(AmmDbContext context) : base(context) { }
}

public class EventoTipoRepository : CatalogRepository<EventoTipo>, IEventoTipoRepository
{
    public EventoTipoRepository(AmmDbContext context) : base(context) { }
}

public class FormaFarmaceuticaRepository : CatalogRepository<FormaFarmaceutica>, IFormaFarmaceuticaRepository
{
    public FormaFarmaceuticaRepository(AmmDbContext context) : base(context) { }
}
