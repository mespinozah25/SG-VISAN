using PROY.Models;

namespace PROY.ViewModels
{
    public class PilotoVM
    {
        public Piloto Piloto { get; set; }
        public List<Piloto> listaPilotos { get; set; }
        public Dictionary<Int64, string>? TransportistaDict { get; set; }
    }
}
