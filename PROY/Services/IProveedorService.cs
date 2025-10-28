using PROY.Models;

namespace PROY.Services
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> GetAll();
        Task<Proveedor> GetProveedor(Int64 id);
        Task<ServiceResponse<Proveedor>> SaveProveedor(Proveedor modelo);
        Task<ServiceResponse<Proveedor>> UpdateProveedor(Proveedor modelo);
        Task<ServiceResponse<Proveedor>> DeleteProveedor(Int64 id);
    }
}
