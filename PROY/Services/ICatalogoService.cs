using Microsoft.AspNetCore.Mvc;
using PROY.Models;

namespace PROY.Services
{
    public interface ICatalogoService
    {
        Task<List<Catalogo>> GetAllCatalogos();
        Task<Catalogo> GetCatalogo(int id);
        Task<List<Catalogo>> GetCatalogo(string tipo);
        Task<ServiceResponse<Catalogo>> SaveCatalogo(Catalogo modelo);
        Task<ServiceResponse<Catalogo>> UpdateCatalogo(Catalogo modelo);
        Task<ServiceResponse<Catalogo>> DeleteCatalogo(int id);
        JsonResult GetCat(string tipo);
        JsonResult GetMunicipios(int orden);
        JsonResult GetMunicipiosFilter(int orden);
        int GetOrdenDepto(int paramId);
        //Task<ServiceResponse<Solicitud>> SaveSolicitud(Solicitud modelo);
        //Task<ServiceResponse<Solicitud>> UpdateSolicitud(Solicitud modelo);
        //Task<ServiceResponse<Solicitud>> DeleteSolicitud(Int64 id);
    }
}
