using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;

namespace PROY.Controllers
{
    [Authorize]
    public class RevAdministrativaController : Controller
    {
        private readonly ICatalogoService _catalogoService;
        private readonly IRevAdministrativaService _revAdministrativaService;

        public RevAdministrativaController(ICatalogoService catalogoService, IRevAdministrativaService evaluacionService)
        {
            _catalogoService = catalogoService;
            _revAdministrativaService = evaluacionService;
        }

        public async Task<IActionResult> Index()
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();
            
            Dictionary<int, string> intervencionDict = (await _catalogoService.GetCatalogo("INTERVENCIONES"))
                                                        .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            Dictionary<int, string> proyectoDict = (await _catalogoService.GetCatalogo("PROYECTO"))
                                                    .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            SolicitudesVM solicitudVM = new SolicitudesVM()
            {
                Solicitudes = await _revAdministrativaService.ObtenerRevAdministrativas(2),
                IntervencionDict = intervencionDict,
                ProyectoDict = proyectoDict,
            };

            return View(solicitudVM);
        }

        public async Task<ActionResult> EnviarSolicitud(int IdSolicitud)
        {
            ServiceResponse<Solicitud> serviceResponse = await _revAdministrativaService.AvanzarEtapa(IdSolicitud);

            TempData["Mensaje"] = serviceResponse.Message;

            return RedirectToAction("Index", "RevAdministrativa");
        }
    }
}
