using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDBContext _dbContext;

        public ProveedorRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Proveedor
                .Select(t => t.IdProveedor)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Proveedor>> GetAll()
        {
            List<Proveedor> listado = await _dbContext.Proveedor.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Proveedor> GetProveedor(long id)
        {
            Proveedor proveedor = await _dbContext.Proveedor.FirstAsync(t => t.IdProveedor == id);

            return proveedor;
        }

        public async Task<Proveedor> SaveProveedor(Proveedor modelo)
        {
            try
            {
                _dbContext.Proveedor.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Proveedor> UpdateProveedor(Proveedor modelo)
        {
            _dbContext.Proveedor.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
