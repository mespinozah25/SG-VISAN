using Microsoft.EntityFrameworkCore;
using PROY.Enum;
using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ReporteService : IReporteService
    {
        private readonly IReporteRepository _reporteRepository;

        public ReporteService(IReporteRepository reporteRepository)
        {
            _reporteRepository = reporteRepository;
        }

        #region Consultas

        public async Task<List<MovimientosProductos>> GetAllMovimientosProductos()
        {
            List<MovimientosProductos> listado = await _reporteRepository.GetAllMovimientosProductos();

            return listado;
        }

        public async Task<List<Kardex>> GetAllExistenciasProducto()
        {
            List<Kardex> listado = await _reporteRepository.GetAllExistenciasProducto();

            return listado;
        }

        public async Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos()
        {
            List<SolicitudSalidaAlimentos> listado = await _reporteRepository.GetAllSolicitudSalidaAlimentos();

            return listado;
        }



        public List<object> ReporteBodegas(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReporteBodegas(filtros);

            return query;
        }

        public List<object> ReporteMovimientoProductos(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReporteMovimientoProductos(filtros);

            return query;
        }

        public List<object> ReportePersonalBodegas(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReportePersonalBodegas(filtros);

            return query;
        }

        public List<dynamic> ReporteExistenciasProducto(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReporteExistenciasProducto(filtros);

            return query;
        }

        public List<object> ReporteConvenios(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReporteConvenios(filtros);

            return query;
        }

        public List<object> ReporteRaciones(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReporteRaciones(filtros);

            return query;
        }

        public List<object> ReporteInventarioAlimentos(FiltrosReportesDTO filtros)
        {
            var query = _reporteRepository.ReporteInventarioAlimentos(filtros);

            return query;
        }

        #endregion

        #region Generación de Documentos

        public async Task<string> GenerarReporte(int tipoReporte, FiltrosReportesDTO filtros)
        {
            var tabla = await _reporteRepository.GenerarReporte(tipoReporte, filtros);

            return tabla;
        }

        #endregion
    }
}
