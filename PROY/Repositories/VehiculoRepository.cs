using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly AppDBContext _dbContext;

        public VehiculoRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> Cuenta()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Vehiculos
                .Select(t => t.IdVehiculo)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<List<Vehiculo>> GetAll()
        {
            List<Vehiculo> listado = await _dbContext.Vehiculos.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Vehiculo> GetVehiculo(long id)
        {
            Vehiculo query = await _dbContext.Vehiculos.FirstAsync(t => t.IdVehiculo == id);

            return query;
        }

        public async Task<Vehiculo>? GetVehiculoPlacaCabezal(string placa)
        {
            Vehiculo? query = await _dbContext.Vehiculos.Where(t => t.PlacaCabezal.ToUpper().Trim() == placa).FirstOrDefaultAsync();

            return query;
        }

        public async Task<Vehiculo> SaveVehiculo(Vehiculo modelo)
        {
            try
            {
                _dbContext.Vehiculos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<Vehiculo> UpdateVehiculo(Vehiculo modelo)
        {
            _dbContext.Vehiculos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
