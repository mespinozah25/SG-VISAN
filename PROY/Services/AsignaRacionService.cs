using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class AsignaRacionService : IAsignaRacionService
    {
        private readonly IAsignaRacionRepository _asignaRacionRepository;

        public AsignaRacionService(IAsignaRacionRepository AsignaRacionRepository, IDetalleSolicitudAPARepository DetalleSolicitudAPARepository, IDetalleEvaluacionAPARepository DetalleEvaluacionAPARepository, IConfiguration Configuration)
        {
            _asignaRacionRepository = AsignaRacionRepository;
        }

        public async Task<List<AsignaRacionV2>> ObtenerAsignacionesRacion(int IdSolicitud)
        {
            return await _asignaRacionRepository.ObtenerAsignacionesRacion(IdSolicitud);
        }

        public async Task<string> GuardarAsignacionRacion(AsignaRacionV2 AsignaRacion)
        {
            try
            {
                await _asignaRacionRepository.GuardarAsignacionRacion(AsignaRacion);

                return "Asignación de ración agregada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al agregar asignación de ración: {ex}";
            }
        }

        public async Task<string> EliminarAsignacionRacion(int IdAsignaRacion)
        {
            try
            {
                AsignaRacionV2? asignaRacion = await _asignaRacionRepository.ObtenerAsignacionRacionSegunId(IdAsignaRacion);

                if (asignaRacion == null) return "No se encontro la asignación de ración";

                asignaRacion.Restrictiva = 1;

                await _asignaRacionRepository.ActulizarAsignacionRacion(asignaRacion);

                return "Asignación de ración eliminada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar asignación de ración: {ex}";
            }
        }

        public async Task<int> ObtenerTotalRacionesAsignadasSegunSolicitud(int IdSolicitud)
        {
            return await _asignaRacionRepository.ObtenerTotalRacionesAsignadasSegunSolicitud(IdSolicitud);
        }
    }
}
