using PROY.Models;

namespace PROY.ViewModels
{
    public class ReingresoAlimentosVM
    {
        public ReingresoAlimentos ReingresoAlimentos { get; set; }
        public List<ReingresoAlimentos>? listaReingresoAlimentos { get; set; }
        public List<DetalleReingresoAlimentos>? listaDetalleReingresoAlimentos { get; set; }
        public string Rol { get; set; }
        public int IdUsuario { get; set; }
        public Dictionary<Int64, string>? ProgramaDict { get; set; }
        public Dictionary<Int64, string> EstadoReingresoDict { get; set; }
        public Dictionary<Int64, string> EstibaDict { get; set; }
    }
}
