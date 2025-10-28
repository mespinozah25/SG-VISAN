using PROY.Models;

namespace PROY.Repositories
{
    public interface IRenglonPresupuestarioRepository
    {
        Task<List<RenglonPresupuestario>> GetAll();
        Task<RenglonPresupuestario> GetRenglonPresupuestario(Int64 id);
        Task<Int64> Cuenta();
        Task<RenglonPresupuestario> SaveRenglonPresupuestario(RenglonPresupuestario modelo);
        Task<RenglonPresupuestario> UpdateRenglonPresupuestario(RenglonPresupuestario modelo);
    }
}
