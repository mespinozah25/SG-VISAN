using PROY.Models;

namespace PROY.Repositories
{
    public interface IVehiculoRepository
    {
        Task<List<Vehiculo>> GetAll();
        Task<Vehiculo> GetVehiculo(Int64 id);
        Task<Vehiculo>? GetVehiculoPlacaCabezal(string placa);
        Task<Int64> Cuenta();
        Task<Vehiculo> SaveVehiculo(Vehiculo modelo);
        Task<Vehiculo> UpdateVehiculo(Vehiculo modelo);
    }
}
