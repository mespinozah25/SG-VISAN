using PROY.Models;

namespace PROY.Repositories
{
    public interface IPilotoRepository
    {
        Task<List<Piloto>> GetAll();
        Task<Piloto> GetPiloto(Int64 id);
        Task<Piloto>? GetPilotoNombreYLicencia(string nombre, string licencia);
        Task<Int64> Cuenta();
        Task<Piloto> SavePiloto(Piloto modelo);
        Task<Piloto> UpdatePiloto(Piloto modelo);
    }
}
