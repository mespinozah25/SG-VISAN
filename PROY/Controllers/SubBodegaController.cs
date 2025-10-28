using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class SubBodegaController : Controller
    {
        private readonly ISubBodegaService _SubBodegaService;
        private readonly IBodegaService _BodegaService;

        public SubBodegaController(ISubBodegaService subBodegaService, IBodegaService bodegaService)
        {
            _SubBodegaService = subBodegaService;
            _BodegaService = bodegaService;
        }

        public async Task<IActionResult> Index(int idBodega, int? idData)
        {
            List<SubBodega> listado = await _SubBodegaService.ObtenerSegunBodega(idBodega);
            #region Catálogos  

            var bodegas = await _BodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodegas, "IdBodega", "NombreBodega");
            var bodegasDict = bodegas.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            #endregion

            if (idData == null || idData == 0)
            {
                var dataVM = new SubBodegaVM()
                {
                    SubBodega = new SubBodega(idBodega),
                    listaSubBodegas = listado,
                    BodegaDict = bodegasDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO SUB BODEGA";
                return View(dataVM);
            }
            else
            {
                var dataVM = new SubBodegaVM()
                {
                    SubBodega = await _SubBodegaService.GetSubBodega((int)idData),
                    listaSubBodegas = await _SubBodegaService.GetAll(),
                    BodegaDict = bodegasDict
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR SUB BODEGA";
                return View(dataVM);
            }
        }

        public async Task<IActionResult> ObtenerSubBodega(int id)
        {
            var dataResp = await _SubBodegaService.GetSubBodega(id);

            if (dataResp == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idSubBodega = dataResp.IdSubBodega,
                nombre = dataResp.NombreSubBodega,
                idBodega = dataResp.IdBodega,
                estado = dataResp.Estado
            });

        }

        public async Task<IActionResult> GuardarSubBodega(SubBodegaVM modelo)
        {
            SubBodega modelData = new SubBodega()
            {
                IdSubBodega = modelo.SubBodega.IdSubBodega,
                NombreSubBodega = modelo.SubBodega.NombreSubBodega,
                IdBodega = modelo.SubBodega.IdBodega,
                Estado = 0,
            };

            if (modelData.IdSubBodega == 0)
            {
                var result = await _SubBodegaService.SaveSubBodega(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "SubBodega", new { idBodega = modelo.SubBodega.IdBodega });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
            else
            {
                var result = await _SubBodegaService.UpdateSubBodega(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "SubBodega", new { idBodega = modelo.SubBodega.IdBodega });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id, int idBodega)
        {
            var result = await _SubBodegaService.DeleteSubBodega(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "SubBodega", new { idBodega });
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
