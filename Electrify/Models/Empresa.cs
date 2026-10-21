namespace Electrify.Models;

public class Empresa
{
    public int IDEmpresa { get; set; }
    public string RazonSocial { get; set; } = "";
    public string CUIT { get; set; } = "";
    public string Email { get; set; } = "";
    public int telefono { get; set; }
}
