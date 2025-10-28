using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class TipoBodegaController : Controller
    {
        private readonly ITipoBodegaService _tipoBodegaService;

        public TipoBodegaController(ITipoBodegaService tipoBodegaService)
        {
            _tipoBodegaService = tipoBodegaService;
        }


        public async Task<IActionResult> Index(int? idTipoBodega)
        {
            List<TipoBodega> listado = await _tipoBodegaService.GetAll();

            #region Catálogos  

            #endregion

            if (idTipoBodega == null || idTipoBodega == 0)
            {
                var tipoBodegaVM = new TipoBodegaVM()
                {
                    tipoBodega = new TipoBodega(),
                    listaTiposBodegas = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO TIPO BODEGA";
                return View(tipoBodegaVM);
            }
            else
            {
                var tipoBodegaVM = new TipoBodegaVM()
                {
                    tipoBodega = await _tipoBodegaService.GetTipoBodega((int)idTipoBodega),
                    listaTiposBodegas = await _tipoBodegaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR TIPO BODEGA ";
                return View(tipoBodegaVM);
            }
        }

        public async Task<IActionResult> ObtenerTipoBodega(int id)
        {
            var tipoBodega = await _tipoBodegaService.GetTipoBodega(id);

            if (tipoBodega == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idTipoBodega = tipoBodega.IdTipoBodega,
                nombre = tipoBodega.NombreTipoBodega,
                estado = tipoBodega.Estado
            });

        }

        public async Task<IActionResult> GuardarTipoBodega(TipoBodegaVM modelo)
        {
            TipoBodega modelTipoBod = new TipoBodega()
            {
                IdTipoBodega = modelo.tipoBodega.IdTipoBodega,
                NombreTipoBodega = modelo.tipoBodega.NombreTipoBodega,
                Estado = 0,
            };

            if (modelTipoBod.IdTipoBodega == 0)
            {
                var result = await _tipoBodegaService.SaveTipoBodega(modelTipoBod);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "TipoBodega");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelTipoBod);
                }
            }
            else
            {
                var result = await _tipoBodegaService.UpdateTipoBodega(modelTipoBod);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "TipoBodega");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelTipoBod);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _tipoBodegaService.DeleteTipoBodega(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "TipoBodega");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
