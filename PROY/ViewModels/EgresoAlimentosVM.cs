using PROY.Models;

namespace PROY.ViewModels
{
    public class EgresoAlimentosVM
    {
        public int IdSolicitud { get; set; }
        public Solicitud Solicitud { get; set; }
        public List<Solicitud> ListaSolicitud { get; set; }
        public EgresoAlimentos EgresoAlimentos { get; set; }
        public List<EgresoAlimentos>? listaEgresoAlimentos { get; set; }
        public List<Bitacora> listaBitacoraInicio { get; set; }
        public List<DetalleEgresoAlimentos>? listaDetalleEgresoAlimentos { get; set; }
        public List<RacionDetalle>? listaDetalleRacion { get; set; }
        public SolicitudSalidaAlimentos? SolicitudSalidaAlimentos { get; set; }
        public List<SolicitudSalidaAlimentos>? listaSolicitudSalidaAlimentos { get; set; }
        public List<DetalleSalidaAlimentos>? listaDetalleSalidaAlimentos { get; set; }
        public TrasladoAlimentos? TrasladoAlimentos { get; set; }
        public List<TrasladoAlimentos>? listaTrasladoAlimentos { get; set; }
        public List<Usuario>? listaUsuarios { get; set; }
        public string Rol { get; set; }
        public int IdUsuario { get; set; }
        public bool ApruebaRechazo { get; set; }
        public Dictionary<Int64, string>? SolicitudesDict { get; set; }
        public Dictionary<Int64, string>? ProgramaDict { get; set; }
        public Dictionary<Int64, string>? ConvenioDict { get; set; }
        public Dictionary<int, string> DepartamentoDict { get; set; }
        public Dictionary<int, string> MunicipioDict { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }
        public Dictionary<Int64, string>? TransportistaDict { get; set; }
        public Dictionary<Int64, string>? VehiculoDict { get; set; }
        public Dictionary<Int64, string>? PilotoDict { get; set; }
        public Dictionary<Int64, string> EstadoEgresoDict { get; set; }
        public Dictionary<Int64, string> EstadoTrasladoDict { get; set; }
        public Dictionary<Int64, string> TipoAsistenciaDict { get; set; }
        public Dictionary<Int64, string> UsuariosDict { get; set; }
    }
}
