using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class EstadoSolicitudRepository : IEstadoSolicitudRepository
    {
        private readonly AppDBContext _dbContext;

        public EstadoSolicitudRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<EstadoSolicitud>> ObtenerTodos()
        {
            return await _dbContext.EstadoSolicitud.ToListAsync();
        }

        public async Task<EstadoSolicitud> ObtenerPorIdEstadoSolicitud(int IdEstadoSolicitud)
        {
            return await _dbContext.EstadoSolicitud.FindAsync(IdEstadoSolicitud);
        }
    }
}
