namespace PROY.Repositories
{
    public interface ITableroDeDatosRepository
    {
        #region Consultas Charts
        List<dynamic> ChartBodegasExistenciaProductos();
        List<dynamic> ChartRaciones();

        #endregion
    }
}
