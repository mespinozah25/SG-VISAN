using PROY.Models;

namespace PROY.ViewModels
{
    public class RacionVM
    {
        public RacionMaestro Racion { get; set; }
        public List<RacionMaestro> listaRacion { get; set; }
        public List<dynamic> listaRacionReporte { get; set; }
        public List<RacionDetalle> listaRacionDetalle { get; set; }
        public Dictionary<Int64, string>? ProductoAlimenticioDict { get; set; }
        public Dictionary<Int64, string>? ConvenioDict { get; set; }
        public Dictionary<Int64, string>? ProgramaDict { get; set; }
        public Dictionary<Int64, string>? PresentacionPADict { get; set; }
        public Dictionary<Int64, string>? UnidadMedidaDict { get; set; }
    }
}
