using PROY.Models;

namespace PROY.Repositories
{
    public interface ITransportistaRepository
    {
        Task<List<Transportista>> GetAll();
        Task<Transportista> GetTransportista(Int64 id);
        Task<Transportista>? GetTransportistaNombre(string nombre);
        Task<Int64> Cuenta();
        Task<Transportista> SaveTransportista(Transportista modelo);
        Task<Transportista> UpdateTransportista(Transportista modelo);
    }
}
