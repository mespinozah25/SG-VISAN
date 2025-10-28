using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class PersonalBodegaRepository : IPersonalBodegaRepository
    {
        private readonly AppDBContext _dbContext;

        public PersonalBodegaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.PersonalBodegas
                .Select(t => t.IdPersonalBodega)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<PersonalBodega>> GetAll()
        {
            List<PersonalBodega> listado = await _dbContext.PersonalBodegas.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<PersonalBodega> GetPersonalBodega(long id)
        {
            PersonalBodega PersonalBodega = await _dbContext.PersonalBodegas.FirstAsync(t => t.IdPersonalBodega == id);

            return PersonalBodega;
        }

        public async Task<PersonalBodega> SavePersonalBodega(PersonalBodega modelo)
        {
            try
            {
                _dbContext.PersonalBodegas.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<PersonalBodega> UpdatePersonalBodega(PersonalBodega modelo)
        {
            _dbContext.PersonalBodegas.Update(modelo);
            await _dbContext.SaveChangesAsync();

            return modelo;
        }
    }
}
