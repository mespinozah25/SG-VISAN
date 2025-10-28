namespace PROY.Models
{
    public class ProductosIngreso
    {
        public Int64 IdProductosIngreso { get; set; }
        public int? IdIngresoAlimentos { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public string? Estiba { get; set; }
        public int? IdPresentacion { get; set; }
        public decimal? CantidadUnidad { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? CantidadRecibir { get; set; }
        public int? IdBoletaWaybill { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? FolioLibroAlmacen { get; set; }
        public decimal? PrecioUnidad { get; set; }
        public decimal? ValorTotal { get; set; }
        public int? Estado { get; set; }
    }
}
