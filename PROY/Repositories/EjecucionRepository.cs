using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;
using System.Runtime.Intrinsics.Arm;

namespace PROY.Repositories
{
    public class EjecucionRepository : IEjecucionRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly IEvaluacionRepository _evaluacionRepository;

        public EjecucionRepository(AppDBContext dBContext, IEvaluacionRepository evaluacionRepository)
        {
            _dbContext = dBContext;
            _evaluacionRepository = evaluacionRepository;
        }
        public async Task<List<Solicitud>> GetAllEjecuciones()
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "EJECUCION" && s.Depto == "1")
                                                                  .Include(d => d.DetallesSolicitud)
                                                                  .Where(s => s.DetallesSolicitud.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();

            foreach (var item in listado)
            {
                item.BotonDespacho = await _evaluacionRepository.VerificarEvaluacion(item.IdSolicitud, DateTime.Now.Date, "EJECUCION");
                //_solicitudRepository.UpdateSolicitud(item);
            }

            // Obtener las solicitudes que cumplen con el filtro principal
            var solicitudesFiltradas = await _dbContext.Solicitudes
                                            .Where(s => s.Restrictiva == 0 && s.Estado == "EJECUCION" && s.Depto == "1")
                                            .ToListAsync();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllEjecucionesAPA()
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "EJECUCION" && s.Depto == "2")
                                                                  .Include(d => d.DetallesSolicitudAPA)
                                                                  .Where(s => s.DetallesSolicitudAPA.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();

            foreach (var item in listado)
            {
                item.BotonDespacho = await _evaluacionRepository.VerificarEvaluacionAPA(item.IdSolicitud, DateTime.Now.Date, "EJECUCION");
                //_solicitudRepository.UpdateSolicitud(item);
            }

            // Obtener las solicitudes que cumplen con el filtro principal
            var solicitudesFiltradas = await _dbContext.Solicitudes
                                            .Where(s => s.Restrictiva == 0 && s.Estado == "EJECUCION" && s.Depto == "2")
                                            .ToListAsync();

            return listado;
        }
        public async Task<List<DetalleSolicitud>> GetAllDetalleEjecucion(long idEjecucion)
        {
            List<DetalleSolicitud> lista_detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdSolicitud == idEjecucion && ds.Restrictiva == 0 && (ds.IdAnulacion==0 || ds.IdAnulacion==null || ds.IdAnulacion==67))
                                                                                               .ToListAsync();

            return lista_detalle_encontrado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleEjecucionAPA(long idEjecucion)
        {
            List<DetalleSolicitudAPA> lista_detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(ds => ds.IdSolicitud == idEjecucion && ds.Restrictiva == 0 && (ds.IdAnulacion == 0 || ds.IdAnulacion == null || ds.IdAnulacion == 67))
                                                                                                      .ToListAsync();

            return lista_detalle_encontrado;
        }        

        public async Task<DetalleSolicitud> GetDetalleEjecucionID(long idDetalleEjecucion)
        {
            DetalleSolicitud detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdDetalleSolicitud == idDetalleEjecucion && ds.Restrictiva == 0)
                                                                                   .FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitud> UpdateDetalleEjecucionID(DetalleSolicitud det)
        {
            _dbContext.DetalleSolicitud.Update(det);
            await _dbContext.SaveChangesAsync();
            return det;
        }

        public async Task<DetalleSolicitudAPA> UpdateDetalleEjecucionIDAPA(DetalleSolicitudAPA det)
        {
            _dbContext.DetalleSolicitudAPA.Update(det);
            await _dbContext.SaveChangesAsync();
            return det;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleEjecucionIDAPA(long idDetalleEjecucion)
        {
            DetalleSolicitudAPA detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(de => de.IdDetalleSolicitud == idDetalleEjecucion && de.Restrictiva == 0)
                                                                                         .FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<List<Agrupado>> GetDetalleSolicitudAgrupado(long id)
        {

            /*var listado = await _dbContext.DetalleSolicitud.Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id)                                                           
                                                            .GroupBy(a => new { a.IdDepartamento, a.IdMunicipio })
                                                            .Select(g => new Agrupado
                                                            {
                                                                Departamento = g.Key.IdDepartamento.ToString(),
                                                                Municipio = g.Key.IdMunicipio.ToString(),
                                                                Cantidad = g.Count()
                                                            })
                                                             .ToListAsync();*/
            var sol=new Solicitud();

            try
            {
                sol = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.IdSolicitud == id).FirstOrDefaultAsync();
            }
            catch
            {
                sol.Raciones = 0;
            }


            var listado = new List<Agrupado>();
            try 
            {
                if (sol.Depto == "1")
                {
                    listado = await (from ds in _dbContext.DetalleSolicitud
                                     join dep in _dbContext.Catalogos on ds.IdDepartamento equals dep.IdCatalogo
                                     join mun in _dbContext.Catalogos on ds.IdMunicipio equals mun.IdCatalogo
                                     where ds.Restrictiva == 0 && ds.IdSolicitud == id && (ds.IdAnulacion == null || ds.IdAnulacion == 67)
                                           && dep.Tipo == "DEPARTAMENTO"
                                           && mun.Tipo == "MUNICIPIO"
                                     group ds by new { NombreDepartamento = dep.Descripcion, NombreMunicipio = mun.Descripcion, NombreComunidad = ds.Comunidad } into g
                                     //group ds by new { NombreDepartamento = dep.Descripcion, NombreMunicipio = mun.Descripcion } into g
                                     select new Agrupado
                                     {
                                         Departamento = g.Key.NombreDepartamento,  // Nombre del departamento
                                         Municipio = g.Key.NombreMunicipio,    // Nombre del municipio
                                         Comunidad = g.Key.NombreComunidad,
                                         Beneficiarios = g.Count(),
                                         Raciones = g.Count() * sol.Raciones
                                     })
                         .ToListAsync();
                }
                else if (sol.Depto == "2")
                {
                    listado = await (from ds in _dbContext.DetalleSolicitudAPA
                                     join s in _dbContext.Solicitudes on ds.IdSolicitud equals s.IdSolicitud
                                     join dep in _dbContext.Catalogos on s.Departamento equals dep.IdCatalogo
                                     join mun in _dbContext.Catalogos on s.Municipio equals mun.IdCatalogo
                                     where s.Restrictiva==0 && ds.Restrictiva == 0 && ds.IdSolicitud == id && (ds.IdAnulacion == null || ds.IdAnulacion == 67)
                                           && dep.Tipo == "DEPARTAMENTO"
                                           && mun.Tipo == "MUNICIPIO"
                                     group ds by new { NombreDepartamento = dep.Descripcion, NombreMunicipio = mun.Descripcion, NombreComunidad = s.Comunidad } into g
                                     //group ds by new { NombreDepartamento = dep.Descripcion, NombreMunicipio = mun.Descripcion } into g
                                     select new Agrupado
                                     {
                                         Departamento = g.Key.NombreDepartamento.ToString(),  // Nombre del departamento
                                         Municipio = g.Key.NombreMunicipio.ToString(),    // Nombre del municipio
                                         Comunidad = g.Key.NombreComunidad.ToString(),
                                         Beneficiarios = g.Count(),
                                         Raciones = g.Count() * sol.Raciones
                                     })
                         .ToListAsync();
                }
                
            }
            catch
            {

            }
            

            return listado;
        }

        public async Task<Int64> CuentaAsignaRacion()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.AsignaRaciones
                .Select(at => at.IdAsignaRacion)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<AsignaRacion> SaveAsignaRacion(AsignaRacion modelo)
        {
            _dbContext.AsignaRaciones.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<AsignaRacion> UpdateAsignaRacion(AsignaRacion modelo)
        {
            _dbContext.AsignaRaciones.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(long id)
        {
            /*List<AsignaRacion> listado = await _dbContext.AsignaRaciones.Where(ar => ar.Restrictiva == 0 && ar.SolicitudId == id)
                                                                        //.Include(t => t.Tecnico).Where(t => t.Tecnico.Restrictiva == 0 && t.TecnicoId == t.Tecnico.IdTecnico)
                                                                        .ToListAsync();*/
            var listado = await (from ar in _dbContext.AsignaRaciones 
                                 join dep in _dbContext.Catalogos on Convert.ToInt32(ar.DepartamentoId) equals dep.IdCatalogo into depJoin from dep in depJoin.DefaultIfEmpty()
                                 join mun in _dbContext.Catalogos on Convert.ToInt32(ar.IdMunicipio) equals mun.IdCatalogo
                                 where ar.Restrictiva == 0 && ar.SolicitudId == id
                                       && dep.Tipo == "DEPARTAMENTO"
                                       && mun.Tipo == "MUNICIPIO"
                                 select new AsignaRacion
                                 {
                                     DepartamentoId =  dep.Descripcion,  // Nombre del departamento
                                     IdMunicipio = mun.Descripcion,    // Nombre del municipio                                     
                                     RacionAsignada = ar.RacionAsignada
                                 }).ToListAsync();

            /*List<AsignaTecnico> listado = await _dbContext.AsignaTecnicos
                                                    .Where(at => at.Restrictiva == 0 && at.SolicitudId == id)
                                                    .Include(at => at.Tecnico) // Incluir la información del técnico
                                                    .Where(at => at.Tecnico != null && at.Tecnico.Restrictiva == 0 && at.Tecnico.IdTecnico==at.TecnicoId) // Filtrar técnicos con Restrictiva == 0
                                                    .ToListAsync();*/

            /*var listado = await _dbContext.AsignaTecnicos
                            .Where(a => a.SolicitudId == id)
                            .Where(at => at.Restrictiva == 0 && at.SolicitudId == id)
                            .Include(at => at.Tecnico)
                            .Where(at => at.Tecnico != null && at.Tecnico.Restrictiva == 0 && at.Tecnico.IdTecnico == at.TecnicoId)
                            .Select(a => new
                            {
                                a.TecnicoId,
                                a.SolicitudId
                                //a.Tecnico.Nombre,
                                //a.Tecnico.Telefono // Campo de la entidad relacionada, si está en la relación
                            })
                            .ToListAsync();*/

            return listado;


            //.Include(s=>s.Solicitud).Where(s=>s.Solicitud.Restrictiva==0)
        }

        public class Agrupado
        {
            public string Departamento { get; set; }
            public string Municipio { get; set; }
            public string Comunidad { get; set; }
            public long Beneficiarios { get; set; }
            public long Raciones { get; set; }
        }
    }
}
