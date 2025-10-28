using PROY.Models;

namespace PROY.Services
{
    public interface IJuntaDeRecepcionService
    {
        #region Consultas
        Task<List<TipoNombramiento>> GetAllTipoNombramiento();

        #endregion

        #region JuntaReceptora
        Task<List<JuntaReceptora>>? GetAllJuntaReceptora();
        Task<JuntaReceptora>? GetJuntaReceptora(Int64 id);
        Task<ServiceResponse<JuntaReceptora>> SaveJuntaReceptora(JuntaReceptora modelo);
        Task<ServiceResponse<JuntaReceptora>> UpdateJuntaReceptora(JuntaReceptora modelo);
        Task<ServiceResponse<JuntaReceptora>> UpdateArchivoJuntaReceptora(JuntaReceptora modelo);
        Task<ServiceResponse<JuntaReceptora>> DeleteJuntaReceptora(Int64 id);

        #endregion

        #region DetalleJuntaReceptora
        Task<List<DetalleJuntaReceptora>>? GetAllDetalleJuntaReceptora(Int64 id);
        Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(Int64 id);
        Task<ServiceResponse<DetalleJuntaReceptora>> SaveDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        Task<ServiceResponse<DetalleJuntaReceptora>> UpdateDetalleJuntaReceptora(DetalleJuntaReceptora modelo);
        Task<ServiceResponse<DetalleJuntaReceptora>> DeleteDetalleJuntaReceptora(Int64 id);

        #endregion
    }
}
