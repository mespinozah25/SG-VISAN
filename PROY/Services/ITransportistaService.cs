using PROY.Models;

namespace PROY.Services
{
    public interface ITransportistaService
    {
        Task<List<Transportista>> GetAll();
        Task<Transportista> GetTransportista(Int64 id);
        Task<ServiceResponse<Transportista>> SaveTransportista(Transportista modelo);
        Task<ServiceResponse<Transportista>> UpdateTransportista(Transportista modelo);
        Task<ServiceResponse<Transportista>> DeleteTransportista(Int64 id);
    }
}
