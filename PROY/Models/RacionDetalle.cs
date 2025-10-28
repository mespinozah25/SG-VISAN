namespace PROY.Models
{
    public class RacionDetalle
    {
        public Int64 IdRacionDetalle { get; set; }
        public int? IdRacionMaestro { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public decimal? CantidadMedida { get; set; }
        public int? IdUnidadMedida { get; set; }
        public decimal? Kilocalorias { get; set; }
        public decimal? Carbohidratos { get; set; }
        public decimal? Proteinas { get; set; }
        public decimal? Grasas { get; set; }
        public int Estado { get; set; }
    }
}
