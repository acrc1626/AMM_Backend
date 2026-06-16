using AMM.Application.Interfaces;
using AMM.Domain.Constants;
using AMM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AMM.Infrastructure.Persistence;

/// <summary>
/// Siembra datos mínimos en Development para poder hacer login sin configuración manual.
/// Solo corre si el usuario QA no existe todavía.
/// </summary>
public static class DevSeeder
{
    public static async Task SeedAsync(AmmDbContext db, IPasswordHasher hasher, ILogger logger)
    {
        const string correoQa = "qa@amm.local";

        if (await db.Usuarios.AnyAsync(u => u.Correo == correoQa))
            return;

        // Rol Administrador (id=1 por convención)
        var rol = await db.Roles.FirstOrDefaultAsync(r => r.Nombre == "Administrador");
        if (rol is null)
        {
            rol = new Rol { Nombre = "Administrador", Descripcion = "Acceso total" };
            db.Roles.Add(rol);
            await db.SaveChangesAsync();
        }

        // EstadoUsuario Activo debe existir (es un catálogo de la BD)
        var estadoActivo = await db.EstadoUsuarios.FindAsync((byte)EstadoUsuarioId.Activo);
        if (estadoActivo is null)
        {
            logger.LogWarning("[DevSeeder] No existe EstadoUsuario id={Id} en la BD. " +
                              "Ejecuta las migraciones o inserta el catálogo.", EstadoUsuarioId.Activo);
            return;
        }

        var usuario = new Usuario
        {
            Correo         = correoQa,
            NombreCompleto = "QA Admin",
            PasswordHash   = hasher.Hash("2d6afed9-f9f5-483a-a987-c8ae1024d09f"),
            EstadoUsuarioId = EstadoUsuarioId.Activo,
            CreadoPor      = "DevSeeder",
            CreadoEn       = DateTime.UtcNow,
        };
        usuario.Roles.Add(rol);

        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();

        logger.LogInformation("[DevSeeder] Usuario QA creado: {Correo}", correoQa);
    }
}
