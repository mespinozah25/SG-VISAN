using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Data;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class CatalogoController : Controller
    {        
        private readonly ICatalogoService _catalogoService;
        private readonly AppDBContext _dbContext;

        public CatalogoController(ICatalogoService catalogoService, AppDBContext dbContext)
        {
            _catalogoService = catalogoService;
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(int ? idCat)
        {
            //List<Catalogo> listado = await _catalogoService.GetAllCatalogos();
            //return View(listado);

            var cat = await _catalogoService.GetAllCatalogos();
            var distinctCat = cat.Select(c => c.Tipo).Distinct();
            ViewBag.Catalogos = new SelectList(distinctCat);
            //var catDict = cat.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);


            if (idCat == null || idCat==0)
            {
                var solicitudVM = new SolicitudesVM()
                {
                    Catalogo = new Catalogo(),//await _solicitudService.GetSolicitud(1),                    
                    listaCatalogos = await _catalogoService.GetAllCatalogos(),
                    //IntervencionDict = intervencionDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA SOLICITUD";
                return View(solicitudVM);
            }
            else
            {
                var solicitudVM = new SolicitudesVM()
                {
                    //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                    Catalogo = await _catalogoService.GetCatalogo((int) idCat),
                    listaCatalogos = await _catalogoService.GetAllCatalogos(),
                                        
                    //IntervencionDict = intervencionDict,
                };
                
                //solicitud = await _solicitudService.GetSolicitud((long)id);
                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR SOLICITUD "; //+ solicitudVM.Solicitud.Codigo;
                return View(solicitudVM);
            }
        }

        public async Task<IActionResult> ObtenerCatalogo(int id)
        {
            var catalogo = await _catalogoService.GetCatalogo(id);

            if (catalogo == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idCatalago = catalogo.IdCatalogo,
                tipo = catalogo.Tipo,
                descripcion = catalogo.Descripcion,
                orden = catalogo.Orden
            });

        }

        public async Task<IActionResult> GuardarCatalogo(SolicitudesVM modelo)
        {
            //var roles = await _usuarioService.GetAllRoles();
            //ViewBag.Roles = new SelectList(roles, "RolId", "Nombre");

            Catalogo modelCat = new Catalogo()
            {
                IdCatalogo = modelo.Catalogo.IdCatalogo,
                Tipo = modelo.Catalogo.Tipo,
                Descripcion = modelo.Catalogo.Descripcion,              
                Orden = modelo.Catalogo.Orden                
            };

            if (modelCat.IdCatalogo == 0)
            {
                var result = await _catalogoService.SaveCatalogo(modelCat);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Catalogo");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelCat);
                }
            }
            else
            {
                var result = await _catalogoService.UpdateCatalogo(modelCat);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Catalogo");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelCat);
                }
            }
            
        }
    }
}
