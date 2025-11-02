using PROY.Models;

namespace PROY.Services
{
    public interface IEstadoSolicitudService
    {
        Task<List<EstadoSolicitud>> ObtenerTodos();
        Task<EstadoSolicitud> ObtenerPorIdEstadoSolicitud(int IdEstadoSolicitud);
    }
}
