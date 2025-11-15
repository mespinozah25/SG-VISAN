using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class AsignaTecnicoRepository : IAsignaTecnicoRepository
    {
        private readonly AppDBContext _dbContext;

        public AsignaTecnicoRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;        
        }

        public async Task<List<AsignaTecnicoV2>> ObtenerAsignacionesTecnico(int IdSolicitud)
        {
            return await _dbContext.AsignaTecnicoV2
                .Where(
                    at => at.Restrictiva == 0 &&
                    at.IdSolicitud == IdSolicitud
                )
                .Include(at => at.Tecnico)
                .OrderByDescending(at => at.IdAsignaTecnico)
                .ToListAsync();
        }

        public async Task<AsignaTecnicoV2?> ObtenerAsignacionTecnicoSegunId(int IdAsignaTecnico)
        {
            AsignaTecnicoV2? asignaTecnico = await _dbContext.AsignaTecnicoV2
                .Where(s => s.IdAsignaTecnico == IdAsignaTecnico)
                .FirstOrDefaultAsync();

            return asignaTecnico;
        }

        public async Task<AsignaTecnicoV2> GuardarAsignacionTecnico(AsignaTecnicoV2 asignaTecnico)
        {
            _dbContext.AsignaTecnicoV2.Add(asignaTecnico);
            
            await _dbContext.SaveChangesAsync();

            return asignaTecnico;
        }

        public async Task<AsignaTecnicoV2> ActualizarAsignacionTecnico(AsignaTecnicoV2 asignaTecnico)
        {
            _dbContext.AsignaTecnicoV2.Update(asignaTecnico);

            await _dbContext.SaveChangesAsync();

            return asignaTecnico;
        }
    }
}
