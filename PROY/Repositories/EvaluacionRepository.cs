using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class EvaluacionRepository:IEvaluacionRepository
    {
        private readonly AppDBContext _dbContext;

        public EvaluacionRepository(AppDBContext dBContext)
        {
            _dbContext = dBContext;        
        }

        public async Task<List<Solicitud>> GetAllEvaluaciones()
        {
            //Listado de solicitudes en evaluacion de DAAN
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado=="EVALUACION" && s.Depto == "1")
                                                                  .Include(d => d.DetallesSolicitud)
                                                                  .Include(e => e.DetallesEvaluacion)
                                                                  .ToListAsync();
            return listado;
        }

        public async Task<List<Solicitud>> GetAllEvaluacionesAPA()
        {
            //Listado de solicitudes en evaluacion de DAPA
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "EVALUACION" && s.Depto=="2")
                                                                  .Include(d => d.DetallesSolicitudAPA)
                                                                  .Include(e => e.DetallesEvaluacionAPA)
                                                                  .ToListAsync();            

            return listado;
        }

        public async Task<List<Solicitud>> GetAllEvaluacionesRechazadas()
        {
            //Listado de solicitudes en evaluacion de DAAN
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "RECHAZADA" && s.Depto == "1")
                                                                  .Include(d => d.DetallesSolicitud)
                                                                  .Include(e => e.DetallesEvaluacion)
                                                                  .ToListAsync();
            return listado;
        }

        public async Task<List<Solicitud>> GetAllEvaluacionesRechazadasAPA()
        {
            //Listado de solicitudes en evaluacion de DAPA
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "RECHAZADA" && s.Depto == "2")
                                                                  .Include(d => d.DetallesSolicitudAPA)
                                                                  .Include(e => e.DetallesEvaluacionAPA)
                                                                  .ToListAsync();

            return listado;
        }        

        public async Task<List<DetalleEvaluacion>> GetAllDetalleEvaluacion(long idEvaluacion)
        {
            //Detalle de documentos de apoyo por validación de la solicitud indicada de DAAN
            List<DetalleEvaluacion> lista_detalle_encontrado = await _dbContext.DetalleEvaluacion.Where(de => de.IdSolicitud == idEvaluacion && de.Restrictiva == 0).ToListAsync();

            return lista_detalle_encontrado;
        }

        public async Task<Boolean> VerificarEvaluacion(long idEvaluacion, DateTime fecha, string proceso)
        {
            Boolean respuesta = false;
            //Detalle de documentos de apoyo por validación de la solicitud indicada de DAAN
            int cuenta= await _dbContext.DetalleEvaluacion.Where(de => de.IdSolicitud==idEvaluacion && de.FechaRegistro==fecha.Date && de.Restrictiva == 0 && de.Proceso==proceso).CountAsync();

            if (cuenta > 0)
                respuesta = true;

            return respuesta;
        }

        public async Task<Boolean> VerificarEvaluacionAPA(long idEvaluacion, DateTime fecha, string proceso)
        {
            Boolean respuesta = false;
            //Detalle de documentos de apoyo por validación de la solicitud indicada de DAAN
            int cuenta = await _dbContext.DetalleEvaluacionAPA.Where(de => de.IdSolicitud == idEvaluacion && de.FechaRegistro == fecha.Date && de.Restrictiva == 0 && de.Proceso == proceso).CountAsync();

            if (cuenta > 0)
                respuesta = true;

            return respuesta;
        }
        public async Task<List<DetalleEvaluacionAPA>> GetAllDetalleEvaluacionAPA(long idEvaluacion)
        {
            //Detalle de documentos de apoyo por validación de la solicitud indicada de DAPA
            List<DetalleEvaluacionAPA> lista_detalle_encontrado = await _dbContext.DetalleEvaluacionAPA.Where(de => de.IdSolicitud == idEvaluacion && de.Restrictiva == 0).ToListAsync();

            return lista_detalle_encontrado;
        }

        public async Task<DetalleEvaluacion> GetDetalleEvaluacionID(long idDetalleEvaluacion)
        {            
            DetalleEvaluacion detalle_encontrado = await _dbContext.DetalleEvaluacion.Where(de => de.IdDetalleEvaluacion == idDetalleEvaluacion && de.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleEvaluacionAPA> GetDetalleEvaluacionIDAPA(long idDetalleEvaluacion)
        {
            DetalleEvaluacionAPA detalle_encontrado = await _dbContext.DetalleEvaluacionAPA.Where(de => de.IdDetalleEvaluacion == idDetalleEvaluacion && de.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<Int64> CuentaDetalle()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleEvaluacion
                .Select(d => d.IdDetalleEvaluacion)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<Int64> CuentaDetalleAPA()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleEvaluacionAPA
                .Select(d => d.IdDetalleEvaluacion)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }
        public async Task<DetalleEvaluacion> SaveDetalleEvaluacion(DetalleEvaluacion modelo)
        {
            _dbContext.DetalleEvaluacion.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<DetalleEvaluacionAPA> SaveDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo)
        {
            _dbContext.DetalleEvaluacionAPA.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<DetalleEvaluacion> UpdateDetalleEvaluacion(DetalleEvaluacion modelo)
        {
            _dbContext.DetalleEvaluacion.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<DetalleEvaluacionAPA> UpdateDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo)
        {
            _dbContext.DetalleEvaluacionAPA.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
