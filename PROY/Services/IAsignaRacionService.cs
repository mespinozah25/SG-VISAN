using PROY.Models;

namespace PROY.Services
{
    public interface IAsignaRacionService
    {
        Task<List<AsignaRacionV2>> ObtenerAsignacionesRacion(int IdSolicitud);
        Task<string> GuardarAsignacionRacion(AsignaRacionV2 AsignaRacion);
        Task<string> EliminarAsignacionRacion(int IdAsignaRacion);
        Task<int> ObtenerTotalRacionesAsignadasSegunSolicitud(int IdSolicitud);
    }
}
