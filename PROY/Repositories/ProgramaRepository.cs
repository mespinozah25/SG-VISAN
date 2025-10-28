using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class ProgramaRepository : IProgramaRepository
    {
        private readonly AppDBContext _dbContext;

        public ProgramaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Programa
                .Select(t => t.IdPrograma)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Programa>> GetAll()
        {
            List<Programa> listado = await _dbContext.Programa.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Programa> GetPrograma(long id)
        {
            Programa programa_encontrado = await _dbContext.Programa.FirstAsync(t => t.IdPrograma == id);

            return programa_encontrado;
        }

        public async Task<Programa> SavePrograma(Programa modelo)
        {
            try
            {
                _dbContext.Programa.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Programa> UpdatePrograma(Programa modelo)
        {
            _dbContext.Programa.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

    }
}
