using PROY.Models;

namespace PROY.Services
{
    public interface IUnidadMedidaService
    {
        Task<List<UnidadMedida>> GetAll();
        Task<UnidadMedida> GetUnidadMedida(Int64 id);
        Task<ServiceResponse<UnidadMedida>> SaveUnidadMedida(UnidadMedida modelo);
        Task<ServiceResponse<UnidadMedida>> UpdateUnidadMedida(UnidadMedida modelo);
        Task<ServiceResponse<UnidadMedida>> DeleteUnidadMedida(Int64 id);
    }
}
