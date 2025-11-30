namespace AMM.Application.DTOs;

public record PacienteDto
{
    public long Id { get; init; }
    public string TipoDocumento { get; init; } = null!;
    public string Documento { get; init; } = null!;
    public string NombreCompleto { get; init; } = null!;
    public string Sexo { get; init; } = null!;
    public DateTime? FechaNacimiento { get; init; }
    public int? Edad { get; init; }
}

public record CrearPacienteRequest
{
    public byte TipoDocumentoId { get; init; }
    public string Documento { get; init; } = null!;
    public string PrimerNombre { get; init; } = null!;
    public string? SegundoNombre { get; init; }
    public string PrimerApellido { get; init; } = null!;
    public string? SegundoApellido { get; init; }
    public byte SexoId { get; init; }
    public DateTime? FechaNacimiento { get; init; }
    public byte? PertenenciaEtnicaId { get; init; }
    public short? PuebloIndigenaId { get; init; }
}

public record UpdatePacienteRequest
{
    public long Id { get; init; }
    public byte TipoDocumentoId { get; init; }
    public string Documento { get; init; } = null!;
    public string PrimerNombre { get; init; } = null!;
    public string? SegundoNombre { get; init; }
    public string PrimerApellido { get; init; } = null!;
    public string? SegundoApellido { get; init; }
    public byte SexoId { get; init; }
    public DateTime? FechaNacimiento { get; init; }
    public byte? PertenenciaEtnicaId { get; init; }
    public short? PuebloIndigenaId { get; init; }
    public byte EstadoId { get; init; }
}
