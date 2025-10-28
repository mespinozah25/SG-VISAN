using PROY.Models;

namespace PROY.ViewModels
{
    public class UsuarioVM
    {
        public Int64 IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Depto { get; set; }
        public string Clave { get; set; }
        public string ConfirmarClave { get; set; }
        public Int64 RolId { get; set; }
        public List<Rol> Roles { get; set; }
    }
}
