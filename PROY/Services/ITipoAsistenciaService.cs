using PROY.Models;

namespace PROY.Services
{
    public interface ITipoAsistenciaService
    {
        Task<List<TipoAsistencia>> GetAll();
        Task<TipoAsistencia> GetTipoAsistencia(Int64 id);
        Task<ServiceResponse<TipoAsistencia>> SaveTipoAsistencia(TipoAsistencia modelo);
        Task<ServiceResponse<TipoAsistencia>> UpdateTipoAsistencia(TipoAsistencia modelo);
        Task<ServiceResponse<TipoAsistencia>> DeleteTipoAsistencia(Int64 id);
    }
}
