using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class AsignaRacionRepository : IAsignaRacionRepository
    {
        private readonly AppDBContext _dbContext;

        public AsignaRacionRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;        
        }

        public async Task<List<AsignaRacionV2>> ObtenerAsignacionesRacion(int IdSolicitud)
        {
            return await _dbContext.AsignaRacionV2
                .Where(
                    ar => ar.Restrictiva == 0 &&
                    ar.IdSolicitud == IdSolicitud
                )
                .OrderByDescending(ar => ar.IdAsignaRacion)
                .ToListAsync();
        }

        public async Task<AsignaRacionV2?> ObtenerAsignacionRacionSegunId(int IdAsignaRacion)
        {
            AsignaRacionV2? asignaRacion = await _dbContext.AsignaRacionV2
                .Where(s => s.IdAsignaRacion == IdAsignaRacion)
                .FirstOrDefaultAsync();

            return asignaRacion;
        }

        public async Task<AsignaRacionV2> GuardarAsignacionRacion(AsignaRacionV2 asignaRacion)
        {
            _dbContext.AsignaRacionV2.Add(asignaRacion);
            
            await _dbContext.SaveChangesAsync();

            return asignaRacion;
        }

        public async Task<AsignaRacionV2> ActulizarAsignacionRacion(AsignaRacionV2 asignaRacion)
        {
            _dbContext.AsignaRacionV2.Update(asignaRacion);

            await _dbContext.SaveChangesAsync();

            return asignaRacion;
        }
        
        public async Task<int> ObtenerTotalRacionesAsignadasSegunSolicitud(int IdSolicitud)
        {
            return await _dbContext.AsignaRacionV2
                .Where(
                    ar => ar.Restrictiva == 0 &&
                    ar.IdSolicitud == IdSolicitud
                )
                .SumAsync(ar => ar.RacionAsignada);
        }
    }
}
