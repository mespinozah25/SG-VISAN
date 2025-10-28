using PROY.Models;

namespace PROY.ViewModels
{
    public class JuntaDeRecepcionVM
    {
        public JuntaReceptora JuntaReceptora { get; set; }
        public List<JuntaReceptora>? listaJuntaReceptora { get; set; }
        public List<DetalleJuntaReceptora> listaDetalleJuntaReceptora { get; set; }
        public string Rol { get; set; }
        public Dictionary<Int64, string> TipoNombramientoDict { get; set; }
    }
}
