namespace PROY.Models
{
    public class TipoIntervencion
    {
        public int IdTipoIntervencion { get; set; }
        public required string Nombre { get; set; }
        public int IdDepto { get; set; }
        public int Estado { get; set; } = 0;
        public required Deptos Depto { get; set; }
    }
}