using PROY.Models;

namespace PROY.Services
{
    public interface IEgresoAlimentosService
    {
        #region Consultas
        Task<List<EstadoEgresos>> GetAllEstadoEgresos();
        Task<List<EstadoTraslados>> GetAllEstadoTraslados();
        Task<List<dynamic>> GetAllDetallesRacionXConvenio(int idConvenio);

        #endregion

        #region EgresoAlimentos
        Task<List<EgresoAlimentos>> GetAllEgresoAlimentos();
        Task<EgresoAlimentos> GetEgresoAlimentos(Int64 id);
        Task<ServiceResponse<EgresoAlimentos>> SaveEgresoAlimentosEstado1(EgresoAlimentos modelo);
        Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado1(EgresoAlimentos modelo);
        Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado2(EgresoAlimentos modelo);
        Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado3(EgresoAlimentos modelo);
        Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado5(EgresoAlimentos modelo);
        Task<ServiceResponse<EgresoAlimentos>> AsignacionUsuarioLogisticaEgreso(int idEgreso, int idUsuario);
        Task<ServiceResponse<EgresoAlimentos>> AsignacionUsuarioEncargadoBodegaEgreso(int idEgreso, int idUsuario);
        Task<ServiceResponse<EgresoAlimentos>> AsignacionUsuarioRecibeTraslado(int idEgreso, int idUsuario);
        Task<ServiceResponse<EgresoAlimentos>> DeleteEgresoAlimentos(Int64 id);
        Task<ServiceResponse<EgresoAlimentos>> CambioEstadoEgresoAlimentos(Int64 id, int estado);

        #endregion

        #region DetalleEgresoAlimentos
        Task<List<dynamic>>? GetAllDetalleEgresoAlimentos(Int64 id);
        Task<DetalleEgresoAlimentos> GetDetalleEgresoAlimentos(Int64 id);
        Task<ServiceResponse<DetalleEgresoAlimentos>> SaveDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo);
        Task<ServiceResponse<DetalleEgresoAlimentos>> UpdateDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo);
        Task<ServiceResponse<DetalleEgresoAlimentos>> UpdateDetalleEgresoAlimentosEstado5(DetalleEgresoAlimentos modelo);
        Task<ServiceResponse<DetalleEgresoAlimentos>> UpdateDetalleEgresoAlimentosTrasladoEstado1(DetalleEgresoAlimentos modelo);
        Task<ServiceResponse<DetalleEgresoAlimentos>> DeleteDetalleEgresoAlimentos(Int64 id);

        #endregion

        #region SolicitudSalidaAlimentos
        Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos(Int64 id);
        Task<SolicitudSalidaAlimentos> GetSolicitudSalidaAlimentos(Int64 id);
        Task<ServiceResponse<SolicitudSalidaAlimentos>> SaveSolicitudSalidaAlimentos(SolicitudSalidaAlimentos modelo);
        Task<ServiceResponse<SolicitudSalidaAlimentos>> DeleteSolicitudSalidaAlimentos(Int64 id);

        #endregion

        #region DetalleSalidaAlimentos
        Task<List<DetalleSalidaAlimentos>> GetAllDetalleSalidaAlimentos(Int64 id);
        Task<DetalleSalidaAlimentos> GetDetalleSalidaAlimentos(Int64 id);
        Task<ServiceResponse<DetalleSalidaAlimentos>> SaveDetalleSalidaAlimentos(DetalleSalidaAlimentos modelo);
        Task<ServiceResponse<DetalleSalidaAlimentos>> DeleteDetalleSalidaAlimentos(Int64 id);

        #endregion

        #region TRASLADO DE ALIMENTOS
        Task<List<TrasladoAlimentos>> GetAllTrasladoAlimentos();
        Task<TrasladoAlimentos> GetTrasladoAlimentos(Int64 id);
        Task<TrasladoAlimentos> GetTrasladoAlimentosDeEgreso(Int64 id);
        Task<ServiceResponse<TrasladoAlimentos>> SaveTrasladoAlimentos(TrasladoAlimentos modelo);
        Task<ServiceResponse<TrasladoAlimentos>> UpdateTrasladoAlimentos(TrasladoAlimentos modelo);
        Task<ServiceResponse<TrasladoAlimentos>> CambioEstadoTrasladoAlimentos(Int64 id, int estado);

        #endregion

        #region Movimientos Productos
        Task<ServiceResponse<int>> MoverProductosEgresoAMovimientos(Int64 id);
        Task<ServiceResponse<int>> MoverProductosTrasladoAMovimientosIngreso(Int64 id);
        Task<ServiceResponse<int>> MoverProductosTrasladoAMovimientosEgreso(Int64 id);

        #endregion

        #region Karex
        Task<ServiceResponse<int>> MoverProductosEgresoAKardex(Int64 id);
        Task<ServiceResponse<int>> MoverProductosTrasladoAKardex(Int64 id);

        #endregion

        #region Bitacora
        Task<List<Bitacora>> GetAllBitacora(Int64 id);
        Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado);
        Task<ServiceResponse<Bitacora>> SaveBitacora(Bitacora modelo);

        #endregion
    }
}
