using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class TipoBodegaRepository : ITipoBodegaRepository
    {
        private readonly AppDBContext _dbContext;

        public TipoBodegaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.TipoBodegas
                .Select(t => t.IdTipoBodega)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<TipoBodega>> GetAll()
        {
            List<TipoBodega> listado = await _dbContext.TipoBodegas.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<TipoBodega> GetTipoBodega(long id)
        {
            TipoBodega tipoBodega = await _dbContext.TipoBodegas.FirstAsync(t => t.IdTipoBodega == id);

            return tipoBodega;
        }

        public async Task<TipoBodega> SaveTipoBodega(TipoBodega modelo)
        {
            try
            {
                _dbContext.TipoBodegas.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<TipoBodega> UpdateTipoBodega(TipoBodega modelo)
        {
            _dbContext.TipoBodegas.Update(modelo);
            await _dbContext.SaveChangesAsync();

            return modelo;
        }
    }
}
