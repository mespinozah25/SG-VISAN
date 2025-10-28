using PROY.Models;

namespace PROY.ViewModels
{
    public class SolicitudSalidaAlimentosVM
    {
        public SolicitudSalidaAlimentos SolicitudSalidaAlimentos { get; set; }
        public List<SolicitudSalidaAlimentos> listaSolicitudSalidaAlimentos { get; set; }
        public DetalleSalidaAlimentos DetalleSalidaAlimentos { get; set; }
        public List<DetalleSalidaAlimentos> listaDetalleSalidaAlimentos { get; set; }
        public Dictionary<Int64, string>? TransportistaDict { get; set; }
        public Dictionary<Int64, string>? PilotoDict { get; set; }
        public Dictionary<Int64, string>? VehiculoDict { get; set; }
        public Dictionary<Int64, string>? ProgramaDict { get; set; }
    }
}
