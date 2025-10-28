namespace PROY.Models
{
    public class Bodega
    {
        public Int64 IdBodega { get; set; }
        public string? NombreBodega { get; set; }
        public string? DireccionBodega { get; set; }
        public int? IdTipoBodega { get; set; }
        public int? IdDepartamento { get; set; }
        public int? IdMunicipio { get; set; }
        public int? IdUsuario { get; set; }
        public int Estado { get; set; }
    }
}
