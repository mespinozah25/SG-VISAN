using PROY.Models;

namespace PROY.Services
{
    public interface IVehiculoService
    {
        Task<List<Vehiculo>> GetAll();
        Task<Vehiculo> GetVehiculo(Int64 id);
        Task<ServiceResponse<Vehiculo>> SaveVehiculo(Vehiculo modelo);
        Task<ServiceResponse<Vehiculo>> UpdateVehiculo(Vehiculo modelo);
        Task<ServiceResponse<Vehiculo>> DeleteVehiculo(Int64 id);
    }
}
