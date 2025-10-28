using PROY.Models;

namespace PROY.Repositories
{
    public interface IReingresoAlimentosRepository
    {
        #region Consultas
        Task<List<EstadoReingresos>> GetAllEstadoReingresos();
        Task<List<object>> GetEgresosLike(string term, int limit);
        Task<EgresoAlimentos> GetEgresoAlimentos(Int64 id);

        #endregion

        #region REINGRESO_ALIMENTOS
        Task<List<ReingresoAlimentos>> GetAllReingresoAlimentos();
        Task<ReingresoAlimentos> GetReingresoAlimentos(Int64 id);
        Task<Int64> CuentaReingresoAlimentos();
        Task<ReingresoAlimentos> SaveReingresoAlimentos(ReingresoAlimentos modelo);
        Task<ReingresoAlimentos> UpdateReingresoAlimentos(ReingresoAlimentos modelo);

        #endregion

        #region DETALLE_REINGRESO_ALIMENTOS
        Task<List<DetalleReingresoAlimentos>> GetAllDetalleReingresoAlimentos(Int64 id);
        Task<DetalleReingresoAlimentos> GetDetalleReingresoAlimentos(Int64 id);
        Task<Int64> CuentaDetalleReingresoAlimentos();
        Task<DetalleReingresoAlimentos> SaveDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo);
        Task<DetalleReingresoAlimentos> UpdateDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo);

        #endregion

        #region MOVIMIENTOS_INGRESO
        Task<Int64> CuentaMovimientosProductos();
        Task<MovimientosProductos>? GetMovimientosProductos(int idBodega, int idProductoAlimenticio, int idPresentacion, int idUnidadMedida);
        Task<MovimientosProductos> SaveMovimientosProductos(MovimientosProductos modelo);
        Task<MovimientosProductos> UpdateMovimientosProductos(MovimientosProductos modelo);

        #endregion

        #region KARDEX
        Task<Int64> CuentaKardex();
        Task<Kardex> SaveKardex(Kardex modelo);

        #endregion

        #region BITACORA
        Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso);
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<Int64> CuentaBitacora();
        Task<Bitacora> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
