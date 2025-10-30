using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class VerificacionCampoService : IVerificacionCampoService
    {
        private readonly IVerificacionCampoRepository _verificacionCampoRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        public VerificacionCampoService(IVerificacionCampoRepository verificacionCampoRepository, ISolicitudRepository solicitudRepository)
        {
            _verificacionCampoRepository = verificacionCampoRepository;
            _solicitudRepository = solicitudRepository;
        }

        public async Task<List<Solicitud>> ObtenerVerificacionesCampo(int idDepartamento)
        {
            List<Solicitud> listado = await _verificacionCampoRepository.ObtenerVerificacionesCampo(idDepartamento);

            return listado;
        }

        public async Task<ServiceResponse<Solicitud>> AvanzarEtapa(int IdSolicitud)
        {
            ServiceResponse<Solicitud> response = new ServiceResponse<Solicitud>();

            Solicitud solicitud = await _solicitudRepository.GetSolicitud(IdSolicitud);

            try
            {
                solicitud.Estado = "EVALUACION";

                await _solicitudRepository.UpdateSolicitud(solicitud);

                response.Data = solicitud;
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

    }
}
