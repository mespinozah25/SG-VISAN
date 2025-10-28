using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class BodegaRepository : IBodegaRepository
    {
        private readonly AppDBContext _dbContext;

        public BodegaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Bodegas
                .Select(t => t.IdBodega)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Bodega>> GetAll()
        {
            List<Bodega> listado = await _dbContext.Bodegas.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Bodega> GetBodega(long id)
        {
            Bodega bodega_encontrado = await _dbContext.Bodegas.FirstAsync(t => t.IdBodega == id);

            return bodega_encontrado;
        }

        public async Task<Bodega> SaveBodega(Bodega modelo)
        {
            try
            {
                _dbContext.Bodegas.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch(Exception exc)
            {
                var error = exc;
            }

            
            return modelo;
        }

        public async Task<Bodega> UpdateBodega(Bodega modelo)
        {
            _dbContext.Bodegas.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #region TIPO_BODEGA

        public async Task<List<TipoBodega>> GetAllTipoBodegas()
        {
            var tipos = await _dbContext.TipoBodegas.Where(t => t.Estado == 0).ToListAsync();

            return tipos;
        }

        #endregion
    }
}
