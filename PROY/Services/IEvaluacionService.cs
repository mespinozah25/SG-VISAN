using PROY.Models;

namespace PROY.Services
{
    public interface IEvaluacionService
    {
        Task<List<Solicitud>> GetAllEvaluaciones();
        Task<List<Solicitud>> GetAllEvaluacionesAPA();
        Task<List<Solicitud>> GetAllEvaluacionesRechazadas();
        Task<List<Solicitud>> GetAllEvaluacionesRechazadasAPA();
        //Task<List<DetalleSolicitud>> GetAllDetalleSolicitud(Int64 id);
        Task<List<DetalleEvaluacion>> GetAllDetalleEvaluacion(Int64 idEvaluacion);
        Task<List<DetalleEvaluacionAPA>> GetAllDetalleEvaluacionAPA(Int64 idEvaluacion);
        Task<Boolean> VerificarEvaluacion(Int64 idEvaluacion, DateTime fecha, string proceso);
        Task<Boolean> VerificarEvaluacionAPA(Int64 idEvaluacion, DateTime fecha, string proceso);
        Task<DetalleEvaluacion> GetDetalleEvaluacionID(Int64 idDetalleEvaluacion);
        Task<DetalleEvaluacionAPA> GetDetalleEvaluacionIDAPA(Int64 idDetalleEvaluacion);
        //Task<ServiceResponse<Solicitud>> SaveSolicitud(Solicitud modelo);
        Task<ServiceResponse<DetalleEvaluacion>> SaveDetalleEvaluacion(DetalleEvaluacion modelo, string proceso);
        Task<ServiceResponse<DetalleEvaluacionAPA>> SaveDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo, string proceso);
        //Task<ServiceResponse<Solicitud>> UpdateSolicitud(Solicitud modelo);
        Task<ServiceResponse<DetalleEvaluacion>> UpdateDetalleEvaluacion(DetalleEvaluacion modelo);
        Task<ServiceResponse<DetalleEvaluacionAPA>> UpdateDetalleEvaluacionAPA(DetalleEvaluacionAPA modelo);
        Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo);
        Task<ServiceResponse<Solicitud>> UpdateRechazada(Solicitud modelo);
        //Task<ServiceResponse<Solicitud>> DeleteSolicitud(Int64 id);
        Task<ServiceResponse<DetalleEvaluacion>> DeleteDetalleEvaluacion(Int64 idDetalleEvaluacion);
        Task<ServiceResponse<DetalleEvaluacionAPA>> DeleteDetalleEvaluacionAPA(Int64 idDetalleEvaluacion);
    }
}
