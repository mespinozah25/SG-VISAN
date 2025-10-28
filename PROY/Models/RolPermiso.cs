namespace PROY.Models
{
    public class RolPermiso
    {
        //public int IdRolPermiso { get; set; }

        // Claves foráneas
        public Int64 RolId { get; set; }
        public Rol Rol { get; set; }

        public Int64 PermisoId { get; set; }
        public Permiso Permiso { get; set; }
    }
}
