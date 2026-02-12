using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class SolicitudRepository : ISolicitudRepository
    {
        private readonly AppDBContext _dbContext;

        public SolicitudRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        //SOLICITUDES DAAN
        public async Task<List<Solicitud>> GetAllSolicitudes()
        {

            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "CREADO" && s.Depto == "1")
                                                                  .Include(d => d.DetallesSolicitud)
                                                                  .Where(s => s.DetallesSolicitud.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();

            // Obtener el catálogo de intervenciones
            /*var intervenciones = await _dbContext.Catalogos.Where(c => c.Tipo == "INTERVENCION" && c.Restrictiva == 0).ToListAsync();

            // Mapear la descripción de la intervención a cada detalle de solicitud
            foreach (var solicitud in listado)
            {
                foreach (var detalle in solicitud.DetalleSolicitud)
                {
                    var intervencion = intervenciones.FirstOrDefault(i => i.IdCatalogo == detalle.IdIntervencion);
                    detalle.IntervencionDescripcion = intervencion != null ? intervencion.Descripcion : "Intervención no encontrada";
                }
            }*/

            return listado;
        }

        public async Task<List<Solicitud>> GetAllSolicitudesAPA()
        {

            List<Solicitud> listado = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.Estado == "CREADO" && s.Depto == "2")
                                                                  .Include(d => d.DetallesSolicitudAPA)
                                                                  .Where(s => s.DetallesSolicitudAPA.All(d => d.Restrictiva == 0))
                                                                  .ToListAsync();

            return listado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudSinFiltro()
        {
            //Busca por id de solicitud
            List<DetalleSolicitudAPA> listado = await _dbContext.DetalleSolicitudAPA
                                                .Where(ds => ds.Restrictiva == 0 && ds.IdAnulacion == null)
                                                .ToListAsync();
            return listado;
        }

        public async Task<List<DetalleSolicitud>> GetAllDetalleSolicitud(long id)
        {

            List<DetalleSolicitud> listado = await _dbContext.DetalleSolicitud.Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id)
                                                                  .Include(d => d.Ninos)
                                                                  .ToListAsync();
            return listado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudAPA(long id)
        {

            List<DetalleSolicitudAPA> listado = await _dbContext.DetalleSolicitudAPA.Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id)
                                                                    .OrderBy(ds => ds.Nombre1Esposa)
                                                                    .ThenBy(ds => ds.Apellido1Esposa)
                                                                    .ToListAsync();
            return listado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudAPACodigo(long id)
        {
            string correlativo = id.ToString();

            var listado = await _dbContext.DetalleSolicitudAPA
                .Include(d => d.Solicitud)
                .Where(d => d.Restrictiva == 0 && d.Solicitud.Correlativo == correlativo)
                .ToListAsync();

            return listado;
        }

        public async Task<Solicitud> GetSolicitud(long id)
        {
            Solicitud solicitud_encontrada = await _dbContext.Solicitudes.Where(s => s.IdSolicitud == id && s.Restrictiva == 0).FirstOrDefaultAsync();

            return solicitud_encontrada;
        }


        public async Task<DetalleSolicitud> GetDetalleSolicitud(long id)
        {
            //Busca por id de solicitud
            DetalleSolicitud detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdDetalleSolicitud == id && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitud> GetDetalleSolicitudByCUI(long id, string paramCUI)
        {
            //Busca por id de solicitud
            DetalleSolicitud detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdSolicitud == id && (ds.CuiEsposo == paramCUI || ds.CuiEsposa == paramCUI) && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleSolicitudByCUIAPA(long id, string paramCUI)
        {
            //Busca por cui y id de solicitud
            DetalleSolicitudAPA detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(ds => ds.IdSolicitud == id && ds.CuiEsposa == paramCUI && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleSolicitudAPA(long id)
        {
            //Busca por id de solicitud
            DetalleSolicitudAPA detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(ds => ds.IdDetalleSolicitud == id && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitud> GetDetalleSolicitudID(long idDetalle)
        {
            //Busca por id de detalle
            DetalleSolicitud detalle_encontrado = await _dbContext.DetalleSolicitud.Where(ds => ds.IdDetalleSolicitud == idDetalle && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleSolicitudIDAPA(long idDetalle)
        {
            //Busca por id de solicitud
            DetalleSolicitudAPA detalle_encontrado = await _dbContext.DetalleSolicitudAPA.Where(ds => ds.IdDetalleSolicitud == idDetalle && ds.Restrictiva == 0).FirstOrDefaultAsync();

            return detalle_encontrado;
        }
        public async Task<Int64> CuentaSolicitud()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Solicitudes
                .Select(s => s.IdSolicitud)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }
        public async Task<Solicitud> SaveSolicitud(Solicitud modelo)
        {
            _dbContext.Solicitudes.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<Int64> CuentaDetalle()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleSolicitud
                .Select(d => d.IdDetalleSolicitud)
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
                maxID = await _dbContext.DetalleSolicitudAPA
                .Select(d => d.IdDetalleSolicitud)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }
        public async Task<DetalleSolicitud> SaveDetalleSolicitud(DetalleSolicitud modelo)
        {
            _dbContext.DetalleSolicitud.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<DetalleSolicitudAPA> SaveDetalleSolicitudAPA(DetalleSolicitudAPA modelo)
        {
            bool existenRegistros = await _dbContext.DetalleSolicitudAPA
               .AnyAsync(ds => ds.Restrictiva == 0
                   && ds.CuiEsposa == modelo.CuiEsposa
                   && ds.IdSolicitud == modelo.IdSolicitud);

            if (existenRegistros)
            {
                modelo.IdAnulacion = 64;
            }

            _dbContext.DetalleSolicitudAPA.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
        public async Task<Solicitud> UpdateSolicitud(Solicitud modelo)
        {
            _dbContext.Solicitudes.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<DetalleSolicitud> UpdateDetalleSolicitud(DetalleSolicitud modelo)
        {
            _dbContext.DetalleSolicitud.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<DetalleSolicitudAPA> UpdateDetalleSolicitudAPA(DetalleSolicitudAPA modelo)
        {
            _dbContext.DetalleSolicitudAPA.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<Int64> CuentaAsignaTecnico()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.AsignaTecnicos
                .Select(at => at.IdAsignaTecnico)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<AsignaTecnico> SaveAsignaTecnico(AsignaTecnico modelo)
        {
            _dbContext.AsignaTecnicos.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        public async Task<AsignaTecnico> UpdateAsignaTecnico(AsignaTecnico modelo)
        {
            _dbContext.AsignaTecnicos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        //public async Task<List<AsignaTecnico>> GetDetalleTecnicosSolicitud(long id)
        public async Task<List<dynamic>> GetDetalleTecnicosSolicitud(long id)
        {
            /*List<AsignaTecnico> listado = await _dbContext.AsignaTecnicos.Where(at => at.Restrictiva == 0 && at.SolicitudId == id)
                                                                         .Include(t => t.Tecnico).Where(t => t.Tecnico.Restrictiva == 0 && t.TecnicoId == t.Tecnico.IdTecnico)
                                                                         .ToListAsync();*/

            List<dynamic> listado = await (from at in _dbContext.AsignaTecnicos
                                           join tec in _dbContext.Tecnicos on at.TecnicoId equals tec.IdTecnico
                                           join dep in _dbContext.Catalogos on Convert.ToInt32(at.DepartamentoId) equals dep.IdCatalogo into depJoin
                                           from dep in depJoin.DefaultIfEmpty()
                                           join mun in _dbContext.Catalogos on Convert.ToInt32(at.IdMunicipio) equals mun.IdCatalogo
                                           join sol in _dbContext.Solicitudes on at.SolicitudId equals sol.IdSolicitud
                                           where at.Restrictiva == 0 && at.SolicitudId == id
                                               && tec.Restrictiva == 0
                                               && (dep == null || dep.Tipo == "DEPARTAMENTO")
                                               && mun.Tipo == "MUNICIPIO"
                                           select new
                                           {
                                               idAsignaTecnico = at.IdAsignaTecnico,
                                               tecnicoId = at.TecnicoId,
                                               tecnico = new
                                               {
                                                   idTecnico = tec.IdTecnico,
                                                   nombre = tec.Nombre,
                                                   telefono = tec.Telefono,
                                                   restrictiva = tec.Restrictiva
                                               },
                                               solicitudId = at.SolicitudId,
                                               solicitud = new
                                               {
                                                   idSolicitud = sol.IdSolicitud,
                                                   correlativo = sol.Correlativo,
                                                   codigo = sol.Codigo,
                                                   fechaIngresoDespacho = sol.FechaIngresoDespacho,
                                                   fechaIngresoDireccion = sol.FechaIngresoDireccion,
                                                   fechaIngresoUnidad = sol.FechaIngresoUnidad,
                                                   idUnidad = sol.IdUnidad,
                                                   idIntervencion = sol.IdIntervencion,
                                                   archivo = sol.Archivo,
                                                   restrictiva = sol.Restrictiva,
                                                   estado = sol.Estado,
                                                   fallecidos = sol.Fallecidos,
                                                   duplicados = sol.Duplicados,
                                                   sinDatos = sol.SinDatos,
                                                   gobierno = sol.Gobierno,
                                                   depto = sol.Depto,
                                                   descripcion = sol.Descripcion,
                                                   idInstitucion = sol.IdInstitucion,
                                                   fInicioEntrega = sol.FInicioEntrega,
                                                   fFinEntrega = sol.FFinEntrega,
                                                   departamento = sol.Departamento,
                                                   municipio = sol.Municipio,
                                                   //direccionEntrega = sol.DireccionEntrega,
                                                   //tecnicoNombre = sol.TecnicoNombre,
                                                   //tecnicoTelefono = sol.TecnicoTelefono,
                                                   tecnicoObservaciones = sol.TecnicoObservaciones,
                                                   raciones = sol.Raciones
                                               },
                                               departamentoId = dep.Descripcion,
                                               idMunicipio = mun.Descripcion,//at.IdMunicipio,
                                               direccionEntrega = at.DireccionEntrega,
                                               restrictiva = at.Restrictiva
                                           }).ToListAsync<Object>();




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

        public async Task<List<Tecnico>> GetAllTecnicos()
        {

            List<Tecnico> listado = await _dbContext.Tecnicos.Where(s => s.Restrictiva == 0).ToListAsync();

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

        public async Task<AsignaRacion> GetAsignaRacion(long idRacion)
        {
            var encontrado = await _dbContext.AsignaRaciones.Where(r => r.IdAsignaRacion == idRacion && r.Restrictiva == 0).FirstOrDefaultAsync();

            return encontrado;
        }

        public async Task<AsignaTecnico> GetAsignaTecnico(long idAsignaTecnico)
        {
            var encontrado = await _dbContext.AsignaTecnicos.Where(t => t.IdAsignaTecnico == idAsignaTecnico && t.Restrictiva == 0).FirstOrDefaultAsync();

            return encontrado;
        }
        public async Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(long id)
        {
            /*List<AsignaRacion> listado = await _dbContext.AsignaRaciones.Where(ar => ar.Restrictiva == 0 && ar.SolicitudId == id)
                                                                        //.Include(t => t.Tecnico).Where(t => t.Tecnico.Restrictiva == 0 && t.TecnicoId == t.Tecnico.IdTecnico)
                                                                        .ToListAsync();*/

            var sol = await _dbContext.Solicitudes.Where(s => s.Restrictiva == 0 && s.IdSolicitud == id).FirstOrDefaultAsync();

            if (sol.Depto == "1")
            {
                var totalBenef = await _dbContext.DetalleSolicitud
                                    .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id && (ds.IdAnulacion == null || ds.IdAnulacion == 67))
                                    .CountAsync();

                var totalRaciones = totalBenef * sol.Raciones;
            }
            else if (sol.Depto == "2")
            {
                var totalBenef = await _dbContext.DetalleSolicitudAPA
                                    .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id && (ds.IdAnulacion == null || ds.IdAnulacion == 67))
                                    .CountAsync();

                var totalRaciones = totalBenef * sol.Raciones;
            }


            var listado = await (from ar in _dbContext.AsignaRaciones
                                 join dep in _dbContext.Catalogos on Convert.ToInt32(ar.DepartamentoId) equals dep.IdCatalogo into depJoin
                                 from dep in depJoin.DefaultIfEmpty()
                                 join mun in _dbContext.Catalogos on Convert.ToInt32(ar.IdMunicipio) equals mun.IdCatalogo
                                 where ar.Restrictiva == 0 && ar.SolicitudId == id
                                       && (dep == null || dep.Tipo == "DEPARTAMENTO")
                                       && mun.Tipo == "MUNICIPIO"
                                 //group ar by new { NombreDepartamento = dep.Descripcion, NombreMunicipio = mun.Descripcion, NombreComunidad = ar.Comunidad } into g
                                 select new AsignaRacion
                                 {
                                     IdAsignaRacion = ar.IdAsignaRacion,
                                     DepartamentoId = dep.Descripcion,  // Nombre del departamento
                                     IdMunicipio = mun.Descripcion,    // Nombre del municipio
                                     Comunidad = ar.Comunidad ?? "",
                                     RacionAsignada = ar.RacionAsignada,
                                     RacionSaldo = ar.RacionSaldo

                                     /*RacionSaldo = (_dbContext.DetalleSolicitud
                                                    .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id)
                                                    .Count()) -
                                                   (_dbContext.AsignaRaciones
                                                    .Where(a => a.IdMunicipio == ar.IdMunicipio
                                                         && a.SolicitudId == ar.SolicitudId
                                                         && a.Restrictiva == 0)
                                                    .Sum(a => a.RacionAsignada))*/
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

        public async Task<int?> GetIntervencionBySolicitudId(Int64? idSolicitud)
        {
            var idIntervencion = await _dbContext.Solicitudes
                .Where(s => s.IdSolicitud == idSolicitud)
                .Select(s => (int?)s.IdIntervencion) // cast por si es nullable
                .FirstOrDefaultAsync();

            return idIntervencion.Value;
        }

    }
}
