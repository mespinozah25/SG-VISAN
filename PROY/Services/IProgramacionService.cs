using PROY.Models;
using static PROY.Repositories.ProgramacionRepository;

namespace PROY.Services
{
    public interface IProgramacionService
    {
        Task<List<Solicitud>> GetAllProgramaciones();
        Task<List<Solicitud>> GetAllProgramacionesAPA();
        //Task<List<DetalleSolicitud>> GetAllDetalleSolicitud(Int64 id);
        Task<List<DetalleSolicitud>> GetAllDetalleProgramacion(Int64 idProgramacion);
        Task<List<DetalleSolicitudAPA>> GetAllDetalleProgramacionAPA(Int64 idProgramacion);
        Task<DetalleSolicitud> GetDetalleProgramacionID(Int64 idDetalleProgramacion);
        Task<DetalleSolicitudAPA> GetDetalleProgramacionIDAPA(Int64 idDetalleProgramacion);
        Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo);
        Task<List<Agrupado>> GetDetalleSolicitudAgrupado(Int64 idProgramacion);
        Task<List<Catalogo>> GetComparar(Int64 idProgramacion);
    }
}
