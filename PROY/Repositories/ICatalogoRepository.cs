using Microsoft.AspNetCore.Mvc;
using PROY.Models;

namespace PROY.Repositories
{
    public interface ICatalogoRepository
    {
        Task<List<Catalogo>> GetAllCatalogos();
        Task<Catalogo> GetCatalogo(int id);
        Task<List<Catalogo>> GetCatalogo(string tipo);
        Task<int> Cuenta();
        Task<Catalogo> SaveCatalogo(Catalogo modelo);
        Task<Catalogo> UpdateCatalogo(Catalogo modelo);
        JsonResult GetMunicipios(int orden);
        JsonResult GetMunicipiosFilter(int orden);
        JsonResult GetCat(string tipo);
        Task<int> GetDepartamento(string paramDepto);
        int GetOrdenDepto(int paramId);
        Task<int> GetMunicipio(string paramDepto, string paramMuni);
        Task<int> GetItemCatalogo(string paramItem, string paramTipo);

        //Task<Catalogo> SaveSolicitud(Catalogo modelo);
        //Task<Catalogo> UpdateSolicitud(Catalogo modelo);
    }
}
