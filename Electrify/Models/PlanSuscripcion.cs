namespace Electrify.Models;

public class PlanSuscripcion
{
    public int IDPlan { get; set; }
    public string Nombre { get; set; } = "";
    public string PrecioMensual { get; set; }
}


//Modelo PlanSuscripcion: el código supone que existe Models/PlanSuscripcion.cs con las propiedades IDPlan, Nombre y PrecioMensual. Si todavía no lo creaste, vas a tener que agregarlo.



//Controlador: la acción RegistrarEmpresa debe cargar ViewBag.Planes con el resultado de sp_PlanSuscripcion_ListarActivos, tanto al mostrar el formulario como cuando se devuelve por un error de validación.



//JavaScript: usé validaciones-empresa.js porque los campos de este formulario son distintos de los del login y registro de empleados. Podés mantener validaciones-auth.js para las otras pantallas.