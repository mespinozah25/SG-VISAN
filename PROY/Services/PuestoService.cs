using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class PuestoService : IPuestoService
    {
        private readonly IPuestoRepository _puestoRepository;

        public PuestoService(IPuestoRepository puestoRepository)
        {
            _puestoRepository = puestoRepository;
        }

        public async Task<List<Puesto>> GetAll()
        {
            List<Puesto> listado = await _puestoRepository.GetAll();

            return listado;
        }

        public async Task<Puesto> GetPuesto(Int64 id)
        {
            Puesto puesto = await _puestoRepository.GetPuesto(id);

            return puesto;
        }

        public async Task<ServiceResponse<Puesto>> SavePuesto(Puesto modelo)
        {
            var response = new ServiceResponse<Puesto>();

            try
            {
                // Crear objeto Bodega
                Puesto puesto = new Puesto()
                {
                    IdPuesto = await _puestoRepository.Cuenta(),
                    NombrePuesto = modelo.NombrePuesto,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _puestoRepository.SavePuesto(puesto);


                response.Data = puesto;
                response.Message = "Puesto creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el puesto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Puesto>> UpdatePuesto(Puesto modelo)
        {
            var response = new ServiceResponse<Puesto>();
            Puesto puesto = await _puestoRepository.GetPuesto(modelo.IdPuesto);

            try
            {
                puesto.NombrePuesto = modelo.NombrePuesto;

                _puestoRepository.UpdatePuesto(puesto);

                response.Data = puesto;
                response.Message = "Puesto actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el puesto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Puesto>> DeletePuesto(Int64 id)
        {
            var response = new ServiceResponse<Puesto>();
            Puesto puesto = await _puestoRepository.GetPuesto(id);

            try
            {
                puesto.Estado = 1;

                _puestoRepository.UpdatePuesto(puesto);

                response.Data = puesto;
                response.Message = "Puesto eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el tipo de bodega: {ex.Message}";
            }

            return response;
        }
    }
}
