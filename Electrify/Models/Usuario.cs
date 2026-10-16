namespace Electrify.Models;

public class Usuario
{
    public int IDUsuario { get; set; }
    public int IDEmpresa { get; set; }
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Email { get; set; } = "";
    public string Contraseña { get; set; } = "";
    public string Rol { get; set; } = "";
}
