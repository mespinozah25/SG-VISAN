using PROY.Models;

namespace PROY.Repositories
{
    public interface IProyectoRepository
    {
        Task<List<Proyecto>> GetAll();
        Task<Proyecto> GetProyecto(Int64 id);
        Task<Int64> Cuenta();
        Task<Proyecto> SaveProyecto(Proyecto modelo);
        Task<Proyecto> UpdateProyecto(Proyecto modelo);
    }
}
