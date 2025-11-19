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
    }
}
