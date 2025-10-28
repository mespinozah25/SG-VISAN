namespace PROY.Models
{
    public class TrasladoAlimentos
    {
        public Int64 IdTrasladoAlimentos { get; set; }
        public int? IdEgresoAlimentos { get; set; }
        public int? AutorizaRecepcion { get; set; }
        public string? ArchivoDespachoAlmacenTraslado { get; set; }
        public int? IdEstadoTraslado { get; set; }
        public int? Estado { get; set; }
    }
}
