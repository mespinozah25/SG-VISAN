using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class EstadoSolicitudService : IEstadoSolicitudService
    {

        private readonly IEstadoSolicitudRepository _estadoSolicitudRepository;

        public EstadoSolicitudService(IEstadoSolicitudRepository EstadoSolicitudRepository)
        {
            _estadoSolicitudRepository = EstadoSolicitudRepository;
        }

        public async Task<List<EstadoSolicitud>> ObtenerTodos()
        {
            return await _estadoSolicitudRepository.ObtenerTodos();
        }

        public async Task<EstadoSolicitud> ObtenerPorIdEstadoSolicitud(int IdEstadoSolicitud)
        {
            return await _estadoSolicitudRepository.ObtenerPorIdEstadoSolicitud(IdEstadoSolicitud);
        }
    }
}
