using PROY.Models;

namespace PROY.Repositories
{
    public interface ISolicitudV2Repository
    {
        Task<List<Solicitud>> ObtenerSolicitudesSegunEstado(int IdEstado, int IdDepartamento, int Rechazada);
        Task<Solicitud?> ObtenerSolicitudSegunIdSolicitud(int IdSolicitud);
        Task<Solicitud> ActualizarSolicitud(Solicitud solicitud);
    }
}
