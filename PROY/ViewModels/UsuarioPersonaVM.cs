using PROY.Models;

namespace PROY.ViewModels
{
    public class UsuarioPersonaVM
    {
        public UsuarioPersona UsuarioPersona { get; set; }
        public List<UsuarioPersona> listaUsuarioPersona { get; set; }
        public Dictionary<Int64, string>? PersonalBodegaDict { get; set; }
        public Dictionary<Int64, string>? UsuarioDict { get; set; }
    }
}
