using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;
using PROY.Models.TableroDeDatos;

namespace PROY.Repositories
{
    public class TableroDeDatosAPARepository : ITableroDeDatosAPARepository
    {
        private readonly AppDBContext _dbContext;

        public TableroDeDatosAPARepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TableroAPATotalSolicitud> ChartTotalSolicitud()
        {
            var total = await _dbContext.Solicitudes
                .Where(c => c.Restrictiva == 0 && c.IdEstadoSolicitud >= 9 && c.Depto == "2")
                .CountAsync();

            return new TableroAPATotalSolicitud
            {
                total_solicitudes = total
            };
        }



        public async Task<TableroAPATotalBeneficiarios> ChartTotalBeneficiarios()
        {
            var total = await (from detalle in _dbContext.DetalleSolicitudAPA
                               join solicitud in _dbContext.Solicitudes
                               on detalle.IdSolicitud equals (int)solicitud.IdSolicitud
                               where solicitud.Depto == "2" && solicitud.Restrictiva == 0 && detalle.Restrictiva == 0 && solicitud.IdEstadoSolicitud >= 9
                               select detalle).CountAsync();

            return new TableroAPATotalBeneficiarios
            {
                total_beneficiarios = total
            };
        }

        public async Task<List<TableroAPADepartamentoSolicitud>> ChartSolicitudDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from solicitud in _dbContext.Solicitudes
                        join catalogo in _dbContext.Catalogos
                        on solicitud.Departamento equals (int)catalogo.IdCatalogo
                        where solicitud.Restrictiva == 0 && solicitud.Depto == "2"
                        select new { solicitud, catalogo };

            // Aplicar filtros de fecha si se proporcionan
            if (fechaInicio.HasValue)
            {
                query = query.Where(x => x.solicitud.FechaRegistro >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(x => x.solicitud.FechaRegistro <= fechaFin.Value);
            }

            return await query
                .GroupBy(x => new { x.catalogo.IdCatalogo, x.catalogo.Descripcion })
                .Select(g => new TableroAPADepartamentoSolicitud
                {
                    Departamento = g.Key.Descripcion,
                    Total = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<TableroAPAMunicipioSolicitud>> ChartSolicitudMunicipio(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from solicitud in _dbContext.Solicitudes
                        join catalogo in _dbContext.Catalogos
                        on solicitud.Municipio equals (int)catalogo.IdCatalogo
                        where solicitud.Restrictiva == 0 && solicitud.Depto == "2"
                        select new { solicitud, catalogo };

            // Aplicar filtros de fecha si se proporcionan
            if (fechaInicio.HasValue)
            {
                query = query.Where(x => x.solicitud.FechaRegistro >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(x => x.solicitud.FechaRegistro <= fechaFin.Value);
            }

            return await query
                .GroupBy(x => new { x.catalogo.IdCatalogo, x.catalogo.Descripcion })
                .Select(g => new TableroAPAMunicipioSolicitud
                {
                    Municipio = g.Key.Descripcion,
                    Total = g.Count()
                })
                .ToListAsync();
        }

    }

}