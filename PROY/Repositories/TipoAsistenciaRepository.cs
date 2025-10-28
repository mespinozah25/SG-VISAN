using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class TipoAsistenciaRepository : ITipoAsistenciaRepository
    {
        private readonly AppDBContext _dbContext;

        public TipoAsistenciaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.TipoAsistencia
                .Select(t => t.IdTipoAsistencia)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<TipoAsistencia>> GetAll()
        {
            List<TipoAsistencia> listado = await _dbContext.TipoAsistencia.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<TipoAsistencia> GetTipoAsistencia(long id)
        {
            TipoAsistencia tipoAsistencia = await _dbContext.TipoAsistencia.FirstAsync(t => t.IdTipoAsistencia == id);

            return tipoAsistencia;
        }

        public async Task<TipoAsistencia> SaveTipoAsistencia(TipoAsistencia modelo)
        {
            try
            {
                _dbContext.TipoAsistencia.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<TipoAsistencia> UpdateTipoAsistencia(TipoAsistencia modelo)
        {
            _dbContext.TipoAsistencia.Update(modelo);
            await _dbContext.SaveChangesAsync();

            return modelo;
        }
    }
}
