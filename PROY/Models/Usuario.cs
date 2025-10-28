namespace PROY.Models
{
    public class Usuario
    {

        public Int64 IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set;}
        public string Depto {  get; set; }
        public int Restrictiva { get; set; }
        public Int64 RolId { get; set; }
        public Rol? Rol { get; set; }
        //public List<RolPermiso> RolPermisos { get; set;}
    }
}
