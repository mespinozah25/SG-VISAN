using PROY.Models;

namespace PROY.Services
{
    public interface IProductoAlimenticioService
    {
        Task<List<ProductoAlimenticio>> GetAll();
        Task<ProductoAlimenticio> GetProductoAlimenticio(Int64 id);
        Task<ServiceResponse<ProductoAlimenticio>> SaveProductoAlimenticio(ProductoAlimenticio modelo);
        Task<ServiceResponse<ProductoAlimenticio>> UpdateProductoAlimenticio(ProductoAlimenticio modelo);
        Task<ServiceResponse<ProductoAlimenticio>> DeleteProductoAlimenticio(Int64 id);
    }
}
