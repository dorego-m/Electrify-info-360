namespace Electrify.Models;

public class Dispositivo
{
    public int IDDispositivo { get; set; }
    public int IDSector { get; set; }
    public string Nombre { get; set; } = "";
    public string Tipo { get; set; } = "";
    public decimal? Potencia { get; set; }
    public string Estado { get; set; } = "Activo";
    public string NombreSector { get; set; } = "";
    public string NombreSede { get; set; } = "";
    public int IDSede { get; set; }
    public bool Activo { get; set; } = true;
}
