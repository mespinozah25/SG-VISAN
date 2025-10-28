using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class DespachoRepository : IDespachoRepository
    {
        private readonly AppDBContext _dbContext;

        public DespachoRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public async Task<List<Solicitud>> GetAllDespachos()
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "DESPACHO" && s.Depto == "1")
                                                                  .Include(d => d.DetallesSolicitud)
                                                                  .Where(s => s.DetallesSolicitud.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();
            return listado;
        }

        public async Task<List<Solicitud>> GetAllDespachosAPA()
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "DESPACHO" && s.Depto == "2")
                                                                  .Include(d => d.DetallesSolicitudAPA)
                                                                  .Where(s => s.DetallesSolicitudAPA.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();

            return listado;
        }

        public async Task<List<DetalleSolicitud>> GetAllDetalleDespacho(long idDespacho)
        {
            List<DetalleSolicitud> lista_detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdSolicitud == idDespacho && ds.Restrictiva == 0 && (ds.IdAnulacion == 0 || ds.IdAnulacion == null))
                                                                                               .ToListAsync();

            return lista_detalle_encontrado;
        }

        public Task<DetalleSolicitud> GetDetalleDespachoID(long idDetalleDespacho)
        {
            throw new NotImplementedException();
        }
    }
}
