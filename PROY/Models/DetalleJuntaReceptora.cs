namespace PROY.Models
{
    public class DetalleJuntaReceptora
    {
        public Int64 IdDetalleJuntaReceptora { get; set; }
        public int? IdJuntaReceptora { get; set; }
        public string? NombreCompleto { get; set; }
        public string? DPI { get; set; }
        public int? IdTipoNombramiento { get; set; }
        public int? EstaPresente { get; set; }
        public int? Estado { get; set; }
    }
}
