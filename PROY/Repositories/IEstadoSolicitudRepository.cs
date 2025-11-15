using PROY.Models;

namespace PROY.Repositories
{
    public interface IEstadoSolicitudRepository
    {
        Task<List<EstadoSolicitud>> ObtenerTodos();
        Task<EstadoSolicitud> ObtenerPorIdEstadoSolicitud(int IdEstadoSolicitud);
    }
}
