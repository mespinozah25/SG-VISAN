using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ProductoAlimenticioService : IProductoAlimenticioService
    {
        private readonly IProductoAlimenticioRepository _productoalimenticioRepository;

        public ProductoAlimenticioService(IProductoAlimenticioRepository productoalimenticioRepository)
        {
            _productoalimenticioRepository = productoalimenticioRepository;
        }

        public async Task<List<ProductoAlimenticio>> GetAll()
        {
            List<ProductoAlimenticio> listado = await _productoalimenticioRepository.GetAll();

            return listado;
        }

        public async Task<ProductoAlimenticio> GetProductoAlimenticio(Int64 id)
        {
            ProductoAlimenticio productoalimenticio_encontrada = await _productoalimenticioRepository.GetProductoAlimenticio(id);

            return productoalimenticio_encontrada;
        }

        public async Task<ServiceResponse<ProductoAlimenticio>> SaveProductoAlimenticio(ProductoAlimenticio modelo)
        {
            var response = new ServiceResponse<ProductoAlimenticio>();

            try
            {
                // Crear objeto
                ProductoAlimenticio productoalimenticio = new ProductoAlimenticio()
                {
                    IdProductoAlimenticio = await _productoalimenticioRepository.Cuenta(),
                    NombrePA = modelo.NombrePA,
                    Estado = 0
                };

                // Guardar el programa en la base de datos
                _productoalimenticioRepository.SaveProductoAlimenticio(productoalimenticio);


                response.Data = productoalimenticio;
                response.Message = "Producto Alimenticio creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el producto alimenticio: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductoAlimenticio>> UpdateProductoAlimenticio(ProductoAlimenticio modelo)
        {
            var response = new ServiceResponse<ProductoAlimenticio>();
            ProductoAlimenticio productoalimenticio_encontrada = await _productoalimenticioRepository.GetProductoAlimenticio(modelo.IdProductoAlimenticio);

            try
            {
                productoalimenticio_encontrada.NombrePA = modelo.NombrePA;
                
                _productoalimenticioRepository.UpdateProductoAlimenticio(productoalimenticio_encontrada);

                response.Data = productoalimenticio_encontrada;
                response.Message = "Producto Alimenticio actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el producto alimenticio: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductoAlimenticio>> DeleteProductoAlimenticio(Int64 id)
        {
            var response = new ServiceResponse<ProductoAlimenticio>();
            ProductoAlimenticio productoalimenticio_encontrada = await _productoalimenticioRepository.GetProductoAlimenticio(id);

            try
            {
                productoalimenticio_encontrada.Estado = 1;

                _productoalimenticioRepository.UpdateProductoAlimenticio(productoalimenticio_encontrada);

                response.Data = productoalimenticio_encontrada;
                response.Message = "Producto Alimenticio eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el producto alimenticio: {ex.Message}";
            }

            return response;
        }
    }
}
