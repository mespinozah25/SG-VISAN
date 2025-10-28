using Microsoft.EntityFrameworkCore;
using PROY.Models;
using PROY.ViewModels;

namespace PROY.Repositories
{
    public interface IUsuarioRepository
    {
        Task<List<Rol>> GetAllRoles();
        Task<List<Catalogo>> GetAllDeptos();
        Task<List<Usuario>> GetAll();
        Task<List<Permiso>> GetAllPermisos();
        //----------------------------------------------
        //Task<List<PermisoRolVM>> GestionarPermisos();
        Task<List<Permiso>> ObtenerPermisos();
        Task<List<Rol>> ObtenerRoles();
        bool TienePermiso(Int64 idPermiso, Int64 idRol);
        //Task<bool> TienePermiso(Int64 idPermiso, Int64 idRol);
        //void AgregarRolPermiso(Int64 idPermiso, Int64 idRol);
        Task AgregarRolPermiso(Int64 idPermiso, Int64 idRol);
        //void EliminarRolPermiso(Int64 idPermiso, Int64 idRol);
        Task EliminarRolPermiso(Int64 idPermiso, Int64 idRol);
        //void GuardarCambios();
        Task GuardarCambios();
        //-----------------------------------------------
        Task<Usuario> GetUsuario(string correo, string clave);
        Task<Usuario> GetUsuario(Int64 id);
        Task<Int64> Cuenta();
        Task<Usuario> SaveUsuario(Usuario modelo);
        Task<Usuario> UpdateUsuario(Usuario modelo);

    }
}
