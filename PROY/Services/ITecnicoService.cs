using PROY.Models;
using PROY.ViewModels;

namespace PROY.Services
{
    public interface ITecnicoService
    {
        Task<List<Tecnico>> GetAll();        
        Task<Tecnico> GetTecnico(Int64 id);
        Task<ServiceResponse<Tecnico>> SaveTecnico(Tecnico modelo);
        Task<ServiceResponse<Tecnico>> UpdateTecnico(Tecnico modelo);
        Task<ServiceResponse<Tecnico>> DeleteTecnico(Int64 id);
    }
}
