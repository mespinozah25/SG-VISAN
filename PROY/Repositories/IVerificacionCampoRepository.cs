using PROY.Models;

namespace PROY.Repositories
{
    public interface IVerificacionCampoRepository
    {
        Task<List<Solicitud>> ObtenerVerificacionesCampo(int idDepartamento);
        
    }
}
