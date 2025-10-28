using PROY.Models;

namespace PROY.Services
{
    public interface IPuestoService
    {
        Task<List<Puesto>> GetAll();
        Task<Puesto> GetPuesto(Int64 id);
        Task<ServiceResponse<Puesto>> SavePuesto(Puesto modelo);
        Task<ServiceResponse<Puesto>> UpdatePuesto(Puesto modelo);
        Task<ServiceResponse<Puesto>> DeletePuesto(Int64 id);
    }
}
