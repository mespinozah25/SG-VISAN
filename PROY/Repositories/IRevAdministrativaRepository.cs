using PROY.Models;

namespace PROY.Repositories
{
    public interface IRevAdministrativaRepository
    {
        Task<List<Solicitud>> ObtenerRevAdministrativas(int idDepartamento);
        
    }
}
