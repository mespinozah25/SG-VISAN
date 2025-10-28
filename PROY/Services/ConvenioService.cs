using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ConvenioService : IConvenioService
    {
        private readonly IConvenioRepository _convenioRepository;

        public ConvenioService(IConvenioRepository convenioRepository)
        {
            _convenioRepository = convenioRepository;
        }

        public async Task<List<Convenio>> GetAll()
        {
            List<Convenio> listado = await _convenioRepository.GetAll();

            return listado;
        }

        public async Task<Convenio> GetConvenio(Int64 id)
        {
            Convenio convenio_encontrada = await _convenioRepository.GetConvenio(id);

            return convenio_encontrada;
        }

        public async Task<ServiceResponse<Convenio>> SaveConvenio(Convenio modelo)
        {
            var response = new ServiceResponse<Convenio>();

            try
            {
                // Crear objeto Convenio
                Convenio convenio = new Convenio()
                {
                    IdConvenio = await _convenioRepository.Cuenta(),
                    NombreConvenio = modelo.NombreConvenio,
                    Descripcion = modelo.Descripcion,
                    VidaUtil = modelo.VidaUtil,
                    TotalRaciones = modelo.TotalRaciones,
                    Estado = 0
                };

                // Guardar el convenio en la base de datos
                _convenioRepository.SaveConvenio(convenio);


                response.Data = convenio;
                response.Message = "Convenio creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el convenio: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Convenio>> UpdateConvenio(Convenio modelo)
        {
            var response = new ServiceResponse<Convenio>();
            Convenio convenio_encontrada = await _convenioRepository.GetConvenio(modelo.IdConvenio);

            try
            {
                convenio_encontrada.NombreConvenio = modelo.NombreConvenio;
                convenio_encontrada.Descripcion = modelo.Descripcion;
                convenio_encontrada.VidaUtil = modelo.VidaUtil;
                convenio_encontrada.TotalRaciones = modelo.TotalRaciones;

                _convenioRepository.UpdateConvenio(convenio_encontrada);

                response.Data = convenio_encontrada;
                response.Message = "Convenio actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el convenio: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Convenio>> DeleteConvenio(Int64 id)
        {
            var response = new ServiceResponse<Convenio>();
            Convenio convenio_encontrada = await _convenioRepository.GetConvenio(id);

            try
            {
                convenio_encontrada.Estado = 1;

                _convenioRepository.UpdateConvenio(convenio_encontrada);

                response.Data = convenio_encontrada;
                response.Message = "Convenio eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el convenio: {ex.Message}";
            }

            return response;
        }
    }
}
