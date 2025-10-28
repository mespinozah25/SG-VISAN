using PROY.Models;

namespace PROY.ViewModels
{
    public class KardexVM
    {
        public Kardex Kardex { get; set; }
        public List<dynamic>? listaKardex { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }
        public Dictionary<Int64, string>? ProductoAlimenticioDict { get; set; }
        public Dictionary<Int64, string>? PresentacionDict { get; set; }
        public Dictionary<Int64, string>? UnidadMedidaDict { get; set; }
    }
}
