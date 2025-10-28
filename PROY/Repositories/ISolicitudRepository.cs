using PROY.Models;
using static PROY.Repositories.SolicitudRepository;

namespace PROY.Repositories
{
    public interface ISolicitudRepository
    {
        Task<List<Solicitud>> GetAllSolicitudes();
        Task<List<Solicitud>> GetAllSolicitudesAPA();
        Task<List<DetalleSolicitud>> GetAllDetalleSolicitud(Int64 id);
        Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudAPA(Int64 id);
        Task<Solicitud> GetSolicitud(Int64 id);        
        Task<DetalleSolicitud> GetDetalleSolicitud(Int64 id);
        Task<DetalleSolicitud> GetDetalleSolicitudByCUI(long id, string paramCUI);
        Task<DetalleSolicitudAPA> GetDetalleSolicitudByCUIAPA(long id, string paramCUI);
        Task<DetalleSolicitudAPA> GetDetalleSolicitudAPA(Int64 id);
        Task<DetalleSolicitud> GetDetalleSolicitudID(Int64 idDetalle);
        Task<DetalleSolicitudAPA> GetDetalleSolicitudIDAPA(Int64 idDetalle);
        Task<Int64> CuentaSolicitud();
        Task<Solicitud> SaveSolicitud(Solicitud modelo);
        Task<Int64> CuentaDetalle();
        Task<Int64> CuentaDetalleAPA();
        Task<DetalleSolicitud> SaveDetalleSolicitud(DetalleSolicitud modelo);
        Task<DetalleSolicitudAPA> SaveDetalleSolicitudAPA(DetalleSolicitudAPA modelo);
        Task<Solicitud> UpdateSolicitud(Solicitud modelo);
        Task<DetalleSolicitud> UpdateDetalleSolicitud(DetalleSolicitud modelo);
        Task<DetalleSolicitudAPA> UpdateDetalleSolicitudAPA(DetalleSolicitudAPA modelo);

        Task<Int64> CuentaAsignaTecnico();
        Task<AsignaTecnico> SaveAsignaTecnico(AsignaTecnico modelo);
        Task<AsignaTecnico> UpdateAsignaTecnico(AsignaTecnico modelo);
        Task<AsignaTecnico> GetAsignaTecnico(Int64 idAsignaTecnico);
        Task<List<dynamic>> GetDetalleTecnicosSolicitud(Int64 id);

        Task<List<Tecnico>> GetAllTecnicos();

        Task<Int64> CuentaAsignaRacion();
        Task<AsignaRacion> SaveAsignaRacion(AsignaRacion modelo);
        Task<AsignaRacion> UpdateAsignaRacion(AsignaRacion modelo);
        Task<AsignaRacion> GetAsignaRacion(Int64 idRacion);
        Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(Int64 id);
        Task<int?> GetIntervencionBySolicitudId(Int64? idSolicitud);
    }
}
