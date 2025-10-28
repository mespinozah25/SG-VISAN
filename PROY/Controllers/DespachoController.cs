using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;

namespace PROY.Controllers
{
    public class DespachoController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ICatalogoService _catalogoService;
        private readonly IDespachoService _despachoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DespachoController(ISolicitudService solicitudService, ICatalogoService catalogoService, IDespachoService despachoService, IWebHostEnvironment webHostEnvironment)
        {
            _solicitudService = solicitudService;
            _catalogoService = catalogoService;
            _despachoService = despachoService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(Int64 idDespacho)
        {
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
            ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
            var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            // Crear un diccionario para búsqueda rápida en la tabla
            var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            var anulaciones = await _catalogoService.GetCatalogo("ANULACION");
            //ViewBag.Anulaciones = new SelectList(anulaciones, "IdCatalogo", "Descripcion");
            var anulacionDict = anulaciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            if (condicion == "1")
            {
                if (idDespacho == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                                                    //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _despachoService.GetAllDespachos(),
                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict
                    };

                    ViewBag.AbrirModalEditar = "new";
                    ViewBag.ModalTitle = "NUEVA SOLICITUD";
                    return View(solicitudVM);
                }
                else
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idDespacho),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _despachoService.GetAllDespachos(),
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                        AnulacionDict = anulacionDict
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD";
                    return View(solicitudVM);
                }
            }
            else if (condicion == "2")
            {
                if (idDespacho == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitudes = await _despachoService.GetAllDespachosAPA(),
                        //InstitucionDict = institucionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict
                    };

                    ViewBag.AbrirModalEditar = "new";
                    ViewBag.ModalTitle = "NUEVA SOLICITUD";
                    return View(solicitudVM);
                }
                else
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idDespacho),
                        Solicitudes = await _despachoService.GetAllDespachosAPA(),
                        //InstitucionDict = institucionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD";
                    return View(solicitudVM);
                }
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
