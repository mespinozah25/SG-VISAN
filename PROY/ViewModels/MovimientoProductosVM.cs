using PROY.Models;

namespace PROY.ViewModels
{
    public class MovimientoProductosVM
    {
        public MovimientosProductos MovimientosProductos { get; set; }
        public List<MovimientosProductos>? listaMovimientosProductos { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }
        public Dictionary<Int64, string>? ProductoAlimenticioDict { get; set; }
        public Dictionary<Int64, string>? PresentacionDict { get; set; }
        public Dictionary<Int64, string>? UnidadMedidaDict { get; set; }
        public Dictionary<Int64, string>? ConvenioDict { get; set; }
    }
}

