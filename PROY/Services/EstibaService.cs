using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class EstibaService : IEstibaService
    {
        private readonly IEstibaRepository _estibaRepository;

        public EstibaService(IEstibaRepository estibaRepository)
        {
            _estibaRepository = estibaRepository;
        }

        public async Task<List<Estiba>> GetAll()
        {
            List<Estiba> listado = await _estibaRepository.GetAll();

            return listado;
        }

        public async Task<List<Estiba>> ObtenerSegunBodegaYSubBodega(int idBodega, int idSubBodega)
        {
            List<Estiba> listado = await _estibaRepository.ObtenerSegunBodegaYSubBodega(idBodega, idSubBodega);

            return listado;
        }

        public async Task<Estiba> GetEstiba(Int64 id)
        {
            Estiba query = await _estibaRepository.GetEstiba(id);

            return query;
        }

        public async Task<ServiceResponse<Estiba>> SaveEstiba(Estiba modelo)
        {
            var response = new ServiceResponse<Estiba>();

            try
            {
                // Crear objeto Estiba
                Estiba dict = new Estiba()
                {
                    IdEstiba = await _estibaRepository.Cuenta(),
                    IdBodega = modelo.IdBodega,
                    IdSubBodega = modelo.IdSubBodega,
                    CodigoEstiba = modelo.CodigoEstiba,
                    DescripcionEstiba = modelo.DescripcionEstiba,
                    IdProducto = modelo.IdProducto,
                    MetrosCuadrados = modelo.MetrosCuadrados,
                    IdUsuario = modelo.IdUsuario,
                    Estado = 0
                };

                // Guardar Estiba en la base de datos
                _estibaRepository.SaveEstiba(dict);


                response.Data = dict;
                response.Message = "Estiba creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la Estiba: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Estiba>> UpdateEstiba(Estiba modelo)
        {
            var response = new ServiceResponse<Estiba>();
            Estiba registro = await _estibaRepository.GetEstiba(modelo.IdEstiba);

            try
            {
                #region Parámetros que se actualizan 

                registro.IdBodega = modelo.IdBodega;
                registro.IdSubBodega = modelo.IdSubBodega;
                registro.CodigoEstiba = modelo.CodigoEstiba;
                registro.DescripcionEstiba = modelo.DescripcionEstiba;
                registro.IdProducto = modelo.IdProducto;
                registro.MetrosCuadrados = modelo.MetrosCuadrados;
                registro.IdUsuario = modelo.IdUsuario;

                #endregion

                _estibaRepository.UpdateEstiba(registro);

                response.Data = registro;
                response.Message = "Estiba actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar Estiba: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Estiba>> DeleteEstiba(Int64 id)
        {
            var response = new ServiceResponse<Estiba>();
            Estiba registro = await _estibaRepository.GetEstiba(id);

            try
            {
                registro.Estado = 1;

                _estibaRepository.UpdateEstiba(registro);

                response.Data = registro;
                response.Message = "Estiba eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar Estiba: {ex.Message}";
            }

            return response;
        }
    }
}
