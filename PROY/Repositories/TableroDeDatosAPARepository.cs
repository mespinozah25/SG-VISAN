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

        public async Task<List<TableroAPAProyectoDepartamentoSolicitud>> ChartSolicitudProyectoDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from solicitud in _dbContext.Solicitudes
                        join departamento in _dbContext.Catalogos
                        on solicitud.Departamento equals departamento.IdCatalogo
                        join proyecto in _dbContext.Catalogos
                        on solicitud.IdProyecto equals proyecto.IdCatalogo
                        where solicitud.Restrictiva == 0 && solicitud.Depto == "2"
                        select new { solicitud, departamento, proyecto };

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
                .GroupBy(x => new
                {
                    ProyectoId = x.proyecto.IdCatalogo,
                    ProyectoDesc = x.proyecto.Descripcion,
                    DepartamentoId = x.departamento.IdCatalogo,
                    DepartamentoDesc = x.departamento.Descripcion
                })
                .Select(g => new TableroAPAProyectoDepartamentoSolicitud
                {
                    Proyecto = g.Key.ProyectoDesc,
                    Departamento = g.Key.DepartamentoDesc,
                    Total = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<TableroAPADepartamentoBeneficiario>> ChartBeneficiarioDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from detalle in _dbContext.DetalleSolicitudAPA
                        join solicitud in _dbContext.Solicitudes
                        on detalle.IdSolicitud equals (int)solicitud.IdSolicitud
                        join catalogo in _dbContext.Catalogos
                        on solicitud.Departamento equals (int)catalogo.IdCatalogo
                        where solicitud.Depto == "2" && solicitud.Restrictiva == 0 && detalle.Restrictiva == 0
                        select new { detalle, solicitud, catalogo };

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
                .Select(g => new TableroAPADepartamentoBeneficiario
                {
                    Departamento = g.Key.Descripcion,
                    Total = g.Count()
                })
                .ToListAsync();
        }

        public async Task<List<TableroAPAProyectoSolicitud>> ChartBeneficiarioProyecto(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = from detalle in _dbContext.DetalleSolicitudAPA
                        join solicitud in _dbContext.Solicitudes
                        on detalle.IdSolicitud equals (int)solicitud.IdSolicitud
                        join proyecto in _dbContext.Catalogos
                        on solicitud.IdProyecto equals (int)proyecto.IdCatalogo
                        where solicitud.Depto == "2" && solicitud.Restrictiva == 0 && detalle.Restrictiva == 0
                        select new { detalle, solicitud, proyecto };

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
                .GroupBy(x => new { x.proyecto.IdCatalogo, x.proyecto.Descripcion })
                .Select(g => new TableroAPAProyectoSolicitud
                {
                    Proyecto = g.Key.Descripcion, // Cambiado de NombreProyecto a Descripcion
                    Total = g.Count()
                })
                .ToListAsync();
        }

    }

}