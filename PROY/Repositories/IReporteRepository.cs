using PROY.Enum;
using PROY.Models;

namespace PROY.Repositories
{
    public interface IReporteRepository
    {
        #region Consultas
        Task<List<MovimientosProductos>> GetAllMovimientosProductos();
        Task<List<Kardex>> GetAllExistenciasProducto();
        Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos();
        List<object> ReporteBodegas(FiltrosReportesDTO filtros);
        List<object> ReporteMovimientoProductos(FiltrosReportesDTO filtros);
        List<object> ReportePersonalBodegas(FiltrosReportesDTO filtros);
        List<dynamic> ReporteExistenciasProducto(FiltrosReportesDTO filtros);
        List<object> ReporteConvenios(FiltrosReportesDTO filtros);
        List<dynamic> ReporteRaciones(FiltrosReportesDTO filtros);
        List<dynamic> ReporteInventarioAlimentos(FiltrosReportesDTO filtros);

        #endregion

        #region Generación de Documentos
        Task<string> GenerarReporte(int tipoReporte, FiltrosReportesDTO filtros);

        #endregion

       
    }
}
