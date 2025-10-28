namespace PROY.Models
{
    public class Kardex
    {
        public Int64 IdKardex { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public int? IdBodega { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public string? Estiba { get; set; }
        public int? IdConvenio { get; set; }
        public int? IdPresentacion { get; set; }
        public decimal? CantidadUnidad { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? IdRegistro { get; set; }
        public string? Tabla { get; set; }
        public int? Ingreso { get; set; }
        public int? Egreso { get; set; }
        public decimal? Saldo { get; set; }
        public int? Estado { get; set; }
    }
}
