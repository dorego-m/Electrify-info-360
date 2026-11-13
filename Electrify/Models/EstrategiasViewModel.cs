
namespace Electrify.Models;

public class EstrategiasViewModel
{
    public List<ConsumoPorSector> Sectores { get; set; } = new();
    public List<ConsumoPorDispositivo> Dispositivos { get; set; } = new();
    public List<Estrategia> EstrategiasAplicadas { get; set; } = new();

    public decimal ConsumoTotal { get; set; }
    public decimal CostoTotal { get; set; }

    public string Diagnostico { get; set; } = "";
    public string DispositivoPrincipal { get; set; } = "";
    public string RecomendacionPrincipal { get; set; } = "";
}

public class ConsumoPorSector
{
    public string NombreSector { get; set; } = "";
    public decimal ConsumoKWh { get; set; }
    public decimal Porcentaje { get; set; }
}

public class ConsumoPorDispositivo
{
    public int IDDispositivo { get; set; }
    public string Nombre { get; set; } = "";
    public string Tipo { get; set; } = "";
    public decimal ConsumoKWh { get; set; }
    public decimal Porcentaje { get; set; }
}

public class Estrategia
{
    public int IDEstrategia { get; set; }
    public int IDEmpresa { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string Objetivo { get; set; } = "";
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Estado { get; set; } = "";
}
