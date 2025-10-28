using PROY.Models;

namespace PROY.Repositories
{
    public interface ITipoBodegaRepository
    {
        Task<List<TipoBodega>> GetAll();
        Task<TipoBodega> GetTipoBodega(Int64 id);
        Task<Int64> Cuenta();
        Task<TipoBodega> SaveTipoBodega(TipoBodega modelo);
        Task<TipoBodega> UpdateTipoBodega(TipoBodega modelo);
    }
}
