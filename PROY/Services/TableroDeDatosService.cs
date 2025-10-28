using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class TableroDeDatosService : ITableroDeDatosService
    {
        private readonly ITableroDeDatosRepository _tableroRepository;

        public TableroDeDatosService(ITableroDeDatosRepository tableroRepository)
        {
            _tableroRepository = tableroRepository;
        }

        public List<dynamic> ChartBodegasExistenciaProductos()
        {
            var query = _tableroRepository.ChartBodegasExistenciaProductos();

            return query;
        }

        public List<dynamic> ChartRaciones()
        {
            var query = _tableroRepository.ChartRaciones();

            return query;
        }
    }
}
