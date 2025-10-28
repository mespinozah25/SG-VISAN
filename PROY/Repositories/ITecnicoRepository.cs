using PROY.Models;

namespace PROY.Repositories
{
    public interface ITecnicoRepository
    {
        Task<List<Tecnico>> GetAll();
        Task<Tecnico> GetTecnico(Int64 id);
        Task<Int64> Cuenta();
        Task<Tecnico> SaveTecnico(Tecnico modelo);
        Task<Tecnico> UpdateTecnico(Tecnico modelo);
    }
}
