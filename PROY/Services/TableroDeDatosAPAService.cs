using PROY.Models.TableroDeDatos;
using PROY.Repositories;

namespace PROY.Services
{
    public class TableroDeDatosAPAService : ITableroDeDatosAPAService
    {
        private readonly ITableroDeDatosAPARepository _tableroRepository;

        public TableroDeDatosAPAService(ITableroDeDatosAPARepository tableroRepository)
        {
            _tableroRepository = tableroRepository;
        }

        public Task<TableroAPATotalSolicitud> ChartTotalSolicitud()
        {
            return _tableroRepository.ChartTotalSolicitud();
        }
        public Task<TableroAPATotalBeneficiarios> ChartTotalBeneficiarios()
        {
            return _tableroRepository.ChartTotalBeneficiarios();
        }

        public async Task<List<TableroAPADepartamentoSolicitud>> ChartSolicitudDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _tableroRepository.ChartSolicitudDepartamento(fechaInicio, fechaFin);
        }

        public async Task<List<TableroAPAMunicipioSolicitud>> ChartSolicitudMunicipio(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _tableroRepository.ChartSolicitudMunicipio(fechaInicio, fechaFin);
        }

        public async Task<List<TableroAPAProyectoDepartamentoSolicitud>> ChartSolicitudProyectoDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _tableroRepository.ChartSolicitudProyectoDepartamento(fechaInicio, fechaFin);
        }

        public async Task<List<TableroAPADepartamentoBeneficiario>> ChartBeneficiarioDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _tableroRepository.ChartBeneficiarioDepartamento(fechaInicio, fechaFin);
        }
        public async Task<List<TableroAPAProyectoSolicitud>> ChartBeneficiarioProyecto(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            return await _tableroRepository.ChartBeneficiarioProyecto(fechaInicio, fechaFin);
        }

    }
}
