using PROY.Models;
using System.Threading.Tasks;

namespace PROY.Repositories
{
    public interface IEvaluacionRepository
    {
        Task<List<Solicitud>> GetAllEvaluaciones();
        Task<List<Solicitud>> GetAllEvaluacionesAPA();
        Task<List<Solicitud>> GetAllEvaluacionesRechazadas();
        Task<List<Solicitud>> GetAllEvaluacionesRechazadasAPA();
        //<List<DetalleEvaluacion>> GetAllDetalleEvaluacion(Int64 idEvaluacion);
        //Task<Solicitud> GetSolicitud(Int64 id);
        Task<List<DetalleEvaluacion>> GetAllDetalleEvaluacion(Int64 idEvaluacion);
        Task<List<DetalleEvaluacionAPA>> GetAllDetalleEvaluacionAPA(Int64 idEvaluacion);
        Task<Boolean> VerificarEvaluacion(Int64 idEvaluacion, DateTime fecha, string proceso);
        Task<Boolean> VerificarEvaluacionAPA(long idEvaluacion, DateTime fecha, string proceso);
        Task<DetalleEvaluacion> GetDetalleEvaluacionID(Int64 idDetalleEvaluacion);
        Task<DetalleEvaluacionAPA> GetDetalleEvaluacionIDAPA(Int64 idDetalleEvaluacion);
        //Task<Solicitud> SaveEvaluacion(Solicitud modelo);
        Task<Int64> CuentaDetalle();
        Task<Int64> CuentaDetalleAPA();
        Task<DetalleEvaluacion> SaveDetalleEvaluacion(DetalleEvaluacion modelo);
        Task<DetalleEvaluacionAPA> SaveDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo);
        //Task<Solicitud> UpdateSolicitud(Solicitud modelo);
        Task<DetalleEvaluacion> UpdateDetalleEvaluacion(DetalleEvaluacion modelo);
        Task<DetalleEvaluacionAPA> UpdateDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo);
    }
}
