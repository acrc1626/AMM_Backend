namespace AMM.Domain.Entities;

public class Estado
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}

public class EstadoUsuario
{
    public byte Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
}

public class TipoDocumento
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class Sexo
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class Etnia
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public string Codigo { get; set; } = null!;
}

public class PuebloIndigena
{
    public short Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class TipoEntorno
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class TipoNovedad
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class PresenciaNovedad
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class TipoSupervision
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class MotivoNoTratamiento
{
    public short Id { get; set; }
    public string Descripcion { get; set; } = null!;
}

public class Parentesco
{
    public byte Id { get; set; }
    public string Descripcion { get; set; } = null!;
}
