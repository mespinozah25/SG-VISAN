using PROY.Models;

namespace PROY.Services
{
    public interface IDespachoService
    {
        Task<List<Solicitud>> GetAllDespachos();
        Task<List<Solicitud>> GetAllDespachosAPA();        
        Task<List<DetalleSolicitud>> GetAllDetalleDespacho(Int64 idDespacho);
        Task<DetalleSolicitud> GetDetalleDespachoID(Int64 idDetalleDespacho);
        Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo);
    }
}
