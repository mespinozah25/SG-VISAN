using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class SolicitudV2Repository : ISolicitudV2Repository
    {
        private readonly AppDBContext _dbContext;

        public SolicitudV2Repository(AppDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<List<Solicitud>> ObtenerSolicitudesSegunEstado(int IdEstado, int IdDepartamento, int Rechazada)
        {
            List<Solicitud> listado = await _dbContext.Solicitudes
                .Where(
                    s => s.Restrictiva == 0 &&
                    s.IdEstadoSolicitud == IdEstado &&
                    s.Depto == $"{IdDepartamento}" &&
                    s.Rechazada == Rechazada
                )
                .ToListAsync();
            return listado;
        }

        public async Task<Solicitud?> ObtenerSolicitudSegunIdSolicitud(int IdSolicitud)
        {
            Solicitud? solicitud = await _dbContext.Solicitudes
                .Include(s => s.EstadoSolicitud)
                .ThenInclude(es => es.EstadoSolicitudSiguiente)
                .Where(s => s.IdSolicitud == IdSolicitud)
                .FirstOrDefaultAsync();

            return solicitud;
        }

        public async Task<Solicitud> ActualizarSolicitud(Solicitud solicitud)
        {
            _dbContext.Solicitudes.Update(solicitud);

            await _dbContext.SaveChangesAsync();

            return solicitud;
        }

        public async Task<List<Solicitud>> ObtenerSolicitudesConFiltros(int idEstado, int IdDepto, int rechazada = 0, int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _dbContext.Solicitudes
                .Where(s => s.Restrictiva == 0 &&
                           s.IdEstadoSolicitud == idEstado &&
                           s.Depto == $"{IdDepto}" && // Aquí usa IdDepto (fijo)
                           s.Rechazada == rechazada);

            // Aplicar filtros opcionales
            if (idDepartamento.HasValue)
            {
                query = query.Where(s => s.Departamento == idDepartamento); // Aquí usa idDepartamento (filtro)
            }

            if (idMunicipio.HasValue)
            {
                query = query.Where(s => s.Municipio == idMunicipio);
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(s => s.FechaRegistro >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(s => s.FechaRegistro <= fechaFin.Value);
            }

            var listado = await query.ToListAsync();
            return listado;
        }


        #region BITACORA

        public async Task<List<Bitacora>> GetAllBitacora(long id)
        {
            List<Bitacora> listado = await _dbContext.Bitacora
                .Where(c => c.IdRegistro == id && c.NombreTabla == "SOLICITUD" && c.Estado == 0)
                .ToListAsync();
            return listado;
        }

        public async Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso)
        {
            var bitacora = await _dbContext.Bitacora
                .FirstOrDefaultAsync(b => b.Accion == idEstado && b.IdRegistro == idReingreso);
            return bitacora;
        }

        public async Task<Bitacora> SaveBitacora(Bitacora modelo)
        {
            try
            {
                var maxId = await _dbContext.Bitacora.MaxAsync(b => (long?)b.IdBitacora) ?? 0;

                modelo.IdBitacora = maxId + 1;

                _dbContext.Bitacora.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }

            return modelo;
        }

        #endregion
    }
}
