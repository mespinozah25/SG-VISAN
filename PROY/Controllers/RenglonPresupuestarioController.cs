using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class RenglonPresupuestarioController : Controller
    {
        private readonly IRenglonPresupuestarioService _RenglonPresupuestarioService;

        public RenglonPresupuestarioController(IRenglonPresupuestarioService renglonPresupuestarioService)
        {
            _RenglonPresupuestarioService = renglonPresupuestarioService;
        }

        public async Task<IActionResult> Index(int? idRenglonPre)
        {
            List<RenglonPresupuestario> listado = await _RenglonPresupuestarioService.GetAll();

            #region Catálogos  

            #endregion

            if (idRenglonPre == null || idRenglonPre == 0)
            {
                var RenglonPreVM = new RenglonPresupuestarioVM()
                {
                    Renglon = new RenglonPresupuestario(),
                    listaRenglones = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO RENGLÓN PRESUPUESTARIO";
                return View(RenglonPreVM);
            }
            else
            {
                var RenglonPreVM = new RenglonPresupuestarioVM()
                {
                    Renglon = await _RenglonPresupuestarioService.GetRenglonPresupuestario((int)idRenglonPre),
                    listaRenglones = await _RenglonPresupuestarioService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR RENGLÓN PRESUPUESTARIO";
                return View(RenglonPreVM);
            }
        }

        public async Task<IActionResult> ObtenerRenglonPresupuestario(int id)
        {
            var renglonPre = await _RenglonPresupuestarioService.GetRenglonPresupuestario(id);

            if (renglonPre == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idRenglonPresupuestario = renglonPre.IdRenglonPresupuestario,
                nombre = renglonPre.Renglon,
                descripcion = renglonPre.Descripcion,
                estado = renglonPre.Estado
            });

        }

        public async Task<IActionResult> GuardarRenglonPresupuestario(RenglonPresupuestarioVM modelo)
        {
            RenglonPresupuestario modelRenglonPre = new RenglonPresupuestario()
            {
                IdRenglonPresupuestario = modelo.Renglon.IdRenglonPresupuestario,
                Renglon = modelo.Renglon.Renglon,
                Descripcion = modelo.Renglon.Descripcion,
                Estado = 0,
            };

            if (modelRenglonPre.IdRenglonPresupuestario == 0)
            {
                var result = await _RenglonPresupuestarioService.SaveRenglonPresupuestario(modelRenglonPre);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "RenglonPresupuestario");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelRenglonPre);
                }
            }
            else
            {
                var result = await _RenglonPresupuestarioService.UpdateRenglonPresupuestario(modelRenglonPre);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "RenglonPresupuestario");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelRenglonPre);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _RenglonPresupuestarioService.DeleteRenglonPresupuestario(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "RenglonPresupuestario");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
