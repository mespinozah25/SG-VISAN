using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class AsignaTecnicoService : IAsignaTecnicoService
    {
        private readonly IAsignaTecnicoRepository _asignaTecnicoRepository;

        public AsignaTecnicoService(IAsignaTecnicoRepository AsignaTecnicoRepository, IDetalleSolicitudAPARepository DetalleSolicitudAPARepository, IDetalleEvaluacionAPARepository DetalleEvaluacionAPARepository, IConfiguration Configuration)
        {
            _asignaTecnicoRepository = AsignaTecnicoRepository;
        }

        public async Task<List<AsignaTecnicoV2>> ObtenerAsignacionesTecnico(int IdSolicitud)
        {
            return await _asignaTecnicoRepository.ObtenerAsignacionesTecnico(IdSolicitud);
        }

        public async Task<string> GuardarAsignacionTecnico(AsignaTecnicoV2 AsignaTecnico)
        {
            try
            {
                await _asignaTecnicoRepository.GuardarAsignacionTecnico(AsignaTecnico);

                return "Asignación de técnico agregada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al agregar asignación de técnico: {ex}";
            }
        }

        public async Task<string> EliminarAsignacionTecnico(int IdAsignaTecnico)
        {
            try
            {
                AsignaTecnicoV2? asignaTecnico = await _asignaTecnicoRepository.ObtenerAsignacionTecnicoSegunId(IdAsignaTecnico);

                if (asignaTecnico == null) return "No se encontro la asignación de técnico";

                asignaTecnico.Restrictiva = 1;

                await _asignaTecnicoRepository.ActualizarAsignacionTecnico(asignaTecnico);

                return "Asignación de técnico eliminada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar asignación de técnico: {ex}";
            }
        }
    }
}
