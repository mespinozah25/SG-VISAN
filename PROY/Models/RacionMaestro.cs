namespace PROY.Models
{
    public class RacionMaestro
    {
        public Int64 IdRacionMaestro { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdConvenio { get; set; }
        public decimal? TotalKilocalorias { get; set; }
        public decimal? TotalCarbohidratos { get; set; }
        public decimal? TotalProteinas { get; set; }
        public decimal? TotalGrasas { get; set; }
        public decimal? PorcentajeKilocalorias { get; set; }
        public decimal? PorcentajeCarbohidratos { get; set; }
        public decimal? PorcentajeProteinas { get; set; }
        public decimal? PorcentajeGrasas { get; set; }
        public int? IdUsuario { get; set; }
        public int Estado { get; set; }
        public List<RacionDetalle> RacionDetalle { get; set; } = new List<RacionDetalle>();
    }
}
