namespace PROY.Services
{
    public interface ITableroDeDatosService
    {
        #region Consultas Charts
        List<dynamic> ChartBodegasExistenciaProductos();
        List<dynamic> ChartRaciones();

        #endregion
    }
}
