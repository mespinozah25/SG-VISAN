using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class RevAdministrativaService: IRevAdministrativaService
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

    }
}
