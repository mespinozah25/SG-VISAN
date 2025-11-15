using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;

namespace PROY.Controllers
{
    [Authorize]
    public class EvaluacionController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ICatalogoService _catalogoService;
        private readonly IEvaluacionService _evaluacionService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EvaluacionController(ISolicitudService solicitudService, ICatalogoService catalogoService, IEvaluacionService evaluacionService, IWebHostEnvironment webHostEnvironment)
        {
            _solicitudService = solicitudService;
            _catalogoService = catalogoService;
            _evaluacionService = evaluacionService;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index(Int64 idEvaluacion)
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

            if (condicion == "1")
            {
                if (idEvaluacion == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                                                    //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _evaluacionService.GetAllEvaluaciones(),
                        SolicitudesRechazadas = await _evaluacionService.GetAllEvaluacionesRechazadas(),
                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                        InstitucionDict = institucionDict,
                        ValidacionDict = validacionDict
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _evaluacionService.GetAllEvaluaciones(),
                        SolicitudesRechazadas = await _evaluacionService.GetAllEvaluacionesRechazadas(),
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                        AnulacionDict = anulacionDict,
                        InstitucionDict = institucionDict,
                        ValidacionDict = validacionDict
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD "; //+ solicitudVM.Solicitud.Codigo;
                    return View(solicitudVM);
                }
            }
            else if (condicion == "2")
            {
                if (idEvaluacion == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitudes = await _evaluacionService.GetAllEvaluacionesAPA(),
                        SolicitudesRechazadas = await _evaluacionService.GetAllEvaluacionesRechazadasAPA(),
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

                    ViewBag.AbrirModalEditar = "new";
                    ViewBag.ModalTitle = "NUEVA SOLICITUD";
                    return View(solicitudVM);
                }
                else
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        Solicitudes = await _evaluacionService.GetAllEvaluacionesAPA(),
                        SolicitudesRechazadas = await _evaluacionService.GetAllEvaluacionesRechazadasAPA(),
                        //InstitucionDict = institucionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                        //InstitucionDict = institucionDict
                        ProyectoDict = proyectoDict,
                        AnulacionDict = anulacionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        ValidacionDict = validacionDict
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD "; //+ solicitudVM.Solicitud.Codigo;
                    return View(solicitudVM);
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        public IActionResult PreviewFile(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "solicitudes", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Determinar el tipo MIME basado en la extensión del archivo
            string mimeType;
            var extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".jpg":
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".gif":
                    mimeType = "image/gif";
                    break;
                case ".bmp":
                    mimeType = "image/bmp";
                    break;
                default:
                    return BadRequest("Tipo de archivo no soportado.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, mimeType);
        }

        /*public IActionResult DownloadFile1(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Archivos", fileName);
            if (!_contentTypeProvider.TryGetContentType(fileName, out var mimeType))
            {
                mimeType = "application/octet-stream";
            }
            return PhysicalFile(filePath, mimeType, fileName);
        }*/

        public IActionResult DownloadFile(string fileName)
        {
            //var filePath = Path.Combine(Server.MapPath("~/Archivos"), fileName);
            //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "evaluaciones", fileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "evaluaciones", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);

            var mimeType = GetMimeType(fileName);
            return File(fileBytes, mimeType, fileName);
        }

        private string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".doc":
                case ".docx":
                    return "application/msword";
                case ".ppt":
                case ".pptx":
                    return "application/vnd.ms-powerpoint";
                default:
                    return "application/octet-stream";
            }
        }

        public async Task<IActionResult> IndexDetalle(Int64? idEvaluacion, Int64? idDetalleEvaluacion)
        {
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
            var varProceso = HttpContext?.Request.Query["proceso"].ToString();
            var condicion = "";

            ViewBag.RC = varRetorno;

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (idDetalleEvaluacion == null || idDetalleEvaluacion == 0)
            {
                /*var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");                
                var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);*/

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

                var validaciones = await _catalogoService.GetCatalogo("VALIDACION");
                ViewBag.Validaciones = new SelectList(validaciones, "IdCatalogo", "Descripcion");
                var validacionDict = validaciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

                var solicitudVM = new SolicitudesVM();

                if (condicion == "1")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),

                        //listaDetallesSolicitud = await _solicitudService.GetAllDetalleSolicitud((long)idSolicitudEvaluacion),
                        listaDetallesEvaluacion = await _evaluacionService.GetAllDetalleEvaluacion((long)idEvaluacion),
                        //DetalleSolicitud = new DetalleSolicitud(),
                        DetalleEvaluacion = new DetalleEvaluacion(),
                        //IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        ValidacionDict = validacionDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        listaDetallesEvaluacionAPA = await _evaluacionService.GetAllDetalleEvaluacionAPA((long)idEvaluacion),
                        DetalleEvaluacionAPA = new DetalleEvaluacionAPA(),
                        //IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        ValidacionDict = validacionDict
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
                /*var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
                // Crear un diccionario para búsqueda rápida
                var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);*/

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

                var validaciones = await _catalogoService.GetCatalogo("VALIDACION");
                ViewBag.Validaciones = new SelectList(validaciones, "IdCatalogo", "Descripcion");
                var validacionDict = validaciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

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
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        //DetalleSolicitud = await _solicitudService.GetDetalleSolicitud((long)idDetalleEvaluacion),
                        DetalleEvaluacion = await _evaluacionService.GetDetalleEvaluacionID((long)idDetalleEvaluacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        ///listaDetallesSolicitud = await _solicitudService.GetAllDetalleSolicitud((long)idSolicitudEvaluacion),
                        listaDetallesEvaluacion = await _evaluacionService.GetAllDetalleEvaluacion((long)idEvaluacion),
                        ///IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        ValidacionDict = validacionDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitudAPA = await _solicitudService.GetDetalleSolicitudAPA((long)idDetalleEvaluacion),
                        DetalleEvaluacionAPA = await _evaluacionService.GetDetalleEvaluacionIDAPA((long)idDetalleEvaluacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitudAPA = await _solicitudService.GetAllDetalleSolicitudAPA((long)idEvaluacion),
                        listaDetallesEvaluacionAPA = await _evaluacionService.GetAllDetalleEvaluacionAPA((long)idEvaluacion),
                        ///IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        ValidacionDict = validacionDict
                    };
                }

                //solicitud = await _solicitudService.GetSolicitud((long)id);
                ViewBag.AbrirModalEditar = "edit";
                return View(solicitudVM);
            }
        }

        public async Task<IActionResult> ExportarExcelDAAN(Int64? idSolicitud)
        {
            // Datos de ejemplo
            var datos = (await _solicitudService.GetAllDetalleSolicitud((long)idSolicitud))
                        .Where(d => d.IdAnulacion == 67 || d.IdAnulacion == 0 || d.IdAnulacion is null);

            // Configuración para EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var hoja = package.Workbook.Worksheets.Add("Reporte");

                // Encabezados                
                hoja.Cells[1, 1].Value = "Departamento";
                hoja.Cells[1, 2].Value = "Municipio";
                hoja.Cells[1, 3].Value = "Comunidad";
                hoja.Cells[1, 4].Value = "Nombre1";
                hoja.Cells[1, 5].Value = "Nombre2";
                hoja.Cells[1, 6].Value = "Nombre3";
                hoja.Cells[1, 7].Value = "Apellido1";
                hoja.Cells[1, 8].Value = "Apellido2";
                hoja.Cells[1, 9].Value = "CUI";
                hoja.Cells[1, 10].Value = "Fecha Nacimiento";
                hoja.Cells[1, 11].Value = "Nombre1";
                hoja.Cells[1, 12].Value = "Nombre2";
                hoja.Cells[1, 13].Value = "Nombre3";
                hoja.Cells[1, 14].Value = "Apellido1";
                hoja.Cells[1, 15].Value = "Apellido2";
                hoja.Cells[1, 16].Value = "Apellido casada";
                hoja.Cells[1, 17].Value = "CUI";
                hoja.Cells[1, 18].Value = "Fecha Nacimiento";
                hoja.Cells[1, 19].Value = "Estado Civil";
                hoja.Cells[1, 20].Value = "Embarazada";
                hoja.Cells[1, 21].Value = "Niño(a) con DA";
                hoja.Cells[1, 22].Value = "Grupo Etnico";
                hoja.Cells[1, 23].Value = "Hombre";
                hoja.Cells[1, 24].Value = "Mujer";
                hoja.Cells[1, 25].Value = "Hombre";
                hoja.Cells[1, 26].Value = "Mujer";
                hoja.Cells[1, 27].Value = "Hombre";
                hoja.Cells[1, 28].Value = "Mujer";
                hoja.Cells[1, 29].Value = "Tipo Persona";
                hoja.Cells[1, 30].Value = "Area de Maiz";
                hoja.Cells[1, 31].Value = "Quintales de Maiz x Año";
                hoja.Cells[1, 32].Value = "Area de Frijol";
                hoja.Cells[1, 33].Value = "Quintales de Frijol x Año";
                hoja.Cells[1, 34].Value = "Tipo de Agricultura que práctica";
                hoja.Cells[1, 35].Value = "Nombre del COCODE";
                hoja.Cells[1, 36].Value = "Nombre del personal que registro la información";
                hoja.Cells[1, 37].Value = "Institución que representa";

                // Aplicar estilo al encabezado
                using (var rango = hoja.Cells[1, 1, 1, 37])
                {
                    rango.Style.Font.Bold = true;
                    rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rango.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Llenar datos
                int fila = 2;
                foreach (var ii in datos)
                {
                    var paramDepto = await _catalogoService.GetCatalogo(ii.IdDepartamento);
                    var paramMuni = await _catalogoService.GetCatalogo(ii.IdMunicipio);
                    var paramEstadoCivil = await _catalogoService.GetCatalogo(ii.EstadoCivil);
                    var paramGrupoEtnico = await _catalogoService.GetCatalogo(ii.GrupoEtnico);

                    hoja.Cells[fila, 1].Value = paramDepto.Descripcion;
                    hoja.Cells[fila, 2].Value = paramMuni.Descripcion;
                    hoja.Cells[fila, 3].Value = ii.Comunidad;
                    hoja.Cells[fila, 4].Value = ii.Nombre1Esposo;
                    hoja.Cells[fila, 5].Value = ii.Nombre2Esposo;
                    hoja.Cells[fila, 6].Value = ii.Nombre3Esposo;
                    hoja.Cells[fila, 7].Value = ii.Apellido1Esposo;
                    hoja.Cells[fila, 8].Value = ii.Apellido2Esposo;
                    hoja.Cells[fila, 9].Value = ii.CuiEsposo;
                    hoja.Cells[fila, 10].Value = ii.FNacimientoEsposo != null ? Convert.ToDateTime(ii.FNacimientoEsposo).Date.ToString("dd/MM/yyyy") : "";
                    hoja.Cells[fila, 11].Value = ii.Nombre1Esposa;
                    hoja.Cells[fila, 12].Value = ii.Nombre2Esposa;
                    hoja.Cells[fila, 13].Value = ii.Nombre3Esposa;
                    hoja.Cells[fila, 14].Value = ii.Apellido1Esposa;
                    hoja.Cells[fila, 15].Value = ii.Apellido2Esposa;
                    hoja.Cells[fila, 16].Value = ii.ApellidoCasada;
                    hoja.Cells[fila, 17].Value = ii.CuiEsposa;
                    hoja.Cells[fila, 18].Value = ii.FNacimientoEsposa != null ? Convert.ToDateTime(ii.FNacimientoEsposa).Date.ToString("dd/MM/yyyy") : "";
                    hoja.Cells[fila, 19].Value = paramEstadoCivil.Descripcion;
                    hoja.Cells[fila, 20].Value = ii.Embarazada;
                    hoja.Cells[fila, 21].Value = ii.NiñoDA;
                    hoja.Cells[fila, 22].Value = paramGrupoEtnico.Descripcion;
                    hoja.Cells[fila, 23].Value = ii.Hombre0a2Años;
                    hoja.Cells[fila, 24].Value = ii.Mujer0a2Años;
                    hoja.Cells[fila, 25].Value = ii.Hombre2a5Años;
                    hoja.Cells[fila, 26].Value = ii.Mujer2a5Años;
                    hoja.Cells[fila, 27].Value = ii.Hombre6a17Años;
                    hoja.Cells[fila, 28].Value = ii.Mujer6a17Años;
                    hoja.Cells[fila, 29].Value = ii.TipoPersona;
                    hoja.Cells[fila, 30].Value = ii.AreaMaiz;
                    hoja.Cells[fila, 31].Value = ii.QuintalesMaiz;
                    hoja.Cells[fila, 32].Value = ii.AreaFrijol;
                    hoja.Cells[fila, 33].Value = ii.QuintalesFrijol;
                    hoja.Cells[fila, 34].Value = ii.TipoAgriculturaPractica;
                    hoja.Cells[fila, 35].Value = ii.NombreCOCODE;
                    hoja.Cells[fila, 36].Value = ii.NombrePersonaRegistro;
                    hoja.Cells[fila, 37].Value = ii.InstitucionRepresenta;

                    fila++;
                }

                // Autoajustar columnas
                hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

                // Generar archivo Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Descargar archivo
                var nombreArchivo = "Reporte.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }

        public async Task<IActionResult> ExportarExcelDAPA(Int64? idSolicitud)
        {
            // Datos de ejemplo
            var datos = (await _solicitudService.GetAllDetalleSolicitudAPA((long)idSolicitud))
                        .Where(d => d.IdAnulacion == 67 || d.IdAnulacion == 0 || d.IdAnulacion is null);

            // Configuración para EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var hoja = package.Workbook.Worksheets.Add("Reporte");

                // Encabezados                                
                hoja.Cells[1, 1].Value = "Nombre1";
                hoja.Cells[1, 2].Value = "Nombre2";
                hoja.Cells[1, 3].Value = "Nombre3";
                hoja.Cells[1, 4].Value = "Apellido1";
                hoja.Cells[1, 5].Value = "Apellido2";
                hoja.Cells[1, 6].Value = "Apellido casada";
                hoja.Cells[1, 7].Value = "CUI";
                hoja.Cells[1, 8].Value = "Género";
                hoja.Cells[1, 9].Value = "Grupo Etnico";

                // Aplicar estilo al encabezado
                using (var rango = hoja.Cells[1, 1, 1, 9])
                {
                    rango.Style.Font.Bold = true;
                    rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rango.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    rango.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                // Llenar datos
                int fila = 2;
                foreach (var ii in datos)
                {
                    //var paramDepto = await _catalogoService.GetCatalogo(ii.IdDepartamento);
                    //var paramMuni = await _catalogoService.GetCatalogo(ii.IdMunicipio);
                    //var paramEstadoCivil = await _catalogoService.GetCatalogo(ii.EstadoCivil);
                    var paramGrupoEtnico = await _catalogoService.GetCatalogo(ii.GrupoEtnico);

                    hoja.Cells[fila, 1].Value = ii.Nombre1Esposa;
                    hoja.Cells[fila, 2].Value = ii.Nombre2Esposa;
                    hoja.Cells[fila, 3].Value = ii.Nombre3Esposa;
                    hoja.Cells[fila, 4].Value = ii.Apellido1Esposa;
                    hoja.Cells[fila, 5].Value = ii.Apellido2Esposa;
                    hoja.Cells[fila, 6].Value = ii.ApellidoCasada;
                    hoja.Cells[fila, 7].Value = ii.CuiEsposa;
                    hoja.Cells[fila, 8].Value = "";
                    hoja.Cells[fila, 9].Value = paramGrupoEtnico.Descripcion;

                    fila++;
                }

                // Autoajustar columnas
                hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

                // Generar archivo Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Descargar archivo
                var nombreArchivo = "Reporte.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarDetalleEvaluacion(SolicitudesVM modeloVM, Int64 idEvaluacion, Int64 idDetalleEvaluacion, IFormFile file)
        {
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
            var varProceso = HttpContext?.Request.Query["proceso"].ToString();

            var condicion = "";

            ViewBag.RC = varRetorno;

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            //var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            //ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            DetalleEvaluacion modelo1 = new DetalleEvaluacion();
            DetalleEvaluacionAPA modelo2 = new DetalleEvaluacionAPA();

            if (condicion == "1")
            {
                modelo1 = new DetalleEvaluacion()
                {
                    IdSolicitud = idEvaluacion, //modeloVM.DetalleSolicitud.IdSolicitud, //Convert.ToInt64(ViewBag.solID.ToString()),
                    IdDetalleEvaluacion = idDetalleEvaluacion, //modeloVM.DetalleSolicitud.IdDetalleSolicitud,
                    //Descripcion = modeloVM.DetalleEvaluacion.Descripcion,
                    IdTipoValidacion = modeloVM.DetalleEvaluacion.IdTipoValidacion,
                    Observaciones = modeloVM.DetalleEvaluacion.Observaciones,
                    Archivo = modeloVM.DetalleEvaluacion.Archivo ?? string.Empty,
                    Restrictiva = modeloVM.DetalleEvaluacion.Restrictiva,
                    FechaRegistro = DateTime.Now.Date, //modeloVM.DetalleEvaluacion.FechaRegistro,
                    UsuarioRegistro = User.FindFirst("Usuario")?.Value //modeloVM.DetalleEvaluacion.UsuarioRegistro
                };
            }
            else if (condicion == "2")
            {
                modelo2 = new DetalleEvaluacionAPA()
                {
                    IdSolicitud = idEvaluacion, //modeloVM.DetalleSolicitud.IdSolicitud, //Convert.ToInt64(ViewBag.solID.ToString()),
                    IdDetalleEvaluacion = idDetalleEvaluacion, //modeloVM.DetalleSolicitud.IdDetalleSolicitud,
                    //Descripcion = modeloVM.DetalleEvaluacionAPA.Descripcion,
                    IdTipoValidacion = modeloVM.DetalleEvaluacionAPA.IdTipoValidacion,
                    Observaciones = modeloVM.DetalleEvaluacionAPA.Observaciones,
                    Archivo = modeloVM.DetalleEvaluacionAPA.Archivo ?? string.Empty,
                    Restrictiva = modeloVM.DetalleEvaluacionAPA.Restrictiva,
                    FechaRegistro = DateTime.Now.Date, //modeloVM.DetalleEvaluacion.FechaRegistro,
                    UsuarioRegistro = User.FindFirst("Usuario")?.Value //modeloVM.DetalleEvaluacion.UsuarioRegistro
                };
            }

            //if (ModelState.IsValid)
            if (condicion == "1")
            {
                if (modelo1.IdDetalleEvaluacion == 0)
                {
                    var result = await _evaluacionService.SaveDetalleEvaluacion(modelo1, varProceso);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------
                        if (file != null && file.Length > 0)
                        {
                            // Obtenemos la extensión del archivo
                            var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdDetalleEvaluacion}{extension}";

                            modelo1.IdDetalleEvaluacion = result.Data.IdDetalleEvaluacion;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            modelo1.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/evaluaciones", nuevoNombre);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            await _evaluacionService.UpdateDetalleEvaluacion(modelo1);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (varRetorno == "Programacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idEvaluacion, k = condicion });
                        }*/

                        TempData["Mensaje"] = result.Message;
                        //return View(modeloVM);
                        return RedirectToAction("IndexDetalle", "Evaluacion", new { idEvaluacion = idEvaluacion, k = condicion, returnController = varRetorno });
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Message;
                        return View(modelo1);
                    }

                }
                else
                {

                    var result = await _evaluacionService.UpdateDetalleEvaluacion(modelo1);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------                        

                        if (file != null && file.Length > 0)
                        {
                            //----------------------------Renombrar archivo viejo---------------------------------
                            var pathActual = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/evaluaciones");
                            string oldArchivo = "";
                            if (modelo1.Archivo != "")
                                oldArchivo = modelo1.Archivo.Insert(modelo1.Archivo.IndexOf("."), "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                            if (System.IO.File.Exists(pathActual + "/" + modelo1.Archivo))
                                System.IO.File.Move(pathActual + "/" + modelo1.Archivo, pathActual + "/" + oldArchivo);
                            //--------------------------------------------------------------------------------------

                            // Obtenemos la extensión del archivo
                            var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdDetalleEvaluacion}{extension}";

                            modelo1.IdDetalleEvaluacion = result.Data.IdDetalleEvaluacion;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            modelo1.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/evaluaciones", nuevoNombre);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            await _evaluacionService.UpdateDetalleEvaluacion(modelo1);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (varRetorno == "Programacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idEvaluacion, k = condicion });
                        }*/

                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("IndexDetalle", "Evaluacion", new { idEvaluacion = idEvaluacion, k = condicion, returnController = varRetorno });
                    }
                }
                return View(modelo1);
            }
            else if (condicion == "2")
            {
                if (modelo2.IdDetalleEvaluacion == 0)
                {
                    var result = await _evaluacionService.SaveDetalleEvaluacionAPA(modelo2, varProceso);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------
                        if (file != null && file.Length > 0)
                        {
                            // Obtenemos la extensión del archivo
                            var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdDetalleEvaluacion}{extension}";

                            modelo2.IdDetalleEvaluacion = result.Data.IdDetalleEvaluacion;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            modelo2.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/evaluaciones", nuevoNombre);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            await _evaluacionService.UpdateDetalleEvaluacionAPA(modelo2);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (varRetorno == "Programacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idEvaluacion, k = condicion });
                        }*/

                        TempData["Mensaje"] = result.Message;
                        //return View(modeloVM);
                        return RedirectToAction("IndexDetalle", "Evaluacion", new { idEvaluacion = idEvaluacion, k = condicion, returnController = varRetorno });
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Message;
                        return View(modelo2);
                    }

                }
                else
                {

                    var result = await _evaluacionService.UpdateDetalleEvaluacionAPA(modelo2);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------                        

                        if (file != null && file.Length > 0)
                        {
                            //----------------------------Renombrar archivo viejo---------------------------------
                            var pathActual = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/evaluaciones");
                            string oldArchivo = "";
                            if (modelo2.Archivo != "")
                                oldArchivo = modelo2.Archivo.Insert(modelo2.Archivo.IndexOf("."), "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                            if (System.IO.File.Exists(pathActual + "/" + modelo2.Archivo))
                                System.IO.File.Move(pathActual + "/" + modelo2.Archivo, pathActual + "/" + oldArchivo);
                            //--------------------------------------------------------------------------------------

                            // Obtenemos la extensión del archivo
                            var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdDetalleEvaluacion}{extension}";

                            modelo2.IdDetalleEvaluacion = result.Data.IdDetalleEvaluacion;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            modelo2.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/evaluaciones", nuevoNombre);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            await _evaluacionService.UpdateDetalleEvaluacionAPA(modelo2);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (varRetorno == "Programacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idEvaluacion, k = condicion });
                        }*/

                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("IndexDetalle", "Evaluacion", new { idEvaluacion = idEvaluacion, k = condicion, returnController = varRetorno });
                    }
                }
                return View(modelo2);
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> ObtenerDetalleEvaluacion(Int64 idDetalleEvaluacion)
        {
            var detalleEvaluacion = await _evaluacionService.GetDetalleEvaluacionID(idDetalleEvaluacion);

            if (detalleEvaluacion == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idDetalleEvaluacion = detalleEvaluacion.IdDetalleEvaluacion,
                //idIntervencion = detalleSolicitud.IdIntervencion, // Maneja nullables
                //descripcion = detalleEvaluacion.Descripcion, // Maneja nullables
                idTipoValidacion = detalleEvaluacion.IdTipoValidacion,
                observaciones = detalleEvaluacion.Observaciones, // Proporciona un valor por defecto si es null                
                Archivo = detalleEvaluacion.Archivo,
                fechaRegistro = detalleEvaluacion.FechaRegistro,
                usuarioRegistro = detalleEvaluacion.UsuarioRegistro
            });
        }

        public async Task<IActionResult> ObtenerDetalleEvaluacionAPA(Int64 idDetalleEvaluacion)
        {
            var detalleEvaluacion = await _evaluacionService.GetDetalleEvaluacionIDAPA(idDetalleEvaluacion);

            if (detalleEvaluacion == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idDetalleEvaluacion = detalleEvaluacion.IdDetalleEvaluacion,
                //idIntervencion = detalleSolicitud.IdIntervencion, // Maneja nullables
                //descripcion = detalleEvaluacion.Descripcion, // Maneja nullables
                idTipoValidacion = detalleEvaluacion.IdTipoValidacion,
                observaciones = detalleEvaluacion.Observaciones, // Proporciona un valor por defecto si es null                
                Archivo = detalleEvaluacion.Archivo,
                fechaRegistro = detalleEvaluacion.FechaRegistro,
                usuarioRegistro = detalleEvaluacion.UsuarioRegistro
            });
        }

        [HttpGet]
        public async Task<IActionResult> EliminarDetalleEvaluacion(Int64 idEvaluacion, Int64 idDetalleEvaluacion)
        {
            var result = await _evaluacionService.DeleteDetalleEvaluacion(idDetalleEvaluacion);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("IndexDetalle", "Evaluacion", new { idEvaluacion = idEvaluacion });
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("IndexDetalle", "Evaluacion");
            }
        }
        //Acceso al detalle desde Evaluacion de las solucitudes para evaluación
        public async Task<IActionResult> ListaDetalle(Int64? idEvaluacion, Int64? idDetalleEvaluacion)
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

            if (idDetalleEvaluacion == null || idDetalleEvaluacion == 0)
            {
                /*var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");                
                var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);*/


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
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        listaDetallesSolicitud = await _solicitudService.GetAllDetalleSolicitud((long)idEvaluacion),
                        DetalleSolicitud = new DetalleSolicitud(),
                        //IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        AnulacionDict = anulacionDict
                    };

                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        Solicitudes = await _solicitudService.GetAllSolicitudesAPA(),
                        listaDetallesSolicitudAPA = await _solicitudService.GetAllDetalleSolicitudAPA((long)idEvaluacion),
                        DetalleSolicitudAPA = new DetalleSolicitudAPA(),

                        //IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        AnulacionDict = anulacionDict
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
                /*var intervenciones = await _catalogoService.GetCatalogo("INTERVENCION");
                ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
                // Crear un diccionario para búsqueda rápida
                var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);*/


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
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitud = await _solicitudService.GetDetalleSolicitud((long)idDetalleEvaluacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitud = await _solicitudService.GetAllDetalleSolicitud((long)idEvaluacion),
                        ///IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        AnulacionDict = anulacionDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitudAPA = await _solicitudService.GetDetalleSolicitudAPA((long)idDetalleEvaluacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitudAPA = await _solicitudService.GetAllDetalleSolicitudAPA((long)idEvaluacion),
                        ///IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict,
                        AnulacionDict = anulacionDict
                    };
                }

                //solicitud = await _solicitudService.GetSolicitud((long)id);
                ViewBag.AbrirModalEditar = "edit";

                return View(solicitudVM);
            }


        }

        public async Task<ActionResult> EnviarEvaluacion(Int64? idEvaluacion)
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

            Solicitud solicitud = new Solicitud();
            solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion);
            var result = await _evaluacionService.UpdateEstado(solicitud);

            if (result.Success && result.Data != null)
            {
                if (varRetorno == "Programacion")
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Programacion", new { idSolicitud = idEvaluacion, k = condicion });
                }

                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Evaluacion", new { k = condicion });
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(solicitud);
            }
        }

        public async Task<ActionResult> EnviarRechazado(Int64? idEvaluacion)
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

            Solicitud solicitud = new Solicitud();
            solicitud = await _solicitudService.GetSolicitud((long)idEvaluacion);
            var result = await _evaluacionService.UpdateRechazada(solicitud);

            if (result.Success && result.Data != null)
            {
                /*if (varRetorno == "Evaluacion")
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Evaluacion", new { idSolicitud = idEvaluacion, k = condicion });
                }*/

                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Evaluacion", new { k = condicion });
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(solicitud);
            }

        }

        public async Task<IActionResult> EliminarDetalleSolicitud(Int64 idSolicitud, Int64 idDetalle, string condicion)
        {
            

            if (condicion == "1")
            {
                var result = await _solicitudService.DeleteDetalleSolicitud(idDetalle);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("ListaDetalle", "Evaluacion", new
                    {
                        idEvaluacion = idSolicitud,
                        k = condicion
                    });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Evaluacion");
                }
            }
            else if (condicion == "2")
            {
                var result = await _solicitudService.DeleteDetalleSolicitudAPA(idDetalle);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("ListaDetalle", "Evaluacion", new
                    {
                        idEvaluacion = idSolicitud,
                        k = condicion
                    });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Evaluacion");
                }
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }
        }

    }
}
