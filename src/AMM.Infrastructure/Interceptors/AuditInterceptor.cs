using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AMM.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AMM.Infrastructure.Interceptors;

/// <summary>
/// Interceptor de EF Core que asigna automáticamente los campos de auditoría
/// (CreadoEn, CreadoPor, ModificadoEn, ModificadoPor) sin repetir lógica en cada UseCase.
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context is null) return;

        var now = DateTime.UtcNow;
        var usuario = GetCurrentUser();

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.CreadoEn == default)
                        entry.Entity.CreadoEn = now;
                    if (entry.Entity.CreadoPor is null)
                        entry.Entity.CreadoPor = usuario;
                    entry.Entity.ModificadoEn = now;
                    entry.Entity.ModificadoPor = usuario;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModificadoEn = now;
                    entry.Entity.ModificadoPor = usuario;
                    break;
            }
        }
    }

    private string GetCurrentUser()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is null) return "system";

        return context.User.FindFirst(JwtRegisteredClaimNames.Email)?.Value
            ?? context.User.FindFirst(ClaimTypes.Email)?.Value
            ?? context.User.Identity?.Name
            ?? "system";
    }
}
