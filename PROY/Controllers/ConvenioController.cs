using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class ConvenioController : Controller
    {

        private readonly IConvenioService _convenioService;

        public ConvenioController(IConvenioService convenioService)
        {
            _convenioService = convenioService;
        }


        public async Task<IActionResult> Index(int? idConvenio)
        {
            List<Convenio> listado = await _convenioService.GetAll();

            if (idConvenio == null || idConvenio == 0)
            {
                var convenioVM = new ConvenioVM()
                {
                    Convenio = new Convenio(),
                    listaConvenios = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO CONVENIO";
                return View(convenioVM);
            }
            else
            {
                var convenioVM = new ConvenioVM()
                {
                    Convenio = await _convenioService.GetConvenio((int)idConvenio),
                    listaConvenios = await _convenioService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR CONVENIO ";
                return View(convenioVM);
            }
        }


        public async Task<IActionResult> ObtenerConvenio(int id)
        {
            var convenio = await _convenioService.GetConvenio(id);

            if (convenio == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idConvenio = convenio.IdConvenio,
                nombre = convenio.NombreConvenio,
                descripcion = convenio.Descripcion,
                vidaUtil = convenio.VidaUtil,
                totalRaciones = convenio.TotalRaciones,
                estado = convenio.Estado
            });

        }

        public async Task<IActionResult> GuardarConvenio(ConvenioVM modelo)
        {
            Convenio modelConvenio = new Convenio()
            {
                IdConvenio = modelo.Convenio.IdConvenio,
                NombreConvenio = modelo.Convenio.NombreConvenio,
                Descripcion = modelo.Convenio.Descripcion,
                VidaUtil = modelo.Convenio.VidaUtil,
                TotalRaciones = modelo.Convenio.TotalRaciones,
                Estado = 0,
            };

            if (modelConvenio.IdConvenio == 0)
            {
                var result = await _convenioService.SaveConvenio(modelConvenio);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Convenio");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelConvenio);
                }
            }
            else
            {
                var result = await _convenioService.UpdateConvenio(modelConvenio);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Convenio");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelConvenio);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _convenioService.DeleteConvenio(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Convenio");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
