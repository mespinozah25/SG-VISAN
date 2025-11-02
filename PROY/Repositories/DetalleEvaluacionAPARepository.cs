using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class DetalleEvaluacionAPARepository : IDetalleEvaluacionAPARepository
    {
        private readonly AppDBContext _dbContext;

        public DetalleEvaluacionAPARepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;        
        }

        public async Task<bool> ExisteRegistroSegunProceso(int IdSolicitud, string Proceso)
        {
            return await _dbContext.DetalleEvaluacionAPA
                .Where(
                    de => de.Restrictiva == 0 &&
                    de.IdSolicitud == IdSolicitud &&
                    de.Proceso == Proceso
                )
                .AnyAsync();
        }
    }
}
