namespace Electrify.Models
{
    public class InformeConsumoDiario
    {
        public DateTime Fecha { get; set; }

        public decimal ConsumoKWhEstimado { get; set; }

        public decimal CostoEstimado { get; set; }
    }
}