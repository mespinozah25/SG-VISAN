using PROY.Models;

namespace PROY.Services
{
    public interface IConvenioService
    {
        Task<List<Convenio>> GetAll();
        Task<Convenio> GetConvenio(Int64 id);
        Task<ServiceResponse<Convenio>> SaveConvenio(Convenio modelo);
        Task<ServiceResponse<Convenio>> UpdateConvenio(Convenio modelo);
        Task<ServiceResponse<Convenio>> DeleteConvenio(Int64 id);
    }
}
