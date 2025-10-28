using Azure;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly AppDBContext _dbContext;

        public ProyectoRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Proyecto
                .Select(t => t.IdProyecto)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Proyecto>> GetAll()
        {
            List<Proyecto> listado = await _dbContext.Proyecto.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Proyecto> GetProyecto(long id)
        {
            Proyecto proyecto_encontrado = await _dbContext.Proyecto.FirstAsync(t => t.IdProyecto == id);

            return proyecto_encontrado;
        }

        public async Task<Proyecto> SaveProyecto(Proyecto modelo)
        {
            try
            {
                _dbContext.Proyecto.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Proyecto> UpdateProyecto(Proyecto modelo)
        {
            _dbContext.Proyecto.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        
    }
}
