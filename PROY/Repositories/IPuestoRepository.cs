using PROY.Models;

namespace PROY.Repositories
{
    public interface IPuestoRepository
    {
        Task<List<Puesto>> GetAll();
        Task<Puesto> GetPuesto(Int64 id);
        Task<Int64> Cuenta();
        Task<Puesto> SavePuesto(Puesto modelo);
        Task<Puesto> UpdatePuesto(Puesto modelo);
    }
}
