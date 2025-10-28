using PROY.Models;

namespace PROY.Repositories
{
    public interface IUsuarioPersonaRepository
    {
        Task<List<UsuarioPersona>> GetAll();
        Task<UsuarioPersona> GetUsuarioPersona(Int64 id);
        Task<Int64> Cuenta();
        Task<UsuarioPersona> SaveUsuarioPersona(UsuarioPersona modelo);
        Task<UsuarioPersona> UpdateUsuarioPersona(UsuarioPersona modelo);
    }
}
