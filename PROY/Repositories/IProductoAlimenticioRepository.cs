using PROY.Models;

namespace PROY.Repositories
{
    public interface IProductoAlimenticioRepository
    {
        Task<List<ProductoAlimenticio>> GetAll();
        Task<ProductoAlimenticio> GetProductoAlimenticio(Int64 id);
        Task<Int64> Cuenta();
        Task<ProductoAlimenticio> SaveProductoAlimenticio(ProductoAlimenticio modelo);
        Task<ProductoAlimenticio> UpdateProductoAlimenticio(ProductoAlimenticio modelo);
    }
}
