using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class PersonalBodegaService : IPersonalBodegaService
    {
        private readonly IPersonalBodegaRepository _personalBodegaRepository;

        public PersonalBodegaService(IPersonalBodegaRepository IPersonalBodegaRepository)
        {
            _personalBodegaRepository = IPersonalBodegaRepository;
        }

        public async Task<List<PersonalBodega>> GetAll()
        {
            List<PersonalBodega> listado = await _personalBodegaRepository.GetAll();

            return listado;
        }

        public async Task<PersonalBodega> GetPersonalBodega(Int64 id)
        {
            PersonalBodega personal = await _personalBodegaRepository.GetPersonalBodega(id);

            return personal;
        }

        public async Task<ServiceResponse<PersonalBodega>> SavePersonalBodega(PersonalBodega modelo)
        {
            var response = new ServiceResponse<PersonalBodega>();

            try
            {
                // Crear objeto Bodega
                PersonalBodega personal = new PersonalBodega()
                {
                    IdPersonalBodega = await _personalBodegaRepository.Cuenta(),
                    Dpi = modelo.Dpi,
                    Nit = modelo.Nit,
                    Nombres = modelo.Nombres,
                    Apellidos = modelo.Apellidos,
                    IdPuesto = modelo.IdPuesto,
                    IdRenglonPresupuestario = modelo.IdRenglonPresupuestario,
                    Email = modelo.Email,
                    Telefono = modelo.Telefono,
                    IdBodega = modelo.IdBodega,
                    IdUsuario = modelo.IdUsuario,
                    Estado = 0
                };

                // Guardar el usuario en la base de datos
                _personalBodegaRepository.SavePersonalBodega(personal);


                response.Data = personal;
                response.Message = "Personal creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el puesto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<PersonalBodega>> UpdatePersonalBodega(PersonalBodega modelo)
        {
            var response = new ServiceResponse<PersonalBodega>();
            PersonalBodega personal = await _personalBodegaRepository.GetPersonalBodega(modelo.IdPersonalBodega);

            try
            {
                personal.Dpi = modelo.Dpi;
                personal.Nit = modelo.Nit;
                personal.Nombres = modelo.Nombres;
                personal.Apellidos = modelo.Apellidos;
                personal.IdPuesto = modelo.IdPuesto;
                personal.IdRenglonPresupuestario = modelo.IdRenglonPresupuestario;
                personal.Email = modelo.Email;
                personal.Telefono = modelo.Telefono;
                personal.IdBodega = modelo.IdBodega;
                personal.IdUsuario = modelo.IdUsuario;

                _personalBodegaRepository.UpdatePersonalBodega(personal);

                response.Data = personal;
                response.Message = "Personal actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el puesto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<PersonalBodega>> DeletePersonalBodega(Int64 id)
        {
            var response = new ServiceResponse<PersonalBodega>();
            PersonalBodega personal = await _personalBodegaRepository.GetPersonalBodega(id);

            try
            {
                personal.Estado = 1;

                _personalBodegaRepository.UpdatePersonalBodega(personal);

                response.Data = personal;
                response.Message = "Personal eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el tipo de bodega: {ex.Message}";
            }

            return response;
        }
    }
}
