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
        private readonly IAsignaTecnicoService _asignaTecnicoService;
        private readonly ITecnicoService _tecnicoService;

        public SolicitudV2Controller(ICatalogoService catalogoService, ISolicitudV2Service solicitudV2Service, IEstadoSolicitudService estadoSolicitudService, IAsignaRacionService AsignaRacionService, IAsignaTecnicoService AsignaTecnicoService, ITecnicoService TecnicoService)
        {
            _catalogoService = catalogoService;
            _solicitudV2Service = solicitudV2Service;
            _estadoSolicitudService = estadoSolicitudService;
            _asignaRacionService = AsignaRacionService;
            _asignaTecnicoService = AsignaTecnicoService;
            _tecnicoService = TecnicoService;
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

            return View(solicitudVM);
        }

        public async Task<IActionResult> DetalleSolicitud(int IdSolicitud, int Rechazada = 0)
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

            ViewData["Rechazada"] = Rechazada;

            return View(solicitudVM);
        }

        public async Task<ActionResult> CambioEtapaSolicitud(int IdSolicitud)
        {
            TempData["Mensaje"] = await _solicitudV2Service.AvanzarASiguienteEtapa(IdSolicitud); ;

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

        [HttpPost]
        public async Task<ActionResult> RechazarSolicitudConComentario(SolicitudesVM modeloVM)
        {
            TempData["Mensaje"] = await _solicitudV2Service.RegresarAEtapaAnterior((int)modeloVM.Solicitud!.IdSolicitud, modeloVM.Solicitud!.ComentarioRechazo);

            return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { modeloVM.Solicitud!.IdSolicitud });
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

        public async Task<IActionResult> FormularioTecnicos(int IdSolicitud)
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();

            SolicitudesVM solicitudVM = new SolicitudesVM();

            solicitudVM.Solicitud = await _solicitudV2Service.ObtenerSolicitudSegunIdSolicitud(IdSolicitud);

            solicitudVM.ListDetalleTecnicos = await _asignaTecnicoService.ObtenerAsignacionesTecnico(IdSolicitud);

            solicitudVM.AsignaRacionV2 = new AsignaRacionV2()
            {
                Comunidad = solicitudVM.Solicitud!.Comunidad!,
                UsuarioRegistro = User.FindFirst("Usuario")?.Value!
            };

            solicitudVM.DepartamentoDict = (await _catalogoService.GetCatalogo("DEPARTAMENTO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.MunicipioDict = (await _catalogoService.GetCatalogo("MUNICIPIO"))
                .ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            solicitudVM.TecnicoDict = (await _tecnicoService.GetAll())
                .ToDictionary(i => (int)i.IdTecnico, i => i.Nombre);

            ViewBag.Departamentos = new SelectList(await _catalogoService.GetCatalogo("DEPARTAMENTO"), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(await _catalogoService.GetCatalogo("MUNICIPIO"), "IdCatalogo", "Descripcion");
            ViewBag.Tecnicos = new SelectList(await _tecnicoService.GetAll(), "IdTecnico", "Nombre");

            return View(solicitudVM);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarFechasEntregaSolicitud(SolicitudesVM modeloVM)
        {
            TempData["Mensaje"] = await _solicitudV2Service.ActualizarFechasEntregaSolicitud((int)modeloVM.Solicitud!.IdSolicitud, (DateTime)modeloVM.Solicitud!.FInicioEntrega!, (DateTime)modeloVM.Solicitud!.FFinEntrega!, modeloVM.Solicitud!.TecnicoObservaciones); ;

            return RedirectToAction("FormularioTecnicos", "SolicitudV2", new { modeloVM.Solicitud!.IdSolicitud, });
        }

        [HttpPost]
        public async Task<IActionResult> AgregarAsignacionTecnico(SolicitudesVM modeloVM)
        {
            modeloVM.AsignaTecnicoV2!.IdSolicitud = (int)modeloVM.Solicitud!.IdSolicitud;
            modeloVM.AsignaTecnicoV2!.IdDepartamento = (int)modeloVM.Solicitud!.Departamento!;
            modeloVM.AsignaTecnicoV2!.IdMunicipio = (int)modeloVM.Solicitud!.Municipio!;

            TempData["Mensaje"] = await _asignaTecnicoService.GuardarAsignacionTecnico(modeloVM.AsignaTecnicoV2!); ;

            return RedirectToAction("FormularioTecnicos", "SolicitudV2", new { modeloVM.Solicitud!.IdSolicitud });
        }

        public async Task<IActionResult> EliminarAsignacionTecnico(int IdSolicitud, int IdAsignaTecnico)
        {
            TempData["Mensaje"] = await _asignaTecnicoService.EliminarAsignacionTecnico(IdAsignaTecnico); ;

            return RedirectToAction("FormularioTecnicos", "SolicitudV2", new { IdSolicitud });
        }


        [HttpPost]
        public async Task<JsonResult> ActualizarRaciones([FromBody] ActualizarRacionesModel model)
        {
            try
            {
                string? departamento = User.FindFirst("depto")?.Value;
                string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

                if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                    return Json(new { success = false, message = "No autorizado" });

                // Validar que el estado sea VALIDACION_DE_BENEFICIARIOS
                var solicitud = await _solicitudV2Service.ObtenerSolicitudSegunIdSolicitud(model.IdSolicitud);

                if (solicitud == null)
                    return Json(new { success = false, message = "Solicitud no encontrada" });

                // Actualizar las raciones
                var resultado = await _solicitudV2Service.ActualizarRaciones(model.IdSolicitud, model.Raciones);

                return Json(new { success = true, message = resultado });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // Modelo para recibir los datos
        public class ActualizarRacionesModel
        {
            public int IdSolicitud { get; set; }
            public int Raciones { get; set; }
        }
    }
}