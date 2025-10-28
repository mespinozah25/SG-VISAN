using PROY.Models;

namespace PROY.Repositories
{
    public interface ISubBodegaRepository
    {
        Task<List<SubBodega>> GetAll();
        Task<List<SubBodega>> ObtenerSegunBodega(int idBodega);
        Task<SubBodega> GetSubBodega(Int64 id);
        Task<Int64> Cuenta();
        Task<SubBodega> SaveSubBodega(SubBodega modelo);
        Task<SubBodega> UpdateSubBodega(SubBodega modelo);
    }
}
