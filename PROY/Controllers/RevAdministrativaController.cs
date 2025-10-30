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
    public class RevAdministrativaController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ICatalogoService _catalogoService;
        private readonly IRevAdministrativaService _revAdministrativaService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RevAdministrativaController(ISolicitudService solicitudService, ICatalogoService catalogoService, IRevAdministrativaService evaluacionService, IWebHostEnvironment webHostEnvironment)
        {
            _solicitudService = solicitudService;
            _catalogoService = catalogoService;
            _revAdministrativaService = evaluacionService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            string? departamento = User.FindFirst("depto")?.Value;
            string? rol = User.FindFirst(ClaimTypes.Role)?.Value;

            if (rol != "Admin" && (departamento == "DAAN" || string.IsNullOrEmpty(departamento)))
                return Unauthorized();

            var intervenciones = new List<Catalogo>();
            var intervencionDict = new Dictionary<int, string>();
            intervenciones = await _catalogoService.GetCatalogo("INTERVENCIONES");
            ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
            intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            // Crear un diccionario para búsqueda rápida en la tabla
            var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            var anulaciones = await _catalogoService.GetCatalogo("ANULACION");
            //ViewBag.Anulaciones = new SelectList(anulaciones, "IdCatalogo", "Descripcion");
            var anulacionDict = anulaciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var validaciones = await _catalogoService.GetCatalogo("VALIDACION");
            ViewBag.Validaciones = new SelectList(validaciones, "IdCatalogo", "Descripcion");
            var validacionDict = validaciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var instituciones = await _catalogoService.GetCatalogo("INSTITUCION");
            ViewBag.Instituciones = new SelectList(instituciones, "IdCatalogo", "Descripcion");
            var institucionDict = instituciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var proyectos = await _catalogoService.GetCatalogo("PROYECTO");
            ViewBag.Proyectos = new SelectList(proyectos, "IdCatalogo", "Descripcion");
            var proyectoDict = proyectos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            SolicitudesVM solicitudVM = new SolicitudesVM()
            {
                Solicitudes = await _revAdministrativaService.ObtenerRevAdministrativas(2),
                //InstitucionDict = institucionDict,
                UnidadDict = unidadDict,
                IntervencionDict = intervencionDict,
                //InstitucionDict = institucionDict,
                ProyectoDict = proyectoDict,
                AnulacionDict = anulacionDict,
                DepartamentoDict = departamentoDict,
                MunicipioDict = municipioDict,
                ValidacionDict = validacionDict
            };

            return View(solicitudVM);
        }
    }
}
