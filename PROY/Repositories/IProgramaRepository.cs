using PROY.Models;

namespace PROY.Repositories
{
    public interface IProgramaRepository
    {
        Task<List<Programa>> GetAll();
        Task<Programa> GetPrograma(Int64 id);
        Task<Int64> Cuenta();
        Task<Programa> SavePrograma(Programa modelo);
        Task<Programa> UpdatePrograma(Programa modelo);
    }
}
