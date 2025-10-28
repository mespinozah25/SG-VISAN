using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class TipoBodegaService : ITipoBodegaService
    {
        private readonly ITipoBodegaRepository _tipoBodegaRepository;

        public TipoBodegaService(ITipoBodegaRepository tipoBodegaRepository)
        {
            _tipoBodegaRepository = tipoBodegaRepository;
        }

        public async Task<List<TipoBodega>> GetAll()
        {
            List<TipoBodega> listado = await _tipoBodegaRepository.GetAll();

            return listado;
        }

        public async Task<TipoBodega> GetTipoBodega(Int64 id)
        {
            TipoBodega tipoBodega = await _tipoBodegaRepository.GetTipoBodega(id);

            return tipoBodega;
        }

        public async Task<ServiceResponse<TipoBodega>> SaveTipoBodega(TipoBodega modelo)
        {
            var response = new ServiceResponse<TipoBodega>();

            try
            {
                // Crear objeto Bodega
                TipoBodega tipoBodega = new TipoBodega()
                {
                    IdTipoBodega = await _tipoBodegaRepository.Cuenta(),
                    NombreTipoBodega = modelo.NombreTipoBodega,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _tipoBodegaRepository.SaveTipoBodega(tipoBodega);


                response.Data = tipoBodega;
                response.Message = "Tipo de Bodega creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el tipo de bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<TipoBodega>> UpdateTipoBodega(TipoBodega modelo)
        {
            var response = new ServiceResponse<TipoBodega>();
            TipoBodega tipoBodega = await _tipoBodegaRepository.GetTipoBodega(modelo.IdTipoBodega);

            try
            {
                tipoBodega.NombreTipoBodega = modelo.NombreTipoBodega;

                _tipoBodegaRepository.UpdateTipoBodega(tipoBodega);

                response.Data = tipoBodega;
                response.Message = "Tipo de Bodega actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el tipo de bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<TipoBodega>> DeleteTipoBodega(Int64 id)
        {
            var response = new ServiceResponse<TipoBodega>();
            TipoBodega tipoBodega = await _tipoBodegaRepository.GetTipoBodega(id);

            try
            {
                tipoBodega.Estado = 1;

                _tipoBodegaRepository.UpdateTipoBodega(tipoBodega);

                response.Data = tipoBodega;
                response.Message = "Tipo de Bodega eliminada exitosamente";
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
