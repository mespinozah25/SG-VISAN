using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;

namespace PROY.Controllers
{
    [Authorize]
    public class SolicitudV2Controller : Controller
    {
        private readonly ICatalogoService _catalogoService;
        private readonly ISolicitudV2Service _solicitudV2Service;
        private readonly IEstadoSolicitudService _estadoSolicitudService;
        private readonly IAsignaRacionService _asignaRacionService;

        public SolicitudV2Controller(ICatalogoService catalogoService, ISolicitudV2Service solicitudV2Service, IEstadoSolicitudService estadoSolicitudService, IAsignaRacionService AsignaRacionService)
        {
            _catalogoService = catalogoService;
            _solicitudV2Service = solicitudV2Service;
            _estadoSolicitudService = estadoSolicitudService;
            _asignaRacionService = AsignaRacionService;
        }

        public async Task<IActionResult> Index(int? IdEstado, int Rechazada = 0)
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();

            SolicitudesVM solicitudVM = new SolicitudesVM();

            if (IdEstado != null) solicitudVM.Solicitudes = await _solicitudV2Service.ObtenerSolicitudesSegunEstado((int)IdEstado, 2, Rechazada);
            else solicitudVM.Solicitudes = new List<Solicitud>();

            solicitudVM.IntervencionDict = (await _catalogoService.GetCatalogo("INTERVENCIONES"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            solicitudVM.ProyectoDict = (await _catalogoService.GetCatalogo("PROYECTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            solicitudVM.EstadosSolicitud = await _estadoSolicitudService.ObtenerTodos();

            ViewData["IdEstado"] = IdEstado;
            ViewData["Rechazada"] = Rechazada;

            return View(solicitudVM);
        }

        public async Task<IActionResult> DetalleSolicitud(int IdSolicitud)
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();

            SolicitudesVM solicitudVM = new SolicitudesVM();

            solicitudVM.Solicitud = await _solicitudV2Service.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

            if (solicitudVM.Solicitud == null)
                return Unauthorized();

            solicitudVM.IntervencionDict = (await _catalogoService.GetCatalogo("INTERVENCIONES"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.ProyectoDict = (await _catalogoService.GetCatalogo("PROYECTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.DepartamentoDict = (await _catalogoService.GetCatalogo("DEPARTAMENTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.MunicipioDict = (await _catalogoService.GetCatalogo("MUNICIPIO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            return View(solicitudVM);
        }

        public async Task<ActionResult> CambioEtapaSolicitud(int IdSolicitud)
        {
            TempData["Mensaje"] = await _solicitudV2Service.AvanzarASiguienteEstapa(IdSolicitud); ;

            return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { IdSolicitud });
        }

        public async Task<IActionResult> FormularioSolicitud(int? IdSolicitud)
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();

            SolicitudesVM solicitudVM = new SolicitudesVM();

            solicitudVM.Solicitud = new Solicitud();

            if (IdSolicitud != null) solicitudVM.Solicitud = await _solicitudV2Service.ObtenerSolicitudSegunIdSolicitud((int)IdSolicitud);

            solicitudVM.IntervencionDict = (await _catalogoService.GetCatalogo("INTERVENCIONES"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.ProyectoDict = (await _catalogoService.GetCatalogo("PROYECTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.DepartamentoDict = (await _catalogoService.GetCatalogo("DEPARTAMENTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.MunicipioDict = (await _catalogoService.GetCatalogo("MUNICIPIO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            ViewBag.Departamentos = new SelectList(await _catalogoService.GetCatalogo("DEPARTAMENTO"), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(await _catalogoService.GetCatalogo("MUNICIPIO"), "IdCatalogo", "Descripcion");
            ViewBag.Proyectos = new SelectList(await _catalogoService.GetCatalogo("PROYECTO"), "IdCatalogo", "Descripcion");
            ViewBag.Intervenciones = new SelectList(await _catalogoService.GetCatalogo("INTERVENCIONES"), "IdCatalogo", "Descripcion");

            return View(solicitudVM);
        }

        public async Task<ActionResult> EliminarSolicitud(int IdSolicitud)
        {
            TempData["Mensaje"] = await _solicitudV2Service.EliminarSolicitud(IdSolicitud); ;

            return RedirectToAction("Index", "SolicitudV2");
        }

        public async Task<ActionResult> RechazarSolicitud(int IdSolicitud)
        {
            TempData["Mensaje"] = await _solicitudV2Service.RechazarSolicitud(IdSolicitud); ;

            return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { IdSolicitud });
        }

        public async Task<ActionResult> HabilitarSolicitud(int IdSolicitud)
        {
            TempData["Mensaje"] = await _solicitudV2Service.HabilitarSolicitud(IdSolicitud); ;

            return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { IdSolicitud });
        }

        public async Task<IActionResult> FormularioDespacho(int IdSolicitud)
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();

            SolicitudesVM solicitudVM = new SolicitudesVM();

            solicitudVM.Solicitud = await _solicitudV2Service.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

            solicitudVM.DetalleRacionesV2 = await _asignaRacionService.ObtenerAsignacionesRacion(IdSolicitud);

            solicitudVM.AsignaRacionV2 = new AsignaRacionV2()
            {
                Comunidad = solicitudVM.Solicitud!.Comunidad!,
                UsuarioRegistro = User.FindFirst("Usuario")?.Value!
            };

            solicitudVM.DepartamentoDict = (await _catalogoService.GetCatalogo("DEPARTAMENTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.MunicipioDict = (await _catalogoService.GetCatalogo("MUNICIPIO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            ViewBag.Departamentos = new SelectList(await _catalogoService.GetCatalogo("DEPARTAMENTO"), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(await _catalogoService.GetCatalogo("MUNICIPIO"), "IdCatalogo", "Descripcion");

            return View(solicitudVM);
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAsignacionRacion(SolicitudesVM modeloVM)
        {
            int totalRaciones = modeloVM.Solicitud!.Raciones * ((int)modeloVM.Solicitud!.Validos! + (int)modeloVM.Solicitud!.NoAplica!);

            int totalAsignado = await _asignaRacionService.ObtenerTotalRacionesAsignadasSegunSolicitud((int)modeloVM.Solicitud!.IdSolicitud);

            int saldoRaciones = totalRaciones - (totalAsignado + modeloVM.AsignaRacionV2!.RacionAsignada!);

            if (saldoRaciones < 0)
            {
                TempData["Mensaje"] = "Ración asignada supera el total de raciones";

                return RedirectToAction("FormularioDespacho", "SolicitudV2", new { modeloVM.Solicitud!.IdSolicitud });
            }

            modeloVM.AsignaRacionV2!.IdSolicitud = (int)modeloVM.Solicitud!.IdSolicitud;
            modeloVM.AsignaRacionV2!.IdDepartamento = (int)modeloVM.Solicitud!.Departamento!;
            modeloVM.AsignaRacionV2!.IdMunicipio = (int)modeloVM.Solicitud!.Municipio!;
            modeloVM.AsignaRacionV2!.Comunidad = modeloVM.Solicitud!.Comunidad!;
            modeloVM.AsignaRacionV2!.RacionSaldo = saldoRaciones;

            TempData["Mensaje"] = await _asignaRacionService.GuardarAsignacionRacion(modeloVM.AsignaRacionV2!); ;

            return RedirectToAction("FormularioDespacho", "SolicitudV2", new { modeloVM.Solicitud!.IdSolicitud });
        }
        
        public async Task<IActionResult> EliminarAsignacionRacion(int IdSolicitud, int IdAsignaRacion)
        {
            TempData["Mensaje"] = await _asignaRacionService.EliminarAsignacionRacion(IdAsignaRacion); ;

            return RedirectToAction("FormularioDespacho", "SolicitudV2", new { IdSolicitud });
        }
    }
}