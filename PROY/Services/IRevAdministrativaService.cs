using PROY.Models;

namespace PROY.Services
{
    public interface IRevAdministrativaService
    {
        Task<List<Solicitud>> ObtenerRevAdministrativas(int idDepartamento);

        Task<ServiceResponse<Solicitud>> AvanzarEtapa(int IdSolicitud);
    }
}
