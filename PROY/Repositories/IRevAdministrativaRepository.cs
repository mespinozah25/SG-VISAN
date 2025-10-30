using PROY.Models;
using System.Threading.Tasks;

namespace PROY.Repositories
{
    public interface IRevAdministrativaRepository
    {
        Task<List<Solicitud>> ObtenerRevAdministrativas(int idDepartamento);
    }
}
