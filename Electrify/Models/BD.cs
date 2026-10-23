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


    // ---------- Infraestructura consultas ----------
    public List<Sede> ObtenerSedes(int idEmpresa, bool soloActivas = false)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Sede>("sp_Infraestructura_SedesPorEmpresa",
            new { IDEmpresa = idEmpresa, SoloActivas = soloActivas },
            commandType: CommandType.StoredProcedure).ToList();
    }

    public List<Sector> ObtenerSectores(int idEmpresa, bool soloActivos = false)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Sector>("sp_Infraestructura_SectoresPorEmpresa",
            new { IDEmpresa = idEmpresa, SoloActivos = soloActivos },
            commandType: CommandType.StoredProcedure).ToList();
    }

    public List<Dispositivo> ObtenerDispositivos(int idEmpresa, bool soloActivos = false)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Dispositivo>("sp_Infraestructura_DispositivosPorEmpresa",
            new { IDEmpresa = idEmpresa, SoloActivos = soloActivos },
            commandType: CommandType.StoredProcedure).ToList();
    }

    // ---------- Infraestructura ----------
    public void CrearSede(int idEmpresa, string nombre, string direccion, string ciudad)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("sp_Sede_Crear", new { IDEmpresa = idEmpresa, Nombre = nombre, Direccion = direccion, Ciudad = ciudad }, commandType: CommandType.StoredProcedure);
    }

    public void CrearSector(int idEmpresa, int idSede, string nombre, string? descripcion)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("sp_Sector_Crear", new { IDEmpresa = idEmpresa, IDSede = idSede, Nombre = nombre, Descripcion = descripcion }, commandType: CommandType.StoredProcedure);
    }

    public void CrearDispositivo(int idEmpresa, int idSector, string nombre, string tipo, decimal? potencia)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("sp_Dispositivo_Crear", new { IDEmpresa = idEmpresa, IDSector = idSector, Nombre = nombre, Tipo = tipo, Potencia = potencia }, commandType: CommandType.StoredProcedure);
    }

    public void CambiarEstadoInfraestructura(string tipo, int id, int idEmpresa, bool activo)
    {
        string procedimiento = tipo switch
        {
            "Sede" => "sp_Sede_CambiarEstado",
            "Sector" => "sp_Sector_CambiarEstado",
            "Dispositivo" => "sp_Dispositivo_CambiarEstado",
            _ => throw new ArgumentException("Tipo de infraestructura invalido.")
        };
        string parametroId = tipo switch
        {
            "Sede" => "IDSede",
            "Sector" => "IDSector",
            _ => "IDDispositivo"
        };
        var parametros = new DynamicParameters();
        parametros.Add("IDEmpresa", idEmpresa);
        parametros.Add(parametroId, id);
        parametros.Add("Activo", activo);
        using var connection = new SqlConnection(_connectionString);
        connection.Execute(procedimiento, parametros, commandType: CommandType.StoredProcedure);
    }

    // ---------- Registros de consumo ----------
    public List<RegistroConsumo> ObtenerRegistrosConsumo(int idEmpresa)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<RegistroConsumo>("sp_RegistroConsumo_ListarPorEmpresa",
            new { IDEmpresa = idEmpresa }, commandType: CommandType.StoredProcedure).ToList();
    }

    public void CrearRegistroConsumo(int idEmpresa, int idUsuario, FormularioConsumoViewModel model)
    {
        var tabla = new DataTable();
        tabla.Columns.Add("IDDispositivo", typeof(int));
        tabla.Columns.Add("Cantidad", typeof(int));
        tabla.Columns.Add("HorasPorDia", typeof(decimal));
        tabla.Columns.Add("DiasUso", typeof(int));

        foreach (var detalle in model.Detalles)
            tabla.Rows.Add(detalle.IDDispositivo, detalle.Cantidad, detalle.HorasPorDia, detalle.DiasUso);

        using var connection = new SqlConnection(_connectionString);
        connection.Execute("sp_RegistroConsumo_Crear", new
        {
            IDEmpresa = idEmpresa,
            IDUsuario = idUsuario,
            IDSede = model.IDSede,
            FechaInicio = model.FechaInicio.Date,
            FechaFin = model.FechaFin.Date,
            TarifaKWh = model.TarifaKWh,
            Detalles = tabla.AsTableValuedParameter("dbo.DetalleConsumoCargaType")
        }, commandType: CommandType.StoredProcedure);
    }


    public (FormularioConsumoViewModel? Modelo, List<DetalleConsumoInput> Detalles) ObtenerRegistroConsumo(int idRegistro, int idEmpresa)
    {
        using var connection = new SqlConnection(_connectionString);
        using var multi = connection.QueryMultiple("sp_RegistroConsumo_Obtener",
            new { IDRegistro = idRegistro, IDEmpresa = idEmpresa }, commandType: CommandType.StoredProcedure);
        var encabezado = multi.Read<FormularioConsumoViewModel>().FirstOrDefault();
        var detalles = multi.Read<DetalleConsumoInput>().ToList();
        if (encabezado != null) encabezado.Detalles = detalles;
        return (encabezado, detalles);
    }

    public void ActualizarRegistroConsumo(int idRegistro, int idEmpresa, int idUsuario, FormularioConsumoViewModel model)
    {
        var tabla = new DataTable();
        tabla.Columns.Add("IDDispositivo", typeof(int));
        tabla.Columns.Add("Cantidad", typeof(int));
        tabla.Columns.Add("HorasPorDia", typeof(decimal));
        tabla.Columns.Add("DiasUso", typeof(int));
        foreach (var detalle in model.Detalles)
            tabla.Rows.Add(detalle.IDDispositivo, detalle.Cantidad, detalle.HorasPorDia, detalle.DiasUso);

        using var connection = new SqlConnection(_connectionString);
        connection.Execute("sp_RegistroConsumo_Actualizar", new
        {
            IDRegistro = idRegistro,
            IDEmpresa = idEmpresa,
            IDUsuario = idUsuario,
            IDSede = model.IDSede,
            FechaInicio = model.FechaInicio.Date,
            FechaFin = model.FechaFin.Date,
            TarifaKWh = model.TarifaKWh,
            Detalles = tabla.AsTableValuedParameter("dbo.DetalleConsumoCargaType")
        }, commandType: CommandType.StoredProcedure);
    }





}
