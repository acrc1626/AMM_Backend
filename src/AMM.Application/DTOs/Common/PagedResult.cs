namespace AMM.Application.DTOs.Common;

/// <summary>
/// Wrapper de respuesta paginada estándar para todos los listados.
/// </summary>
public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Total,
    int Page,
    int PageSize
)
{
    public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}
