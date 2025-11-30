namespace AMM.Domain.Entities;

public class Departamento
{
    public short Id { get; set; }
    public string CodigoDane { get; set; } = null!;
    public string Nombre { get; set; } = null!;
}

public class Municipio
{
    public int Id { get; set; }
    public short DepartamentoId { get; set; }
    public string CodigoDaneDpto { get; set; } = null!;
    public string Nombre { get; set; } = null!;

    public virtual Departamento Departamento { get; set; } = null!;
}

public class Territorio
{
    public int Id { get; set; }
    public int MunicipioId { get; set; }
    public string Nombre { get; set; } = null!;

    public virtual Municipio Municipio { get; set; } = null!;
}

public class Microterritorio
{
    public int Id { get; set; }
    public int TerritorioId { get; set; }
    public string Nombre { get; set; } = null!;

    public virtual Territorio Territorio { get; set; } = null!;
}

public class Area
{
    public int Id { get; set; }
    public int MicroterritorioId { get; set; }
    public string Nombre { get; set; } = null!;

    public virtual Microterritorio Microterritorio { get; set; } = null!;
}

public class Ubicacion
{
    public long Id { get; set; }
    public string? Direccion { get; set; }
    public double? Lat { get; set; }
    public double? Lon { get; set; }
    public object? Geo { get; set; }
}
