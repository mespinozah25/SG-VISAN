using PROY.Models;

namespace PROY.Repositories
{
    public interface IAsignaTecnicoRepository
    {
        Task<List<AsignaTecnicoV2>> ObtenerAsignacionesTecnico(int IdSolicitud);
        Task<AsignaTecnicoV2?> ObtenerAsignacionTecnicoSegunId(int IdAsignaTecnico);
        Task<AsignaTecnicoV2> GuardarAsignacionTecnico(AsignaTecnicoV2 asignaTecnico);
        Task<AsignaTecnicoV2> ActualizarAsignacionTecnico(AsignaTecnicoV2 asignaTecnico);
    }
}
