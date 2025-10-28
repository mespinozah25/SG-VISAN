using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class PersonalBodegaController : Controller
    {
        private readonly IPersonalBodegaService _personalBodegaService;
        private readonly IPuestoService _puestoService;
        private readonly IRenglonPresupuestarioService _renglonPresupuestarioService;
        private readonly IBodegaService _bodegaService;
        private readonly IUsuarioService _usuarioService;

        public PersonalBodegaController(IPersonalBodegaService personalBodega, IPuestoService puestoService, IRenglonPresupuestarioService renglonPreService, IBodegaService bodegaService, IUsuarioService usuarioService)
        {
            _personalBodegaService = personalBodega;
            _puestoService = puestoService;
            _renglonPresupuestarioService = renglonPreService;
            _bodegaService = bodegaService;
            _usuarioService = usuarioService;
        }


        public async Task<IActionResult> Index(int? idPersonalBodega)
        {
            List<PersonalBodega> listado = await _personalBodegaService.GetAll();

            #region Catálogos

            var puestos = await _puestoService.GetAll();
            ViewBag.Puestos = new SelectList(puestos, "IdPuesto", "NombrePuesto");
            var puestosDict = puestos.ToDictionary(i => i.IdPuesto, i => i.NombrePuesto);

            var renglones = await _renglonPresupuestarioService.GetAll();
            ViewBag.RenglonesPresupuestarios = new SelectList(renglones, "IdRenglonPresupuestario", "Renglon");
            var renglonesPreDict = renglones.ToDictionary(i => i.IdRenglonPresupuestario, i => i.Renglon);

            var bodegas = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodegas, "IdBodega", "NombreBodega");
            var bodegasDict = bodegas.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var usuarios = await _usuarioService.GetAll();
            ViewBag.Usuarios = new SelectList(usuarios, "IdUsuario", "Nombre");
            var usuariosDict = usuarios.ToDictionary(i => i.IdUsuario, i => i.Nombre);

            #endregion

            if (idPersonalBodega == null || idPersonalBodega == 0)
            {
                var personalBodegaVM = new PersonalBodegaVM()
                {
                    PersonalBodega = new PersonalBodega(),
                    listaPersonalBodegas = listado,
                    PuestoDict = puestosDict,
                    RenglonPreDict = renglonesPreDict,
                    BodegaDict = bodegasDict,
                    UsuarioDict = usuariosDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PERSONAL DE BODEGA";
                return View(personalBodegaVM);
            }
            else
            {
                var personalBodegaVM = new PersonalBodegaVM()
                {
                    PersonalBodega = await _personalBodegaService.GetPersonalBodega((int)idPersonalBodega),
                    listaPersonalBodegas = await _personalBodegaService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PERSONAL DE BODEGA ";
                return View(personalBodegaVM);
            }
        }

        public async Task<IActionResult> ObtenerPersonalBodega(int id)
        {
            var personalBodega = await _personalBodegaService.GetPersonalBodega(id);

            if (personalBodega == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idPersonalBodega = personalBodega.IdPersonalBodega,
                dpi = personalBodega.Dpi,
                nit = personalBodega.Nit,
                nombres = personalBodega.Nombres,
                apellidos = personalBodega.Apellidos,
                idPuesto = personalBodega.IdPuesto,
                idRenglonPresupuestario = personalBodega.IdRenglonPresupuestario,
                email = personalBodega.Email,
                telefono = personalBodega.Telefono,
                idBodega = personalBodega.IdBodega,
                idUsuario = personalBodega.IdUsuario,
                estado = personalBodega.Estado
            });

        }

        public async Task<IActionResult> GuardarPersonalBodega(PersonalBodegaVM modelo)
        {
            PersonalBodega modelPerBod = new PersonalBodega()
            {
                IdPersonalBodega = modelo.PersonalBodega.IdPersonalBodega,
                Dpi = modelo.PersonalBodega.Dpi,
                Nit = modelo.PersonalBodega.Nit,
                Nombres = modelo.PersonalBodega.Nombres.Trim(),
                Apellidos = modelo.PersonalBodega.Apellidos.Trim(),
                IdPuesto = modelo.PersonalBodega.IdPuesto,
                IdRenglonPresupuestario = modelo.PersonalBodega.IdRenglonPresupuestario,
                Email = modelo.PersonalBodega.Email,
                Telefono = modelo.PersonalBodega.Telefono,
                IdBodega = modelo.PersonalBodega.IdBodega,
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                Estado = 0,
            };

            if (modelPerBod.IdPersonalBodega == 0)
            {
                var result = await _personalBodegaService.SavePersonalBodega(modelPerBod);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "PersonalBodega");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPerBod);
                }
            }
            else
            {
                var result = await _personalBodegaService.UpdatePersonalBodega(modelPerBod);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "PersonalBodega");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelPerBod);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _personalBodegaService.DeletePersonalBodega(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "PersonalBodega");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
