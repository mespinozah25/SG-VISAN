using Microsoft.EntityFrameworkCore;
using PROY.Models;
using PROY.ViewModels;

namespace PROY.Services
{
    public interface IUsuarioService
    {
        Task<List<Rol>> GetAllRoles();
        Task<List<Catalogo>> GetAllDeptos();
        Task<List<Permiso>> GetAllPermisos();
        //---------------------------------------------------------------------------
        Task<List<PermisoRolVM>> ObtenerPermisosConRoles();
        //void GuardarPermisos(List<PermisoRolVM> permisosRol);
        Task GuardarPermisos(List<PermisoRolVM> permisosRol);

        //---------------------------------------------------------------------------
        Task<List<Usuario>> GetAll();
        Task<ServiceResponse<Usuario>> GetUsuario(string correo, string clave);
        Task<Usuario> GetUsuario(Int64 id);
        Task<ServiceResponse<Usuario>> SaveUsuario(UsuarioVM modelo);
        Task<ServiceResponse<Usuario>> UpdateUsuario(UsuarioVM modelo);
        Task<ServiceResponse<Usuario>> DeleteUsuario(Int64 id);
        Task<ServiceResponse<Usuario>> ReiniciarClaveUsuario(UsuarioVM modelo);
    }
}
