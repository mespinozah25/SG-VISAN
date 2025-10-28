using PROY.Models;

namespace PROY.Repositories
{
    public interface IUnidadMedidaRepository
    {
        Task<List<UnidadMedida>> GetAll();
        Task<UnidadMedida> GetUnidadMedida(Int64 id);
        Task<Int64> Cuenta();
        Task<UnidadMedida> SaveUnidadMedida(UnidadMedida modelo);
        Task<UnidadMedida> UpdateUnidadMedida(UnidadMedida modelo);
    }
}
