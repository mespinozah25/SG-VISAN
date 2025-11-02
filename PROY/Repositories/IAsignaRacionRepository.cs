using PROY.Models;

namespace PROY.Repositories
{
    public interface IAsignaRacionRepository
    {
        Task<List<AsignaRacionV2>> ObtenerAsignacionesRacion(int IdSolicitud);
        Task<AsignaRacionV2?> ObtenerAsignacionRacionSegunId(int IdAsignaRacion);
        Task<AsignaRacionV2> GuardarAsignacionRacion(AsignaRacionV2 asignaRacion);
        Task<AsignaRacionV2> ActulizarAsignacionRacion(AsignaRacionV2 asignaRacion);
        Task<int> ObtenerTotalRacionesAsignadasSegunSolicitud(int IdSolicitud);
    }
}
