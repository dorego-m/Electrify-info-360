
namespace Electrify.Models
{
    public class InformeConsumoCategoria
    {
        public int IDSector { get; set; }
        public int IDDispositivo { get; set; }

        public string NombreSector { get; set; } = "";
        public string NombreDispositivo { get; set; } = "";

        public decimal ConsumoKWh { get; set; }
        public decimal CostoEstimado { get; set; }
    }
}
