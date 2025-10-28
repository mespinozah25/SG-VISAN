namespace PROY.Models
{
    public class DetalleWaybill
    {
        public Int64 IdDetalleWaybill { get; set; }
        public int? IdIngresoAlimentos { get; set; }
        public string? NoBoleta { get; set; }
        public DateTime? FechaBoleta { get; set; }
        public int Estado { get; set; }
    }
}
