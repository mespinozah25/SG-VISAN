namespace PROY.Models
{
    public class DetalleEvaluacionAPA
    {
        public Int64 IdDetalleEvaluacion { get; set; }
        public Int64 IdSolicitud { get; set; }
        public Solicitud Solicitud { get; set; }
        //public string Descripcion { get; set; }
        public int IdTipoValidacion { get; set; }
        public string? Archivo { get; set; }
        public string Observaciones { get; set; }
        public string Proceso { get; set; }
        public int Restrictiva { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
    }
}
