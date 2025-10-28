using PROY.Enum;
using PROY.Models;

namespace PROY.Services
{
    public interface IReporteService
    {
        #region Consultas

        Task<List<MovimientosProductos>> GetAllMovimientosProductos();
        Task<List<Kardex>> GetAllExistenciasProducto();
        Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos();
        List<object> ReporteBodegas(FiltrosReportesDTO filtros);
        List<object> ReporteMovimientoProductos(FiltrosReportesDTO filtros);
        List<object> ReportePersonalBodegas(FiltrosReportesDTO filtros);
        List<dynamic> ReporteExistenciasProducto (FiltrosReportesDTO filtros);
        List<object> ReporteConvenios(FiltrosReportesDTO filtros);
        List<object> ReporteRaciones(FiltrosReportesDTO filtros);
        List<dynamic> ReporteInventarioAlimentos(FiltrosReportesDTO filtros);

        #endregion

        #region Generación de Documento
        Task<string> GenerarReporte(int tipoReporte, FiltrosReportesDTO filtros);

        #endregion
    }
}
