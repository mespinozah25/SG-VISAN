using PROY.Models;

namespace PROY.Services
{
    public interface IVerificacionCampoService
    {
        Task<List<Solicitud>> ObtenerVerificacionesCampo(int idDepartamento);

        Task<ServiceResponse<Solicitud>> AvanzarEtapa(int IdSolicitud);
    }
}
