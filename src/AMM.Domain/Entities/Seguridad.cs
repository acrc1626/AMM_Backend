using System;
using System.Collections.Generic;
using AMM.Domain.Common;

namespace AMM.Domain.Entities;

public class Usuario : AuditableEntity
{
    public int Id { get; set; }
    public Guid? AzureAdObjectId { get; set; }
    public string Correo { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public int? EntidadId { get; set; }
    public byte EstadoUsuarioId { get; set; }

    public virtual EstadoUsuario EstadoUsuario { get; set; } = null!;
    public virtual ICollection<Rol> Roles { get; set; } = new List<Rol>();
}

public class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
    public virtual ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
}

public class Permiso
{
    public int Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string? Descripcion { get; set; }

    public virtual ICollection<Rol> Roles { get; set; } = new List<Rol>();
}

public class Menu
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Ruta { get; set; } = null!;

    public virtual ICollection<Rol> Roles { get; set; } = new List<Rol>();
}
