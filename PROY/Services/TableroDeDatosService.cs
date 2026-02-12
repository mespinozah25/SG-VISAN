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

        public List<TableroConvenio> ChartRacionesDisponibles()
        {
            return _tableroRepository.ChartRacionesDisponibles();
        }
        public List<TableroConvenioAvance> ChartRacionesAvance()
        {
            return _tableroRepository.ChartRacionesAvance();
        }
        public List<TableroBodegaProducto> ChartBodegasExistenciaProductos()
        {
            return _tableroRepository.ChartBodegasExistenciaProductos();
        }

        public List<TableroRacion> ChartRaciones()
        {
            return _tableroRepository.ChartRaciones();
        }
    }
}
