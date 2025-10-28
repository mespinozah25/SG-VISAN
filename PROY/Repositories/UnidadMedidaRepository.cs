using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        private readonly AppDBContext _dbContext;

        public UnidadMedidaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.UnidadMedida
                .Select(t => t.IdUnidadMedida)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<UnidadMedida>> GetAll()
        {
            List<UnidadMedida> listado = await _dbContext.UnidadMedida.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<UnidadMedida> GetUnidadMedida(long id)
        {
            UnidadMedida unidadmedida_encontrado = await _dbContext.UnidadMedida.FirstAsync(t => t.IdUnidadMedida == id);

            return unidadmedida_encontrado;
        }

        public async Task<UnidadMedida> SaveUnidadMedida(UnidadMedida modelo)
        {
            try
            {
                _dbContext.UnidadMedida.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<UnidadMedida> UpdateUnidadMedida(UnidadMedida modelo)
        {
            _dbContext.UnidadMedida.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

    }
}
