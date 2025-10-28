using PROY.Models;
using static PROY.Repositories.EjecucionRepository;

namespace PROY.Services
{
    public interface IEjecucionService
    {
        Task<List<Solicitud>> GetAllEjecuciones();
        Task<List<Solicitud>> GetAllEjecucionesAPA();        
        Task<List<DetalleSolicitud>> GetAllDetalleEjecucion(Int64 idEjecucion);
        Task<List<DetalleSolicitudAPA>> GetAllDetalleEjecucionAPA(Int64 idEjecucion);
        Task<DetalleSolicitud> GetDetalleEjecucionID(Int64 idDetalleEjecucion);
        Task<ServiceResponse<DetalleSolicitud>> UpdateDetalleEjecucionID(DetalleSolicitud det);
        Task<ServiceResponse<DetalleSolicitudAPA>> UpdateDetalleEjecucionIDAPA(DetalleSolicitudAPA det);
        Task<DetalleSolicitudAPA> GetDetalleEjecucionIDAPA(Int64 idDetalleEjecucion);

        Task<List<Agrupado>> GetDetalleSolicitudAgrupado(Int64 id);
        Task<ServiceResponse<AsignaRacion>> SaveAsignaRacion(AsignaRacion modelo);
        Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(Int64 id);
        Task<List<Catalogo>> GetComparar(Int64 idEjecucion);
    }
}
