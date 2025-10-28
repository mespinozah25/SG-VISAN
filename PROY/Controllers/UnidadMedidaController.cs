using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class UnidadMedidaController : Controller
    {
        private readonly IUnidadMedidaService _unidadmedidaService;

        public UnidadMedidaController(IUnidadMedidaService unidadmedidaService)
        {
            _unidadmedidaService = unidadmedidaService;
        }


        public async Task<IActionResult> Index(int? idUnidadMedida)
        {
            List<UnidadMedida> listado = await _unidadmedidaService.GetAll();

            if (idUnidadMedida == null || idUnidadMedida == 0)
            {
                var unidadmedidaVM = new UnidadMedidaVM()
                {
                    UnidadMedida = new UnidadMedida(),
                    listaUnidadMedida = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA UNIDAD DE MEDIDA";
                return View(unidadmedidaVM);
            }
            else
            {
                var unidadmedidaVM = new UnidadMedidaVM()
                {
                    UnidadMedida = await _unidadmedidaService.GetUnidadMedida((int)idUnidadMedida),
                    listaUnidadMedida = await _unidadmedidaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR UNIDAD DE MEDIDA ";
                return View(unidadmedidaVM);
            }
        }


        public async Task<IActionResult> ObtenerUnidadMedida(int id)
        {
            var unidadmedida = await _unidadmedidaService.GetUnidadMedida(id);

            if (unidadmedida == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idUnidadMedida = unidadmedida.IdUnidadMedida,
                nombre = unidadmedida.NombreUnidadMedida,
                umplural = unidadmedida.UMPlural,
                abreviatura = unidadmedida.Abreviatura,
                estado = unidadmedida.Estado
            });

        }

        public async Task<IActionResult> GuardarUnidadMedida(UnidadMedidaVM modelo)
        {
            UnidadMedida modelUnidadMedida = new UnidadMedida()
            {
                IdUnidadMedida = modelo.UnidadMedida.IdUnidadMedida,
                NombreUnidadMedida = modelo.UnidadMedida.NombreUnidadMedida,
                UMPlural = modelo.UnidadMedida.UMPlural,
                Abreviatura = modelo.UnidadMedida.Abreviatura,
                Estado = 0,
            };

            if (modelUnidadMedida.IdUnidadMedida == 0)
            {
                var result = await _unidadmedidaService.SaveUnidadMedida(modelUnidadMedida);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "UnidadMedida");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelUnidadMedida);
                }
            }
            else
            {
                var result = await _unidadmedidaService.UpdateUnidadMedida(modelUnidadMedida);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "UnidadMedida");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelUnidadMedida);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _unidadmedidaService.DeleteUnidadMedida(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "UnidadMedida");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
