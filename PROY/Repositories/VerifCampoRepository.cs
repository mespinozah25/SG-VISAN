using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class VerificacionCampoRepository: IVerificacionCampoRepository
    {
        private readonly AppDBContext _dbContext;

        public VerificacionCampoRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;        
        }

        public async Task<List<Solicitud>> ObtenerVerificacionesCampo(int idDepartamento)
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "VERIFICACION CAMPO" && s.Depto == $"{idDepartamento}")
                                                                .Include(d => d.DetallesSolicitud)
                                                                .Include(e => e.DetallesEvaluacion)

                                                                .Where(s => s.DetallesSolicitud.All(d => d.Restrictiva == 0))
                                                                .Where(s => s.DetallesEvaluacion.All(e => e.Restrictiva == 0))
                                                                .ToListAsync();
            return listado;
        }
    }
}
