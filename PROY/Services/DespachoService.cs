using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class DespachoService : IDespachoService
    {
        private readonly IDespachoRepository _despachoRepository;
        private readonly ISolicitudRepository _solicitudRepository;

        public DespachoService(IDespachoRepository despachoRepository, ISolicitudRepository solicitudRepository)
        {
            _despachoRepository = despachoRepository;
            _solicitudRepository = solicitudRepository;
        }
        public async Task<List<Solicitud>> GetAllDespachos()
        {
            List<Solicitud> listado = await _despachoRepository.GetAllDespachos();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllDespachosAPA()
        {
            List<Solicitud> listado = await _despachoRepository.GetAllDespachosAPA();

            return listado;
        }

        public async Task<List<DetalleSolicitud>> GetAllDetalleDespacho(long idDespacho)
        {
            List<DetalleSolicitud> lista_detalle_encontrado = await _despachoRepository.GetAllDetalleDespacho(idDespacho);

            return lista_detalle_encontrado;
        }

        public Task<DetalleSolicitud> GetDetalleDespachoID(long idDetalleDespacho)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.Estado = "LIQUIDACION";

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
    }
}
