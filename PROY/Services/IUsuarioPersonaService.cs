using PROY.Models;

namespace PROY.Services
{
    public interface IUsuarioPersonaService
    {
        Task<List<UsuarioPersona>> GetAll();
        Task<UsuarioPersona> GetUsuarioPersona(Int64 id);
        Task<ServiceResponse<UsuarioPersona>> SaveUsuarioPersona(UsuarioPersona modelo);
        Task<ServiceResponse<UsuarioPersona>> UpdateUsuarioPersona(UsuarioPersona modelo);
        Task<ServiceResponse<UsuarioPersona>> DeleteUsuarioPersona(Int64 id);
    }
}
