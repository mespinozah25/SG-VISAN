using PROY.Data;
using PROY.Models;
using System.Collections.Generic;
using static PROY.Repositories.EjecucionRepository;
using static PROY.Repositories.SolicitudRepository;

namespace PROY.ViewModels
{
    public class SolicitudesVM
    {
        public Solicitud? Solicitud { get; set; }
        public List<Solicitud>? Solicitudes { get; set; }
        public List<Solicitud>? SolicitudesRechazadas { get; set; }
        public DetalleSolicitud? DetalleSolicitud { get; set; }
        public DetalleSolicitudAPA? DetalleSolicitudAPA { get; set; }
        public List<DetalleSolicitud>? listaDetallesSolicitud { get; set; }
        public List<DetalleSolicitudAPA>? listaDetallesSolicitudAPA { get; set; }
        public DetalleEvaluacion? DetalleEvaluacion { get; set; }
        public DetalleEvaluacionAPA? DetalleEvaluacionAPA { get; set; }
        public List<DetalleEvaluacion>? listaDetallesEvaluacion { get; set; }        
        public List<DetalleEvaluacionAPA>? listaDetallesEvaluacionAPA { get; set; }
        public Catalogo Catalogo { get; set; }
        public List<Catalogo>? listaCatalogos { get; set; }
        public Dictionary<int, string> InstitucionDict { get; set; }
        public Dictionary<int, string> UnidadDict { get; set; }
        public Dictionary<int, string> IntervencionDict { get; set; }
        //public Dictionary<int, string> Intervencion2Dict { get; set; }
        public Dictionary<int, string> DepartamentoDict { get; set; }
        public Dictionary<int, string> MunicipioDict { get; set; }
        public Dictionary<int, string> AnulacionDict { get; set; }
        public Dictionary<int, string> EstadoCivilDict { get; set; }
        public Dictionary<int, string> GrupoEtnicoDict { get; set; }
        public Dictionary<int, string> TecnicoDict { get; set; }
        public Dictionary<int, string> ProyectoDict { get; set; }
        public Dictionary<int, string> ValidacionDict { get; set; }
        public AsignaTecnico AsignaTecnico { get; set; }
        public AsignaTecnicoV2 AsignaTecnicoV2 { get; set; }
        public List<AsignaTecnicoV2> ListDetalleTecnicos { get; set; }
        public List<dynamic> DetalleTecnicos { get; set; }
        public List<Tecnico> Tecnicos { get; set; }
        public List<Agrupado> Agrupados { get; set; }
        public AsignaRacion AsignaRacion { get; set; }
        public List<AsignaRacion> DetalleRaciones { get; set; }
        public AsignaRacionV2 AsignaRacionV2 { get; set; }
        public List<AsignaRacionV2> DetalleRacionesV2 { get; set; }
        public List<EstadoSolicitud> EstadosSolicitud { get; set; } = new List<EstadoSolicitud>();
        public string rutaApp { get; } = ControlGlobal.rutaApp;
    }
 
}
