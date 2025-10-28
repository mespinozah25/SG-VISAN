using PROY.Models;
using PROY.Services;

namespace PROY.Repositories
{
    public interface IIngresoAlimentosRepository
    {
        #region Consultas
        Task<List<EstadoIngresos>> GetAllEstadoIngresos();
        Task<List<TipoNombramiento>> GetAllTipoNombramiento();

        #endregion

        #region INGRESO_ALIMENTOS
        Task<List<IngresoAlimentos>> GetAllIngresoAlimentos();
        Task<IngresoAlimentos> GetIngresoAlimentos(Int64 id);
        Task<Int64> CuentaIngresoAlimentos();
        Task<IngresoAlimentos> SaveIngresoAlimentos(IngresoAlimentos modelo);
        Task<IngresoAlimentos> UpdateIngresoAlimentos(IngresoAlimentos modelo);

        #endregion

        #region PRODUCTOS_INGRESO
        Task<List<ProductosIngreso>> GetAllProductosIngreso(Int64 id);
        Task<ProductosIngreso> GetProductosIngreso(Int64 id);
        Task<Int64> CuentaProductosIngreso();
        Task<ProductosIngreso> SaveProductosIngreso(ProductosIngreso modelo);
        Task<ProductosIngreso> UpdateProductosIngreso(ProductosIngreso modelo);

        #endregion

        #region DETALLE_WAYBILL
        Task<List<DetalleWaybill>>? GetDetalleWaybillDeIngreso(Int64 id);
        Task<DetalleWaybill> GetDetalleWaybill(Int64 id);
        Task<Int64> CuentaDetalleWaybill();
        Task<DetalleWaybill> SaveDetalleWaybill(DetalleWaybill modelo);
        Task<DetalleWaybill> UpdateDetalleWaybill(DetalleWaybill modelo);

        #endregion

        #region JUNTA_RECEPTORA
        /*
        Task<JuntaReceptora>? GetJuntaReceptoraDeIngreso(Int64 id);
        Task<JuntaReceptora>? GetJuntaReceptora(Int64 id);
        Task<Int64> CuentaJuntaReceptora();
        Task<JuntaReceptora> SaveJuntaReceptora(JuntaReceptora modelo);
        Task<JuntaReceptora> UpdateJuntaReceptora(JuntaReceptora modelo);
        */
        #endregion

        #region DETALLE_JUNTA_RECEPTORA
        /*
        Task<List<DetalleJuntaReceptora>>? GetAllDetalleJuntaReceptora(Int64 id);
        Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(Int64 id);
        Task<Int64> CuentaDetalleJuntaReceptora();
        Task<DetalleJuntaReceptora> SaveDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        Task<DetalleJuntaReceptora> UpdateDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        */
        #endregion

        #region DETALLE_BOLETA_ANALISIS
        Task<List<DetalleBoletaAnalisis>>? GetAllDetalleBoletaAnalisis(Int64 id);
        Task<DetalleBoletaAnalisis> GetDetalleBoletaAnalisis(Int64 id);
        Task<Int64> CuentaDetalleBoletaAnalisis();
        Task<DetalleBoletaAnalisis> SaveDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo);
        Task<DetalleBoletaAnalisis> UpdateDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo);

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
        Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado);
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<Int64> CuentaBitacora();
        Task<Bitacora> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
