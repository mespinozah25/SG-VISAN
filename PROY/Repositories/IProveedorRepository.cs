using PROY.Models;

namespace PROY.Repositories
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> GetAll();
        Task<Proveedor> GetProveedor(Int64 id);
        Task<Int64> Cuenta();
        Task<Proveedor> SaveProveedor(Proveedor modelo);
        Task<Proveedor> UpdateProveedor(Proveedor modelo);
    }
}
