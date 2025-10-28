using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class ProgramaController : Controller
    {

        private readonly IProgramaService _programaService;

        public ProgramaController(IProgramaService programaService)
        {
            _programaService = programaService;
        }


        public async Task<IActionResult> Index(int? idPrograma)
        {
            List<Programa> listado = await _programaService.GetAll();

            if (idPrograma == null || idPrograma == 0)
            {
                var programaVM = new ProgramaVM()
                {
                    Programa = new Programa(),
                    listaProgramas = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PROGRAMA";
                return View(programaVM);
            }
            else
            {
                var programaVM = new ProgramaVM()
                {
                    Programa = await _programaService.GetPrograma((int)idPrograma),
                    listaProgramas = await _programaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR BODEGA ";
                return View(programaVM);
            }
        }


        public async Task<IActionResult> ObtenerPrograma(int id)
        {
            var programa = await _programaService.GetPrograma(id);

            if (programa == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idPrograma = programa.IdPrograma,
                nombre = programa.NombrePrograma,
                estado = programa.Estado
            });

        }

        public async Task<IActionResult> GuardarPrograma(ProgramaVM modelo)
        {
            Programa modelPrograma = new Programa()
            {
                IdPrograma = modelo.Programa.IdPrograma,
                NombrePrograma = modelo.Programa.NombrePrograma,
                Estado = 0,
            };

            if (modelPrograma.IdPrograma == 0)
            {
                var result = await _programaService.SavePrograma(modelPrograma);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Programa");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPrograma);
                }
            }
            else
            {
                var result = await _programaService.UpdatePrograma(modelPrograma);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Programa");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPrograma);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _programaService.DeletePrograma(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Programa");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }


    }
}
