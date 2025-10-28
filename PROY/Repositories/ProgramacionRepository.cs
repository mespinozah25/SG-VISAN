using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;
using static PROY.Repositories.ProgramacionRepository;

namespace PROY.Repositories
{
    public class ProgramacionRepository : IProgramacionRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly IEvaluacionRepository _evaluacionRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        public ProgramacionRepository(AppDBContext dBContext, IEvaluacionRepository evaluacionRepository, ISolicitudRepository solicitudRepository)
        {
            _dbContext = dBContext;
            _evaluacionRepository = evaluacionRepository;
            _solicitudRepository = solicitudRepository;
        }

        public async Task<List<Solicitud>> GetAllProgramaciones()
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "PROGRAMACION" && s.Depto == "1")
                                                                  .Include(d => d.DetallesSolicitud)
                                                                  .Where(s => s.DetallesSolicitud.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();
            
            foreach (var item in listado)
            {
                item.BotonDespacho = await _evaluacionRepository.VerificarEvaluacion(item.IdSolicitud, DateTime.Now.Date, "PROGRAMACION");
                //_solicitudRepository.UpdateSolicitud(item);
            }

            // Obtener las solicitudes que cumplen con el filtro principal
            var solicitudesFiltradas = await _dbContext.Solicitudes
                                            .Where(s => s.Restrictiva == 0 && s.Estado == "PROGRAMACION" && s.Depto == "1")
                                            .ToListAsync();

            // Filtrar las solicitudes cuyas DetallesSolicitud cumplen la condición
            /*var solicitudesConDetallesValidos = solicitudesFiltradas
                .Where(s => s.DetallesSolicitud.All(d => d.Restrictiva == 0))
                .ToList();*/

            // Cargar las relaciones de DetallesSolicitud para las solicitudes que pasaron el filtro
            /*await _dbContext.Entry(solicitudesConDetallesValidos)
                .Collection(s => s.DetallesSolicitud)
                .LoadAsync();*/

            return listado;            
        }

        public async Task<List<Solicitud>> GetAllProgramacionesAPA()
        {
            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "PROGRAMACION" && s.Depto == "2")
                                                                  .Include(d => d.DetallesSolicitudAPA)                                                                  
                                                                  .Where(s => s.DetallesSolicitudAPA.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();

            foreach (var item in listado)
            {
                item.BotonDespacho = await _evaluacionRepository.VerificarEvaluacionAPA(item.IdSolicitud, DateTime.Now.Date, "PROGRAMACION");
                //_solicitudRepository.UpdateSolicitud(item);
            }

            // Obtener las solicitudes que cumplen con el filtro principal
            var solicitudesFiltradas = await _dbContext.Solicitudes
                                            .Where(s => s.Restrictiva == 0 && s.Estado == "PROGRAMACION" && s.Depto == "2")
                                            .ToListAsync();

            return listado;
        }

        public async Task<List<DetalleSolicitud>> GetAllDetalleProgramacion(long idProgramacion)
        {
            List<DetalleSolicitud> lista_detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdSolicitud == idProgramacion && ds.Restrictiva == 0 /*&& (ds.IdAnulacion==0 || ds.IdAnulacion==null)*/)
                                                                                               .ToListAsync();
                
            return lista_detalle_encontrado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleProgramacionAPA(long idProgramacion)
        {
            //Detalle de documentos de apoyo por validación de la solicitud indicada de DAPA
            List<DetalleSolicitudAPA> lista_detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(ds => ds.IdSolicitud == idProgramacion && ds.Restrictiva == 0 /*&& (ds.IdAnulacion == 0 || ds.IdAnulacion == null)*/)
                                                                                                      .ToListAsync();

            return lista_detalle_encontrado;
        }


        public async Task<DetalleSolicitud> GetDetalleProgramacionID(long idDetalleProgramacion)
        {
            DetalleSolicitud detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdDetalleSolicitud == idDetalleProgramacion && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleProgramacionIDAPA(long idDetalleProgramacion)
        {
            DetalleSolicitudAPA detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(de => de.IdDetalleSolicitud == idDetalleProgramacion && de.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<List<Agrupado>> GetDetalleSolicitudAgrupado(long idProgramacion)
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
            var sol = new Solicitud();

            try
            {
                sol = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.IdSolicitud == idProgramacion).FirstOrDefaultAsync();
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
                                     where ds.Restrictiva == 0 && ds.IdSolicitud == idProgramacion && (ds.IdAnulacion == null || ds.IdAnulacion == 67)
                                           && dep.Tipo == "DEPARTAMENTO"
                                           && mun.Tipo == "MUNICIPIO"
                                     group ds by new { NombreDepartamento = dep.IdCatalogo, NombreMunicipio = mun.IdCatalogo } into g
                                     select new Agrupado
                                     {
                                         Departamento = g.Key.NombreDepartamento.ToString(),  // Nombre del departamento
                                         Municipio = g.Key.NombreMunicipio.ToString(),    // Nombre del municipio
                                         Beneficiarios = g.Count(),
                                         Raciones = g.Count() * sol.Raciones
                                     })
                              .ToListAsync();
                }
                else if (sol.Depto == "2")
                {
                    listado = await (//from ds in _dbContext.DetalleSolicitudAPA
                                     from s in _dbContext.Solicitudes 
                                     join ds in _dbContext.DetalleSolicitudAPA on s.IdSolicitud equals ds.IdSolicitud into det from d in det.DefaultIfEmpty()
                                     join dep in _dbContext.Catalogos on s.Departamento equals dep.IdCatalogo
                                     join mun in _dbContext.Catalogos on s.Municipio equals mun.IdCatalogo
                                     where s.Restrictiva==0 && s.IdSolicitud==idProgramacion && (d==null || (d.Restrictiva == 0 && d.IdSolicitud == idProgramacion && (d.IdAnulacion == null || d.IdAnulacion == 67)))
                                           && dep.Tipo == "DEPARTAMENTO"
                                           && mun.Tipo == "MUNICIPIO"
                                     group s by new { NombreDepartamento = dep.IdCatalogo, NombreMunicipio = mun.IdCatalogo } into g
                                     select new Agrupado
                                     {
                                         Departamento = g.Key.NombreDepartamento.ToString(),  // Nombre del departamento
                                         Municipio = g.Key.NombreMunicipio.ToString(),    // Nombre del municipio
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

        public class Agrupado
        {
            public string Departamento { get; set; }
            public string Municipio { get; set; }
            public long Beneficiarios { get; set; }
            public long Raciones { get; set; }
        }
    }
}
