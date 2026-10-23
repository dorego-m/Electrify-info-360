namespace Electrify.Models;

public class FormularioConsumoViewModel
{
    public int? IDRegistro { get; set; }
    public int IDSede { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.Today.AddDays(-29);
    public DateTime FechaFin { get; set; } = DateTime.Today;
    public decimal TarifaKWh { get; set; } = 150m;
    public List<DetalleConsumoInput> Detalles { get; set; } = new() { new DetalleConsumoInput() };
}

public class DetalleConsumoInput
{
    public int IDDispositivo { get; set; }
    public int Cantidad { get; set; } = 1;
    public decimal HorasPorDia { get; set; } = 8m;
    public int DiasUso { get; set; } = 22;
}
