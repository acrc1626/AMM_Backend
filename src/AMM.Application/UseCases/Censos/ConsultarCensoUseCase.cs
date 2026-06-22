using AMM.Application.DTOs.Censos;
using AMM.Domain.Entities;
using AMM.Domain.Ports.Repositories;

namespace AMM.Application.UseCases.Censos;

/// <summary>
/// Consulta el detalle completo de un censo (HU-004):
/// caracterización, lista de personas con parentesco y estado,
/// y enfermedades por persona. Todo en un único DTO de respuesta.
/// </summary>
public sealed class ConsultarCensoUseCase
{
    private readonly ICensoRepository _censoRepository;

    /// <summary>Inicializa el caso de uso con el repositorio de censos.</summary>
    public ConsultarCensoUseCase(ICensoRepository censoRepository)
    {
        this._censoRepository = censoRepository;
    }

    /// <summary>
    /// Retorna el detalle completo del censo identificado por <paramref name="id"/>,
    /// o <c>null</c> si no existe.
    /// Incluye cantidad de personas y lista completa con enfermedades resueltas.
    /// </summary>
    public async Task<CensoDetalleDto?> GetDetalleAsync(long id, CancellationToken ct = default)
    {
        var censo = await this._censoRepository.GetDetalleAsync(id, ct);
        return censo is null ? null : this.MapToDetalle(censo);
    }

    // ─── Mapeo ───────────────────────────────────────────────────────────────

    private CensoDetalleDto MapToDetalle(Censo censo) => new(
        Id:                      censo.Id,
        TipoEntornoId:           censo.TipoEntornoId,
        TipoEntorno:             censo.TipoEntorno?.Descripcion ?? "",
        UbicacionId:             censo.UbicacionId,
        UbicacionDireccion:      censo.Ubicacion?.Direccion,
        DepartamentoId:          censo.DepartamentoId,
        DepartamentoNombre:      censo.Departamento?.Nombre,
        MunicipioId:             censo.MunicipioId,
        MunicipioNombre:         censo.Municipio?.Nombre,
        Fecha:                   censo.Fecha,
        Observacion:             censo.Observacion,
        Direccion:               censo.Direccion,
        ObjetoInterventor:       censo.ObjetoInterventor,
        Territorio:              censo.Territorio,
        Microterritorio:         censo.Microterritorio,
        Area:                    censo.Area,
        Geolocalizacion:         censo.Geolocalizacion,
        TotalMiembros:           censo.TotalMiembros,
        VisitantesTemporalesCol: censo.VisitantesTemporalesCol,
        VisitantesTemporalesMig: censo.VisitantesTemporalesMig,
        EstadoId:                censo.EstadoId,
        Estado:                  censo.Estado?.Nombre ?? "",
        CantidadPersonas:        censo.Personas.Count,
        Personas:                censo.Personas.Select(this.MapPersona).ToList()
    );

    private CensoPersonaDetalleDto MapPersona(CensoPersona p) => new(
        Id:                     p.Id,
        PacienteId:             p.PacienteId,
        PrimerNombre:           p.Paciente?.PrimerNombre,
        SegundoNombre:          p.Paciente?.SegundoNombre,
        PrimerApellido:         p.Paciente?.PrimerApellido,
        SegundoApellido:        p.Paciente?.SegundoApellido,
        Documento:              p.Paciente?.Documento,
        TipoDocumentoId:        p.Paciente?.TipoDocumentoId ?? 0,
        TipoDocumento:          p.Paciente?.TipoDocumento?.Tipo,
        SexoId:                 p.Paciente?.SexoId ?? 0,
        Sexo:                   p.Paciente?.Sexo?.Descripcion,
        FechaNacimiento:        p.Paciente?.FechaNacimiento,
        ParentescoId:           p.ParentescoId,
        Parentesco:             p.Parentesco?.Descripcion,
        EsJefeHogar:            p.EsJefeHogar,
        Comunidad:              p.Comunidad,
        GrupoPoblacionEspecial: p.GrupoPoblacionEspecial,
        EstadoPersonaId:        p.EstadoPersonaId,
        EstadoPersona:          p.EstadoPersona?.Nombre ?? "",
        Enfermedades:           p.Enfermedades.Select(this.MapEnfermedad).ToList()
    );

    private PersonaEnfermedadDetalleDto MapEnfermedad(PersonaEnfermedad e) => new(
        Id:           e.Id,
        EnfermedadId: e.EnfermedadId,
        Enfermedad:   e.Enfermedad?.Nombre ?? "",
        EstadoId:     e.EstadoId,
        Estado:       e.Estado?.Nombre ?? ""
    );
}
