using PROY.Models;

namespace PROY.Repositories
{
    public interface IPersonalBodegaRepository
    {
        Task<List<PersonalBodega>> GetAll();
        Task<PersonalBodega> GetPersonalBodega(Int64 id);
        Task<Int64> Cuenta();
        Task<PersonalBodega> SavePersonalBodega(PersonalBodega modelo);
        Task<PersonalBodega> UpdatePersonalBodega(PersonalBodega modelo);
    }
}
