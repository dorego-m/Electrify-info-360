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
