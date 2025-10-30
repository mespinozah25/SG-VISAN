using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.ViewModels;
using static PROY.Repositories.SolicitudRepository;

namespace PROY.Services
{
    public interface ISolicitudService
    {
        Task<List<Solicitud>> GetAllSolicitudes();
        Task<List<Solicitud>> GetAllSolicitudesAPA();
        Task<List<Solicitud>> GetSolicitudesProgramacion();
        Task ActualizarEstadoEgreso(Int64 idSolicitud, int nuevoEstado);
        Task<List<DetalleSolicitud>> GetAllDetalleSolicitud(Int64 id);
        Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudAPA(Int64 id);
        List<SelectListItem> GetComunidades(Int64 id);
        List<SelectListItem> GetComunidadesAPA(Int64 id);
        Task<Solicitud> GetSolicitud(Int64 id);
        Task<DetalleSolicitud> GetDetalleSolicitud(Int64 id);
        Task<DetalleSolicitud> GetDetalleSolicitudByCUI(Int64 id, string paramCUI);
        Task<DetalleSolicitudAPA> GetDetalleSolicitudByCUIAPA(Int64 id, string paramCUI);
        Task<DetalleSolicitudAPA> GetDetalleSolicitudAPA(Int64 id);
        Task<ServiceResponse<Solicitud>> SaveSolicitud(Solicitud modelo);
        Task<ServiceResponse<DetalleSolicitud>> SaveDetalleSolicitud(DetalleSolicitud modelo, string proceso);
        Task<ServiceResponse<DetalleSolicitudAPA>> SaveDetalleSolicitudAPA(DetalleSolicitudAPA modelo);
        Task<ServiceResponse<DetalleSolicitud>> SaveListaDetalleSolicitud(List<ExcelRow> datos);
        Task<ServiceResponse<DetalleSolicitud>> SaveListaDetalleSolicitudxIntervencion(List<ExcelRowDinamic> datos,int idIntervencion);
        Task<ServiceResponse<DetalleSolicitudAPA>> SaveListaDetalleSolicitudAPA(List<ExcelRowAPA> datos);
        Task<ServiceResponse<DetalleSolicitud>> UpdateListaDetalleSolicitud(List<ExcelRowDinamic> datos);
        Task<ServiceResponse<DetalleSolicitudAPA>> UpdateListaDetalleSolicitudAPA(List<Models.ExcelRowAPA> datos);
        Task<ServiceResponse<Solicitud>> UpdateSolicitud(Solicitud modelo);
        Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo);
        Task<ServiceResponse<Solicitud>> UpdateEstadoARevAdministrativa(Solicitud modelo);
        Task<ServiceResponse<DetalleSolicitud>> UpdateDetalleSolicitud(DetalleSolicitud modelo, string proceso);
        Task<ServiceResponse<DetalleSolicitudAPA>> UpdateDetalleSolicitudAPA(DetalleSolicitudAPA modelo);
        Task<ServiceResponse<Solicitud>> DeleteSolicitud(Int64 id);
        Task<ServiceResponse<DetalleSolicitud>> DeleteDetalleSolicitud(Int64 id);
        Task<ServiceResponse<DetalleSolicitudAPA>> DeleteDetalleSolicitudAPA(Int64 id);
        Task<Boolean> GetTotalRacionesAsignadas(Int64 id);
        Task<ServiceResponse<AsignaTecnico>> SaveAsignaTecnico(AsignaTecnico modelo);
        //Task<List<AsignaTecnico>> GetDetalleTecnicosSolicitud(long id);
        Task<ServiceResponse<AsignaTecnico>> DeleteAsignaTecnico(long idAsignaTecnico);
        Task<List<dynamic>> GetDetalleTecnicosSolicitud(long id);

        Task<List<Tecnico>> GetAllTecnicos();

        Task<ServiceResponse<AsignaRacion>> SaveAsignaRacion(AsignaRacion modelo);
        Task<ServiceResponse<AsignaRacion>> DeleteAsignaRacion(long idRacion);
        Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(Int64 id);
        Task<int?> GetIntervencionBySolicitudId(Int64? idSolicitud);
    }
    
}
