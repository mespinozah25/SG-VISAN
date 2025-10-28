using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class PilotoRepository : IPilotoRepository
    {
        private readonly AppDBContext _dbContext;

        public PilotoRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Pilotos
                .Select(t => t.IdPiloto)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Piloto>> GetAll()
        {
            List<Piloto> listado = await _dbContext.Pilotos.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Piloto> GetPiloto(long id)
        {
            Piloto query = await _dbContext.Pilotos.FirstAsync(t => t.IdPiloto == id);

            return query;
        }

        public async Task<Piloto>? GetPilotoNombreYLicencia(string mombre, string licencia)
        {
            
            
            var query = await _dbContext.Pilotos.Where(t => t.NombresPiloto.ToUpper().Trim() == mombre.ToUpper().Trim() && t.NoLicencia.ToUpper().Trim() == licencia.ToUpper().Trim()).FirstOrDefaultAsync();

            return query;
        }

        public async Task<Piloto> SavePiloto(Piloto modelo)
        {
            try
            {
                _dbContext.Pilotos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Piloto> UpdatePiloto(Piloto modelo)
        {
            _dbContext.Pilotos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
