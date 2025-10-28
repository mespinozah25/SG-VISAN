using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class UsuarioPersonaController : Controller
    {
        private readonly IUsuarioPersonaService _usuarioPersonaService;
        private readonly IPersonalBodegaService _personalBodegaService;
        private readonly IUsuarioService _usuarioService;

        public UsuarioPersonaController(IUsuarioPersonaService usuarioPersona, IPersonalBodegaService personalBodegaService, IUsuarioService usuarioService)
        {
            _usuarioPersonaService = usuarioPersona;
            _personalBodegaService = personalBodegaService;
            _usuarioService = usuarioService;
        }


        public async Task<IActionResult> Index(int? idUsuarioPersona)
        {
            List<UsuarioPersona> listado = await _usuarioPersonaService.GetAll();

            #region Catálogos

            var personalBodega = await _personalBodegaService.GetAll();
            ViewBag.PersonalBodega = new SelectList(personalBodega, "IdPersonalBodega", "Nombres", "Apellidos");
            var PersonalBodegaDict = personalBodega.ToDictionary(i => i.IdPersonalBodega, i => $"{i.Nombres} {i.Apellidos}");

            var usuarios = await _usuarioService.GetAll();
            ViewBag.Usuarios = new SelectList(usuarios, "IdUsuario", "Correo");
            var usuariosDict = usuarios.ToDictionary(i => i.IdUsuario, i => i.Correo);

            #endregion

            if (idUsuarioPersona == null || idUsuarioPersona == 0)
            {
                var UsuarioPersonaVM = new UsuarioPersonaVM()
                {
                    UsuarioPersona = new UsuarioPersona(),
                    listaUsuarioPersona = listado,
                    PersonalBodegaDict = PersonalBodegaDict,
                    UsuarioDict = usuariosDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA RELACIÓN ENTRE USUARIO Y PERSONA";
                return View(UsuarioPersonaVM);
            }
            else
            {
                var usuarioPersonaVM = new UsuarioPersonaVM()
                {
                    UsuarioPersona = await _usuarioPersonaService.GetUsuarioPersona((int)idUsuarioPersona),
                    listaUsuarioPersona = await _usuarioPersonaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR LA RELACIÓN ENTRE USUARIO Y PERSONA";
                return View(usuarioPersonaVM);
            }
        }

        public async Task<IActionResult> ObtenerUsuarioPersona(int id)
        {
            var usuarioPersona = await _usuarioPersonaService.GetUsuarioPersona(id);

            if (usuarioPersona == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idUsuarioPersona = usuarioPersona.IdUsuarioPersona,
                idUsuario = usuarioPersona.IdUsuario,
                idPersona = usuarioPersona.IdPersona,
                estado = usuarioPersona.Estado
            });

        }

        public async Task<IActionResult> GuardarUsuarioPersona(UsuarioPersonaVM modelo)
        {
            UsuarioPersona modelUsuarioPersona = new UsuarioPersona()
            {
                IdUsuarioPersona = modelo.UsuarioPersona.IdUsuarioPersona,
                IdUsuario = modelo.UsuarioPersona.IdUsuario,
                IdPersona = modelo.UsuarioPersona.IdPersona,
                Estado = 0,
            };

            if (modelUsuarioPersona.IdUsuarioPersona == 0)
            {
                var result = await _usuarioPersonaService.SaveUsuarioPersona(modelUsuarioPersona);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "UsuarioPersona");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelUsuarioPersona);
                }
            }
            else
            {
                var result = await _usuarioPersonaService.UpdateUsuarioPersona(modelUsuarioPersona);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "UsuarioPersona");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelUsuarioPersona);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _usuarioPersonaService.DeleteUsuarioPersona(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "UsuarioPersona");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
