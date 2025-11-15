using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class TipoIntervencionRepository : ITipoIntervencionRepository
    {
        private readonly AppDBContext _dbContext;

        public TipoIntervencionRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TipoIntervencion>> ObtenerPorDepartamento(int IdDepto)
        {
            List<TipoIntervencion> listado = await _dbContext.TipoIntervencion.Where(c => c.Estado == 0 && c.IdDepto == IdDepto).ToListAsync();

            return listado;
        }
    }
}
