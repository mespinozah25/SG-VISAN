using PROY.Models;

namespace PROY.Repositories
{
    public interface IEstibaRepository
    {
        Task<List<Estiba>> GetAll();
        Task<List<Estiba>> ObtenerSegunBodegaYSubBodega(int idBodega, int idSubBodega);
        Task<Estiba> GetEstiba(Int64 id);
        Task<Int64> Cuenta();
        Task<Estiba> SaveEstiba(Estiba modelo);
        Task<Estiba> UpdateEstiba(Estiba modelo);
    }
}
