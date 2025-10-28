using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class TransportistaRepository : ITransportistaRepository
    {
        private readonly AppDBContext _dbContext;

        public TransportistaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Transportistas
                .Select(t => t.IdTransportista)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Transportista>> GetAll()
        {
            List<Transportista> listado = await _dbContext.Transportistas.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Transportista> GetTransportista(long id)
        {
            Transportista query = await _dbContext.Transportistas.FirstAsync(t => t.IdTransportista == id);

            return query;
        }

        public async Task<Transportista>? GetTransportistaNombre(string nombre)
        {
            Transportista? query = await _dbContext.Transportistas.Where(t => t.NombreTransportista.ToUpper().Trim() == nombre.ToUpper().Trim()).FirstOrDefaultAsync();

            return query;
        }

        public async Task<Transportista> SaveTransportista(Transportista modelo)
        {
            try
            {
                _dbContext.Transportistas.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Transportista> UpdateTransportista(Transportista modelo)
        {
            _dbContext.Transportistas.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
