using PROY.Models;

namespace PROY.Services
{
    public interface IPersonalBodegaService
    {
        Task<List<PersonalBodega>> GetAll();
        Task<PersonalBodega> GetPersonalBodega(Int64 id);
        Task<ServiceResponse<PersonalBodega>> SavePersonalBodega(PersonalBodega modelo);
        Task<ServiceResponse<PersonalBodega>> UpdatePersonalBodega(PersonalBodega modelo);
        Task<ServiceResponse<PersonalBodega>> DeletePersonalBodega(Int64 id);
    }
}
