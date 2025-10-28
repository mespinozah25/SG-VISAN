using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class ProductoAlimenticioRepository : IProductoAlimenticioRepository
    {
        private readonly AppDBContext _dbContext;

        public ProductoAlimenticioRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.ProductoAlimenticio
                .Select(t => t.IdProductoAlimenticio)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<ProductoAlimenticio>> GetAll()
        {
            List<ProductoAlimenticio> listado = await _dbContext.ProductoAlimenticio.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<ProductoAlimenticio> GetProductoAlimenticio(long id)
        {
            ProductoAlimenticio productoalimenticio_encontrado = await _dbContext.ProductoAlimenticio.FirstAsync(t => t.IdProductoAlimenticio == id);

            return productoalimenticio_encontrado;
        }

        public async Task<ProductoAlimenticio> SaveProductoAlimenticio(ProductoAlimenticio modelo)
        {
            try
            {
                _dbContext.ProductoAlimenticio.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<ProductoAlimenticio> UpdateProductoAlimenticio(ProductoAlimenticio modelo)
        {
            _dbContext.ProductoAlimenticio.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
