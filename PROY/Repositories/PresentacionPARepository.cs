using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class PresentacionPARepository : IPresentacionPARepository
    {
        private readonly AppDBContext _dbContext;

        public PresentacionPARepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.PresentacionPA
                .Select(t => t.IdPresentacionPA)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<PresentacionPA>> GetAll()
        {
            List<PresentacionPA> listado = await _dbContext.PresentacionPA.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<PresentacionPA> GetPresentacionPA(long id)
        {
            PresentacionPA presentacionpa_encontrado = await _dbContext.PresentacionPA.FirstAsync(t => t.IdPresentacionPA == id);

            return presentacionpa_encontrado;
        }

        public async Task<PresentacionPA> SavePresentacionPA(PresentacionPA modelo)
        {
            try
            {
                _dbContext.PresentacionPA.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<PresentacionPA> UpdatePresentacionPA(PresentacionPA modelo)
        {
            _dbContext.PresentacionPA.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
