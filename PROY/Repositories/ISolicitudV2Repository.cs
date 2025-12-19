using PROY.Models;

namespace PROY.Repositories
{
    public interface ISolicitudV2Repository
    {
        Task<List<Solicitud>> ObtenerSolicitudesSegunEstado(int IdEstado, int IdDepartamento, int Rechazada);
        Task<Solicitud?> ObtenerSolicitudSegunIdSolicitud(int IdSolicitud);
        Task<Solicitud> ActualizarSolicitud(Solicitud solicitud);
        Task<List<Solicitud>> ObtenerSolicitudesTotalesConFiltros(int idEstado, int IdDepto, int rechazada = 0, int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<List<Solicitud>> ObtenerSolicitudesConFiltros(int idEstado, int IdDepto, int rechazada = 0, int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<List<ReporteEjecucion>> ObtenerEjecucionesAPAConFiltros(int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<List<ReporteDespacho>> ObtenerDespachosAPAConFiltros(int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);

        #region Bitacora
        Task<List<Bitacora>> GetAllBitacora(long id);
        Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso);
        Task<Bitacora> SaveBitacora(Bitacora modelo); // Cambiado a retornar Bitacora directamente
        #endregion
    }
}
