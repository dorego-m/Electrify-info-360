namespace Electrify.Models;

public class InfraestructuraViewModel
{
    public string NombreSede { get; set; } = "";
    public string DireccionSede { get; set; } = "";
    public string CiudadSede { get; set; } = "";

    public int IDSede { get; set; }
    public string NombreSector { get; set; } = "";
    public string? DescripcionSector { get; set; }

    public int IDSector { get; set; }
    public string NombreDispositivo { get; set; } = "";
    public string TipoDispositivo { get; set; } = "";
    public decimal? PotenciaDispositivo { get; set; }
}
