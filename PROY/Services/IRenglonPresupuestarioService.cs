using PROY.Models;

namespace PROY.Services
{
    public interface IRenglonPresupuestarioService
    {
        Task<List<RenglonPresupuestario>> GetAll();
        Task<RenglonPresupuestario> GetRenglonPresupuestario(Int64 id);
        Task<ServiceResponse<RenglonPresupuestario>> SaveRenglonPresupuestario(RenglonPresupuestario modelo);
        Task<ServiceResponse<RenglonPresupuestario>> UpdateRenglonPresupuestario(RenglonPresupuestario modelo);
        Task<ServiceResponse<RenglonPresupuestario>> DeleteRenglonPresupuestario(Int64 id);
    }
}
