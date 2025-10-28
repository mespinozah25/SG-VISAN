using PROY.Models;

namespace PROY.ViewModels
{
    public class IngresoAlimentosVM
    {
        public IngresoAlimentos IngresoAlimentos { get; set; }
        public List<IngresoAlimentos> listaIngresoAlimentos { get; set; }
        public List<Bitacora> listaBitacoraInicio { get; set; }
        public List<Bitacora> listaBitacoraIngreso { get; set; }
        public List<ProductosIngreso> listaProductosIngreso { get; set; }
        public JuntaReceptora JuntaReceptora { get; set; }
        public DetalleWaybill DetalleWaybill { get; set; }
        public List<DetalleJuntaReceptora> listaDetalleJuntaReceptora { get; set; }
        public List<DetalleBoletaAnalisis> listaDetalleBoletaAnalisis { get; set; }
        public string Rol { get; set; }
        public int IdUsuario { get; set; }
        public List<Bodega> ListaBodegas { get; set; }
        public List<Bodega> BodegaRecibe { get; set; }
        public Dictionary<Int64, string>? ProgramaDict { get; set; }
        public Dictionary<Int64, string>? ConvenioDict { get; set; }
        public Dictionary<Int64, string>? JuntaReceptoraDict { get; set; }
        public Dictionary<int, string> DepartamentoDict { get; set; }
        public Dictionary<int, string> MunicipioDict { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }
        public Dictionary<Int64, string>? EstibaDict { get; set; }
        public Dictionary<Int64, string>? TransportistaDict { get; set; }
        public Dictionary<Int64, string>? VehiculoDict { get; set; }
        public Dictionary<Int64, string>? PilotoDict { get; set; }
        public Dictionary<Int64, string> TipoNombramientoDict { get; set; }
        public Dictionary<Int64, string> ProveedorDict { get; set; }
        public Dictionary<Int64, string> EstadoIngresoDict { get; set; }
    }
}
