using PROY.Models.TableroDeDatos;

namespace PROY.Services
{
    public interface ITableroDeDatosService
    {
        #region Consultas Charts
        List<BodegaProducto> ChartBodegasExistenciaProductos();
        List<Racion> ChartRaciones();

        #endregion
    }
}
