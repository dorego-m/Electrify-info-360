namespace Electrify.Models;

public class RegistroConsumo
{
    public int IDRegistro { get; set; }
    public int IDSede { get; set; }
    public string NombreSede { get; set; } = "";
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public DateTime FechaCreacion { get; set; }
    public decimal ConsumoTotal { get; set; }
    public decimal CostoEstimado { get; set; }
}
