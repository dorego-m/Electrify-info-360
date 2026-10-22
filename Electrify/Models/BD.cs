using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Electrify.Models;

public class BD
{
    private readonly string _connectionString =
    @"Server=localhost;
      Database=Electrify;
      Integrated Security=True;
      TrustServerCertificate=True;";

    public Usuario? ValidarUsuario(string email, string contraseña)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.QueryFirstOrDefault<Usuario>(
            "sp_Usuario_Login",
            new { Email = email, Contraseña = contraseña },
            commandType: CommandType.StoredProcedure);
    }

    public Usuario? ValidarEmail(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.QueryFirstOrDefault<Usuario>(
            "sp_Usuario_ObtenerPorEmail",
            new { Email = email },
            commandType: CommandType.StoredProcedure);
    }

    public List<Empresa> ObtenerEmpresas()
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Empresa>(
            "sp_Empresa_Listar",
            commandType: CommandType.StoredProcedure).ToList();
    }

    public bool ExisteEmpresa(int idEmpresa)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.QueryFirstOrDefault<int>(
            "sp_Empresa_Existe",
            new { IDEmpresa = idEmpresa },
            commandType: CommandType.StoredProcedure) == 1;
    }

    public void NuevoUsuario(int idEmpresa, string nombre, string apellido,
                             string email, string contraseña, string rol)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute(
            "sp_Usuario_Crear",
            new
            {
                IDEmpresa = idEmpresa,
                Nombre = nombre,
                Apellido = apellido,
                Email = email,
                Contraseña = contraseña,
                Rol = rol
            },
            commandType: CommandType.StoredProcedure);
    }

    public string ObtenerNombreEmpresa(int idEmpresa)
{
    using var connection = new SqlConnection(_connectionString);

    return connection.QueryFirstOrDefault<string>(
        "sp_Empresa_ObtenerNombre",
        new { IDEmpresa = idEmpresa },
        commandType: CommandType.StoredProcedure
    ) ?? "Empresa no encontrada";
}

public List<PlanSuscripcion> ObtenerPlanes()
{
    using var connection = new SqlConnection(_connectionString);

    return connection.Query<PlanSuscripcion>(
        "sp_PlanSuscripcion_ListarActivos",
        commandType: CommandType.StoredProcedure
    ).ToList();
}

public void RegistrarEmpresa(
    string razonSocial, string? cuit, string? emailEmpresa,
    string? telefono, string nombreAdmin, string apellidoAdmin,
    string emailAdmin, string contrasenaAdmin, int idPlan,
    string metodoPago)
{
    using var connection = new SqlConnection(_connectionString);

    connection.Execute(
        "sp_Empresa_Registrar",
        new
        {
            RazonSocial = razonSocial,
            CUIT = cuit,
            EmailEmpresa = emailEmpresa,
            Telefono = telefono,
            NombreAdmin = nombreAdmin,
            ApellidoAdmin = apellidoAdmin,
            EmailAdmin = emailAdmin,
            ContrasenaAdmin = contrasenaAdmin,
            IDPlan = idPlan,
            MetodoPago = metodoPago
        },
        commandType: CommandType.StoredProcedure
    );
}


}
