using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;

namespace PROY.Controllers
{
    public class EjecucionController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ICatalogoService _catalogoService;
        private readonly IEjecucionService _ejecucionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProgramacionService _programacionService;

        public EjecucionController(ISolicitudService solicitudService, ICatalogoService catalogoService, IEjecucionService ejecucionService, IWebHostEnvironment webHostEnvironment, IProgramacionService programacionService)
        {
            _solicitudService = solicitudService;
            _catalogoService = catalogoService;
            _ejecucionService = ejecucionService;
            _webHostEnvironment = webHostEnvironment;
            _programacionService = programacionService;
        }
        public async Task<IActionResult> Index(Int64 idEjecucion)
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

            var intervenciones = new List<Catalogo>();
            var intervencionDict = new Dictionary<int, string>();
            if (condicion == "1")
            {
                intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
                intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            }
            else if (condicion == "2")
            {
                intervenciones = await _catalogoService.GetCatalogo("INTERVENCIONES");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
                intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            }

            var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            // Crear un diccionario para búsqueda rápida en la tabla
            var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            var anulaciones = await _catalogoService.GetCatalogo("ANULACION");
            //ViewBag.Anulaciones = new SelectList(anulaciones, "IdCatalogo", "Descripcion");
            var anulacionDict = anulaciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

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

            if (condicion == "1")
            {
                if (idEjecucion == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                                                    //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _ejecucionService.GetAllEjecuciones(),
                        Agrupados = new List<Repositories.EjecucionRepository.Agrupado>(),
                        
                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        InstitucionDict = institucionDict,
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict
                        
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idEjecucion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _ejecucionService.GetAllEjecuciones(),
                        Agrupados = await _ejecucionService.GetDetalleSolicitudAgrupado((long)idEjecucion),
                        DetalleRaciones = await _ejecucionService.GetDetalleRacionesSolicitud((long)idEjecucion),
                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        InstitucionDict = institucionDict,
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict
                        
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD";
                    return View(solicitudVM);
                }
            }
            else if (condicion == "2")
            {
                if (idEjecucion == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitudes = await _ejecucionService.GetAllEjecucionesAPA(),
                        Agrupados = new List<Repositories.EjecucionRepository.Agrupado>(),
                        
                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        //InstitucionDict = institucionDict,
                        ProyectoDict = proyectoDict,
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict
                        
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idEjecucion),
                        Solicitudes = await _ejecucionService.GetAllEjecucionesAPA(),
                        Agrupados = await _ejecucionService.GetDetalleSolicitudAgrupado((long)idEjecucion),
                        DetalleRaciones = await _ejecucionService.GetDetalleRacionesSolicitud((long)idEjecucion),
                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        //InstitucionDict = institucionDict,
                        ProyectoDict = proyectoDict,
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict
                        
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

        public async Task<IActionResult> IndexDetalle(Int64? idEjecucion, Int64? idDetalleEjecucion)
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

            if (idDetalleEjecucion == null || idDetalleEjecucion == 0)
            {
                var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
                var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);



                var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
                ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
                var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
                ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
                var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var estadocivil = await _catalogoService.GetCatalogo("ESTADO CIVIL");
                ViewBag.EstadoCivil = new SelectList(estadocivil, "IdCatalogo", "Descripcion");
                var estadocivilDict = estadocivil.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var grupoetnico = await _catalogoService.GetCatalogo("GRUPO ETNICO");
                ViewBag.GrupoEtnico = new SelectList(grupoetnico, "IdCatalogo", "Descripcion");
                var grupoetnicoDict = grupoetnico.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var anulacion = await _catalogoService.GetCatalogo("ANULACION");
                ViewBag.Anulacion = new SelectList(anulacion, "IdCatalogo", "Descripcion");
                var anulacionDict = anulacion.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var solicitudVM = new SolicitudesVM();

                if (condicion == "1")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idEjecucion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),

                        listaDetallesSolicitud = await _ejecucionService.GetAllDetalleEjecucion((long)idEjecucion),
                        //listaDetallesEvaluacion = await _evaluacionService.GetAllDetalleEvaluacion((long)idSolicitud),
                        DetalleSolicitud = new DetalleSolicitud(),
                        //DetalleEvaluacion = new DetalleEvaluacion(),
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idEjecucion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        listaDetallesSolicitudAPA = await _ejecucionService.GetAllDetalleEjecucionAPA((long)idEjecucion),
                        DetalleSolicitudAPA = new DetalleSolicitudAPA(),
                        //IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        AnulacionDict = anulacionDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict
                    };
                }


                ViewBag.AbrirModalEditar = "new";
                return View(solicitudVM);
            }
            else
            {

                /*var solicitudVM = new SolicitudesVM()
                {
                    //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                    Solicitud = await _solicitudService.GetSolicitud((long)id),
                    Solicitudes = await _solicitudService.GetAllSolicitudes()
                };*/

                // Obtener el catálogo de intervenciones
                var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
                // Crear un diccionario para búsqueda rápida
                var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);


                var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
                ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
                var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
                ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
                var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var estadocivil = await _catalogoService.GetCatalogo("ESTADO CIVIL");
                ViewBag.EstadoCivil = new SelectList(estadocivil, "IdCatalogo", "Descripcion");
                var estadocivilDict = estadocivil.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var grupoetnico = await _catalogoService.GetCatalogo("GRUPO ETNICO");
                ViewBag.GrupoEtnico = new SelectList(grupoetnico, "IdCatalogo", "Descripcion");
                var grupoetnicoDict = grupoetnico.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                // Pasar el listado y el diccionario a la vista
                /*var solicitudVM = new SolicitudesVM
                {
                    Solicitudes = listado,
                    IntervencionDict = intervencionDict
                };*/

                //ViewBag.solID = id;
                //TempData["idSol"] = id;

                var solicitudVM = new SolicitudesVM();

                if (condicion == "1")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idEjecucion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitud = await _solicitudService.GetDetalleSolicitud((long)idEjecucion),
                        //DetalleEvaluacion = await _evaluacionService.GetDetalleEvaluacionID((long)idDetalleEvaluacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitud = await _ejecucionService.GetAllDetalleEjecucion((long)idEjecucion),
                        //listaDetallesEvaluacion = await _evaluacionService.GetAllDetalleEvaluacion((long)idEvaluacion),
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idEjecucion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitudAPA = await _solicitudService.GetDetalleSolicitudAPA((long)idDetalleEjecucion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitudAPA = await _ejecucionService.GetAllDetalleEjecucionAPA((long)idEjecucion),
                        ///IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        IntervencionDict = intervencionDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict
                    };
                }

                //solicitud = await _solicitudService.GetSolicitud((long)id);
                ViewBag.AbrirModalEditar = "edit";

                return View(solicitudVM);
            }
        }

        public async Task<ActionResult> LiquidarBeneficiario(Int64? idDetalleEjecucion)
        {
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();

            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            DetalleSolicitud detSolicitud = new DetalleSolicitud();
            DetalleSolicitudAPA detSolicitudAPA = new DetalleSolicitudAPA();

            if (condicion == "1")
            {
                detSolicitud = await _ejecucionService.GetDetalleEjecucionID((long)idDetalleEjecucion);

                if (detSolicitud.Estado == "CREADO")
                    detSolicitud.Estado = "LIQUIDADO";
                else
                    detSolicitud.Estado = "CREADO";

                var result = await _ejecucionService.UpdateDetalleEjecucionID(detSolicitud);

                if (result.Success && result.Data != null)
                {
                    /*if (varRetorno == "Evaluacion")
                    {
                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("Index", "Evaluacion", new { idSolicitud = id, k = condicion });
                    }*/

                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Ejecucion", new { idEjecucion = detSolicitud.IdSolicitud, k = condicion, returnController = "Ejecucion" });
                    //return View(detSolicitud);
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(detSolicitud);
                }
            }
            else if (condicion == "2")
            {
                detSolicitudAPA = await _ejecucionService.GetDetalleEjecucionIDAPA((long)idDetalleEjecucion);

                if (detSolicitudAPA.Estado == "CREADO")
                    detSolicitudAPA.Estado = "LIQUIDADO";
                else
                    detSolicitudAPA.Estado = "CREADO";

                var result = await _ejecucionService.UpdateDetalleEjecucionIDAPA(detSolicitudAPA); //Agregar para dapa

                if (result.Success && result.Data != null)
                {
                    /*if (varRetorno == "Evaluacion")
                    {
                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("Index", "Evaluacion", new { idSolicitud = id, k = condicion });
                    }*/

                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Ejecucion", new { idEjecucion = detSolicitudAPA.IdSolicitud, k = condicion, returnController = "Ejecucion" });
                    //return View(detSolicitud);
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(detSolicitudAPA);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> getAgrupado(Int64 id)
        {
            SolicitudesVM modeloVM = new SolicitudesVM()
            {
                Solicitud = await _solicitudService.GetSolicitud((long)id),
                Agrupados = await _ejecucionService.GetDetalleSolicitudAgrupado((long)id),
                DetalleRaciones = await _ejecucionService.GetDetalleRacionesSolicitud((long)id)
            };            

            return Json(modeloVM);
        }

        [HttpPost]
        public async Task<IActionResult> AsignaRacion([FromBody] SolicitudesVM solicitudVM)
        {
            var depto = User.FindFirst("depto")?.Value;
            var idDepto = User.FindFirst("idDepto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            //var info = HttpContext?.Request.Query["data"];

            //var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            //Verificar DEPTO
            if (rol == "Admin")
            {
                if (valorK == "1")
                    idDepto = "1";
                else if (valorK == "2")
                    idDepto = "2";
            }

            /*SolicitudesVM modeloVM = new SolicitudesVM()
            {
                Solicitud = await _solicitudService.GetSolicitud((long)id),
                Agrupados = await _ejecucionService.GetDetalleSolicitudAgrupado((long)id),
                DetalleRaciones = await _ejecucionService.GetDetalleRacionesSolicitud((long)id)
            };*/
            //var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            //var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            //ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            //var instituciones = await _catalogoService.GetCatalogo("INSTITUCION");
            //var institucionDict = instituciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            AsignaRacion modelo = new AsignaRacion()
            {
                //IdAsignaTecnico = _solicitudService.cuenta,                
                SolicitudId = solicitudVM.AsignaRacion.SolicitudId,
                IdMunicipio = solicitudVM.AsignaRacion.IdMunicipio,
                RacionAsignada = solicitudVM.AsignaRacion.RacionAsignada,
                RacionSaldo = solicitudVM.AsignaRacion.RacionSaldo,
                Restrictiva = 0
            };

            var result = await _ejecucionService.SaveAsignaRacion(modelo);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Ejecucion", new { k = condicion });
            }

            return View(modelo);
        }

        public async Task<IActionResult> ObtenerDetalleRacionesSolicitud(Int64 id)
        {
            SolicitudesVM solicitudVM = new SolicitudesVM()
            {
                Solicitud = await _solicitudService.GetSolicitud((long)id),
                AsignaRacion = new AsignaRacion(),
                DetalleRaciones = await _ejecucionService.GetDetalleRacionesSolicitud((long)id)
            };

            /*var solicitudVM = new SolicitudesVM()
            {
                //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                Solicitud = await _solicitudService.GetSolicitud((long)id),
                //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                Solicitudes = await _ejecucionService.GetAllEjecuciones(),
                Agrupados = await _ejecucionService.GetDetalleSolicitudAgrupado((long)id),
                DetalleRaciones = await _ejecucionService.GetDetalleRacionesSolicitud((long)id),
                //UnidadDict = unidadDict,
                //IntervencionDict = intervencionDict,
                //AnulacionDict = anulacionDict
            };*/

            return Json(solicitudVM);
        }

        public async Task<IActionResult> cargarCombos(Int64 id)
        {
            var listado = _ejecucionService.GetComparar(id);

            return Json(listado);
        }

    }
}
