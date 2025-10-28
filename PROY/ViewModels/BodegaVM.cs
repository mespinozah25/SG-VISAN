using PROY.Models;

namespace PROY.ViewModels
{
    public class BodegaVM
    {
        public Bodega Bodega { get; set; }
        public List<Bodega> listaBodegas { get; set; }
        public Dictionary<Int64, string>? TipoBodegaDict { get; set; }
        public Dictionary<int, string> DepartamentoDict { get; set; }
        public Dictionary<int, string> MunicipioDict { get; set; }
    }
}
