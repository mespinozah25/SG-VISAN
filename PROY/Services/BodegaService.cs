using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class BodegaService : IBodegaService
    {
        private readonly IBodegaRepository _bodegaRepository;

        public BodegaService(IBodegaRepository bodegaRepository)
        {
            _bodegaRepository = bodegaRepository;
        }

        public async Task<List<Bodega>> GetAll()
        {
            List<Bodega> listado = await _bodegaRepository.GetAll();

            return listado;
        }

        public async Task<Bodega> GetBodega(Int64 id)
        {
            Bodega bodega_encontrada = await _bodegaRepository.GetBodega(id);

            return bodega_encontrada;
        }

        public async Task<ServiceResponse<Bodega>> SaveBodega(Bodega modelo)
        {
            var response = new ServiceResponse<Bodega>();

            try
            {
                // Crear objeto Bodega
                Bodega bodega = new Bodega()
                {
                    IdBodega = await _bodegaRepository.Cuenta(),
                    NombreBodega = modelo.NombreBodega,
                    DireccionBodega = modelo.DireccionBodega,
                    IdTipoBodega = modelo.IdTipoBodega,
                    IdDepartamento = modelo.IdDepartamento,
                    IdMunicipio = modelo.IdMunicipio,
                    IdUsuario = modelo.IdUsuario,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _bodegaRepository.SaveBodega(bodega);


                response.Data = bodega;
                response.Message = "Bodega creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Bodega>> UpdateBodega(Bodega modelo)
        {
            var response = new ServiceResponse<Bodega>();
            Bodega bodega_encontrada = await _bodegaRepository.GetBodega(modelo.IdBodega);

            try
            {
                bodega_encontrada.NombreBodega = modelo.NombreBodega;
                bodega_encontrada.DireccionBodega = modelo.DireccionBodega;
                bodega_encontrada.IdTipoBodega = modelo.IdTipoBodega;
                bodega_encontrada.IdDepartamento = modelo.IdDepartamento;
                bodega_encontrada.IdMunicipio = modelo.IdMunicipio;
                bodega_encontrada.IdUsuario = modelo.IdUsuario;

                _bodegaRepository.UpdateBodega(bodega_encontrada);

                response.Data = bodega_encontrada;
                response.Message = "Bodega actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Bodega>> DeleteBodega(Int64 id)
        {
            var response = new ServiceResponse<Bodega>();
            Bodega bodega_encontrada = await _bodegaRepository.GetBodega(id);

            try
            {
                bodega_encontrada.Estado = 1;

                _bodegaRepository.UpdateBodega(bodega_encontrada);

                response.Data = bodega_encontrada;
                response.Message = "Bodega eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<List<TipoBodega>> GetAllTipoBodegas()
        {
            List<TipoBodega> listado = await _bodegaRepository.GetAllTipoBodegas();

            return listado;
        }
    }
}
