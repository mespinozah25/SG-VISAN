using PROY.Models;

namespace PROY.Services
{
    public interface IRacionService
    {
        #region RACION_MAESTRO

        Task<List<RacionMaestro>> GetAllRacionMaestro();
        Task<RacionMaestro> GetRacionMaestro(Int64 id);
        Task<ServiceResponse<RacionMaestro>> SaveRacionMaestro(RacionMaestro modelo);
        Task<ServiceResponse<RacionMaestro>> UpdateRacionMaestro(RacionMaestro modelo);
        Task<ServiceResponse<RacionMaestro>> DeleteRacionMaestro(Int64 id);

        #endregion

        #region RACION_DETALLE

        Task<List<RacionDetalle>> GetAllRacionDetalle(Int64 id);
        Task<RacionDetalle> GetRacionDetalle(Int64 id);
        Task<ServiceResponse<RacionDetalle>> SaveRacionDetalle(RacionDetalle modelo);
        Task<ServiceResponse<RacionDetalle>> UpdateRacionDetalle(RacionDetalle modelo);
        Task<ServiceResponse<RacionDetalle>> DeleteRacionDetalle(Int64 id);

        #endregion
    }
}
