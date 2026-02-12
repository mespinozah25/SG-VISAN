using PROY.Models;

namespace PROY.Repositories
{
    public interface IDetalleEvaluacionAPARepository
    {
        Task<bool> ExisteRegistroSegunProceso(int IdSolicitud, string Proceso);
    }
}
