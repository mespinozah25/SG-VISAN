namespace PROY.Models
{
    public class MovimientosProductos
    {
        public Int64 IdMovimientosProductos { get; set; }
        public int? IdBodega { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public string? Estiba { get; set; }
        public int? IdConvenio { get; set; }
        public int? IdPresentacion { get; set; }
        public decimal? CantidadUnidad { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? Existencias { get; set; }        
        public DateTime? FechaVencimiento { get; set; }
        public int? IdRegistro { get; set; }
        public string? Tabla { get; set; }
        public int? Estado { get; set; }
    }
}
