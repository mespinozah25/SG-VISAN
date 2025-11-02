using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class TipoIntervencionService : ITipoIntervencionService
    {

        private readonly ITipoIntervencionRepository _tipoIntervencionRepository;

        public TipoIntervencionService(ITipoIntervencionRepository tipoIntervencionRepository)
        {
            _tipoIntervencionRepository = tipoIntervencionRepository;
        }

        public async Task<List<TipoIntervencion>> ObtenerPorDepartamento(int IdDepto)
        {
            List<TipoIntervencion> listado = await _tipoIntervencionRepository.ObtenerPorDepartamento(IdDepto);

            return listado;
        }
    }
}
