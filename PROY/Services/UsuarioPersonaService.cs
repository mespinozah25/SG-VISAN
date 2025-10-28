using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class UsuarioPersonaService : IUsuarioPersonaService
    {
        private readonly IUsuarioPersonaRepository _usuarioPersonaRepository;

        public UsuarioPersonaService(IUsuarioPersonaRepository IUsuarioPersonaRepository)
        {
            _usuarioPersonaRepository = IUsuarioPersonaRepository;
        }

        public async Task<List<UsuarioPersona>> GetAll()
        {
            List<UsuarioPersona> listado = await _usuarioPersonaRepository.GetAll();

            return listado;
        }

        public async Task<UsuarioPersona> GetUsuarioPersona(Int64 id)
        {
            UsuarioPersona usuarioPersona = await _usuarioPersonaRepository.GetUsuarioPersona(id);

            return usuarioPersona;
        }

        public async Task<ServiceResponse<UsuarioPersona>> SaveUsuarioPersona(UsuarioPersona modelo)
        {
            var response = new ServiceResponse<UsuarioPersona>();

            try
            {
                // Crear objeto Bodega
                UsuarioPersona usuarioPersona = new UsuarioPersona()
                {
                    IdUsuarioPersona = await _usuarioPersonaRepository.Cuenta(),
                    IdUsuario = modelo.IdUsuario,
                    IdPersona = modelo.IdPersona,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _usuarioPersonaRepository.SaveUsuarioPersona(usuarioPersona);


                response.Data = usuarioPersona;
                response.Message = "La relación entre el Usuario y la Persona se creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la relación entre el Usuario y la Persona: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<UsuarioPersona>> UpdateUsuarioPersona(UsuarioPersona modelo)
        {
            var response = new ServiceResponse<UsuarioPersona>();
            UsuarioPersona usuarioPersona = await _usuarioPersonaRepository.GetUsuarioPersona(modelo.IdUsuarioPersona);

            try
            {
                usuarioPersona.IdUsuario = modelo.IdUsuario;
                usuarioPersona.IdPersona = modelo.IdPersona;

                _usuarioPersonaRepository.UpdateUsuarioPersona(usuarioPersona);

                response.Data = usuarioPersona;
                response.Message = "La relación entre el Usuario y la Persona se ha actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la relación entre el Usuario y la Persona: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<UsuarioPersona>> DeleteUsuarioPersona(Int64 id)
        {
            var response = new ServiceResponse<UsuarioPersona>();
            UsuarioPersona usuarioPersona = await _usuarioPersonaRepository.GetUsuarioPersona(id);

            try
            {
                usuarioPersona.Estado = 1;

                _usuarioPersonaRepository.UpdateUsuarioPersona(usuarioPersona);

                response.Data = usuarioPersona;
                response.Message = "La relación entre el Usuario y la Persona ha sido eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la relación entre el Usuario y la Persona: {ex.Message}";
            }

            return response;
        }
    }
}
