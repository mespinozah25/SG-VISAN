using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class RenglonPresupuestarioRepository : IRenglonPresupuestarioRepository
    {
        private readonly AppDBContext _dbContext;

        public RenglonPresupuestarioRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.RenglonesPresupuestarios
                .Select(t => t.IdRenglonPresupuestario)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<RenglonPresupuestario>> GetAll()
        {
            List<RenglonPresupuestario> listado = await _dbContext.RenglonesPresupuestarios.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<RenglonPresupuestario> GetRenglonPresupuestario(long id)
        {
            RenglonPresupuestario renglon = await _dbContext.RenglonesPresupuestarios.FirstAsync(t => t.IdRenglonPresupuestario == id);

            return renglon;
        }

        public async Task<RenglonPresupuestario> SaveRenglonPresupuestario(RenglonPresupuestario modelo)
        {
            try
            {
                _dbContext.RenglonesPresupuestarios.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<RenglonPresupuestario> UpdateRenglonPresupuestario(RenglonPresupuestario modelo)
        {
            _dbContext.RenglonesPresupuestarios.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
