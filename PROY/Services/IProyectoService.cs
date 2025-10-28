using PROY.Models;

namespace PROY.Services
{
    public interface IProyectoService
    {
        Task<List<Proyecto>> GetAll();
        Task<Proyecto> GetProyecto(Int64 id);
        Task<ServiceResponse<Proyecto>> SaveProyecto(Proyecto modelo);
        Task<ServiceResponse<Proyecto>> UpdateProyecto(Proyecto modelo);
        Task<ServiceResponse<Proyecto>> DeleteProyecto(Int64 id);
    }
}
