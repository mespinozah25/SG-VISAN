using PROY.Helpers;
using PROY.Models;
using PROY.Repositories;
using PROY.ViewModels;

namespace PROY.Services
{
    public class TecnicoService: ITecnicoService
    {
        private readonly ITecnicoRepository _tecnicoRepository;

        public TecnicoService(ITecnicoRepository tecnicoRepository)
        {
            _tecnicoRepository = tecnicoRepository;     
        }

        public async Task<List<Tecnico>> GetAll()
        {
            List<Tecnico> listado = await _tecnicoRepository.GetAll();

            return listado;
        }

        public async Task<Tecnico> GetTecnico(Int64 id)
        {
            Tecnico tecnico_encontrado = await _tecnicoRepository.GetTecnico(id);

            return tecnico_encontrado;
        }

        public async Task<ServiceResponse<Tecnico>> SaveTecnico(Tecnico modelo)
        {
            var response = new ServiceResponse<Tecnico>();

            try
            {                
                // Crear objeto Tecnico
                Tecnico tecnico = new Tecnico()
                {
                    IdTecnico = await _tecnicoRepository.Cuenta(),
                    Nombre = modelo.Nombre,
                    Telefono = modelo.Telefono,                    
                    Restrictiva = 0                    
                };

                // Guardar el usuario en la base de datos
                _tecnicoRepository.SaveTecnico(tecnico);
                //_dbContext.Usuarios.Add(usuario);
                //await _dbContext.SaveChangesAsync();

                response.Data = tecnico;
                response.Message = "Técnico creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el técnico: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Tecnico>> UpdateTecnico(Tecnico modelo)
        {
            var response = new ServiceResponse<Tecnico>();
            Tecnico tecnico_encontrado = await _tecnicoRepository.GetTecnico(modelo.IdTecnico);

            try
            {                
                tecnico_encontrado.Nombre = modelo.Nombre;
                tecnico_encontrado.Telefono = modelo.Telefono;                    
                
                _tecnicoRepository.UpdateTecnico(tecnico_encontrado);

                response.Data = tecnico_encontrado;
                response.Message = "Técnico actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el técnico: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Tecnico>> DeleteTecnico(Int64 id)
        {
            var response = new ServiceResponse<Tecnico>();
            Tecnico tecnico_encontrado = await _tecnicoRepository.GetTecnico(id);

            try
            {
                tecnico_encontrado.Restrictiva = 1;
             
                _tecnicoRepository.UpdateTecnico(tecnico_encontrado);

                response.Data = tecnico_encontrado;
                response.Message = "Técnico eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el técnico: {ex.Message}";
            }

            return response;
        }
    }
}
