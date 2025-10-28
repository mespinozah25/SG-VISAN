using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class BodegaController : Controller
    {
        private readonly IBodegaService _bodegaService;
        private readonly ITipoBodegaService _tipoBodegaService;
        private readonly ICatalogoService _catalogoService;

        public BodegaController(IBodegaService bodegaService, ICatalogoService catalogoService, ITipoBodegaService tipoBodegaService)
        {
            _bodegaService = bodegaService;
            _catalogoService = catalogoService;
            _tipoBodegaService = tipoBodegaService;
        }


        public async Task<IActionResult> Index(int? idBodega)
        {
            List<Bodega> listado = await _bodegaService.GetAll();

            #region Catálogos

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var tipoBodegas = await _tipoBodegaService.GetAll();
            ViewBag.tipoBodegas = new SelectList(tipoBodegas, "IdTipoBodega", "NombreTipoBodega");
            var tipoBodegasDict = tipoBodegas.ToDictionary(i => i.IdTipoBodega, i => i.NombreTipoBodega);

            #endregion

            if (idBodega == null || idBodega == 0)
            {
                var bodegaVM = new BodegaVM()
                {
                    Bodega = new Bodega(),
                    listaBodegas = listado,
                    TipoBodegaDict = tipoBodegasDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA BODEGA";
                return View(bodegaVM);
            }
            else
            {
                var bodegaVM = new BodegaVM()
                {
                    Bodega = await _bodegaService.GetBodega((int)idBodega),
                    listaBodegas = await _bodegaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR BODEGA "; 
                return View(bodegaVM);
            }
        }

        public async Task<IActionResult> ObtenerBodega(int id)
        {
            var bodega = await _bodegaService.GetBodega(id);

            if (bodega == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idBodega = bodega.IdBodega,
                nombre = bodega.NombreBodega,
                direccion = bodega.DireccionBodega,
                idTipoBodega = bodega.IdTipoBodega,
                idDepartamento = bodega.IdDepartamento,
                idMunicipio = bodega.IdMunicipio,
                idUsuario = bodega.IdUsuario,
                estado = bodega.Estado
            });

        }

        public async Task<IActionResult> GuardarBodega(BodegaVM modelo)
        {
            Bodega modelBod = new Bodega()
            {
                IdBodega = modelo.Bodega.IdBodega,
                NombreBodega = modelo.Bodega.NombreBodega,
                DireccionBodega = modelo.Bodega.DireccionBodega,
                IdTipoBodega = modelo.Bodega.IdTipoBodega,
                IdDepartamento = modelo.Bodega.IdDepartamento,
                IdMunicipio = modelo.Bodega.IdMunicipio,
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                Estado = 0,
            };

            if (modelBod.IdBodega == 0)
            {
                var result = await _bodegaService.SaveBodega(modelBod);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Bodega");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelBod);
                }
            }
            else
            {
                var result = await _bodegaService.UpdateBodega(modelBod);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Bodega");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelBod);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _bodegaService.DeleteBodega(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Bodega");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
