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
                           s.IdEstadoSolicitud >= idEstado &&
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

        public async Task<List<ReporteEjecucion>> ObtenerEjecucionesAPAConFiltros(int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from arv in _dbContext.AsignaRacionV2
                        join solicitud in _dbContext.Solicitudes on arv.IdSolicitud equals (int)solicitud.IdSolicitud
                        join municipio in _dbContext.Catalogos on arv.IdMunicipio equals (int)municipio.IdCatalogo
                        join departamento in _dbContext.Catalogos on arv.IdDepartamento equals (int)departamento.IdCatalogo
                        where solicitud.Restrictiva == 0 &&
                              solicitud.IdEstadoSolicitud >= 8 &&
                              solicitud.Depto == "2"
                        select new { arv, solicitud, municipio, departamento };

            // Aplicar filtros opcionales
            if (idDepartamento.HasValue)
            {
                query = query.Where(x => x.arv.IdDepartamento == idDepartamento.Value);
            }

            if (idMunicipio.HasValue)
            {
                query = query.Where(x => x.arv.IdMunicipio == idMunicipio.Value);
            }

            if (fechaInicio.HasValue)
            {
                query = query.Where(x => x.solicitud.FechaRegistro >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(x => x.solicitud.FechaRegistro <= fechaFin.Value);
            }

            var listado = await query
                .GroupBy(x => new
                {
                    Departamento = x.departamento.Descripcion,
                    Municipio = x.municipio.Descripcion,
                })
                .Select(g => new ReporteEjecucion
                {
                    Departamento = g.Key.Departamento,
                    Municipio = g.Key.Municipio,
                    CantidadComunidades = g.Select(x => x.arv.Comunidad).Distinct().Count(),
                    TotalRaciones = g.Sum(x => x.solicitud.Raciones)
                })
                .OrderBy(x => x.Departamento)
                .ThenBy(x => x.Municipio)
                .ToListAsync();

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
