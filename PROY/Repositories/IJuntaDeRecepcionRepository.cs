using PROY.Models;

namespace PROY.Repositories
{
    public interface IJuntaDeRecepcionRepository
    {
        #region Consultas
        Task<List<TipoNombramiento>> GetAllTipoNombramiento();

        #endregion
        #region JUNTA_RECEPTORA
        Task<List<JuntaReceptora>>? GetAllJuntaReceptora();
        Task<JuntaReceptora>? GetJuntaReceptora(Int64 id);
        Task<Int64> CuentaJuntaReceptora();
        Task<JuntaReceptora> SaveJuntaReceptora(JuntaReceptora modelo);
        Task<JuntaReceptora> UpdateJuntaReceptora(JuntaReceptora modelo);

        #endregion

        #region DETALLE_JUNTA_RECEPTORA
        Task<List<DetalleJuntaReceptora>>? GetAllDetalleJuntaReceptora(Int64 id);
        Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(Int64 id);
        Task<Int64> CuentaDetalleJuntaReceptora();
        Task<DetalleJuntaReceptora> SaveDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        Task<DetalleJuntaReceptora> UpdateDetalleJuntaReceptora(DetalleJuntaReceptora modelo);

        #endregion
    }
}
