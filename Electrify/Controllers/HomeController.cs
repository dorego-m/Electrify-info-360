using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Electrify.Models;

namespace Electrify.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Email")))
            return RedirectToAction("Bienvenida");

        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contraseña))
        {
            ViewBag.Error = "Completa el email y la contraseña.";
            return View();
        }

        BD bd = new BD();
        Usuario? usuario = bd.ValidarUsuario(email, contraseña);

        if (usuario == null)
        {
            ViewBag.Error = "Email o contraseña incorrectos.";
            return View();
        }

        HttpContext.Session.SetInt32("IDUsuario", usuario.IDUsuario);
        HttpContext.Session.SetInt32("IDEmpresa", usuario.IDEmpresa);
        HttpContext.Session.SetString("Nombre", usuario.Nombre);
        HttpContext.Session.SetString("Apellido", usuario.Apellido);
        HttpContext.Session.SetString("Email", usuario.Email);
        HttpContext.Session.SetString("Rol", usuario.Rol);

        return RedirectToAction("Bienvenida");
    }

    [HttpGet]
    public IActionResult CrearCuenta()
    {
        CargarEmpresas();
        return View();
    }

    [HttpPost]
    public IActionResult CrearCuenta(int idEmpresa, string nombre, string apellido,
                                    string email, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(contraseña) || idEmpresa <= 0)
        {
            ViewBag.Error = "Completa todos los campos y elegi una empresa.";
            CargarEmpresas();
            return View();
        }

        BD bd = new BD();

        if (bd.ValidarEmail(email) != null)
        {
            ViewBag.Error = "Ya existe una cuenta registrada con ese email.";
            CargarEmpresas();
            return View();
        }

        if (!bd.ExisteEmpresa(idEmpresa))
        {
            ViewBag.Error = "La empresa seleccionada no es valida. Elegi una de la lista.";
            CargarEmpresas();
            return View();
        }


        // Las cuentas nuevas se registran con el rol Usuario.
        bd.NuevoUsuario(idEmpresa, nombre, apellido, email, contraseña, "Usuario");

        Usuario? usuario = bd.ValidarUsuario(email, contraseña);
        if (usuario != null)
        {
            HttpContext.Session.SetInt32("IDUsuario", usuario.IDUsuario);
            HttpContext.Session.SetInt32("IDEmpresa", usuario.IDEmpresa);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);
            HttpContext.Session.SetString("Apellido", usuario.Apellido);
            HttpContext.Session.SetString("Email", usuario.Email);
            HttpContext.Session.SetString("Rol", usuario.Rol);
        }

        return RedirectToAction("Bienvenida");
    }

        [HttpGet]
        public IActionResult RegistrarEmpresa()
        {
            BD bd = new BD();
            ViewBag.Planes = bd.ObtenerPlanes();

            return View();
        }

        [HttpPost]
    public IActionResult RegistrarEmpresa(
        string RazonSocial, string? CUIT,
        string? EmailEmpresa, string? Telefono,
        string NombreAdmin, string ApellidoAdmin,
        string EmailAdmin, string ContrasenaAdmin,
        int IDPlan, string MetodoPago)
    {
        BD bd = new BD();

        if (string.IsNullOrWhiteSpace(RazonSocial) ||
            string.IsNullOrWhiteSpace(NombreAdmin) ||
            string.IsNullOrWhiteSpace(ApellidoAdmin) ||
            string.IsNullOrWhiteSpace(EmailAdmin) ||
            string.IsNullOrWhiteSpace(ContrasenaAdmin) ||
            IDPlan <= 0)
        {
            ViewBag.Error = "Completa todos los campos obligatorios.";
            ViewBag.Planes = bd.ObtenerPlanes();
            return View();
        }

        try
        {
            bd.RegistrarEmpresa(
                RazonSocial, CUIT, EmailEmpresa, Telefono,
                NombreAdmin, ApellidoAdmin, EmailAdmin,
                ContrasenaAdmin, IDPlan, MetodoPago);

            return RedirectToAction("Login");
        }
        catch (Exception)
        {
            ViewBag.Error = "No se pudo registrar la empresa. Verifica los datos e intenta nuevamente.";
            ViewBag.Planes = bd.ObtenerPlanes();
            return View();
        }
    }
        
    private void CargarEmpresas()
    {
        BD bd = new BD();
        ViewBag.Empresas = bd.ObtenerEmpresas();
    }

    [HttpGet]
public IActionResult Bienvenida()
{
    int? idEmpresa = HttpContext.Session.GetInt32("IDEmpresa");

    if (idEmpresa == null)
        return RedirectToAction("Login");

    BD bd = new BD();

    ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
    ViewBag.Apellido = HttpContext.Session.GetString("Apellido");
    ViewBag.Email = HttpContext.Session.GetString("Email");
    ViewBag.Rol = HttpContext.Session.GetString("Rol");
    ViewBag.NombreEmpresa = bd.ObtenerNombreEmpresa(idEmpresa.Value);

    return View();
}

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    
    private bool EsAdministradorEmpresa()
    {
        return HttpContext.Session.GetInt32("IDUsuario") != null
            && HttpContext.Session.GetInt32("IDEmpresa") != null
            && (string.Equals(HttpContext.Session.GetString("Rol"), "AdministradorEmpresa", StringComparison.OrdinalIgnoreCase)
                || string.Equals(HttpContext.Session.GetString("Rol"), "AdminEmpresa", StringComparison.OrdinalIgnoreCase));
    }

    private IActionResult? VerificarAdministrador()
    {
        if (HttpContext.Session.GetInt32("IDUsuario") == null)
            return RedirectToAction("Login");
        if (!EsAdministradorEmpresa())
            return RedirectToAction("Usuario");
        return null;
    }

    [HttpGet]
    public IActionResult Usuario()
    {
        if (HttpContext.Session.GetInt32("IDUsuario") == null)
            return RedirectToAction("Login");
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        BD bd = new BD();
        ViewBag.Nombre = HttpContext.Session.GetString("Nombre");
        ViewBag.Apellido = HttpContext.Session.GetString("Apellido");
        ViewBag.Rol = HttpContext.Session.GetString("Rol");
        ViewBag.NombreEmpresa = bd.ObtenerNombreEmpresa(idEmpresa);
        ViewBag.CantidadSedes = bd.ObtenerSedes(idEmpresa, true).Count;
        ViewBag.CantidadSectores = bd.ObtenerSectores(idEmpresa, true).Count;
        ViewBag.CantidadDispositivos = bd.ObtenerDispositivos(idEmpresa, true).Count;
        ViewBag.UltimosRegistros = bd.ObtenerRegistrosConsumo(idEmpresa).Take(5).ToList();
        return View();
    }

    [HttpGet]
    public IActionResult Infraestructura()
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        CargarInfraestructura();
        return View(new InfraestructuraViewModel());
    }

    [HttpPost]
    public IActionResult CrearSede(InfraestructuraViewModel model)
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        if (string.IsNullOrWhiteSpace(model.NombreSede) || string.IsNullOrWhiteSpace(model.DireccionSede) || string.IsNullOrWhiteSpace(model.CiudadSede))
            TempData["ErrorInfraestructura"] = "Completa nombre, direccion y ciudad de la sede.";
        else
        {
            try { new BD().CrearSede(idEmpresa, model.NombreSede.Trim(), model.DireccionSede.Trim(), model.CiudadSede.Trim()); TempData["MensajeInfraestructura"] = "Sede creada correctamente."; }
            catch { TempData["ErrorInfraestructura"] = "No se pudo crear la sede. Verifica los datos."; }
        }
        return RedirectToAction("Infraestructura");
    }

    [HttpPost]
    public IActionResult CrearSector(InfraestructuraViewModel model)
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        if (model.IDSede <= 0 || string.IsNullOrWhiteSpace(model.NombreSector))
            TempData["ErrorInfraestructura"] = "Selecciona una sede e ingresa el nombre del sector.";
        else
        {
            try { new BD().CrearSector(idEmpresa, model.IDSede, model.NombreSector.Trim(), model.DescripcionSector); TempData["MensajeInfraestructura"] = "Sector creado correctamente."; }
            catch { TempData["ErrorInfraestructura"] = "No se pudo crear el sector. Verifica que la sede este activa y pertenezca a tu empresa."; }
        }
        return RedirectToAction("Infraestructura");
    }

    [HttpPost]
    public IActionResult CrearDispositivo(InfraestructuraViewModel model)
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        if (model.IDSector <= 0 || string.IsNullOrWhiteSpace(model.NombreDispositivo) || string.IsNullOrWhiteSpace(model.TipoDispositivo) || model.PotenciaDispositivo <= 0)
            TempData["ErrorInfraestructura"] = "Completa el sector, nombre, tipo y una potencia mayor a cero.";
        else
        {
            try { new BD().CrearDispositivo(idEmpresa, model.IDSector, model.NombreDispositivo.Trim(), model.TipoDispositivo.Trim(), model.PotenciaDispositivo); TempData["MensajeInfraestructura"] = "Dispositivo creado correctamente."; }
            catch { TempData["ErrorInfraestructura"] = "No se pudo crear el dispositivo. Verifica que el sector este activo y pertenezca a tu empresa."; }
        }
        return RedirectToAction("Infraestructura");
    }

    [HttpPost]
    public IActionResult CambiarEstadoInfraestructura(string tipo, int id, bool activo)
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        try
        {
            new BD().CambiarEstadoInfraestructura(tipo, id, HttpContext.Session.GetInt32("IDEmpresa")!.Value, activo);
            TempData["MensajeInfraestructura"] = activo ? "Elemento reactivado." : "Elemento desactivado. Se conserva su historial.";
        }
        catch { TempData["ErrorInfraestructura"] = "No se pudo cambiar el estado del elemento."; }
        return RedirectToAction("Infraestructura");
    }

    [HttpGet]
    public IActionResult Formulario()
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        CargarDatosFormulario();
        return View(new FormularioConsumoViewModel());
    }

    [HttpGet]
    public IActionResult EditarFormulario(int id)
    {
        var acceso = VerificarAdministrador();
        if (acceso != null) return acceso;
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        var resultado = new BD().ObtenerRegistroConsumo(id, idEmpresa);
        if (resultado.Modelo == null) return NotFound();
        CargarDatosFormulario();
        return View("Formulario", resultado.Modelo);
    }

    [HttpPost]
    public IActionResult Formulario(FormularioConsumoViewModel model)
    {
        var acceso = VerificarAdministrador();

        // Esta nos ayudo la IA pq sino se nos re complicaba y lo terminabamos en 10 años 
        if (acceso != null) return acceso;
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        int idUsuario = HttpContext.Session.GetInt32("IDUsuario")!.Value;

        model.Detalles = model.Detalles?.Where(d => d.IDDispositivo > 0).ToList() ?? new List<DetalleConsumoInput>();
        if (model.IDSede <= 0 || model.FechaInicio.Date > model.FechaFin.Date || model.TarifaKWh < 0 || model.Detalles.Count == 0 ||
            model.Detalles.Any(d => d.Cantidad < 1 || d.HorasPorDia <= 0 || d.HorasPorDia > 24 || d.DiasUso < 1 || d.DiasUso > (model.FechaFin.Date - model.FechaInicio.Date).Days + 1))
        {
            ViewBag.Error = "Revisa la sede, el período, la tarifa y los datos de cada dispositivo.";
            if (model.Detalles.Count == 0) model.Detalles.Add(new DetalleConsumoInput());
            CargarDatosFormulario();
            return View(model);
        }

        try
        {
            if (model.IDRegistro.HasValue && model.IDRegistro.Value > 0)
                new BD().ActualizarRegistroConsumo(model.IDRegistro.Value, idEmpresa, idUsuario, model);
            else
                new BD().CrearRegistroConsumo(idEmpresa, idUsuario, model);
            TempData["MensajeConsumo"] = model.IDRegistro.HasValue ? "La carga se actualizo correctamente." : "La carga de consumo se guardo correctamente.";
            return RedirectToAction("Formulario");
        }
        catch
        {
            ViewBag.Error = "No se pudo guardar la carga. Verifica que los dispositivos esten activos y pertenezcan a la sede seleccionada.";
            if (model.Detalles.Count == 0) model.Detalles.Add(new DetalleConsumoInput());
            CargarDatosFormulario();
            return View(model);
        }
    }

    private void CargarInfraestructura()
    {
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        BD bd = new BD();
        ViewBag.Sedes = bd.ObtenerSedes(idEmpresa, false);
        ViewBag.Sectores = bd.ObtenerSectores(idEmpresa, false);
        ViewBag.Dispositivos = bd.ObtenerDispositivos(idEmpresa, false);
    }

    private void CargarDatosFormulario()
    {
        int idEmpresa = HttpContext.Session.GetInt32("IDEmpresa")!.Value;
        BD bd = new BD();
        ViewBag.Sedes = bd.ObtenerSedes(idEmpresa, true);
        ViewBag.Sectores = bd.ObtenerSectores(idEmpresa, true);
        ViewBag.Dispositivos = bd.ObtenerDispositivos(idEmpresa, true);
        ViewBag.Registros = bd.ObtenerRegistrosConsumo(idEmpresa);
    }

       [HttpGet]
    public IActionResult Estrategias()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Informes()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
