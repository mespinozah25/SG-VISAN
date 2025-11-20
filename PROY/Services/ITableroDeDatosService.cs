using PROY.Models.TableroDeDatos;

namespace PROY.Services
{
    public interface ITableroDeDatosService
    {
        #region Consultas Charts
        List<TableroConvenio> ChartRacionesDisponibles();
        List<TableroConvenioAvance> ChartRacionesAvance();
        List<TableroBodegaProducto> ChartBodegasExistenciaProductos();
        List<TableroRacion> ChartRaciones();

        #endregion
    }
}
