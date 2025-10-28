using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class PresentacionPAController : Controller
    {
        private readonly IPresentacionPAService _presentacionpaService;

        public PresentacionPAController(IPresentacionPAService presentacionpaService)
        {
            _presentacionpaService = presentacionpaService;
        }


        public async Task<IActionResult> Index(int? idPresentacionPA)
        {
            List<PresentacionPA> listado = await _presentacionpaService.GetAll();

            if (idPresentacionPA == null || idPresentacionPA == 0)
            {
                var presentacionpaVM = new PresentacionPAVM()
                {
                    PresentacionPA = new PresentacionPA(),
                    listaPresentacionPA = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA PRESENTACIÓN";
                return View(presentacionpaVM);
            }
            else
            {
                var presentacionpaVM = new PresentacionPAVM()
                {
                    PresentacionPA = await _presentacionpaService.GetPresentacionPA((int)idPresentacionPA),
                    listaPresentacionPA = await _presentacionpaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PRESENTACIÓN ";
                return View(presentacionpaVM);
            }
        }


        public async Task<IActionResult> ObtenerPresentacionPA(int id)
        {
            var presentacionpa = await _presentacionpaService.GetPresentacionPA(id);

            if (presentacionpa == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idPresentacionPA = presentacionpa.IdPresentacionPA,
                nombre = presentacionpa.NombrePresentacionPA,
                estado = presentacionpa.Estado
            });

        }

        public async Task<IActionResult> GuardarPresentacionPA(PresentacionPAVM modelo)
        {
            PresentacionPA modelPresentacionPA = new PresentacionPA()
            {
                IdPresentacionPA = modelo.PresentacionPA.IdPresentacionPA,
                NombrePresentacionPA = modelo.PresentacionPA.NombrePresentacionPA,
                Estado = 0,
            };

            if (modelPresentacionPA.IdPresentacionPA == 0)
            {
                var result = await _presentacionpaService.SavePresentacionPA(modelPresentacionPA);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "PresentacionPA");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPresentacionPA);
                }
            }
            else
            {
                var result = await _presentacionpaService.UpdatePresentacionPA(modelPresentacionPA);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "PresentacionPA");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPresentacionPA);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _presentacionpaService.DeletePresentacionPA(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "PresentacionPA");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
