using PROY.Models;

namespace PROY.Repositories
{
    public interface INinoRepository
    {
        Task InsertarNinoAsync(Nino nino);
    }
}
