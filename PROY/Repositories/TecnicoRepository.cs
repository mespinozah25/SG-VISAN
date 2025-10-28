using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class TecnicoRepository: ITecnicoRepository
    {
        private readonly AppDBContext _dbContext;

        public TecnicoRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;        
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Tecnicos
                .Select(t => t.IdTecnico)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Tecnico>> GetAll()
        {
            List<Tecnico> listado = await _dbContext.Tecnicos.Where(t => t.Restrictiva == 0).ToListAsync();

            return listado;
        }
        public async Task<Tecnico> GetTecnico(long id)
        {
            Tecnico tecnico_encontrado = await _dbContext.Tecnicos.FirstAsync(t => t.IdTecnico == id && t.Restrictiva == 0);

            return tecnico_encontrado;
        }

        public async Task<Tecnico> SaveTecnico(Tecnico modelo)
        {
            await _dbContext.Tecnicos.AddAsync(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<Tecnico> UpdateTecnico(Tecnico modelo)
        {
            _dbContext.Tecnicos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
