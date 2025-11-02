using PROY.Models;

namespace PROY.Repositories
{
    public interface ITipoIntervencionRepository
    {
        Task<List<TipoIntervencion>> ObtenerPorDepartamento(int IdDepto);
    }
}
