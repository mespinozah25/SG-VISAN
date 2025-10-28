using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class PresentacionPAService : IPresentacionPAService
    {
        private readonly IPresentacionPARepository _presentacionpaRepository;

        public PresentacionPAService(IPresentacionPARepository presentacionpaRepository)
        {
            _presentacionpaRepository = presentacionpaRepository;
        }

        public async Task<List<PresentacionPA>> GetAll()
        {
            List<PresentacionPA> listado = await _presentacionpaRepository.GetAll();

            return listado;
        }

        public async Task<PresentacionPA> GetPresentacionPA(Int64 id)
        {
            PresentacionPA presentacionpa_encontrada = await _presentacionpaRepository.GetPresentacionPA(id);

            return presentacionpa_encontrada;
        }

        public async Task<ServiceResponse<PresentacionPA>> SavePresentacionPA(PresentacionPA modelo)
        {
            var response = new ServiceResponse<PresentacionPA>();

            try
            {
                // Crear objeto
                PresentacionPA presentacionpa = new PresentacionPA()
                {
                    IdPresentacionPA = await _presentacionpaRepository.Cuenta(),
                    NombrePresentacionPA = modelo.NombrePresentacionPA,
                    Estado = 0
                };

                // Guardar la información en la base de datos
                _presentacionpaRepository.SavePresentacionPA(presentacionpa);


                response.Data = presentacionpa;
                response.Message = "Presentación de Producto Alimenticio creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la Presentación de Producto Alimenticio: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<PresentacionPA>> UpdatePresentacionPA(PresentacionPA modelo)
        {
            var response = new ServiceResponse<PresentacionPA>();
            PresentacionPA presentacionpa_encontrada = await _presentacionpaRepository.GetPresentacionPA(modelo.IdPresentacionPA);

            try
            {
                presentacionpa_encontrada.NombrePresentacionPA = modelo.NombrePresentacionPA;

                _presentacionpaRepository.UpdatePresentacionPA(presentacionpa_encontrada);

                response.Data = presentacionpa_encontrada;
                response.Message = "Presentación de Producto Alimentario actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la ¨Presentación de Producto Alimentario: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<PresentacionPA>> DeletePresentacionPA(Int64 id)
        {
            var response = new ServiceResponse<PresentacionPA>();
            PresentacionPA presentacionpa_encontrada = await _presentacionpaRepository.GetPresentacionPA(id);

            try
            {
                presentacionpa_encontrada.Estado = 1;

                _presentacionpaRepository.UpdatePresentacionPA(presentacionpa_encontrada);

                response.Data = presentacionpa_encontrada;
                response.Message = "Presentación de Producto Alimentario eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la Presentación de Producto Alimentario: {ex.Message}";
            }

            return response;
        }
    }
}
