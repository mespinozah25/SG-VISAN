using PROY.Models.TableroDeDatos;
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

        public List<BodegaProducto> ChartBodegasExistenciaProductos()
        {
            return _tableroRepository.ChartBodegasExistenciaProductos();
        }

        public List<Racion> ChartRaciones()
        {
            return _tableroRepository.ChartRaciones();
        }
    }
}
