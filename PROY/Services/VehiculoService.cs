using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _subBodegaRepository;

        public VehiculoService(IVehiculoRepository vehiculoRepository)
        {
            _subBodegaRepository = vehiculoRepository;
        }

        public async Task<List<Vehiculo>> GetAll()
        {
            List<Vehiculo> listado = await _subBodegaRepository.GetAll();

            return listado;
        }

        public async Task<Vehiculo> GetVehiculo(Int64 id)
        {
            Vehiculo query = await _subBodegaRepository.GetVehiculo(id);

            return query;
        }

        public async Task<ServiceResponse<Vehiculo>> SaveVehiculo(Vehiculo modelo)
        {
            var response = new ServiceResponse<Vehiculo>();

            try
            {
                // Crear objeto Vehiculo
                Vehiculo dict = new Vehiculo()
                {
                    IdVehiculo = await _subBodegaRepository.Cuenta(),
                    IdTransportista = modelo.IdTransportista,
                    PlacaCabezal = modelo.PlacaCabezal,
                    PlacaPlataforma = modelo.PlacaPlataforma,
                    Estado = 0
                };

                // Guardar Vehiculo en la base de datos
                _subBodegaRepository.SaveVehiculo(dict);


                response.Data = dict;
                response.Message = "Vehiculo creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Vehiculo: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Vehiculo>> UpdateVehiculo(Vehiculo modelo)
        {
            var response = new ServiceResponse<Vehiculo>();
            Vehiculo registro = await _subBodegaRepository.GetVehiculo(modelo.IdVehiculo);

            try
            {
                #region Parámetros que se actualizan 

                registro.IdTransportista = modelo.IdTransportista;
                registro.PlacaCabezal = modelo.PlacaCabezal;
                registro.PlacaPlataforma = modelo.PlacaPlataforma;

                #endregion

                _subBodegaRepository.UpdateVehiculo(registro);

                response.Data = registro;
                response.Message = "Vehiculo actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la Vehiculo: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Vehiculo>> DeleteVehiculo(Int64 id)
        {
            var response = new ServiceResponse<Vehiculo>();
            Vehiculo registro = await _subBodegaRepository.GetVehiculo(id);

            try
            {
                registro.Estado = 1;

                _subBodegaRepository.UpdateVehiculo(registro);

                response.Data = registro;
                response.Message = "Vehiculo eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la Vehiculo: {ex.Message}";
            }

            return response;
        }
    }
}
