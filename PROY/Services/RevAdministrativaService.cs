using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class RevAdministrativaService : IRevAdministrativaService
    {
        private readonly IRevAdministrativaRepository _revAdministrativaRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        public RevAdministrativaService(IRevAdministrativaRepository revAdministrativaRepository, ISolicitudRepository solicitudRepository)
        {
            _revAdministrativaRepository = revAdministrativaRepository;
            _solicitudRepository = solicitudRepository;
        }

        public async Task<List<Solicitud>> ObtenerRevAdministrativas(int idDepartamento)
        {
            List<Solicitud> listado = await _revAdministrativaRepository.ObtenerRevAdministrativas(idDepartamento);

            return listado;
        }

        public async Task<ServiceResponse<Solicitud>> AvanzarEtapa(int IdSolicitud)
        {
            ServiceResponse<Solicitud> response = new ServiceResponse<Solicitud>();

            Solicitud solicitud = await _solicitudRepository.GetSolicitud(IdSolicitud);

            try
            {
                solicitud.Estado = "VERIFICACION CAMPO";

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
