using PROY.Models;

namespace PROY.Repositories
{
    public interface ITipoAsistenciaRepository
    {
        Task<List<TipoAsistencia>> GetAll();
        Task<TipoAsistencia> GetTipoAsistencia(Int64 id);
        Task<Int64> Cuenta();
        Task<TipoAsistencia> SaveTipoAsistencia(TipoAsistencia modelo);
        Task<TipoAsistencia> UpdateTipoAsistencia(TipoAsistencia modelo);
    }
}
