using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;

namespace PROY.Controllers
{
    public class ProgramacionController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ICatalogoService _catalogoService;
        private readonly IProgramacionService _programacionService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEvaluacionService _evaluacionService;

        public ProgramacionController(ISolicitudService solicitudService, ICatalogoService catalogoService, IProgramacionService programacionService, IWebHostEnvironment webHostEnvironment, IEvaluacionService evaluacionService)
        {
            _solicitudService = solicitudService;
            _catalogoService = catalogoService;
            _programacionService= programacionService;            
            _webHostEnvironment = webHostEnvironment;
            _evaluacionService = evaluacionService;
        }
        public async Task<IActionResult> Index(Int64 idProgramacion)
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

            //var compa = await _programacionService.GetComparar(37);
            var proyectos = await _catalogoService.GetCatalogo("PROYECTO");
            ViewBag.Proyectos = new SelectList(proyectos, "IdCatalogo", "Descripcion");
            var proyectoDict = proyectos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var tecnicos = await _solicitudService.GetAllTecnicos();
            ViewBag.Tecnicos = new SelectList(tecnicos, "ID_TECNICO", "NOMBRE");
            var tecnicoDict = tecnicos.ToDictionary(i => Convert.ToInt32(i.IdTecnico), i => i.Nombre);

            var comunidades = _solicitudService.GetComunidades(idProgramacion);
            ViewBag.Comunidades = new SelectList(comunidades, "Value", "Text");
            //var tecnicoDict = tecnicos.ToDictionary(i => Convert.ToInt32(i.IdTecnico), i => i.Nombre);

            if (condicion == "1")
            {
                if (idProgramacion == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                                                    //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _programacionService.GetAllProgramaciones(),

                        Tecnicos = await _solicitudService.GetAllTecnicos(),

                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        InstitucionDict = institucionDict,
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        TecnicoDict = tecnicoDict
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idProgramacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        Solicitudes = await _programacionService.GetAllProgramaciones(),

                        //DetalleRaciones = await _solicitudService.GetDetalleRacionesSolicitud((long)idProgramacion),

                        Tecnicos = await _solicitudService.GetAllTecnicos(),

                        UnidadDict = unidadDict,
                        InstitucionDict = institucionDict,
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        AnulacionDict = anulacionDict,
                        TecnicoDict=tecnicoDict
                        
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD";

                    comunidades = _solicitudService.GetComunidades(idProgramacion);
                    ViewBag.Comunidades = new SelectList(comunidades, "Value", "Text");

                    return View(solicitudVM);
                }
            }
            else if (condicion == "2")
            {
                if (idProgramacion == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitudes = await _programacionService.GetAllProgramacionesAPA(),

                        Tecnicos = await _solicitudService.GetAllTecnicos(),

                        AnulacionDict = anulacionDict,
                        UnidadDict = unidadDict,
                        //InstitucionDict = institucionDict,
                        IntervencionDict = intervencionDict,
                        ProyectoDict = proyectoDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        TecnicoDict = tecnicoDict
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idProgramacion),
                        Solicitudes = await _programacionService.GetAllProgramacionesAPA(),

                        Tecnicos = await _solicitudService.GetAllTecnicos(),

                        UnidadDict = unidadDict,
                        //InstitucionDict = institucionDict,
                        IntervencionDict = intervencionDict,
                        ProyectoDict = proyectoDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        AnulacionDict = anulacionDict,
                        TecnicoDict = tecnicoDict
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

        public IActionResult PreviewFile(string fileName)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "evaluaciones", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf");
        }
        public async Task<IActionResult> ListaDetalle(Int64? idProgramacion, Int64? idDetalleProgramacion)
        {
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
            
            ViewBag.RC = varRetorno;

            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (idDetalleProgramacion == null || idDetalleProgramacion == 0)
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idProgramacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),

                        listaDetallesSolicitud = await _programacionService.GetAllDetalleProgramacion((long)idProgramacion),
                        //listaDetallesEvaluacion = await _evaluacionService.GetAllDetalleEvaluacion((long)idSolicitud),
                        DetalleSolicitud = new DetalleSolicitud(),
                        //DetalleEvaluacion = new DetalleEvaluacion(),
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict=estadocivilDict,
                        GrupoEtnicoDict=grupoetnicoDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idProgramacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        listaDetallesSolicitudAPA = await _programacionService.GetAllDetalleProgramacionAPA((long)idProgramacion),
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idProgramacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitud = await _solicitudService.GetDetalleSolicitud((long)idProgramacion),
                        //DetalleEvaluacion = await _evaluacionService.GetDetalleEvaluacionID((long)idDetalleEvaluacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/ 
                        listaDetallesSolicitud = await _programacionService.GetAllDetalleProgramacion((long)idProgramacion),
                        //listaDetallesEvaluacion = await _evaluacionService.GetAllDetalleEvaluacion((long)idEvaluacion),
                        IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        EstadoCivilDict=estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict
                    };
                }
                else if (condicion == "2")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        //Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitud = await _solicitudService.GetSolicitud((long)idProgramacion),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitudAPA = await _solicitudService.GetDetalleSolicitudAPA((long)idDetalleProgramacion),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitudAPA = await _programacionService.GetAllDetalleProgramacionAPA((long)idProgramacion),
                        ///IntervencionDict = intervencionDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict,
                        //IntervencionDict = intervencionDict,
                        EstadoCivilDict = estadocivilDict,
                        GrupoEtnicoDict = grupoetnicoDict
                    };
                }

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";

                return View(solicitudVM);
            }
        }

        public async Task<IActionResult> ExportarExcelDAAN(Int64? idSolicitud)
        {
            try
            {
                var datos = (await _programacionService.GetAllDetalleProgramacion((long)idSolicitud))
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
            catch (Exception ex)
            {
                // Registrar error en el log del sistema
                Console.WriteLine($"Error al exportar a Excel: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno al generar el reporte.");
            }
            
        }

        public async Task<IActionResult> ExportarExcelDAPA(Int64? idSolicitud)
        {
            // Datos de ejemplo
            var datos = (await _programacionService.GetAllDetalleProgramacionAPA((long)idSolicitud))
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
                    hoja.Cells[fila, 8].Value = ii.Genero;
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

        public async Task<ActionResult> EnviarProgramacion(Int64? idProgramacion)
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
            solicitud = await _solicitudService.GetSolicitud((long)idProgramacion);
            

            Boolean resultado= await _solicitudService.GetTotalRacionesAsignadas((long)idProgramacion);

            if (resultado)
            {
                var result = await _programacionService.UpdateEstado(solicitud);

                if (result.Success && result.Data != null)
                {
                    if (varRetorno == "Programacion")
                    {
                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("Index", "Programacion", new { idSolicitud = idProgramacion, k = condicion });
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
            else
            {
                TempData["MensajeError"] = "No se han asignado la totalidad de las raciones";
                //return View(solicitud);
                //return BadRequest("....." + TempData["Mensaje"]);
                return RedirectToAction("Index", "Programacion", new { k = condicion });
            }
        }

        public async Task<Boolean> VerificarEvaluacion()
        {
            Boolean respuesta = false;

            //respuesta = await _evaluacionService.VerificarEvaluacion(DateTime.Now.Date, "PROGRAMACION");

            return respuesta;
        }
        public async Task<IActionResult> cargarCombos(Int64 id)
        {
            var listado = _programacionService.GetComparar(id);

            //var comunidades = _solicitudService.GetComunidades((long)id);
            //ViewBag.Comunidades = new SelectList(comunidades, "Value", "Text");

            return Json(listado);
        }

    }
}
