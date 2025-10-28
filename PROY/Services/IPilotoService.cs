using PROY.Models;

namespace PROY.Services
{
    public interface IPilotoService
    {
        Task<List<Piloto>> GetAll();
        Task<Piloto> GetPiloto(Int64 id);
        Task<ServiceResponse<Piloto>> SavePiloto(Piloto modelo);
        Task<ServiceResponse<Piloto>> UpdatePiloto(Piloto modelo);
        Task<ServiceResponse<Piloto>> DeletePiloto(Int64 id);
    }
}
