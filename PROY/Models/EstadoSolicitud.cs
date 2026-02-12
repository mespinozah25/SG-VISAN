namespace PROY.Models
{
    public class EstadoSolicitud
    {
        public int IdEstadoSolicitud { get; set; }
        public required string Nombre { get; set; }
        public int? IdEstadoSolicitudAnterior { get; set; }
        public int? IdEstadoSolicitudSiguiente { get; set; }
        public int IdDepto { get; set; }
        public EstadoSolicitud? EstadoSolicitudAnterior { get; set; }
        public EstadoSolicitud? EstadoSolicitudSiguiente { get; set; }
        public required Deptos Depto { get; set; }
    }
}