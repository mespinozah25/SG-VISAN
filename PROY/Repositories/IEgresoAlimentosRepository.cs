using PROY.Models;

namespace PROY.Repositories
{
    public interface IEgresoAlimentosRepository
    {
        Task<List<EstadoEgresos>> GetAllEstadoEgresos();
        Task<List<EstadoTraslados>> GetAllEstadoTraslados();
        Task<List<dynamic>> GetAllDetallesRacionXConvenio(int idConvenio);

        #region EGRESO_ALIMENTOS
        Task<List<EgresoAlimentos>> GetAllEgresoAlimentos();
        Task<EgresoAlimentos> GetEgresoAlimentos(Int64 id);
        Task<Int64> CuentaEgresoAlimentos();
        Task<EgresoAlimentos> SaveEgresoAlimentos(EgresoAlimentos modelo);
        Task<EgresoAlimentos> UpdateEgresoAlimentos(EgresoAlimentos modelo);

        #endregion

        #region DETALLE_EGRESO_ALIMENTOS
        Task<List<dynamic>>? GetAllDetalleEgresoAlimentos(Int64 id);
        Task<DetalleEgresoAlimentos> GetDetalleEgresoAlimentos(Int64 id);
        Task<Int64> CuentaDetalleEgresoAlimentos();
        Task<DetalleEgresoAlimentos> SaveDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo);
        Task<DetalleEgresoAlimentos> UpdateDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo);

        #endregion

        #region SOLICITUD_SALIDA_ALIMENTOS
        Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos(Int64 id);
        Task<SolicitudSalidaAlimentos> GetSolicitudSalidaAlimentos(Int64 id);
        Task<Int64> CuentaSolicitudSalidaAlimentos();
        Task<SolicitudSalidaAlimentos> SaveSolicitudSalidaAlimentos(SolicitudSalidaAlimentos modelo);
        Task<SolicitudSalidaAlimentos> UpdateSolicitudSalidaAlimentos(SolicitudSalidaAlimentos modelo);

        #endregion

        #region DETALLE_SALIDA_ALIMENTOS
        Task<List<DetalleSalidaAlimentos>> GetAllDetalleSalidaAlimentos(Int64 id);
        Task<DetalleSalidaAlimentos> GetDetalleSalidaAlimentos(Int64 id);
        Task<Int64> CuentaDetalleSalidaAlimentos();
        Task<DetalleSalidaAlimentos> SaveDetalleSalidaAlimentos(DetalleSalidaAlimentos modelo);
        Task<DetalleSalidaAlimentos> UpdateDetalleSalidaAlimentos(DetalleSalidaAlimentos modelo);

        #endregion

        #region TRASLADO_ALIMENTOS
        Task<List<TrasladoAlimentos>> GetAllTrasladoAlimentos();
        Task<TrasladoAlimentos> GetTrasladoAlimentos(Int64 id);
        Task<TrasladoAlimentos>? GetTrasladoAlimentosDeEgreso(Int64 id);
        Task<Int64> CuentaTrasladoAlimentos();
        Task<TrasladoAlimentos> SaveTrasladoAlimentos(TrasladoAlimentos modelo);
        Task<TrasladoAlimentos> UpdateTrasladoAlimentos(TrasladoAlimentos modelo);

        #endregion

        #region MOVIMIENTOS_INGRESO
        Task<Int64> CuentaMovimientosProductos();
        Task<MovimientosProductos>? GetMovimientosProductos(int idBodega, int idProductoAlimenticio, int idUnidadMedida);
        Task<MovimientosProductos> SaveMovimientosProductos(MovimientosProductos modelo);
        Task<MovimientosProductos> UpdateMovimientosProductos(MovimientosProductos modelo);

        #endregion

        #region KARDEX
        Task<Int64> CuentaKardex();
        Task<Kardex> SaveKardex(Kardex modelo);

        #endregion

        #region BITACORA
        Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado);
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<Int64> CuentaBitacora();
        Task<Bitacora> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
