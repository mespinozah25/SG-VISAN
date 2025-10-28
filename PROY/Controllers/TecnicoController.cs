using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class TecnicoController : Controller
    {
        private readonly ITecnicoService _tecnicoService;

        public TecnicoController(ITecnicoService tecnicoService)
        {
            _tecnicoService= tecnicoService;
        }
        public async Task<IActionResult> Index(int ? idTecnico)
        {
            List<Tecnico> listado = await _tecnicoService.GetAll();

            if (idTecnico == null || idTecnico == 0)
            {
                var tecnicoVM = new TecnicoVM()
                {
                    Tecnico = new Tecnico(),                 
                    listaTecnicos = await _tecnicoService.GetAll(),                    
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA SOLICITUD";
                return View(tecnicoVM);
            }
            else
            {
                var tecnicoVM = new TecnicoVM()
                {
                    //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                    Tecnico = await _tecnicoService.GetTecnico((int)idTecnico),
                    listaTecnicos = await _tecnicoService.GetAll(),

                    //IntervencionDict = intervencionDict,
                };

                //solicitud = await _solicitudService.GetSolicitud((long)id);
                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR TECNICO "; //+ solicitudVM.Solicitud.Codigo;
                return View(tecnicoVM);
            }
        }

        public async Task<IActionResult> ObtenerTecnico(int id)
        {
            var tecnico = await _tecnicoService.GetTecnico(id);

            if (tecnico == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idCatalago = tecnico.IdTecnico,
                nombre = tecnico.Nombre,
                telefono = tecnico.Telefono                
            });

        }
        public async Task<IActionResult> GuardarTecnico(TecnicoVM modelo)
        {
            //var roles = await _usuarioService.GetAllRoles();
            //ViewBag.Roles = new SelectList(roles, "RolId", "Nombre");

            Tecnico modelTecnico = new Tecnico()
            {
                IdTecnico = modelo.Tecnico.IdTecnico,
                Nombre = modelo.Tecnico.Nombre,
                Telefono = modelo.Tecnico.Telefono                
            };

            if (modelTecnico.IdTecnico == 0)
            {
                var result = await _tecnicoService.SaveTecnico(modelTecnico);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Tecnico");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelTecnico);
                }
            }
            else
            {
                var result = await _tecnicoService.UpdateTecnico(modelTecnico);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Tecnico");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelTecnico);
                }
            }

        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _tecnicoService.DeleteTecnico(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Tecnico");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
