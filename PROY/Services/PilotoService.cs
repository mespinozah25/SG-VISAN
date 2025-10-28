using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class PilotoService : IPilotoService
    {
        private readonly IPilotoRepository _pilotoRepository;

        public PilotoService(IPilotoRepository pilotoRepository)
        {
            _pilotoRepository = pilotoRepository;
        }

        public async Task<List<Piloto>> GetAll()
        {
            List<Piloto> listado = await _pilotoRepository.GetAll();

            return listado;
        }

        public async Task<Piloto> GetPiloto(Int64 id)
        {
            Piloto query = await _pilotoRepository.GetPiloto(id);

            return query;
        }

        public async Task<ServiceResponse<Piloto>> SavePiloto(Piloto modelo)
        {
            var response = new ServiceResponse<Piloto>();

            try
            {
                // Crear objeto Piloto
                Piloto dict = new Piloto()
                {
                    IdPiloto = await _pilotoRepository.Cuenta(),
                    IdTransportista = modelo.IdTransportista,
                    NombresPiloto = modelo.NombresPiloto,
                    ApellidosPiloto = modelo.ApellidosPiloto,
                    NoLicencia = modelo.NoLicencia,
                    Estado = 0
                };

                // Guardar Piloto en la base de datos
                _pilotoRepository.SavePiloto(dict);


                response.Data = dict;
                response.Message = "Piloto creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Piloto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Piloto>> UpdatePiloto(Piloto modelo)
        {
            var response = new ServiceResponse<Piloto>();
            Piloto registro = await _pilotoRepository.GetPiloto(modelo.IdPiloto);

            try
            {
                #region Parámetros que se actualizan 

                registro.IdTransportista = modelo.IdTransportista;
                registro.NombresPiloto = modelo.NombresPiloto;
                registro.ApellidosPiloto = modelo.ApellidosPiloto;
                registro.NoLicencia = modelo.NoLicencia;

                #endregion

                _pilotoRepository.UpdatePiloto(registro);

                response.Data = registro;
                response.Message = "Piloto actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la Piloto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Piloto>> DeletePiloto(Int64 id)
        {
            var response = new ServiceResponse<Piloto>();
            Piloto registro = await _pilotoRepository.GetPiloto(id);

            try
            {
                registro.Estado = 1;

                _pilotoRepository.UpdatePiloto(registro);

                response.Data = registro;
                response.Message = "Piloto eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la Piloto: {ex.Message}";
            }

            return response;
        }
    }
}
