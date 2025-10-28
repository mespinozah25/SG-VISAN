using PROY.Models;

namespace PROY.Repositories
{
    public interface IConvenioRepository
    {
        Task<List<Convenio>> GetAll();
        Task<Convenio> GetConvenio(Int64 id);
        Task<Int64> Cuenta();
        Task<Convenio> SaveConvenio(Convenio modelo);
        Task<Convenio> UpdateConvenio(Convenio modelo);
    }
}
