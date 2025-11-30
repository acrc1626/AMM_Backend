using Microsoft.AspNetCore.Mvc;

namespace AMM.Api.Controllers.Base;

/// <summary>
/// Base controller for catalog entities with standard CRUD operations
/// </summary>
public abstract class CatalogControllerBase<TDto, TId> : ControllerBase
{
    protected readonly ILogger Logger;

    protected CatalogControllerBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Get all items from catalog
    /// </summary>
    public abstract Task<ActionResult<IReadOnlyList<TDto>>> GetAll(CancellationToken cancellationToken);

    /// <summary>
    /// Get catalog item by ID
    /// </summary>
    public abstract Task<ActionResult<TDto>> GetById(TId id, CancellationToken cancellationToken);

    protected ActionResult<T> HandleResult<T>(T? result) where T : class
    {
        if (result == null)
        {
            return NotFound(new ProblemDetails
            {
                Type = "https://amm.ins.gov.co/errors/not-found",
                Title = "Recurso no encontrado",
                Status = StatusCodes.Status404NotFound,
                Instance = HttpContext.Request.Path
            });
        }
        return Ok(result);
    }
}
