namespace Electrify.Models;

public class Sede
{
    public int IDSede { get; set; }
    public int IDEmpresa { get; set; }
    public string Nombre { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Ciudad { get; set; } = "";
    public bool Activo { get; set; } = true;
}
