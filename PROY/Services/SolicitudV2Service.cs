using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class SolicitudV2Service : ISolicitudV2Service
    {
        private readonly ISolicitudV2Repository _solicitudV2Repository;
        private readonly IDetalleSolicitudAPARepository _detalleSolicitudAPARepository;
        private readonly IDetalleEvaluacionAPARepository _detalleEvaluacionAPARepository;
        private readonly IAsignaRacionService _asignaRacionService;
        private readonly IConfiguration _configuration;

        private readonly IUsuarioService _usuarioService;

        public SolicitudV2Service(ISolicitudV2Repository SolicitudV2Repository, IDetalleSolicitudAPARepository DetalleSolicitudAPARepository, IDetalleEvaluacionAPARepository DetalleEvaluacionAPARepository, IAsignaRacionService AsignaRacionService, IConfiguration Configuration, IUsuarioService usuarioService)
        {
            _solicitudV2Repository = SolicitudV2Repository;
            _detalleSolicitudAPARepository = DetalleSolicitudAPARepository;
            _detalleEvaluacionAPARepository = DetalleEvaluacionAPARepository;
            _asignaRacionService = AsignaRacionService;
            _configuration = Configuration;
            _usuarioService = usuarioService;
        }

        public async Task<List<Solicitud>> ObtenerSolicitudesSegunEstado(int IdEstado, int IdDepartamento, int Rechazada)
        {
            return await _solicitudV2Repository.ObtenerSolicitudesSegunEstado(IdEstado, IdDepartamento, Rechazada);
        }

        public async Task<Solicitud?> ObtenerSolicitudSegunIdSolicitud(int IdSolicitud)
        {
            Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

            if (solicitud == null) return solicitud;

            solicitud.Validos = await _detalleSolicitudAPARepository.TotalRegistrosSegunAnulacion(IdSolicitud, null);

            solicitud.Fallecidos = await _detalleSolicitudAPARepository.TotalRegistrosSegunAnulacion(IdSolicitud, int.Parse(_configuration["Catalogos:Anulaciones:FALLECIDO"]!));

            solicitud.Duplicados = await _detalleSolicitudAPARepository.TotalRegistrosSegunAnulacion(IdSolicitud, int.Parse(_configuration["Catalogos:Anulaciones:DUPLICADO"]!));

            solicitud.SinDatos = await _detalleSolicitudAPARepository.TotalRegistrosSegunAnulacion(IdSolicitud, int.Parse(_configuration["Catalogos:Anulaciones:SIN_DATOS"]!));

            solicitud.Gobierno = await _detalleSolicitudAPARepository.TotalRegistrosSegunAnulacion(IdSolicitud, int.Parse(_configuration["Catalogos:Anulaciones:GOBIERNO"]!));

            solicitud.NoAplica = await _detalleSolicitudAPARepository.TotalRegistrosSegunAnulacion(IdSolicitud, int.Parse(_configuration["Catalogos:Anulaciones:NO_APLICA"]!));

            solicitud.MostrarContinuidadVerificacionCampo = await _detalleEvaluacionAPARepository.ExisteRegistroSegunProceso(IdSolicitud, _configuration["Procesos:APA:VERIFICACION_CAMPO"]!);

            solicitud.MostrarDespachoEnProgramacion = await _detalleEvaluacionAPARepository.ExisteRegistroSegunProceso(IdSolicitud, _configuration["Procesos:APA:PROGRAMACION"]!);

            solicitud.MostrarDespachoEnEjecucion = await _detalleEvaluacionAPARepository.ExisteRegistroSegunProceso(IdSolicitud, _configuration["Procesos:APA:EJECUCION"]!);

            int totalRaciones = solicitud.Raciones * ((int)solicitud.Validos + (int)solicitud.NoAplica);

            int totalAsignado = await _asignaRacionService.ObtenerTotalRacionesAsignadasSegunSolicitud(IdSolicitud);

            solicitud.AsignacionDeRacionesCompleta = totalRaciones - totalAsignado == 0;

            return solicitud;
        }

        public async Task<string> AvanzarASiguienteEtapa(int IdSolicitud)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                if (solicitud.EstadoSolicitud.IdEstadoSolicitudSiguiente == null) return "No se encontro la etapa siguiente";

                solicitud.IdEstadoSolicitud = (int)solicitud.EstadoSolicitud.IdEstadoSolicitudSiguiente;

                solicitud.Rechazada = 0;

                solicitud.ComentarioRechazo = null;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                

                return "Solicitud enviada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al enviar solicitud: {ex}";
            }
        }

        public async Task<string> RegresarAEtapaAnterior(int IdSolicitud, string? ComentarioRechazo = null)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                if (solicitud.EstadoSolicitud.IdEstadoSolicitudAnterior == null) return "No se encontro la etapa anterior";

                solicitud.IdEstadoSolicitud = (int)solicitud.EstadoSolicitud.IdEstadoSolicitudAnterior;

                solicitud.Rechazada = 1;

                solicitud.ComentarioRechazo = ComentarioRechazo;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                return "Solicitud regresada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al regresar solicitud: {ex}";
            }
        }

        public async Task<string> EliminarSolicitud(int IdSolicitud)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                solicitud.Restrictiva = 1;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                return "Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al eliminar solicitud: {ex}";
            }
        }

        public async Task<string> RechazarSolicitud(int IdSolicitud)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                solicitud.Rechazada = 1;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                return "Solicitud rechazada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al rechazar solicitud: {ex}";
            }
        }

        public async Task<string> HabilitarSolicitud(int IdSolicitud)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                solicitud.Rechazada = 0;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                return "Solicitud habilitada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al habilitar solicitud: {ex}";
            }
        }

        public async Task<string> ActualizarFechasEntregaSolicitud(int IdSolicitud, DateTime FechaInicio, DateTime FechaFin, string? Observaciones)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                solicitud.FInicioEntrega = FechaInicio;

                solicitud.FFinEntrega = FechaFin;

                solicitud.TecnicoObservaciones = Observaciones;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                return "Solicitud actualizada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al actualizada solicitud: {ex}";
            }
        }

        public async Task<string> ActualizarRaciones(int idSolicitud, int nuevasRaciones)
        {
            try
            {
                Solicitud? solicitud = await _solicitudV2Repository.ObtenerSolicitudSegunIdSolicitud(idSolicitud);

                if (solicitud == null) return "No se encontro la solicitud";

                solicitud.Raciones = nuevasRaciones;

                await _solicitudV2Repository.ActualizarSolicitud(solicitud);

                return "Solicitud actualizada exitosamente";
            }
            catch (Exception ex)
            {
                return $"Error al actualizada solicitud: {ex}";
            }
        }

        public async Task<List<Solicitud>> ObtenerSolicitudesConFiltros(int idEstado, int idDepto, int? idDepartamento = null, int? idMunicipio = null, DateTime? fechaInicio = null, DateTime? fechaFin = null, int rechazada = 0)
        {
            return await _solicitudV2Repository.ObtenerSolicitudesConFiltros(
                idEstado,
                idDepto,
                rechazada,
                idDepartamento,
                idMunicipio,
                fechaInicio,
                fechaFin);
        }


        #region Bitacora - SIN ServiceResponse

        public async Task<List<Bitacora>> GetAllBitacora(long id)
        {
            return await _solicitudV2Repository.GetAllBitacora(id);
        }

        public async Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso)
        {
            return await _solicitudV2Repository.GetBitacoraEstado(idEstado, idReingreso);
        }

        public async Task<Bitacora> SaveBitacora(Bitacora modelo)
        {
            // Si necesitas lógica adicional antes de guardar, la pones aquí
            return await _solicitudV2Repository.SaveBitacora(modelo);
        }

        #endregion         
    }

}
