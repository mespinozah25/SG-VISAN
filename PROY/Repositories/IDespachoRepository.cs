using PROY.Models;

namespace PROY.Repositories
{
    public interface IDespachoRepository
    {
        Task<List<Solicitud>> GetAllDespachos();
        Task<List<Solicitud>> GetAllDespachosAPA();
        Task<List<DetalleSolicitud>> GetAllDetalleDespacho(Int64 idDespacho);
        Task<DetalleSolicitud> GetDetalleDespachoID(Int64 idDetalleDespacho);
    }
}
