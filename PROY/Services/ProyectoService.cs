using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ProyectoService : IProyectoService
    {
        private readonly IProyectoRepository _proyectoRepository;

        public ProyectoService(IProyectoRepository proyectoRepository)
        {
            _proyectoRepository = proyectoRepository;
        }

        public async Task<List<Proyecto>> GetAll()
        {
            List<Proyecto> listado = await _proyectoRepository.GetAll();

            return listado;
        }

        public async Task<Proyecto> GetProyecto(Int64 id)
        {
            Proyecto proyecto_encontrada = await _proyectoRepository.GetProyecto(id);

            return proyecto_encontrada;
        }

        public async Task<ServiceResponse<Proyecto>> SaveProyecto(Proyecto modelo)
        {
            var response = new ServiceResponse<Proyecto>();

            try
            {
                // Crear objeto Proyecto
                Proyecto proyecto = new Proyecto()
                {
                    IdProyecto = await _proyectoRepository.Cuenta(),
                    NombreProyecto = modelo.NombreProyecto,
                    Anio = modelo.Anio,
                    Estado = 0
                };

                // Guardar el proyecto en la base de datos
                _proyectoRepository.SaveProyecto(proyecto);


                response.Data = proyecto;
                response.Message = "Proyecto creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el proyecto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Proyecto>> UpdateProyecto(Proyecto modelo)
        {
            var response = new ServiceResponse<Proyecto>();
            Proyecto proyecto_encontrada = await _proyectoRepository.GetProyecto(modelo.IdProyecto);

            try
            {
                proyecto_encontrada.NombreProyecto = modelo.NombreProyecto;
                proyecto_encontrada.Anio = modelo.Anio;
               

                _proyectoRepository.UpdateProyecto(proyecto_encontrada);

                response.Data = proyecto_encontrada;
                response.Message = "Proyecto actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el proyecto: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Proyecto>> DeleteProyecto(Int64 id)
        {
            var response = new ServiceResponse<Proyecto>();
            Proyecto proyecto_encontrada = await _proyectoRepository.GetProyecto(id);

            try
            {
                proyecto_encontrada.Estado = 1;

                _proyectoRepository.UpdateProyecto(proyecto_encontrada);

                response.Data = proyecto_encontrada;
                response.Message = "Proyecto eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el proyecto: {ex.Message}";
            }

            return response;
        }

    }
}
