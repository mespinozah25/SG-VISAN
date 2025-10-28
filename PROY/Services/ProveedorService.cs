using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _proveedorRepository;

        public ProveedorService(IProveedorRepository proveedorRepository)
        {
            _proveedorRepository = proveedorRepository;
        }

        public async Task<List<Proveedor>> GetAll()
        {
            List<Proveedor> listado = await _proveedorRepository.GetAll();

            return listado;
        }

        public async Task<Proveedor> GetProveedor(Int64 id)
        {
            Proveedor proveedor = await _proveedorRepository.GetProveedor(id);

            return proveedor;
        }

        public async Task<ServiceResponse<Proveedor>> SaveProveedor(Proveedor modelo)
        {
            var response = new ServiceResponse<Proveedor>();

            try
            {
                // Crear objeto Proveedor
                Proveedor proveedor = new Proveedor()
                {
                    IdProveedor = await _proveedorRepository.Cuenta(),
                    NombreProveedor = modelo.NombreProveedor,
                    Estado = 0
                };

                // Guardar el programa en la base de datos
                _proveedorRepository.SaveProveedor(proveedor);


                response.Data = proveedor;
                response.Message = "Proveedor creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el proveedor: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Proveedor>> UpdateProveedor(Proveedor modelo)
        {
            var response = new ServiceResponse<Proveedor>();
            Proveedor proveedor = await _proveedorRepository.GetProveedor(modelo.IdProveedor);

            try
            {
                proveedor.NombreProveedor = modelo.NombreProveedor;

                _proveedorRepository.UpdateProveedor(proveedor);

                response.Data = proveedor;
                response.Message = "Proveedor actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el proveedor: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Proveedor>> DeleteProveedor(Int64 id)
        {
            var response = new ServiceResponse<Proveedor>();
            Proveedor proveedor = await _proveedorRepository.GetProveedor(id);

            try
            {
                proveedor.Estado = 1;

                _proveedorRepository.UpdateProveedor(proveedor);

                response.Data = proveedor;
                response.Message = "Proveedor eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el proveedor: {ex.Message}";
            }

            return response;
        }
    }
}
