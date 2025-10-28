using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class ProyectoController : Controller
    {

        private readonly IProyectoService _proyectoService;

        public ProyectoController(IProyectoService proyectoService)
        {
            _proyectoService = proyectoService;
        }


        public async Task<IActionResult> Index(int? idProyecto)
        {
            List<Proyecto> listado = await _proyectoService.GetAll();

           

            if (idProyecto == null || idProyecto == 0)
            {
                var proyectoVM = new ProyectoVM()
                {
                    Proyecto = new Proyecto(),
                    listaProyecto = listado,
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PROYECTO";
                return View(proyectoVM);
            }
            else
            {
                var proyectoVM = new ProyectoVM()
                {
                    Proyecto = await _proyectoService.GetProyecto((int)idProyecto),
                    listaProyecto = await _proyectoService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PROYECTO ";
                return View(proyectoVM);
            }
        }


        public async Task<IActionResult> ObtenerProyecto(int id)
        {
            var proyecto = await _proyectoService.GetProyecto(id);

            if (proyecto == null)
            {
                return NotFound();
            }

            return Json(new
            {
                id = proyecto.IdProyecto,
                nombre = proyecto.NombreProyecto,
                anio = proyecto.Anio,
                estado = proyecto.Estado
            });

        }

        public async Task<IActionResult> GuardarProyecto(ProyectoVM modelo)
        {
            Proyecto modelProy = new Proyecto()
            {
                IdProyecto = modelo.Proyecto.IdProyecto,
                NombreProyecto = modelo.Proyecto.NombreProyecto,
                Anio = modelo.Proyecto.Anio,
                Estado = 0,
            };

            if (modelProy.IdProyecto == 0)
            {
                var result = await _proyectoService.SaveProyecto(modelProy);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Proyecto");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelProy);
                }
            }
            else
            {
                var result = await _proyectoService.UpdateProyecto(modelProy);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Proyecto");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelProy);
                }
            }
        }



        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _proyectoService.DeleteProyecto(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Proyecto");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }



    }
}
