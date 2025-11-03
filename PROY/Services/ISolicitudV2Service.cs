using PROY.Models;

namespace PROY.Services
{
    public interface ISolicitudV2Service
    {
        Task<List<Solicitud>> ObtenerSolicitudesSegunEstado(int IdEstado, int IdDepartamento, int Rechazada);
        Task<Solicitud?> ObtenerSolicitudSegunIdSolicitud(int IdSolicitud);
        Task<string> AvanzarASiguienteEtapa(int IdSolicitud);
        Task<string> RegresarAEtapaAnterior(int IdSolicitud, string? ComentarioRechazo = null);
        Task<string> EliminarSolicitud(int IdSolicitud);
        Task<string> RechazarSolicitud(int IdSolicitud);
        Task<string> HabilitarSolicitud(int IdSolicitud);
        Task<string> ActualizarFechasEntregaSolicitud(int IdSolicitud, DateTime FechaInicio, DateTime FechaFin, string? Observaciones);
    }
}
