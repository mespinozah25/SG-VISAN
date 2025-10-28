namespace PROY.Models
{
    public class Permiso
    {
        public Int64 IdPermiso { get; set; }
        public string Nombre { get; set; }        
        public int Restrictiva { get; set; }
        //public List<RolPermiso> RolPermisos { get; set;}
        public List<Rol> Roles { get; set; }
    }
}
