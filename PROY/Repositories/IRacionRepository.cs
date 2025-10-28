using PROY.Models;

namespace PROY.Repositories
{
    public interface IRacionRepository
    {
        #region RACION_MAESTRO

        Task<List<RacionMaestro>> GetAllRacionMaestro();
        Task<RacionMaestro> GetRacionMaestro(Int64 id);
        Task<Int64> CuentaRacionMaestro();
        Task<RacionMaestro> SaveRacionMaestro(RacionMaestro modelo);
        Task<RacionMaestro> UpdateRacionMaestro(RacionMaestro modelo);

        #endregion

        #region RACION_DETALLE

        Task<List<RacionDetalle>> GetAllRacionDetalle(Int64 id);
        Task<RacionDetalle> GetRacionDetalle(Int64 id);
        Task<Int64> CuentaRacionDetalle();
        Task<RacionDetalle> SaveRacionDetalle(RacionDetalle modelo);
        Task<RacionDetalle> UpdateRacionDetalle(RacionDetalle modelo);

        #endregion
    }
}
