using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _unidadmedidaRepository;

        public UnidadMedidaService(IUnidadMedidaRepository unidadmedidaRepository)
        {
            _unidadmedidaRepository = unidadmedidaRepository;
        }

        public async Task<List<UnidadMedida>> GetAll()
        {
            List<UnidadMedida> listado = await _unidadmedidaRepository.GetAll();

            return listado;
        }

        public async Task<UnidadMedida> GetUnidadMedida(Int64 id)
        {
            UnidadMedida unidadmedida_encontrada = await _unidadmedidaRepository.GetUnidadMedida(id);

            return unidadmedida_encontrada;
        }

        public async Task<ServiceResponse<UnidadMedida>> SaveUnidadMedida(UnidadMedida modelo)
        {
            var response = new ServiceResponse<UnidadMedida>();

            try
            {
                // Crear objeto
                UnidadMedida unidadmedida = new UnidadMedida()
                {
                    IdUnidadMedida = await _unidadmedidaRepository.Cuenta(),
                    NombreUnidadMedida = modelo.NombreUnidadMedida,
                    UMPlural = modelo.UMPlural,
                    Abreviatura = modelo.Abreviatura,
                    Estado = 0
                };

                // Guardar el programa en la base de datos
                _unidadmedidaRepository.SaveUnidadMedida(unidadmedida);


                response.Data = unidadmedida;
                response.Message = "Unidad de Medida creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la Unidad de Medida: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<UnidadMedida>> UpdateUnidadMedida(UnidadMedida modelo)
        {
            var response = new ServiceResponse<UnidadMedida>();
            UnidadMedida unidadmedida_encontrada = await _unidadmedidaRepository.GetUnidadMedida(modelo.IdUnidadMedida);

            try
            {
                unidadmedida_encontrada.NombreUnidadMedida = modelo.NombreUnidadMedida;
                unidadmedida_encontrada.UMPlural = modelo.UMPlural;
                unidadmedida_encontrada.Abreviatura = modelo.Abreviatura;

                _unidadmedidaRepository.UpdateUnidadMedida(unidadmedida_encontrada);

                response.Data = unidadmedida_encontrada;
                response.Message = "Unidad de Medida actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la Unidad de Medida: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<UnidadMedida>> DeleteUnidadMedida(Int64 id)
        {
            var response = new ServiceResponse<UnidadMedida>();
            UnidadMedida unidadmedida_encontrada = await _unidadmedidaRepository.GetUnidadMedida(id);

            try
            {
                unidadmedida_encontrada.Estado = 1;

                _unidadmedidaRepository.UpdateUnidadMedida(unidadmedida_encontrada);

                response.Data = unidadmedida_encontrada;
                response.Message = "Unidad de Medida eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la Unidad de Medida: {ex.Message}";
            }

            return response;
        }
    }
}
