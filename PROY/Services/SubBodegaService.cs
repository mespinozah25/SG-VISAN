using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class SubBodegaService : ISubBodegaService
    {
        private readonly ISubBodegaRepository _subBodegaRepository;

        public SubBodegaService(ISubBodegaRepository subBodegaRepository)
        {
            _subBodegaRepository = subBodegaRepository;
        }

        public async Task<List<SubBodega>> GetAll()
        {
            List<SubBodega> listado = await _subBodegaRepository.GetAll();

            return listado;
        }

        public async Task<List<SubBodega>> ObtenerSegunBodega(int idBodega)
        {
            List<SubBodega> listado = await _subBodegaRepository.ObtenerSegunBodega(idBodega);

            return listado;
        }

        public async Task<SubBodega> GetSubBodega(Int64 id)
        {
            SubBodega query = await _subBodegaRepository.GetSubBodega(id);

            return query;
        }

        public async Task<ServiceResponse<SubBodega>> SaveSubBodega(SubBodega modelo)
        {
            var response = new ServiceResponse<SubBodega>();

            try
            {
                // Crear objeto SubBodega
                SubBodega dict = new SubBodega()
                {
                    IdSubBodega = await _subBodegaRepository.Cuenta(),
                    NombreSubBodega = modelo.NombreSubBodega,
                    IdBodega = modelo.IdBodega,
                    Estado = 0
                };

                // Guardar Sub Bodega en la base de datos
                _subBodegaRepository.SaveSubBodega(dict);


                response.Data = dict;
                response.Message = "Sub Bodega creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Sub Bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<SubBodega>> UpdateSubBodega(SubBodega modelo)
        {
            var response = new ServiceResponse<SubBodega>();
            SubBodega registro = await _subBodegaRepository.GetSubBodega(modelo.IdSubBodega);

            try
            {
                #region Parámetros que se actualizan 

                registro.NombreSubBodega = modelo.NombreSubBodega;
                registro.IdBodega = modelo.IdBodega;

                #endregion

                _subBodegaRepository.UpdateSubBodega(registro);

                response.Data = registro;
                response.Message = "Sub Bodega actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la sub bodega: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<SubBodega>> DeleteSubBodega(Int64 id)
        {
            var response = new ServiceResponse<SubBodega>();
            SubBodega registro = await _subBodegaRepository.GetSubBodega(id);

            try
            {
                registro.Estado = 1;

                _subBodegaRepository.UpdateSubBodega(registro);

                response.Data = registro;
                response.Message = "Sub Bodega eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la Sub Bodega: {ex.Message}";
            }

            return response;
        }
    }
}
