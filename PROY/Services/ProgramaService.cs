using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ProgramaService : IProgramaService
    {
        private readonly IProgramaRepository _programaRepository;

        public ProgramaService(IProgramaRepository programaRepository)
        {
            _programaRepository = programaRepository;
        }

        public async Task<List<Programa>> GetAll()
        {
            List<Programa> listado = await _programaRepository.GetAll();

            return listado;
        }

        public async Task<Programa> GetPrograma(Int64 id)
        {
            Programa programa_encontrada = await _programaRepository.GetPrograma(id);

            return programa_encontrada;
        }

        public async Task<ServiceResponse<Programa>> SavePrograma(Programa modelo)
        {
            var response = new ServiceResponse<Programa>();

            try
            {
                // Crear objeto Programa
                Programa programa = new Programa()
                {
                    IdPrograma = await _programaRepository.Cuenta(),
                    NombrePrograma = modelo.NombrePrograma,
                    Estado = 0
                };

                // Guardar el programa en la base de datos
                _programaRepository.SavePrograma(programa);


                response.Data = programa;
                response.Message = "Programa creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el programa: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Programa>> UpdatePrograma(Programa modelo)
        {
            var response = new ServiceResponse<Programa>();
            Programa programa_encontrada = await _programaRepository.GetPrograma(modelo.IdPrograma);

            try
            {
                programa_encontrada.NombrePrograma = modelo.NombrePrograma;

                _programaRepository.UpdatePrograma(programa_encontrada);

                response.Data = programa_encontrada;
                response.Message = "Programa actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el programa: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Programa>> DeletePrograma(Int64 id)
        {
            var response = new ServiceResponse<Programa>();
            Programa programa_encontrada = await _programaRepository.GetPrograma(id);

            try
            {
                programa_encontrada.Estado = 1;

                _programaRepository.UpdatePrograma(programa_encontrada);

                response.Data = programa_encontrada;
                response.Message = "Programa eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el programa: {ex.Message}";
            }

            return response;
        }

    }
}
