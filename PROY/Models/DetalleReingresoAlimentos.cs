namespace PROY.Models
{
    public class DetalleReingresoAlimentos
    {
        public Int64 IdDetalleReingresoAlimentos { get; set; }
        public int? IdReingresoAlimentos { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public int? IdPresentacion { get; set; }
        public int? CantidadUnidad { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? Cantidad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? Estiba { get; set; }
        public int Estado { get; set; }
    }
}
