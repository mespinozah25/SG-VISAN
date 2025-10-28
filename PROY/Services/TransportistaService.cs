using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class TransportistaService : ITransportistaService
    {
        private readonly ITransportistaRepository _transportistaRepository;

        public TransportistaService(ITransportistaRepository transportistaRepository)
        {
            _transportistaRepository = transportistaRepository;
        }

        public async Task<List<Transportista>> GetAll()
        {
            List<Transportista> listado = await _transportistaRepository.GetAll();

            return listado;
        }

        public async Task<Transportista> GetTransportista(Int64 id)
        {
            Transportista query = await _transportistaRepository.GetTransportista(id);

            return query;
        }

        public async Task<ServiceResponse<Transportista>> SaveTransportista(Transportista modelo)
        {
            var response = new ServiceResponse<Transportista>();

            try
            {
                // Crear objeto transportista
                Transportista dict = new Transportista()
                {
                    IdTransportista = await _transportistaRepository.Cuenta(),
                    NombreTransportista = modelo.NombreTransportista,
                    Telefono = modelo.Telefono,
                    NombreContacto = modelo.NombreContacto,
                    IdUsuario = modelo.IdUsuario,
                    Estado = 0
                };

                // Guardar el transportista en la base de datos
                _transportistaRepository.SaveTransportista(dict);


                response.Data = dict;
                response.Message = "Transportista creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Transportista: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Transportista>> UpdateTransportista(Transportista modelo)
        {
            var response = new ServiceResponse<Transportista>();
            Transportista registro = await _transportistaRepository.GetTransportista(modelo.IdTransportista);

            try
            {
                #region Parámetros que se actualizan 

                registro.NombreTransportista = modelo.NombreTransportista;
                registro.Telefono = modelo.Telefono;
                registro.NombreContacto = modelo.NombreContacto;
                registro.IdUsuario = modelo.IdUsuario;

                #endregion

                _transportistaRepository.UpdateTransportista(registro);

                response.Data = registro;
                response.Message = "Transportista actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la Transportista: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Transportista>> DeleteTransportista(Int64 id)
        {
            var response = new ServiceResponse<Transportista>();
            Transportista registro = await _transportistaRepository.GetTransportista(id);

            try
            {
                registro.Estado = 1;

                _transportistaRepository.UpdateTransportista(registro);

                response.Data = registro;
                response.Message = "Transportista eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la Transportista: {ex.Message}";
            }

            return response;
        }
    }
}
