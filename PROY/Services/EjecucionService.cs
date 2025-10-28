using PROY.Models;
using PROY.Repositories;
using static PROY.Repositories.EjecucionRepository;

namespace PROY.Services
{
    public class EjecucionService: IEjecucionService
    {
        private readonly IEjecucionRepository _ejecucionRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ICatalogoRepository _catalogoRepository;
        public EjecucionService(IEjecucionRepository ejecucionRepository, ISolicitudRepository solicitudRepository, ICatalogoRepository catalogoRepository)
        {
            _ejecucionRepository = ejecucionRepository;
            _solicitudRepository = solicitudRepository;
            _catalogoRepository = catalogoRepository;
        }

        public async Task<List<Solicitud>> GetAllEjecuciones()
        {
            List<Solicitud> listado = await _ejecucionRepository.GetAllEjecuciones();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllEjecucionesAPA()
        {
            List<Solicitud> listado = await _ejecucionRepository.GetAllEjecucionesAPA();

            return listado;
        }

        public async Task<List<DetalleSolicitud>> GetAllDetalleEjecucion(long idEjecucion)
        {
            List<DetalleSolicitud> lista_detalle_encontrado = await _ejecucionRepository.GetAllDetalleEjecucion(idEjecucion);

            return lista_detalle_encontrado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleEjecucionAPA(long idEjecucion)
        {
            List<DetalleSolicitudAPA> lista_detalle_encontrado = await _ejecucionRepository.GetAllDetalleEjecucionAPA(idEjecucion);

            return lista_detalle_encontrado;
        }
                
        public async Task<DetalleSolicitud> GetDetalleEjecucionID(long idDetalleEjecucion)
        {
            DetalleSolicitud detalle_encontrado = await _ejecucionRepository.GetDetalleEjecucionID(idDetalleEjecucion);

            return detalle_encontrado;
        }

        public async Task<ServiceResponse<DetalleSolicitud>> UpdateDetalleEjecucionID(DetalleSolicitud det)
        {
            var response = new ServiceResponse<DetalleSolicitud>();
            try
            {
                DetalleSolicitud detalle_encontrado = await _ejecucionRepository.GetDetalleEjecucionID(det.IdDetalleSolicitud);

                await _ejecucionRepository.UpdateDetalleEjecucionID(detalle_encontrado);

                response.Data = detalle_encontrado;
                response.Success = true;
                response.Message = "Beneficiario actualizado exitosamente";
            }
            catch (Exception ex) 
            {
                response.Success = false;
                response.Message = $"Error al actualizar el beneficiario: {ex.Message}";
            }            

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitudAPA>> UpdateDetalleEjecucionIDAPA(DetalleSolicitudAPA det)
        {
            var response = new ServiceResponse<DetalleSolicitudAPA>();
            try
            {
                DetalleSolicitudAPA detalle_encontrado = await _ejecucionRepository.GetDetalleEjecucionIDAPA(det.IdDetalleSolicitud);

                await _ejecucionRepository.UpdateDetalleEjecucionIDAPA(detalle_encontrado);

                response.Data = detalle_encontrado;
                response.Success = true;
                response.Message = "Beneficiario actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el beneficiario: {ex.Message}";
            }

            return response;
        }
        public async Task<DetalleSolicitudAPA> GetDetalleEjecucionIDAPA(long idDetalleEjecucion)
        {
            DetalleSolicitudAPA detalle_encontrado = await _ejecucionRepository.GetDetalleEjecucionIDAPA(idDetalleEjecucion);

            return detalle_encontrado;
        }

        public async Task<List<Agrupado>> GetDetalleSolicitudAgrupado(long id)
        {
            var listado = await _ejecucionRepository.GetDetalleSolicitudAgrupado(id);

            return listado;
        }

        public async Task<ServiceResponse<AsignaRacion>> SaveAsignaRacion(AsignaRacion modelo)
        {
            var response = new ServiceResponse<AsignaRacion>();

            try
            {
                // Crear objeto Usuario
                AsignaRacion asignaRacion = new AsignaRacion()
                {
                    IdAsignaRacion = await _ejecucionRepository.CuentaAsignaRacion(),                    
                    SolicitudId = modelo.SolicitudId,
                    IdMunicipio = modelo.IdMunicipio,
                    RacionAsignada = modelo.RacionAsignada,
                    RacionSaldo = modelo.RacionSaldo,
                    Restrictiva = 0
                };

                // Guardar solicitud en la base de datos
                await _ejecucionRepository.SaveAsignaRacion(asignaRacion);

                response.Data = asignaRacion;
                response.Success = true;
                response.Message = "Racion asignada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al asignar la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(long id)
        {
            //Busca por id de solicitud
            List<AsignaRacion> detalle_encontrado = await _ejecucionRepository.GetDetalleRacionesSolicitud(id);

            return detalle_encontrado;
        }

        public async Task<List<Catalogo>> GetComparar(long idEjecucion)
        {
            var lista1 = await _ejecucionRepository.GetDetalleSolicitudAgrupado(idEjecucion);

            var lista2 = await _catalogoRepository.GetCatalogo("DEPARTAMENTO");

            List<Catalogo> lista3 = new List<Catalogo>();
            //List<Catalogo> lista4 = new List<Catalogo>();

            foreach (var todos in lista2)
            {
                foreach (var grupo in lista1)
                {
                    if (grupo.Departamento == todos.IdCatalogo.ToString())
                    {
                        if (!lista3.Any(c => c.IdCatalogo == todos.IdCatalogo))
                        {
                            Catalogo nuevoCatalogo = new Catalogo
                            {
                                IdCatalogo = todos.IdCatalogo,
                                Descripcion = todos.Descripcion,
                                Tipo = todos.Tipo,
                                Orden = todos.Orden,
                                Restrictiva = todos.Restrictiva
                            };

                            lista3.Add(nuevoCatalogo);
                        }

                    }
                }
            }

            return lista3;
        }
    }
}
