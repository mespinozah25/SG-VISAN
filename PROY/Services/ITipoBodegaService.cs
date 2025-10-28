using PROY.Models;

namespace PROY.Services
{
    public interface ITipoBodegaService
    {
        Task<List<TipoBodega>> GetAll();
        Task<TipoBodega> GetTipoBodega(Int64 id);
        Task<ServiceResponse<TipoBodega>> SaveTipoBodega(TipoBodega modelo);
        Task<ServiceResponse<TipoBodega>> UpdateTipoBodega(TipoBodega modelo);
        Task<ServiceResponse<TipoBodega>> DeleteTipoBodega(Int64 id);
    }
}
