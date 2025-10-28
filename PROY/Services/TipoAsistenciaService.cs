using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class TipoAsistenciaService : ITipoAsistenciaService
    {

        private readonly ITipoAsistenciaRepository _tipoAsistenciaRepository;

        public TipoAsistenciaService(ITipoAsistenciaRepository tipoAsistenciaRepository)
        {
            _tipoAsistenciaRepository = tipoAsistenciaRepository;
        }

        public async Task<List<TipoAsistencia>> GetAll()
        {
            List<TipoAsistencia> listado = await _tipoAsistenciaRepository.GetAll();

            return listado;
        }

        public async Task<TipoAsistencia> GetTipoAsistencia(Int64 id)
        {
            TipoAsistencia tipoAsistencia = await _tipoAsistenciaRepository.GetTipoAsistencia(id);

            return tipoAsistencia;
        }

        public async Task<ServiceResponse<TipoAsistencia>> SaveTipoAsistencia(TipoAsistencia modelo)
        {
            var response = new ServiceResponse<TipoAsistencia>();

            try
            {
                // Crear objeto Bodega
                TipoAsistencia tipoAsistencia = new TipoAsistencia()
                {
                    IdTipoAsistencia = await _tipoAsistenciaRepository.Cuenta(),
                    NombreTipoAsistencia = modelo.NombreTipoAsistencia,
                    Abreviatura = modelo.Abreviatura,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _tipoAsistenciaRepository.SaveTipoAsistencia(tipoAsistencia);


                response.Data = tipoAsistencia;
                response.Message = "Tipo Asistencia creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el tipo asistencia: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<TipoAsistencia>> UpdateTipoAsistencia(TipoAsistencia modelo)
        {
            var response = new ServiceResponse<TipoAsistencia>();
            TipoAsistencia tipoAsistencia = await _tipoAsistenciaRepository.GetTipoAsistencia(modelo.IdTipoAsistencia);

            try
            {
                tipoAsistencia.NombreTipoAsistencia = modelo.NombreTipoAsistencia;
                tipoAsistencia.Abreviatura = modelo.Abreviatura;

                _tipoAsistenciaRepository.UpdateTipoAsistencia(tipoAsistencia);

                response.Data = tipoAsistencia;
                response.Message = "Tipo asistencia actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el tipo asistencia: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<TipoAsistencia>> DeleteTipoAsistencia(Int64 id)
        {
            var response = new ServiceResponse<TipoAsistencia>();
            TipoAsistencia tipoAsistencia = await _tipoAsistenciaRepository.GetTipoAsistencia(id);

            try
            {
                tipoAsistencia.Estado = 1;

                _tipoAsistenciaRepository.UpdateTipoAsistencia(tipoAsistencia);

                response.Data = tipoAsistencia;
                response.Message = "Tipo de Asistencia eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el tipo de asistencia: {ex.Message}";
            }

            return response;
        }
    }
}
