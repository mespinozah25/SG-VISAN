namespace PROY.Models
{
    public class Rol
    {
        public Int64 IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }        
        public int Restrictiva { get; set; }        
        public List<Usuario> Usuarios { get; set; }
        public List<Permiso> Permisos { get; set; }
        //public List<RolPermiso> RolPermisos { get; set; }

    }
}
