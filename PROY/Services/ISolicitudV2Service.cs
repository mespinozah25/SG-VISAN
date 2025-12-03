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
        Task<string> ActualizarRaciones(int idSolicitud, int nuevasRaciones);

        Task<List<Solicitud>> ObtenerSolicitudesConFiltros(int idEstado, int idDepto, int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, int rechazada = 0);
        Task<List<ReporteEjecucion>> ObtenerEjecucionesAPAConFiltros(int?  idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);

        #region Bitacora
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso);
        Task<Bitacora> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
