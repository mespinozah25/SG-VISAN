using PROY.Models.TableroDeDatos;

namespace PROY.Repositories
{
    public interface ITableroDeDatosRepository
    {
        #region Consultas Charts
        List<TableroConvenio> ChartRacionesDisponibles();
        List<TableroConvenioAvance> ChartRacionesAvance();
        List<TableroBodegaProducto> ChartBodegasExistenciaProductos();
        List<TableroRacion> ChartRaciones();

        #endregion
    }
}
