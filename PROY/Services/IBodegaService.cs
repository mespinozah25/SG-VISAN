using PROY.Models;

namespace PROY.Services
{
    public interface IBodegaService
    {
        Task<List<Bodega>> GetAll();
        Task<Bodega> GetBodega(Int64 id);
        Task<ServiceResponse<Bodega>> SaveBodega(Bodega modelo);
        Task<ServiceResponse<Bodega>> UpdateBodega(Bodega modelo);
        Task<ServiceResponse<Bodega>> DeleteBodega(Int64 id);
        Task<List<TipoBodega>> GetAllTipoBodegas();
    }
}
