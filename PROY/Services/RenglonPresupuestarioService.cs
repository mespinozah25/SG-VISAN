using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class RenglonPresupuestarioService : IRenglonPresupuestarioService
    {
        private readonly IRenglonPresupuestarioRepository _renglonPresupuestarioRepository;

        public RenglonPresupuestarioService(IRenglonPresupuestarioRepository renglonRepository)
        {
            _renglonPresupuestarioRepository = renglonRepository;
        }

        public async Task<List<RenglonPresupuestario>> GetAll()
        {
            List<RenglonPresupuestario> listado = await _renglonPresupuestarioRepository.GetAll();

            return listado;
        }

        public async Task<RenglonPresupuestario> GetRenglonPresupuestario(Int64 id)
        {
            RenglonPresupuestario puesto = await _renglonPresupuestarioRepository.GetRenglonPresupuestario(id);

            return puesto;
        }

        public async Task<ServiceResponse<RenglonPresupuestario>> SaveRenglonPresupuestario(RenglonPresupuestario modelo)
        {
            var response = new ServiceResponse<RenglonPresupuestario>();

            try
            {
                // Crear objeto Bodega
                RenglonPresupuestario renglon = new RenglonPresupuestario()
                {
                    IdRenglonPresupuestario = await _renglonPresupuestarioRepository.Cuenta(),
                    Renglon = modelo.Renglon,
                    Descripcion = modelo.Descripcion,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _renglonPresupuestarioRepository.SaveRenglonPresupuestario(renglon);


                response.Data = renglon;
                response.Message = "Renglón Presupuestario creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el puesto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<RenglonPresupuestario>> UpdateRenglonPresupuestario(RenglonPresupuestario modelo)
        {
            var response = new ServiceResponse<RenglonPresupuestario>();
            RenglonPresupuestario renglon = await _renglonPresupuestarioRepository.GetRenglonPresupuestario(modelo.IdRenglonPresupuestario);

            try
            {
                #region Parámetros que se actualizan 

                renglon.Renglon = modelo.Renglon;
                renglon.Descripcion = modelo.Descripcion;

                #endregion

                _renglonPresupuestarioRepository.UpdateRenglonPresupuestario(renglon);

                response.Data = renglon;
                response.Message = "Renglon Presupuestario actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Renglon Presupuestario: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<RenglonPresupuestario>> DeleteRenglonPresupuestario(Int64 id)
        {
            var response = new ServiceResponse<RenglonPresupuestario>();
            RenglonPresupuestario renglon = await _renglonPresupuestarioRepository.GetRenglonPresupuestario(id);

            try
            {
                renglon.Estado = 1;

                _renglonPresupuestarioRepository.UpdateRenglonPresupuestario(renglon);

                response.Data = renglon;
                response.Message = "Renglon Presupuestario eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Renglon Presupuestario: {ex.Message}";
            }

            return response;
        }
    }
}
