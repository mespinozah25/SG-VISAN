using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class PuestoController : Controller
    {
        private readonly IPuestoService _puestoService;

        public PuestoController(IPuestoService puestoService)
        {
            _puestoService = puestoService;
        }


        public async Task<IActionResult> Index(int? idPuesto)
        {
            List<Puesto> listado = await _puestoService.GetAll();

            #region Catálogos  

            #endregion

            if (idPuesto == null || idPuesto == 0)
            {
                var puestoVM = new PuestoVM()
                {
                    Puesto = new Puesto(),
                    listaPuestos = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PUESTO";
                return View(puestoVM);
            }
            else
            {
                var puestoVM = new PuestoVM()
                {
                    Puesto = await _puestoService.GetPuesto((int)idPuesto),
                    listaPuestos = await _puestoService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PUESTO";
                return View(puestoVM);
            }
        }

        public async Task<IActionResult> ObtenerPuesto(int id)
        {
            var puesto = await _puestoService.GetPuesto(id);

            if (puesto == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idPuesto = puesto.IdPuesto,
                nombre = puesto.NombrePuesto,
                estado = puesto.Estado
            });

        }

        public async Task<IActionResult> GuardarPuesto(PuestoVM modelo)
        {
            Puesto modelPuesto = new Puesto()
            {
                IdPuesto = modelo.Puesto.IdPuesto,
                NombrePuesto = modelo.Puesto.NombrePuesto,
                Estado = 0,
            };

            if (modelPuesto.IdPuesto == 0)
            {
                var result = await _puestoService.SavePuesto(modelPuesto);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Puesto");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPuesto);
                }
            }
            else
            {
                var result = await _puestoService.UpdatePuesto(modelPuesto);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Puesto");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPuesto);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _puestoService.DeletePuesto(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Puesto");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
