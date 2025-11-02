using PROY.Models;

namespace PROY.Services
{
    public interface ISolicitudV2Service
    {
        Task<List<Solicitud>> ObtenerSolicitudesSegunEstado(int IdEstado, int IdDepartamento, int Rechazada);
        Task<Solicitud?> ObtenerSolicitudSegunIdSolicitud(int IdSolicitud);
        Task<string> AvanzarASiguienteEstapa(int IdSolicitud);
        Task<string> EliminarSolicitud(int IdSolicitud);
        Task<string> RechazarSolicitud(int IdSolicitud);
        Task<string> HabilitarSolicitud(int IdSolicitud);
    }
}
