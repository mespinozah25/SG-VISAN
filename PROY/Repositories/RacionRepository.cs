using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class RacionRepository : IRacionRepository
    {
        private readonly AppDBContext _dbContext;

        public RacionRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region RACION_MAESTRO

        public async Task<long> CuentaRacionMaestro()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.RacionMaestro
                .Select(t => t.IdRacionMaestro)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<RacionMaestro>> GetAllRacionMaestro()
        {
            List<RacionMaestro> listado = await _dbContext.RacionMaestro.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<RacionMaestro> GetRacionMaestro(long id)
        {
            RacionMaestro racion_encontrado = await _dbContext.RacionMaestro.FirstAsync(t => t.IdRacionMaestro == id);

            return racion_encontrado;
        }

        public async Task<RacionMaestro> SaveRacionMaestro(RacionMaestro modelo)
        {
            try
            {
                _dbContext.RacionMaestro.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<RacionMaestro> UpdateRacionMaestro(RacionMaestro modelo)
        {
            _dbContext.RacionMaestro.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

        #region RACION_DETALLE

        public async Task<long> CuentaRacionDetalle()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.RacionDetalle
                .Select(t => t.IdRacionDetalle)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<RacionDetalle>> GetAllRacionDetalle(Int64 id)
        {
            List<RacionDetalle> listado = await _dbContext.RacionDetalle.Where(c => c.IdRacionMaestro == id && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<RacionDetalle> GetRacionDetalle(long id)
        {
            RacionDetalle racion_encontrado = await _dbContext.RacionDetalle.FirstAsync(t => t.IdRacionDetalle == id);

            return racion_encontrado;
        }

        public async Task<RacionDetalle> SaveRacionDetalle(RacionDetalle modelo)
        {
            try
            {
                _dbContext.RacionDetalle.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<RacionDetalle> UpdateRacionDetalle(RacionDetalle modelo)
        {
            _dbContext.RacionDetalle.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

    }
}
