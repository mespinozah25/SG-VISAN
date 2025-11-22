using PROY.Models;

namespace PROY.Repositories
{
    public interface IDetalleSolicitudAPARepository
    {
        Task<int> TotalRegistrosSegunAnulacion(int IdSolicitud, int? IdAnulacion);

        Task<int> EliminarRegistrosSolicitud(int IdSolicitud);
    }
}
