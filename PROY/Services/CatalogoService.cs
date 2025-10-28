using Microsoft.AspNetCore.Mvc;
using PROY.Helpers;
using PROY.Models;
using PROY.Repositories;
using PROY.ViewModels;

namespace PROY.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ICatalogoRepository _catalogoRepository;

        public CatalogoService(ICatalogoRepository catalogoRepository)
        {
            _catalogoRepository = catalogoRepository;
        }
        public async Task<List<Catalogo>> GetAllCatalogos()
        {
            List<Catalogo> listado = await _catalogoRepository.GetAllCatalogos();

            return listado;
        }

        public async Task<Catalogo> GetCatalogo(int id)
        {
            Catalogo catalogo_encontrado = await _catalogoRepository.GetCatalogo(id);

            return catalogo_encontrado;
        }
        public async Task<List<Catalogo>> GetCatalogo(string tipo)
        {
            List<Catalogo> lista_encontrada = await _catalogoRepository.GetCatalogo(tipo);

            return lista_encontrada;
        }

        public async Task<ServiceResponse<Catalogo>> SaveCatalogo(Catalogo modelo)
        {
            var response = new ServiceResponse<Catalogo>();

            try
            {                
                // Crear objeto Usuario
                Catalogo catalogo = new Catalogo()
                {
                    IdCatalogo = await _catalogoRepository.Cuenta(),
                    Tipo = modelo.Tipo,
                    Descripcion = modelo.Descripcion,
                    Orden = modelo.Orden,                    
                    Restrictiva = 0                    
                };

                // Guardar el item del catalogo en la base de datos
                await _catalogoRepository.SaveCatalogo(catalogo);
                
                response.Data = catalogo;
                response.Message = "Registro de catalogo creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el registro del catalogo: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Catalogo>> UpdateCatalogo(Catalogo modelo)
        {
            var response = new ServiceResponse<Catalogo>();
            Catalogo catalogo_encontrado = await _catalogoRepository.GetCatalogo(modelo.IdCatalogo);

            try
            {
                catalogo_encontrado.Tipo = modelo.Tipo;
                catalogo_encontrado.Descripcion = modelo.Descripcion;
                catalogo_encontrado.Orden = modelo.Orden;                
                
                await _catalogoRepository.UpdateCatalogo(catalogo_encontrado);

                response.Data = catalogo_encontrado;
                response.Message = "Catalogo actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Catalogo: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Catalogo>> DeleteCatalogo(int id)
        {
            var response = new ServiceResponse<Catalogo>();
            Catalogo catalogo_encontrado = await _catalogoRepository.GetCatalogo(id);

            try
            {
                catalogo_encontrado.Restrictiva = 1;
                
                await _catalogoRepository.UpdateCatalogo(catalogo_encontrado);

                response.Data = catalogo_encontrado;
                response.Message = "Registro de catalogo eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el usuario: {ex.Message}";
            }

            return response;
        }
        public JsonResult GetCat(string tipo)
        {
            var lista_encontrada = _catalogoRepository.GetCat(tipo);                                    

            return new JsonResult(lista_encontrada);
        }

        public JsonResult GetMunicipios(int orden)
        {
            var lista_encontrada = _catalogoRepository.GetMunicipios(orden);

            return new JsonResult(lista_encontrada);
        }

        public JsonResult GetMunicipiosFilter(int orden)
        {
            var lista_encontrada = _catalogoRepository.GetMunicipiosFilter(orden);

            return new JsonResult(lista_encontrada);
        }

        public int GetOrdenDepto(int paramId)
        {
            int resultado = Convert.ToInt32(_catalogoRepository.GetOrdenDepto(paramId).ToString());

            return resultado;
        }
    }
}
