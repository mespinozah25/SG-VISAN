using PROY.Models;
using static PROY.Repositories.EjecucionRepository;

namespace PROY.Repositories
{
    public interface IEjecucionRepository
    {
        Task<List<Solicitud>> GetAllEjecuciones();
        Task<List<Solicitud>> GetAllEjecucionesAPA();
        Task<List<DetalleSolicitud>> GetAllDetalleEjecucion(Int64 idEjecucion);
        Task<List<DetalleSolicitudAPA>> GetAllDetalleEjecucionAPA(Int64 idEjecucion);
        Task<DetalleSolicitud> GetDetalleEjecucionID(Int64 idDetalleEjecucion);
        Task<DetalleSolicitud> UpdateDetalleEjecucionID(DetalleSolicitud det);        
        Task<DetalleSolicitudAPA> UpdateDetalleEjecucionIDAPA(DetalleSolicitudAPA det);
        Task<DetalleSolicitudAPA> GetDetalleEjecucionIDAPA(Int64 idDetalleEjecucion);
        Task<List<Agrupado>> GetDetalleSolicitudAgrupado(Int64 id);

        Task<Int64> CuentaAsignaRacion();
        Task<AsignaRacion> SaveAsignaRacion(AsignaRacion modelo);
        Task<AsignaRacion> UpdateAsignaRacion(AsignaRacion modelo);
        Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(Int64 id);
    }
}
