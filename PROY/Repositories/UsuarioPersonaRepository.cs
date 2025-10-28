using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class UsuarioPersonaRepository : IUsuarioPersonaRepository
    {
        private readonly AppDBContext _dbContext;

        public UsuarioPersonaRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.UsuarioPersona
                .Select(t => t.IdUsuarioPersona)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<UsuarioPersona>> GetAll()
        {
            List<UsuarioPersona> listado = await _dbContext.UsuarioPersona.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<UsuarioPersona> GetUsuarioPersona(long id)
        {
            UsuarioPersona UsuarioPersona = await _dbContext.UsuarioPersona.FirstAsync(t => t.IdUsuarioPersona == id);

            return UsuarioPersona;
        }

        public async Task<UsuarioPersona> SaveUsuarioPersona(UsuarioPersona modelo)
        {
            try
            {
                _dbContext.UsuarioPersona.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<UsuarioPersona> UpdateUsuarioPersona(UsuarioPersona modelo)
        {
            _dbContext.UsuarioPersona.Update(modelo);
            await _dbContext.SaveChangesAsync();

            return modelo;
        }
    }
}
