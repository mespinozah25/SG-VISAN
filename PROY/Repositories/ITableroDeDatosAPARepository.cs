using PROY.Models.TableroDeDatos;

namespace PROY.Repositories
{
    public interface ITableroDeDatosAPARepository
    {
        #region Consultas Charts

        Task<TableroAPATotalSolicitud> ChartTotalSolicitud();

        Task<TableroAPATotalBeneficiarios> ChartTotalBeneficiarios();

        Task<List<TableroAPADepartamentoSolicitud>> ChartSolicitudDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<List<TableroAPAMunicipioSolicitud>> ChartSolicitudMunicipio(DateTime? fechaInicio = null, DateTime? fechaFin = null);

        Task<List<TableroAPAProyectoDepartamentoSolicitud>> ChartSolicitudProyectoDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<List<TableroAPADepartamentoBeneficiario>> ChartBeneficiarioDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<List<TableroAPAProyectoSolicitud>> ChartBeneficiarioProyecto(DateTime? fechaInicio = null, DateTime? fechaFin = null);

        #endregion
    }
}