namespace Electrify.Models;

public class Sector
{
    public int IDSector { get; set; }
    public int IDSede { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; }
    public string NombreSede { get; set; } = "";
    public bool Activo { get; set; } = true;
}
