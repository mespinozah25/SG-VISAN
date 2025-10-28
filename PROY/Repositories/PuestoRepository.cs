using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class PuestoRepository : IPuestoRepository
    {
        private readonly AppDBContext _dbContext;

        public PuestoRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Puestos
                .Select(t => t.IdPuesto)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Puesto>> GetAll()
        {
            List<Puesto> listado = await _dbContext.Puestos.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Puesto> GetPuesto(long id)
        {
            Puesto puesto = await _dbContext.Puestos.FirstAsync(t => t.IdPuesto == id);

            return puesto;
        }

        public async Task<Puesto> SavePuesto(Puesto modelo)
        {
            try
            {
                _dbContext.Puestos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Puesto> UpdatePuesto(Puesto modelo)
        {
            _dbContext.Puestos.Update(modelo);
            await _dbContext.SaveChangesAsync();

            return modelo;
        }
    }
}
