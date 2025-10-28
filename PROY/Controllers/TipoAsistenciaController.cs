using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class TipoAsistenciaController : Controller
    {
        private readonly ITipoAsistenciaService _tipoAsistenciaService;

        public TipoAsistenciaController(ITipoAsistenciaService tipoAsistenciaService)
        {
            _tipoAsistenciaService = tipoAsistenciaService;
        }


        public async Task<IActionResult> Index(int? idTipoAsistencia)
        {
            List<TipoAsistencia> listado = await _tipoAsistenciaService.GetAll();

            #region Catálogos  

            #endregion

            if (idTipoAsistencia == null || idTipoAsistencia == 0)
            {
                var tipoAsistenciaVM = new TipoAsistenciaVM()
                {
                    tipoAsistencia = new TipoAsistencia(),
                    listaTiposAsistencia = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO TIPO ASISTENCIA";
                return View(tipoAsistenciaVM);
            }
            else
            {
                var tipoAsistenciaVM = new TipoAsistenciaVM()
                {
                    tipoAsistencia = await _tipoAsistenciaService.GetTipoAsistencia((int)idTipoAsistencia),
                    listaTiposAsistencia = await _tipoAsistenciaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR TIPO ASISTENCIA ";
                return View(tipoAsistenciaVM);
            }
        }

        public async Task<IActionResult> ObtenerTipoAsistencia(int id)
        {
            var tipoAsistencia = await _tipoAsistenciaService.GetTipoAsistencia(id);

            if (tipoAsistencia == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idTipoAsistencia = tipoAsistencia.IdTipoAsistencia,
                nombreTipoBodega = tipoAsistencia.NombreTipoAsistencia,
                abreviatura = tipoAsistencia.Abreviatura,
                estado = tipoAsistencia.Estado
            });

        }

        public async Task<IActionResult> GuardarTipoAsistencia(TipoAsistenciaVM modelo)
        {
            TipoAsistencia modelTipoAsistencia = new TipoAsistencia()
            {
                IdTipoAsistencia = modelo.tipoAsistencia.IdTipoAsistencia,
                NombreTipoAsistencia = modelo.tipoAsistencia.NombreTipoAsistencia,
                Abreviatura = modelo.tipoAsistencia.Abreviatura,
                Estado = 0,
            };

            if (modelTipoAsistencia.IdTipoAsistencia == 0)
            {
                var result = await _tipoAsistenciaService.SaveTipoAsistencia(modelTipoAsistencia);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "TipoAsistencia");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelTipoAsistencia);
                }
            }
            else
            {
                var result = await _tipoAsistenciaService.UpdateTipoAsistencia(modelTipoAsistencia);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "TipoAsistencia");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelTipoAsistencia);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _tipoAsistenciaService.DeleteTipoAsistencia(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "TipoAsistencia");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }

    }
}
