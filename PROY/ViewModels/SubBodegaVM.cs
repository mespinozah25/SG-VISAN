using PROY.Models;

namespace PROY.ViewModels
{
    public class SubBodegaVM
    {
        public SubBodega SubBodega { get; set; }
        public List<SubBodega> listaSubBodegas { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }      
    }
}
