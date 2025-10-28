using PROY.Models;

namespace PROY.ViewModels
{
    public class EstibaVM
    {
        public Estiba Estiba { get; set; }
        public List<Estiba> listaEstibas { get; set; }
        public Dictionary<Int64, string>? SubBodegaDict { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }
        public Dictionary<Int64, string>? ProductoDict { get; set; }
    }
}
