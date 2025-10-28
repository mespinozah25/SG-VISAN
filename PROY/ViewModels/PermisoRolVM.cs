namespace PROY.ViewModels
{
    public class PermisoRolVM
    {
        public Int64 IdPermiso { get; set; }
        public string NombrePermiso { get; set; }
        public List<RolPermisoVM> RolesPermisos { get; set; }
    }
}
