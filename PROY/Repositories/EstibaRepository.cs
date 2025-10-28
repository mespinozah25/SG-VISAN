using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class EstibaRepository : IEstibaRepository
    {
        private readonly AppDBContext _dbContext;

        public EstibaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Estibas
                .Select(t => t.IdEstiba)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Estiba>> GetAll()
        {
            List<Estiba> listado = await _dbContext.Estibas.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<Estiba>> ObtenerSegunBodegaYSubBodega(int idBodega, int idSubBodega)
        {
            List<Estiba> listado = await _dbContext.Estibas.Where(c => c.Estado == 0 && c.IdBodega == idBodega && c.IdSubBodega == idSubBodega).ToListAsync();

            return listado;
        }

        public async Task<Estiba> GetEstiba(long id)
        {
            Estiba query = await _dbContext.Estibas.FirstAsync(t => t.IdEstiba == id);

            return query;
        }

        public async Task<Estiba> SaveEstiba(Estiba modelo)
        {
            try
            {
                _dbContext.Estibas.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Estiba> UpdateEstiba(Estiba modelo)
        {
            _dbContext.Estibas.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
