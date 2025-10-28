using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class SubBodegaRepository : ISubBodegaRepository
    {
        private readonly AppDBContext _dbContext;

        public SubBodegaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.SubBodegas
                .Select(t => t.IdSubBodega)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<SubBodega>> GetAll()
        {
            List<SubBodega> listado = await _dbContext.SubBodegas.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<SubBodega>> ObtenerSegunBodega(int idBodega)
        {
            List<SubBodega> listado = await _dbContext.SubBodegas.Where(c => c.Estado == 0 && c.IdBodega == idBodega).ToListAsync();

            return listado;
        }

        public async Task<SubBodega> GetSubBodega(long id)
        {
            SubBodega query = await _dbContext.SubBodegas.FirstAsync(t => t.IdSubBodega == id);

            return query;
        }

        public async Task<SubBodega> SaveSubBodega(SubBodega modelo)
        {
            try
            {
                _dbContext.SubBodegas.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<SubBodega> UpdateSubBodega(SubBodega modelo)
        {
            _dbContext.SubBodegas.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
