using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class EvaluacionService:IEvaluacionService
    {
        private readonly IEvaluacionRepository _evaluacionRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        public EvaluacionService(IEvaluacionRepository evaluacionRepository, ISolicitudRepository solicitudRepository)
        {
            _evaluacionRepository = evaluacionRepository;
            _solicitudRepository = solicitudRepository;
        }                

        public async Task<List<Solicitud>> GetAllEvaluaciones()
        {
            List<Solicitud> listado = await _evaluacionRepository.GetAllEvaluaciones();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllEvaluacionesAPA()
        {
            List<Solicitud> listado = await _evaluacionRepository.GetAllEvaluacionesAPA();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllEvaluacionesRechazadas()
        {
            List<Solicitud> listado = await _evaluacionRepository.GetAllEvaluacionesRechazadas();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllEvaluacionesRechazadasAPA()
        {
            List<Solicitud> listado = await _evaluacionRepository.GetAllEvaluacionesRechazadasAPA();

            return listado;
        }

        public async Task<List<DetalleEvaluacion>> GetAllDetalleEvaluacion(long idEvaluacion)
        {
            //Busca por id de solicitud
            List<DetalleEvaluacion> lista_detalle_encontrado = await _evaluacionRepository.GetAllDetalleEvaluacion(idEvaluacion);

            return lista_detalle_encontrado;
        }

        public async Task<List<DetalleEvaluacionAPA>> GetAllDetalleEvaluacionAPA(long idEvaluacion)
        {
            //Busca por id de solicitud
            List<DetalleEvaluacionAPA> lista_detalle_encontrado = await _evaluacionRepository.GetAllDetalleEvaluacionAPA(idEvaluacion);

            return lista_detalle_encontrado;
        }

        public async Task<Boolean> VerificarEvaluacion(long idEvaluacion, DateTime fecha, string proceso)
        {
            Boolean result = false;
            result= await _evaluacionRepository.VerificarEvaluacion(idEvaluacion, fecha, proceso);

            return result;
        }

        public async Task<Boolean> VerificarEvaluacionAPA(long idEvaluacion, DateTime fecha, string proceso)
        {
            Boolean result = false;
            result = await _evaluacionRepository.VerificarEvaluacionAPA(idEvaluacion, fecha, proceso);

            return result;
        }

        public async Task<DetalleEvaluacion> GetDetalleEvaluacionID(long idDetalleEvaluacion)
        {
            DetalleEvaluacion detalle_encontrado = await _evaluacionRepository.GetDetalleEvaluacionID(idDetalleEvaluacion);

            return detalle_encontrado;
        }

        public async Task<DetalleEvaluacionAPA> GetDetalleEvaluacionIDAPA(long idDetalleEvaluacion)
        {
            DetalleEvaluacionAPA detalle_encontrado = await _evaluacionRepository.GetDetalleEvaluacionIDAPA(idDetalleEvaluacion);

            return detalle_encontrado;
        }

        public async Task<ServiceResponse<DetalleEvaluacion>> SaveDetalleEvaluacion(DetalleEvaluacion modelo, string proceso)
        {
            var response = new ServiceResponse<DetalleEvaluacion>();

            try
            {
                // Crear objeto detalle evaluacion
                DetalleEvaluacion detEvaluacion = new DetalleEvaluacion()
                {
                    IdDetalleEvaluacion = await _evaluacionRepository.CuentaDetalle(),
                    IdSolicitud = modelo.IdSolicitud,                    
                    //Descripcion = modelo.Descripcion,
                    IdTipoValidacion = modelo.IdTipoValidacion,
                    Archivo = modelo.Archivo,                    
                    Observaciones = modelo.Observaciones,                
                    Proceso = "VALIDACION",
                    Restrictiva = 0,
                    FechaRegistro=modelo.FechaRegistro,
                    UsuarioRegistro=modelo.UsuarioRegistro
                };

                if (proceso == "EJECUCION")
                    detEvaluacion.Proceso = "EJECUCION";
                else if (proceso == "PROGRAMACION")
                    detEvaluacion.Proceso = "PROGRAMACION";                
                else if (proceso== "VALIDACION")
                    detEvaluacion.Proceso = "VALIDACION";

                // Guardar evaluacion en la base de datos
                await _evaluacionRepository.SaveDetalleEvaluacion(detEvaluacion);

                response.Data = detEvaluacion;
                response.Success = true;
                response.Message = "Detalle Evaluación creado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle de evaluación: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEvaluacionAPA>> SaveDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo, string proceso)
        {
            var response = new ServiceResponse<DetalleEvaluacionAPA>();

            try
            {
                // Crear objeto detalle evaluacion
                DetalleEvaluacionAPA detEvaluacion = new DetalleEvaluacionAPA()
                {
                    IdDetalleEvaluacion = await _evaluacionRepository.CuentaDetalleAPA(),
                    IdSolicitud = modelo.IdSolicitud,
                    //Descripcion = modelo.Descripcion,
                    IdTipoValidacion = modelo.IdTipoValidacion,
                    Archivo = modelo.Archivo,
                    Observaciones = modelo.Observaciones,
                    Proceso = proceso,
                    Restrictiva = 0,
                    FechaRegistro = modelo.FechaRegistro,
                    UsuarioRegistro = modelo.UsuarioRegistro
                };

                // Guardar evaluacion en la base de datos
                await _evaluacionRepository.SaveDetalleEvaluacionAPA(detEvaluacion);

                response.Data = detEvaluacion;
                response.Success = true;
                response.Message = "Detalle Evaluación creado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle de evaluación: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEvaluacion>> UpdateDetalleEvaluacion(DetalleEvaluacion modelo)
        {
            var response = new ServiceResponse<DetalleEvaluacion>();
            DetalleEvaluacion solicitud_encontrada = await _evaluacionRepository.GetDetalleEvaluacionID(modelo.IdDetalleEvaluacion);

            try
            {
                solicitud_encontrada.IdDetalleEvaluacion = modelo.IdDetalleEvaluacion;
                ///solicitud_encontrada.IdIntervencion = modelo.IdIntervencion;
                //solicitud_encontrada.Descripcion = modelo.Descripcion;
                solicitud_encontrada.IdTipoValidacion = modelo.IdTipoValidacion == null ? solicitud_encontrada.IdTipoValidacion : modelo.IdTipoValidacion;
                solicitud_encontrada.Archivo = !string.IsNullOrEmpty(modelo.Archivo) ? modelo.Archivo : solicitud_encontrada.Archivo;
                //solicitud_encontrada.Archivo = modelo.Archivo ?? solicitud_encontrada.Archivo;
                solicitud_encontrada.Observaciones = modelo.Observaciones;

                await _evaluacionRepository.UpdateDetalleEvaluacion(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Detalle Evaluación actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle evaluación: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEvaluacionAPA>> UpdateDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo)
        {
            var response = new ServiceResponse<DetalleEvaluacionAPA>();
            DetalleEvaluacionAPA solicitud_encontrada = await _evaluacionRepository.GetDetalleEvaluacionIDAPA(modelo.IdDetalleEvaluacion);

            try
            {
                solicitud_encontrada.IdDetalleEvaluacion = modelo.IdDetalleEvaluacion;
                ///solicitud_encontrada.IdIntervencion = modelo.IdIntervencion;
                //solicitud_encontrada.Descripcion = modelo.Descripcion;
                solicitud_encontrada.IdTipoValidacion = modelo.IdTipoValidacion == null ? solicitud_encontrada.IdTipoValidacion : modelo.IdTipoValidacion;
                solicitud_encontrada.Archivo = modelo.Archivo ?? solicitud_encontrada.Archivo;
                solicitud_encontrada.Observaciones = modelo.Observaciones;

                await _evaluacionRepository.UpdateDetalleEvaluacionAPA(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Detalle Evaluación actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle evaluación: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.Estado = "PROGRAMACION";

                await _solicitudRepository.UpdateSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Solicitud enviada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al enviar la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Solicitud>> UpdateRechazada(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.Estado = "RECHAZADA";

                await _solicitudRepository.UpdateSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Solicitud ha sido rechazada";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar actualizar el estado de la solicitud: {ex.Message}";
            }

            return response;
        }
        public async Task<ServiceResponse<DetalleEvaluacion>> DeleteDetalleEvaluacion(long idDetalleEvaluacion)
        {
            var response = new ServiceResponse<DetalleEvaluacion>();
            DetalleEvaluacion solicitud_encontrada = await _evaluacionRepository.GetDetalleEvaluacionID(idDetalleEvaluacion);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _evaluacionRepository.UpdateDetalleEvaluacion(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Detalle de Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle de la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEvaluacionAPA>> DeleteDetalleEvaluacionAPA(long idDetalleEvaluacion)
        {
            var response = new ServiceResponse<DetalleEvaluacionAPA>();
            DetalleEvaluacionAPA solicitud_encontrada = await _evaluacionRepository.GetDetalleEvaluacionIDAPA(idDetalleEvaluacion);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _evaluacionRepository.UpdateDetalleEvaluacionAPA(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Detalle de Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle de la solicitud: {ex.Message}";
            }

            return response;
        }

    }
}
