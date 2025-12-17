using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class DetalleSolicitudAPARepository : IDetalleSolicitudAPARepository
    {
        private readonly AppDBContext _dbContext;

        public DetalleSolicitudAPARepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;        
        }

        public async Task<int> TotalRegistrosSegunAnulacion(int IdSolicitud, int? IdAnulacion)
        {
            int total = await _dbContext.DetalleSolicitudAPA
                .Where(
                    ds => ds.Restrictiva == 0 &&
                    ds.IdSolicitud == IdSolicitud &&
                    ds.IdAnulacion == IdAnulacion
                )
                .CountAsync();
            return total;
        }

        public async Task<int> EliminarRegistrosSolicitud(int IdSolicitud)
        {
            int total = await _dbContext.DetalleSolicitudAPA
                .Where(
                    ds => ds.Restrictiva == 0 &&
                    ds.IdSolicitud == IdSolicitud
                )
                .ExecuteUpdateAsync(setter => setter
                    .SetProperty(p => p.Restrictiva, 1) 
                );
            return total;
        }
    }
}
