using PROY.Models;

namespace PROY.Services
{
    public interface ITipoIntervencionService
    {
        Task<List<TipoIntervencion>> ObtenerPorDepartamento(int IdDepto);
    }
}
