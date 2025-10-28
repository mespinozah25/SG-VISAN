using PROY.Models;

namespace PROY.Services
{
    public interface IEstibaService
    {
        Task<List<Estiba>> GetAll();
        Task<List<Estiba>> ObtenerSegunBodegaYSubBodega(int idBodega, int idSubBodega);
        Task<Estiba> GetEstiba(Int64 id);
        Task<ServiceResponse<Estiba>> SaveEstiba(Estiba modelo);
        Task<ServiceResponse<Estiba>> UpdateEstiba(Estiba modelo);
        Task<ServiceResponse<Estiba>> DeleteEstiba(Int64 id);
    }
}
