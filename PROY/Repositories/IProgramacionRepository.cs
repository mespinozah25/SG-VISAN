using PROY.Models;
using static PROY.Repositories.ProgramacionRepository;

namespace PROY.Repositories
{
    public interface IProgramacionRepository
    {
        Task<List<Solicitud>> GetAllProgramaciones();
        Task<List<Solicitud>> GetAllProgramacionesAPA();        
        Task<List<DetalleSolicitud>> GetAllDetalleProgramacion(Int64 idProgramacion);
        Task<List<DetalleSolicitudAPA>> GetAllDetalleProgramacionAPA(Int64 idProgramacion);
        Task<DetalleSolicitud> GetDetalleProgramacionID(Int64 idDetalleProgramacion);
        Task<DetalleSolicitudAPA> GetDetalleProgramacionIDAPA(Int64 idDetalleProgramacion);
        Task<List<Agrupado>> GetDetalleSolicitudAgrupado(long idProgramacion);
        //Task<Int64> CuentaDetalle();
        //Task<DetalleEvaluacion> SaveDetalleEvaluacion(DetalleEvaluacion modelo);
        //Task<Solicitud> UpdateSolicitud(Solicitud modelo);
        //Task<DetalleEvaluacion> UpdateDetalleEvaluacion(DetalleEvaluacion modelo);
    }
}
