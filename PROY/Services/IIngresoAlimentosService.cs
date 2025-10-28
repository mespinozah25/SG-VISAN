using PROY.Models;

namespace PROY.Services
{
    public interface IIngresoAlimentosService
    {
        #region Consultas
        Task<List<EstadoIngresos>> GetAllEstadoIngresos();
        Task<List<TipoNombramiento>> GetAllTipoNombramiento();

        #endregion

        #region IngresoAlimentos
        Task<List<IngresoAlimentos>> GetAllIngresoAlimentos();
        Task<IngresoAlimentos> GetIngresoAlimentos(Int64 id);
        Task<ServiceResponse<IngresoAlimentos>> SaveIngresoAlimentosEstado1(IngresoAlimentos modelo);
        Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado1(IngresoAlimentos modelo);
        Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado2(IngresoAlimentos modelo);
        Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado3(IngresoAlimentos modelo);
        Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado4(IngresoAlimentos modelo);
        Task<ServiceResponse<IngresoAlimentos>> AsignacionUsuarioPendienteFinalizar(int idIngreso, int idUsuario);
        Task<ServiceResponse<IngresoAlimentos>> DeleteIngresoAlimentos(Int64 id);
        Task<ServiceResponse<IngresoAlimentos>> CambioEsadoIngresoAlimentos(Int64 id, int estado);

        #endregion

        #region ProductosIngreso
        Task<List<ProductosIngreso>> GetAllProductosIngreso(Int64 id);
        Task<ProductosIngreso> GetProductosIngreso(Int64 id);
        Task<ServiceResponse<ProductosIngreso>> SaveProductosIngreso(ProductosIngreso modelo);
        Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngreso(ProductosIngreso modelo);
        Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngresoWaybillEstado2(ProductosIngreso modelo);
        Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngresoEstado2(ProductosIngreso modelo);
        Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngresoEstado3(ProductosIngreso modelo);
        Task<ServiceResponse<ProductosIngreso>> DeleteProductosIngreso(Int64 id);

        #endregion

        #region DetalleWaybill

        Task<List<DetalleWaybill>>? GetDetalleWaybillDeIngreso(Int64 id);
        Task<DetalleWaybill>? GetDetalleWaybill(Int64 id);
        Task<ServiceResponse<DetalleWaybill>> SaveDetalleWaybill(DetalleWaybill modelo);
        Task<ServiceResponse<DetalleWaybill>> DeleteDetalleWaybill(Int64 id);

        #endregion

        #region JuntaReceptora
        /*
        Task<JuntaReceptora>? GetJuntaReceptoraDeIngreso(Int64 id);
        Task<JuntaReceptora>? GetJuntaReceptora(Int64 id);
        Task<ServiceResponse<JuntaReceptora>> SaveJuntaReceptora(JuntaReceptora modelo);
        Task<ServiceResponse<JuntaReceptora>> UpdateJuntaReceptora(JuntaReceptora modelo);
        Task<ServiceResponse<JuntaReceptora>> UpdateArchivoJuntaReceptora(JuntaReceptora modelo);
        Task<ServiceResponse<JuntaReceptora>> DeleteJuntaReceptora(Int64 id);
        */
        #endregion

        #region DetalleJuntaReceptora
        Task<ServiceResponse<DetalleJuntaReceptora>> UpdateDetalleJuntaReceptoraEstado2(DetalleJuntaReceptora modelo);
        /*
        Task<List<DetalleJuntaReceptora>>? GetAllDetalleJuntaReceptora(Int64 id);
        Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(Int64 id);
        Task<ServiceResponse<DetalleJuntaReceptora>> SaveDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        Task<ServiceResponse<DetalleJuntaReceptora>> UpdateDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        Task<ServiceResponse<DetalleJuntaReceptora>> UpdateDetalleJuntaReceptoraEstado2(DetalleJuntaReceptora modelo);
        Task<ServiceResponse<DetalleJuntaReceptora>> DeleteDetalleJuntaReceptora(Int64 id);
        */
        #endregion

        #region DetalleBoletaAnalisis
        Task<List<DetalleBoletaAnalisis>>? GetAllDetalleBoletaAnalisis(Int64 id);
        Task<DetalleBoletaAnalisis> GetDetalleBoletaAnalisis(Int64 id);
        Task<ServiceResponse<DetalleBoletaAnalisis>> SaveDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo);
        Task<ServiceResponse<DetalleBoletaAnalisis>> UpdateDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo);
        Task<ServiceResponse<DetalleBoletaAnalisis>> DeleteDetalleBoletaAnalisis(Int64 id);

        #endregion

        #region Movimientos Productos
        Task<ServiceResponse<int>> MoverProductosIngresoAMovimientos(Int64 id);

        #endregion

        #region Karex
        Task<ServiceResponse<int>> MoverProductosIngresoAKardex(Int64 id);

        #endregion

        #region Bitacora
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado);
        Task<ServiceResponse<Bitacora>> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
