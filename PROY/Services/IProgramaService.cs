using PROY.Models;

namespace PROY.Services
{
    public interface IProgramaService
    {
        Task<List<Programa>> GetAll();
        Task<Programa> GetPrograma(Int64 id);
        Task<ServiceResponse<Programa>> SavePrograma(Programa modelo);
        Task<ServiceResponse<Programa>> UpdatePrograma(Programa modelo);
        Task<ServiceResponse<Programa>> DeletePrograma(Int64 id);
    }
}
