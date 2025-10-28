using PROY.Models;

namespace PROY.ViewModels
{
    public class PersonalBodegaVM
    {
        public PersonalBodega PersonalBodega { get; set; }
        public List<PersonalBodega> listaPersonalBodegas { get; set; }
        public Dictionary<Int64, string>? PuestoDict { get; set; }
        public Dictionary<Int64, string>? RenglonPreDict { get; set; }
        public Dictionary<Int64, string>? BodegaDict { get; set; }
        public Dictionary<Int64, string>? UsuarioDict { get; set; }
    }
}
