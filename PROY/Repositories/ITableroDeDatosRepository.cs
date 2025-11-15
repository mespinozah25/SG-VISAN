using PROY.Models.TableroDeDatos;

namespace PROY.Repositories
{
    public interface ITableroDeDatosRepository
    {
        #region Consultas Charts
        List<BodegaProducto> ChartBodegasExistenciaProductos();
        List<Racion> ChartRaciones();

        #endregion
    }
}
