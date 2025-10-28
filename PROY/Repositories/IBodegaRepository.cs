using PROY.Models;

namespace PROY.Repositories
{
    public interface IBodegaRepository
    {
        Task<List<Bodega>> GetAll();
        Task<Bodega> GetBodega(Int64 id);
        Task<Int64> Cuenta();
        Task<Bodega> SaveBodega(Bodega modelo);
        Task<Bodega> UpdateBodega(Bodega modelo);
        Task<List<TipoBodega>> GetAllTipoBodegas();
    }
}
