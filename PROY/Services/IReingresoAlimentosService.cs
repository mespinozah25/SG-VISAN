using PROY.Models;

namespace PROY.Services
{
    public interface IReingresoAlimentosService
    {
        #region Consultas
        Task<List<EstadoReingresos>> GetAllEstadoReingresos();
        Task<List<object>> GetEgresosLike(string term, int limit);

        #endregion

        #region ReingresoAlimentos
        Task<List<ReingresoAlimentos>> GetAllReingresoAlimentos();
        Task<ReingresoAlimentos> GetReingresoAlimentos(Int64 id);
        Task<ServiceResponse<ReingresoAlimentos>> SaveReingresoAlimentosEstado1(ReingresoAlimentos modelo);
        Task<ServiceResponse<ReingresoAlimentos>> UpdateReingresoAlimentosEstado1(ReingresoAlimentos modelo);
        Task<ServiceResponse<ReingresoAlimentos>> UpdateReingresoAlimentosEstado2(ReingresoAlimentos modelo);
        Task<ServiceResponse<ReingresoAlimentos>> AsignacionUsuarioEncargado(int idEgreso, int idUsuario);
        Task<ServiceResponse<ReingresoAlimentos>> DeleteReingresoAlimentos(Int64 id);
        Task<ServiceResponse<ReingresoAlimentos>> CambioEstadoReingresoAlimentos(Int64 id, int estado);

        #endregion

        #region DetalleReingresoAlimentos
        Task<List<DetalleReingresoAlimentos>> GetAllDetalleReingresoAlimentos(Int64 id);
        Task<DetalleReingresoAlimentos> GetDetalleReingresoAlimentos(Int64 id);
        Task<ServiceResponse<DetalleReingresoAlimentos>> SaveDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo);
        Task<ServiceResponse<DetalleReingresoAlimentos>> UpdateDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo);
        Task<ServiceResponse<DetalleReingresoAlimentos>> DeleteDetalleReingresoAlimentos(Int64 id);

        #endregion

        #region Movimientos Productos
        Task<ServiceResponse<int>> MoverProductosReingresoAMovimientos(Int64 id);

        #endregion

        #region Karex
        Task<ServiceResponse<int>> MoverProductosReingresoAKardex(Int64 id);

        #endregion

        #region Bitacora
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso);
        Task<ServiceResponse<Bitacora>> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
