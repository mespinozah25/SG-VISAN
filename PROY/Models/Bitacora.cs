namespace PROY.Models
{
    public class Bitacora
    {
        public Int64 IdBitacora { get; set; }
        public int? Accion { get; set; }
        public int? IdRegistro { get; set; }
        public string? NombreTabla { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime FechaHora { get; set; }
        public int? Estado { get; set; }
    }
}
