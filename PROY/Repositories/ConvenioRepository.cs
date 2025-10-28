using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class ConvenioRepository : IConvenioRepository
    {
        private readonly AppDBContext _dbContext;

        public ConvenioRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Convenio
                .Select(t => t.IdConvenio)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Convenio>> GetAll()
        {
            List<Convenio> listado = await _dbContext.Convenio.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Convenio> GetConvenio(long id)
        {
            Convenio convenio_encontrado = await _dbContext.Convenio.FirstAsync(t => t.IdConvenio == id);

            return convenio_encontrado;
        }

        public async Task<Convenio> SaveConvenio(Convenio modelo)
        {
            try
            {
                _dbContext.Convenio.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Convenio> UpdateConvenio(Convenio modelo)
        {
            _dbContext.Convenio.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

    }
}
