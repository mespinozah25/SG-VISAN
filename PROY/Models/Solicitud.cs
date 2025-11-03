using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROY.Models
{
    public class Solicitud
    {
        public Int64 IdSolicitud { get; set; }
        
        public string Correlativo { get; set; }  //Correlativo interno de los memos
        public string? Codigo { get; set; }

        [Required]
        public DateTime FechaIngresoDespacho { get; set; }

        public DateTime FechaIngresoDireccion { get; set; }

        [Required]
        public DateTime FechaIngresoUnidad { get; set; }

        [Required]
        public int EstadoEgreso { get; set; }
        public int IdUnidad { get; set; }
        public int IdIntervencion { get; set; }
        public string? Archivo {  get; set; }
        public int Restrictiva {  get; set; }
        public string Estado { get; set; }
        
        [NotMapped]
        public int? Validos { get; set; }
        [NotMapped]
        public int? NoAplica { get; set; }
        public int? Fallecidos { get; set; }
        public int? Duplicados { get; set; }
        public int? SinDatos { get; set; }
        public int? Gobierno { get; set; }
        public string? Depto { get; set; }
        public string ? Descripcion { get; set; }
        public int ? IdInstitucion { get; set; }
        public int ? IdProyecto { get; set; }
        public DateTime ? FInicioEntrega { get; set; }
        public DateTime ? FFinEntrega { get; set; }
        public string ? DireccionEntrega { get; set; }
        public string ? TecnicoNombre {  get; set; }
        public string ? TecnicoTelefono { get; set; }
        public string ? TecnicoObservaciones {  get; set; }
        public int Raciones { get; set; }
        public int ? Departamento { get; set; }
        public int ? Municipio { get; set; }
        public string ? Comunidad { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public List<DetalleSolicitud> DetallesSolicitud { get; set;}        
        public List<DetalleSolicitudAPA> DetallesSolicitudAPA { get; set; }
        public List<DetalleEvaluacion> DetallesEvaluacion { get; set; }
        public List<DetalleEvaluacionAPA> DetallesEvaluacionAPA { get; set; }
        //public List<AsignaTecnico> AsignaTecnicos { get; set; }
        [NotMapped]
        public Boolean? BotonDespacho { get; set; }

        [NotMapped]
        public bool MostrarContinuidadVerificacionCampo { get; set; } = false;

        [NotMapped]
        public bool MostrarDespachoEnProgramacion { get; set; } = false;

        [NotMapped]
        public bool MostrarDespachoEnEjecucion { get; set; } = false;

        [NotMapped]
        public bool AsignacionDeRacionesCompleta { get; set; } = false;
        
        public string? NombreCocode { get; set; }
        public string? ComunidadCocode { get; set; }
        public string? TelefonoCocode { get; set; }
        public int IdEstadoSolicitud { get; set; } = 1;
        public EstadoSolicitud EstadoSolicitud { get; set; }
        public int Rechazada { get; set; } = 0;
        public string? ComentarioRechazo { get; set; }
    }
}
