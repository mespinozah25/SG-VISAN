using PROY.Models;

namespace PROY.Services
{
    public interface IAsignaTecnicoService
    {
        Task<List<AsignaTecnicoV2>> ObtenerAsignacionesTecnico(int IdSolicitud);
        Task<string> GuardarAsignacionTecnico(AsignaTecnicoV2 asignaTecnico);
        Task<string> EliminarAsignacionTecnico(int IdAsignaTecnico);
    }
}
