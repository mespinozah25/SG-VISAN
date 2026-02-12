using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PROY.Data;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ExcelRow = PROY.Models.ExcelRow;

namespace PROY.Controllers
{
    [Authorize]
    public class SolicitudController : Controller
    {
        private readonly ISolicitudService _solicitudService;
        private readonly ICatalogoService _catalogoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly ISolicitudV2Service _solicitudV2Service;

        //private readonly AppDBContext _dbContext;

        public SolicitudController(ISolicitudService solicitudService, ICatalogoService catalogoService, IWebHostEnvironment webHostEnvironment, ISolicitudV2Service solicitudV2Service)
        {
            _solicitudService = solicitudService;
            _catalogoService = catalogoService;
            _webHostEnvironment = webHostEnvironment;
            _solicitudV2Service = solicitudV2Service;
        }

        public void verificarDEPTO(string paramRol, ref string paramDepto, string paramK)
        {
            if (paramRol == "Admin")
            {
                if (paramK == "1")
                    paramDepto = "DAAN";
                else if (paramK == "2")
                    paramDepto = "DAPA";
            }
        }
        public async Task<IActionResult> Index(Int64? id)
        {
            /*List<Solicitud> listado = await _solicitudService.GetAllSolicitudes();

            return View(listado);*/

            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            //verificarDEPTO(rol, ref depto, valorK);
            //var resultado = depto;

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

            //var intervenciones2 = await _catalogoService.GetCatalogo("INTERVENCIONES");
            //ViewBag.Intervenciones2 = new SelectList(intervenciones2, "IdCatalogo", "Descripcion");
            //var intervencion2Dict = intervenciones2.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            ViewBag.Deptos = new SelectList(unidades, "IdCatalogo", "Descripcion");
            var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

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


            //-------------------------
            if (condicion == "1")
            {
                if (id == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitudes = await _solicitudService.GetAllSolicitudes(),

                        InstitucionDict = institucionDict,
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
                        Solicitud = await _solicitudService.GetSolicitud((long)id),
                        Solicitudes = await _solicitudService.GetAllSolicitudes(),

                        //DetalleRaciones = await _solicitudService.GetDetalleRacionesSolicitud((long)id),

                        InstitucionDict = institucionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD " + solicitudVM.Solicitud.Codigo;
                    return View(solicitudVM);
                }
            }
            else if (condicion == "2")
            {
                if (id == null)
                {
                    var solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = new Solicitud(),//await _solicitudService.GetSolicitud(1),
                        Solicitudes = await _solicitudService.GetAllSolicitudesAPA(),
                        //InstitucionDict = institucionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                        ProyectoDict = proyectoDict,
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
                        Solicitud = await _solicitudService.GetSolicitud((long)id),
                        Solicitudes = await _solicitudService.GetAllSolicitudesAPA(),
                        //InstitucionDict = institucionDict,
                        UnidadDict = unidadDict,
                        IntervencionDict = intervencionDict,
                        ProyectoDict = proyectoDict,
                        DepartamentoDict = departamentoDict,
                        MunicipioDict = municipioDict
                    };

                    //solicitud = await _solicitudService.GetSolicitud((long)id);
                    ViewBag.AbrirModalEditar = "edit";
                    ViewBag.ModalTitle = "MODIFICAR SOLICITUD " + solicitudVM.Solicitud.Codigo;
                    return View(solicitudVM);
                }
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }

            //-------------------------


            //return View(solicitudVM);
        }

        public IActionResult DownloadFile(string fileName)
        {
            //var filePath = Path.Combine(Server.MapPath("~/Archivos"), fileName);
            //var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "evaluaciones", fileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "solicitudes", fileName);

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

        //Le cambie el nombre del parametro de idSolicitud a id ya que siempre hiba 0
        public async Task<IActionResult> ObtenerSolicitud(Int64 id)
        {
            var solicitud = await _solicitudService.GetSolicitud(id);

            if (solicitud == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idSolicitud = solicitud.IdSolicitud,
                correlativo = solicitud.Correlativo,
                codigo = solicitud.Codigo,
                fechaDespacho = solicitud.FechaIngresoDespacho.ToString("yyyy-MM-dd"), // Maneja nullables
                fechaDireccion = solicitud.FechaIngresoDireccion.ToString("yyyy-MM-dd"),
                fechaUnidad = solicitud.FechaIngresoUnidad.ToString("yyyy-MM-dd"), // Maneja nullables
                idInstitucion = solicitud.IdInstitucion,
                //idUnidad = solicitud.IdUnidad,// Proporciona un valor por defecto si es null
                Depto = solicitud.Depto,
                idIntervencion = solicitud.IdIntervencion,
                Archivo = solicitud.Archivo,
                raciones = solicitud.Raciones,

                Fallecidos = solicitud.Fallecidos,
                Duplicados = solicitud.Duplicados,
                SinDatos = solicitud.SinDatos,
                Gobierno = solicitud.Gobierno,
                Descripcion = solicitud.Descripcion,

                //FInicioEntrega=solicitud.FInicioEntrega!=null ? solicitud.FInicioEntrega.ToString("yyyy-MM-dd") : null,
                FInicioEntrega = solicitud.FInicioEntrega.HasValue ? solicitud.FInicioEntrega.Value.ToString("yyyy-MM-dd") : null,
                //FFinEntrega =solicitud.FFinEntrega.ToString("yyyy-MM-dd"), 
                FFinEntrega = solicitud.FFinEntrega.HasValue ? solicitud.FFinEntrega.Value.ToString("yyyy-MM-dd") : null,
                //DireccionEntrega =solicitud.DireccionEntrega,
                //TecnicoNombre=solicitud.TecnicoNombre,
                //TecnicoTelefono=solicitud.TecnicoTelefono,
                TecnicoObservaciones = solicitud.TecnicoObservaciones,

                IdProyecto = solicitud.IdProyecto,
                Departamento = solicitud.Departamento,
                Municipio = solicitud.Municipio,
                Comunidad = solicitud.Comunidad,
                nombreCocode = solicitud.NombreCocode,
                comunidadCocode = solicitud.ComunidadCocode,
                telefonoCocode = solicitud.TelefonoCocode
            });

        }

        public async Task<IActionResult> VistaSolicitudes()
        {
            var solicitudVM = new SolicitudesVM
            {
                Solicitud = await _solicitudService.GetSolicitud(1),
                Solicitudes = await _solicitudService.GetAllSolicitudes()
            };

            return View(solicitudVM);
        }

        public JsonResult GetCat(string tipoCatalogo)
        {
            var catalogo = _catalogoService.GetCat(tipoCatalogo);

            return new JsonResult(catalogo);
        }

        public JsonResult GetMunicipios(string id)
        {
            var catalogo = _catalogoService.GetMunicipios(Convert.ToInt32(id));

            //var listaAgrupada = _solicitudService.GetDetalleRacionesSolicitud(idSolicitud);

            return new JsonResult(catalogo);
        }

        public List<SelectListItem> GetComunidades(Int64 id)
        {
            var soli = _solicitudService.GetComunidades(id);

            return soli;
        }

        public List<SelectListItem> GetComunidadesAPA(Int64 id)
        {
            var soli = _solicitudService.GetComunidadesAPA(id);

            return soli;
        }
        public JsonResult GetMunicipiosFilter(string id)
        {
            var catalogo = _catalogoService.GetMunicipiosFilter(Convert.ToInt32(id));

            return new JsonResult(catalogo);
        }

        public string GetOrdenDepto(string id)
        {
            string orden = _catalogoService.GetOrdenDepto(Convert.ToInt32(id)).ToString();

            return orden;
        }

        public async Task<IActionResult> NuevaSolicitud(Int64? id)
        {
            //Solicitud solicitud=new Solicitud();
            Solicitud solicitud = new Solicitud();
            var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");
            //TempData["Mensaje"] = null;

            if (id == null)
            {
                return View(solicitud);
            }
            else
            {
                solicitud = await _solicitudService.GetSolicitud((long)id);
                return View(solicitud);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarSolicitud(SolicitudesVM modeloVM, IFormFile file)
        {
            var depto = User.FindFirst("depto")?.Value;
            var idDepto = User.FindFirst("idDepto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            var info = HttpContext?.Request.Query["data"];


            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
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

            var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            //ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            var instituciones = await _catalogoService.GetCatalogo("INSTITUCION");
            var institucionDict = instituciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            ViewBag.Instituciones = new SelectList(instituciones, "IdCatalogo", "Descripcion");

            var proyectos = await _catalogoService.GetCatalogo("PROYECTO");
            ViewBag.Proyectos = new SelectList(proyectos, "IdCatalogo", "Descripcion");
            var proyectoDict = proyectos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            Solicitud modelo = new Solicitud()
            {
                IdSolicitud = modeloVM.Solicitud.IdSolicitud,
                Correlativo = modeloVM.Solicitud.Correlativo,
                Codigo = modeloVM.Solicitud.Codigo,
                FechaIngresoDespacho = Convert.ToDateTime(modeloVM.Solicitud.FechaIngresoUnidad).Date,
                FechaIngresoDireccion = Convert.ToDateTime(modeloVM.Solicitud.FechaIngresoDireccion).Date,
                FechaIngresoUnidad = Convert.ToDateTime(modeloVM.Solicitud.FechaIngresoDespacho).Date,
                IdInstitucion = modeloVM.Solicitud.IdInstitucion,
                IdIntervencion = modeloVM.Solicitud.IdIntervencion,
                IdUnidad = modeloVM.Solicitud.IdUnidad,
                Archivo = modeloVM.Solicitud.Archivo ?? null,
                Raciones = modeloVM.Solicitud.Raciones,
                Restrictiva = modeloVM.Solicitud.Restrictiva,
                Estado = modeloVM.Solicitud.Estado,

                Fallecidos = modeloVM.Solicitud.Fallecidos,
                Duplicados = modeloVM.Solicitud.Duplicados,
                SinDatos = modeloVM.Solicitud.SinDatos,
                Gobierno = modeloVM.Solicitud.Gobierno,
                Depto = modeloVM.Solicitud.Depto ?? idDepto,
                Descripcion = modeloVM.Solicitud.Descripcion,
                FInicioEntrega = modeloVM.Solicitud.FInicioEntrega,
                //FInicioEntrega = Convert.ToDateTime(modeloVM.Solicitud.FInicioEntrega),
                FFinEntrega = modeloVM.Solicitud.FFinEntrega,
                //FFinEntrega = Convert.ToDateTime(modeloVM.Solicitud.FFinEntrega),
                DireccionEntrega = modeloVM.Solicitud.DireccionEntrega,
                TecnicoNombre = modeloVM.Solicitud.TecnicoNombre,
                TecnicoTelefono = modeloVM.Solicitud.TecnicoTelefono,
                TecnicoObservaciones = modeloVM.Solicitud.TecnicoObservaciones,
                IdProyecto = modeloVM.Solicitud.IdProyecto ?? null,
                Departamento = modeloVM.Solicitud.Departamento ?? null,
                Municipio = modeloVM.Solicitud.Municipio ?? null,
                Comunidad = modeloVM.Solicitud.Comunidad ?? null,
                FechaRegistro = DateTime.Now.Date, //modeloVM.DetalleEvaluacion.FechaRegistro,
                UsuarioRegistro = User.FindFirst("Usuario")?.Value,
                NombreCocode = modeloVM.Solicitud.NombreCocode ?? null,
                ComunidadCocode = modeloVM.Solicitud.ComunidadCocode ?? null,
                TelefonoCocode = modeloVM.Solicitud.TelefonoCocode ?? null
            };

            if (modelo.Depto == "2")
            {
                //modelo.Raciones = 1;
                modelo.IdInstitucion = null;
            }


            string nombreUnidad;
            //if (intervencionDict.TryGetValue(modeloVM.Solicitud.IdUnidad, out nombreUnidad))
            //string nombreUnidad = modeloVM.UnidadDict.TryGetValue(modeloVM.Solicitud.IdUnidad, out nombreUnidad).ToString();
            //var intervenciones = await _catalogoService.GetCatalogo("UNIDAD");
            //ViewBag.Intervenciones = new SelectList(intervenciones, "IdCatalogo", "Descripcion");
            //var intervencionDict = intervenciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            //if (ModelState.IsValid)
            {
                if (modelo.IdSolicitud == 0)
                {
                    var result = await _solicitudService.SaveSolicitud(modelo);

                    if (result.Success && result.Data != null)
                    {
                        await CrearBitacora((int)result.Data.IdSolicitud, 1, "Se crea la solicitud");
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
                            var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdSolicitud}{extension}";

                            modelo.IdSolicitud = result.Data.IdSolicitud;

                            if (unidadDict.TryGetValue(result.Data.IdUnidad, out nombreUnidad))
                                modelo.Codigo = nombreUnidad + "-" + result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();
                            else
                                modelo.Codigo = result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();

                            modelo.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes", nuevoNombre);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            await _solicitudService.UpdateSolicitud(modelo);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }

                        modelo.IdSolicitud = result.Data.IdSolicitud;

                        if (unidadDict.TryGetValue(result.Data.IdUnidad, out nombreUnidad))
                            modelo.Codigo = nombreUnidad + "-" + result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();
                        else
                            modelo.Codigo = result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();

                        await _solicitudService.UpdateSolicitud(modelo);
                        //----------------------------

                        /*if (rol=="Admin" && modelo.Depto == "DAPA")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Solicitud",new { k = "2" });
                        }
                        else if (rol == "Admin" && modelo.Depto == "DAAN")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Solicitud", new { k = "1" });
                        }
                        else
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Solicitud");
                        }*/

                        if (idDepto == "2") return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { idSolicitud = modelo.IdSolicitud });

                        if (varRetorno == "Programacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Programacion", new { k = condicion });
                        }

                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("Index", "Solicitud", new { k = condicion });
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Message;

                        if (idDepto == "2") return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { idSolicitud = modelo.IdSolicitud });

                        return View(modelo);
                    }

                }
                else
                {

                    var result = await _solicitudService.UpdateSolicitud(modelo);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------                        

                        if (file != null && file.Length > 0)
                        {
                            try
                            {
                                //----------------------------Renombrar archivo viejo---------------------------------
                                var pathActual = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes");
                                string oldArchivo = modelo.Archivo.Insert(modelo.Archivo.IndexOf("."), "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                                if (System.IO.File.Exists(pathActual + "/" + modelo.Archivo))
                                    System.IO.File.Move(pathActual + "/" + modelo.Archivo, pathActual + "/" + oldArchivo);
                                //--------------------------------------------------------------------------------------
                            }
                            catch
                            {

                            }

                            // Obtenemos la extensión del archivo
                            var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdSolicitud}{extension}";

                            modelo.IdSolicitud = result.Data.IdSolicitud;

                            if (unidadDict.TryGetValue(result.Data.IdUnidad, out nombreUnidad))
                                modelo.Codigo = nombreUnidad + "-" + result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();
                            else
                                modelo.Codigo = result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();

                            modelo.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes", nuevoNombre);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            await _solicitudService.UpdateSolicitud(modelo);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        else
                        {
                            modelo.IdSolicitud = result.Data.IdSolicitud;

                            if (unidadDict.TryGetValue(result.Data.IdUnidad, out nombreUnidad))
                                modelo.Codigo = nombreUnidad + "-" + result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();
                            else
                                modelo.Codigo = result.Data.IdSolicitud.ToString() + "-" + DateTime.Now.Year.ToString();

                            await _solicitudService.UpdateSolicitud(modelo);
                        }
                        //----------------------------

                        /*foreach (var asignacion in modelo.Asignaciones)
                        {
                            asignacion.InformacionId = nuevaInformacion.Id;
                            _context.Asignacion.Add(asignacion);
                        }
                        await _context.SaveChangesAsync();*/

                        //TempData["Mensaje"] = result.Message;
                        //return RedirectToAction("Index", "Solicitud");

                        /*if (rol == "Admin" && modelo.Depto == "DAPA")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Solicitud", new { k = "2" });
                        }
                        else if (rol == "Admin" && modelo.Depto == "DAAN")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Solicitud", new { k = "1" });
                        }                        
                        else
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Solicitud");
                        }*/

                        if (idDepto == "2") return RedirectToAction("DetalleSolicitud", "SolicitudV2", new { idSolicitud = modelo.IdSolicitud });

                        if (varRetorno == "Programacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("Index", "Programacion", new { k = condicion });
                        }

                        TempData["Mensaje"] = result.Message;

                        return RedirectToAction("Index", "Solicitud", new { k = condicion });
                    }
                }
            }
            if (idDepto == "2") return RedirectToAction("Index", "SolicitudV2");

            return View(modelo);
        }

        public async Task<ActionResult> EnviarSolicitud(Int64? id)
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
            solicitud = await _solicitudService.GetSolicitud((long)id);
            ServiceResponse<Solicitud> result = new ServiceResponse<Solicitud>();
            if (condicion == "1")
            {
                result = await _solicitudService.UpdateEstado(solicitud);
                await CrearBitacora((int)solicitud.IdSolicitud, 2, "Se actualiza el estado de la solicitud");
            }
            else if (condicion == "2")
            {
                result = await _solicitudService.UpdateEstadoARevAdministrativa(solicitud);
                await CrearBitacora((int)solicitud.IdSolicitud, 2, "Se actualiza la solicitud");
            }

            if (result.Success && result.Data != null)
            {
                if (varRetorno == "Evaluacion")
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Evaluacion", new { idSolicitud = id, k = condicion });
                }

                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Solicitud", new { k = condicion });
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(solicitud);
            }

        }

        public async Task<IActionResult> IndexDetalle(Int64? idSolicitud, Int64? idDetalle)
        {
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var intervecion = _solicitudService.GetIntervencionBySolicitudId(idSolicitud).Result;
            var NombreIntervencion = _catalogoService.GetCatalogo(intervecion.Value).Result;
            ViewBag.NombreIntervencion = NombreIntervencion.Descripcion;
            ViewBag.IdIntervencion = intervecion;
            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (idDetalle == null || idDetalle == 0)
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

                var solicitudVM = new SolicitudesVM();

                if (condicion == "1")
                {
                    solicitudVM = new SolicitudesVM()
                    {
                        Solicitud = await _solicitudService.GetSolicitud((long)idSolicitud),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        listaDetallesSolicitud = await _solicitudService.GetAllDetalleSolicitud((long)idSolicitud),
                        DetalleSolicitud = new DetalleSolicitud(),
                        //IntervencionDict = intervencionDict,
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idSolicitud),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        listaDetallesSolicitudAPA = await _solicitudService.GetAllDetalleSolicitudAPA((long)idSolicitud),
                        DetalleSolicitudAPA = new DetalleSolicitudAPA(),
                        //IntervencionDict = intervencionDict,
                        //DepartamentoDict = departamentoDict,
                        //MunicipioDict = municipioDict,
                        //EstadoCivilDict = estadocivilDict,
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idSolicitud),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitud = await _solicitudService.GetDetalleSolicitud((long)idDetalle),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitud = await _solicitudService.GetAllDetalleSolicitud((long)idSolicitud),
                        ///IntervencionDict = intervencionDict,
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
                        Solicitud = await _solicitudService.GetSolicitud((long)idSolicitud),
                        //Solicitudes = await _solicitudService.GetAllSolicitudes(),
                        DetalleSolicitudAPA = await _solicitudService.GetDetalleSolicitudAPA((long)idDetalle),
                        /*DetalleSolicitud = new DetalleSolicitud()
                        {
                            IdSolicitud=(long)idSolicitud
                        },*/
                        listaDetallesSolicitudAPA = await _solicitudService.GetAllDetalleSolicitudAPA((long)idSolicitud),
                        ///IntervencionDict = intervencionDict,
                        ////DepartamentoDict = departamentoDict,
                        ////MunicipioDict = municipioDict,
                        ////EstadoCivilDict = estadocivilDict,
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
            // Datos de ejemplo
            var datos = await _solicitudService.GetAllDetalleSolicitud((long)idSolicitud);

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
            var datos = await _solicitudService.GetAllDetalleSolicitudAPA((long)idSolicitud);

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

        public async Task<IActionResult> ObtenerDetalleSolicitud(Int64 idDetalle)
        {
            //var claimDepto = User.FindFirst("Depto");

            var detalleSolicitud = await _solicitudService.GetDetalleSolicitud(idDetalle);

            if (detalleSolicitud == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idDetalleSolicitud = detalleSolicitud.IdDetalleSolicitud,
                //idIntervencion = detalleSolicitud.IdIntervencion, // Maneja nullables
                idDepartamento = detalleSolicitud.IdDepartamento, // Maneja nullables
                idMunicipio = detalleSolicitud.IdMunicipio,// Proporciona un valor por defecto si es null
                comunidad = detalleSolicitud.Comunidad,
                Nombre1Esposo = detalleSolicitud.Nombre1Esposo,
                Nombre2Esposo = detalleSolicitud.Nombre2Esposo,
                Nombre3Esposo = detalleSolicitud.Nombre3Esposo,
                Apellido1Esposo = detalleSolicitud.Apellido1Esposo,
                Apellido2Esposo = detalleSolicitud.Apellido2Esposo,
                CuiEsposo = detalleSolicitud.CuiEsposo,
                //FNacimientoEsposo = detalleSolicitud.FNacimientoEsposo.ToString("yyyy-MM-dd"),
                FNacimientoEsposo = detalleSolicitud.FNacimientoEsposo.HasValue ? detalleSolicitud.FNacimientoEsposo.Value.ToString("yyyy-MM-dd") : null,
                Nombre1Esposa = detalleSolicitud.Nombre1Esposa,
                Nombre2Esposa = detalleSolicitud.Nombre2Esposa,
                Nombre3Esposa = detalleSolicitud.Nombre3Esposa,
                Apellido1Esposa = detalleSolicitud.Apellido1Esposa,
                Apellido2Esposa = detalleSolicitud.Apellido2Esposa,
                ApellidoCasada = detalleSolicitud.ApellidoCasada,
                CuiEsposa = detalleSolicitud.CuiEsposa,
                FNacimientoEsposa = detalleSolicitud.FNacimientoEsposa.HasValue ? detalleSolicitud.FNacimientoEsposa.Value.ToString("yyyy-MM-dd") : null,
                EstadoCivil = detalleSolicitud.EstadoCivil,
                Embarazada = detalleSolicitud.Embarazada,
                NiñoDA = detalleSolicitud.NiñoDA,
                GrupoEtnico = detalleSolicitud.GrupoEtnico,
                Hombre0a2Años = detalleSolicitud.Hombre0a2Años,
                Mujer0a2Años = detalleSolicitud.Mujer0a2Años,
                Hombre2a5Años = detalleSolicitud.Hombre2a5Años,
                Mujer2a5Años = detalleSolicitud.Mujer2a5Años,
                Hombre6a17Años = detalleSolicitud.Hombre6a17Años,
                Mujer6a17Años = detalleSolicitud.Mujer6a17Años,
                TipoPersona = detalleSolicitud.TipoPersona,
                AreaMaiz = detalleSolicitud.AreaMaiz,
                QuintalesMaiz = detalleSolicitud.QuintalesMaiz,
                AreaFrijol = detalleSolicitud.AreaFrijol,
                QuintalesFrijol = detalleSolicitud.QuintalesFrijol,
                TipoAgriculturaPractica = detalleSolicitud.TipoAgriculturaPractica,
                NombreCOCODE = detalleSolicitud.NombreCOCODE,
                NombrePersonaRegistro = detalleSolicitud.NombrePersonaRegistro,
                InstitucionRepresenta = detalleSolicitud.InstitucionRepresenta,
                //idAnulacion = detalleSolicitud.IdAnulacion.HasValue ? detalleSolicitud.IdAnulacion : 67
                idAnulacion = (detalleSolicitud.IdAnulacion == null || detalleSolicitud.IdAnulacion == 0) ? 67 : detalleSolicitud.IdAnulacion

                //cantidadBeneficiarios=detalleSolicitud.CantidadBeneficiarios
                //Archivo = detalleSolicitud.Archivo.ToString()
            });
        }
        public async Task<IActionResult> ObtenerDetalleSolicitudAPA(Int64 idDetalle)
        {
            //var claimDepto = User.FindFirst("Depto");

            var detalleSolicitud = await _solicitudService.GetDetalleSolicitudAPA(idDetalle);

            if (detalleSolicitud == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idDetalleSolicitud = detalleSolicitud.IdDetalleSolicitud,
                //idIntervencion = detalleSolicitud.IdIntervencion, // Maneja nullables
                ////idDepartamento = detalleSolicitud.IdDepartamento, // Maneja nullables
                ////idMunicipio = detalleSolicitud.IdMunicipio,// Proporciona un valor por defecto si es null
                ////comunidad = detalleSolicitud.Comunidad,
                ////Nombre1Esposo = detalleSolicitud.Nombre1Esposo,
                ////Nombre2Esposo = detalleSolicitud.Nombre2Esposo,
                ////Nombre3Esposo = detalleSolicitud.Nombre3Esposo,
                ////Apellido1Esposo = detalleSolicitud.Apellido1Esposo,
                ////Apellido2Esposo = detalleSolicitud.Apellido2Esposo,
                ////CuiEsposo = detalleSolicitud.CuiEsposo,
                //FNacimientoEsposo = detalleSolicitud.FNacimientoEsposo.ToString("yyyy-MM-dd"),
                //////FNacimientoEsposo = detalleSolicitud.FNacimientoEsposo.HasValue ? detalleSolicitud.FNacimientoEsposo.Value.ToString("yyyy-MM-dd") : null,
                Nombre1Esposa = detalleSolicitud.Nombre1Esposa,
                Nombre2Esposa = detalleSolicitud.Nombre2Esposa,
                Nombre3Esposa = detalleSolicitud.Nombre3Esposa,
                Apellido1Esposa = detalleSolicitud.Apellido1Esposa,
                Apellido2Esposa = detalleSolicitud.Apellido2Esposa,
                ApellidoCasada = detalleSolicitud.ApellidoCasada,
                CuiEsposa = detalleSolicitud.CuiEsposa,
                ////FNacimientoEsposa = detalleSolicitud.FNacimientoEsposa.HasValue ? detalleSolicitud.FNacimientoEsposa.Value.ToString("yyyy-MM-dd") : null,
                ////EstadoCivil = detalleSolicitud.EstadoCivil,
                ////Embarazada = detalleSolicitud.Embarazada,
                ////NiñoDA = detalleSolicitud.NiñoDA,
                Genero = detalleSolicitud.Genero,
                GrupoEtnico = detalleSolicitud.GrupoEtnico,
                ////Hombre0a2Años = detalleSolicitud.Hombre0a2Años,
                ////Mujer0a2Años = detalleSolicitud.Mujer0a2Años,
                ////Hombre2a5Años = detalleSolicitud.Hombre2a5Años,
                ////Mujer2a5Años = detalleSolicitud.Mujer2a5Años,
                ////Hombre6a17Años = detalleSolicitud.Hombre6a17Años,
                ////Mujer6a17Años = detalleSolicitud.Mujer6a17Años,
                ////TipoPersona = detalleSolicitud.TipoPersona,
                ////AreaMaiz = detalleSolicitud.AreaMaiz,
                ////QuintalesMaiz = detalleSolicitud.QuintalesMaiz,
                ////AreaFrijol = detalleSolicitud.AreaFrijol,
                ////QuintalesFrijol = detalleSolicitud.QuintalesFrijol,
                ////TipoAgriculturaPractica = detalleSolicitud.TipoAgriculturaPractica,
                ////NombreCOCODE = detalleSolicitud.NombreCOCODE,
                ////NombrePersonaRegistro = detalleSolicitud.NombrePersonaRegistro,
                ////InstitucionRepresenta = detalleSolicitud.InstitucionRepresenta,
                //idAnulacion = detalleSolicitud.IdAnulacion.HasValue ? detalleSolicitud.IdAnulacion : 67
                idAnulacion = (detalleSolicitud.IdAnulacion == null || detalleSolicitud.IdAnulacion == 0) ? 67 : detalleSolicitud.IdAnulacion
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarDetalleSolicitud(SolicitudesVM modeloVM, Int64 idSolicitud, Int64 idDetalle, IFormFile file)
        {
            //var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            //ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
            ViewBag.RC = varRetorno;

            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (condicion == "1")
            {
                DetalleSolicitud modelo = new DetalleSolicitud()
                {
                    IdSolicitud = idSolicitud, //modeloVM.DetalleSolicitud.IdSolicitud, //Convert.ToInt64(ViewBag.solID.ToString()),
                    IdDetalleSolicitud = idDetalle, //modeloVM.DetalleSolicitud.IdDetalleSolicitud,
                                                    //IdIntervencion = modeloVM.DetalleSolicitud.IdIntervencion,
                    IdDepartamento = modeloVM.DetalleSolicitud.IdDepartamento,
                    IdMunicipio = modeloVM.DetalleSolicitud.IdMunicipio,
                    Comunidad = modeloVM.DetalleSolicitud.Comunidad,
                    Nombre1Esposo = modeloVM.DetalleSolicitud.Nombre1Esposo,
                    Nombre2Esposo = modeloVM.DetalleSolicitud.Nombre2Esposo,
                    Nombre3Esposo = modeloVM.DetalleSolicitud.Nombre3Esposo,
                    Apellido1Esposo = modeloVM.DetalleSolicitud.Apellido1Esposo,
                    Apellido2Esposo = modeloVM.DetalleSolicitud.Apellido2Esposo,
                    CuiEsposo = modeloVM.DetalleSolicitud.CuiEsposo,
                    FNacimientoEsposo = modeloVM.DetalleSolicitud.FNacimientoEsposo,
                    Nombre1Esposa = modeloVM.DetalleSolicitud.Nombre1Esposa,
                    Nombre2Esposa = modeloVM.DetalleSolicitud.Nombre2Esposa,
                    Nombre3Esposa = modeloVM.DetalleSolicitud.Nombre3Esposa,
                    Apellido1Esposa = modeloVM.DetalleSolicitud.Apellido1Esposa,
                    Apellido2Esposa = modeloVM.DetalleSolicitud.Apellido2Esposa,
                    ApellidoCasada = modeloVM.DetalleSolicitud.ApellidoCasada,
                    CuiEsposa = modeloVM.DetalleSolicitud.CuiEsposa,
                    FNacimientoEsposa = modeloVM.DetalleSolicitud.FNacimientoEsposa,
                    EstadoCivil = modeloVM.DetalleSolicitud.EstadoCivil,
                    Embarazada = modeloVM.DetalleSolicitud.Embarazada,
                    NiñoDA = modeloVM.DetalleSolicitud.NiñoDA,
                    GrupoEtnico = modeloVM.DetalleSolicitud.GrupoEtnico,
                    Hombre0a2Años = modeloVM.DetalleSolicitud.Hombre0a2Años,
                    Mujer0a2Años = modeloVM.DetalleSolicitud.Mujer0a2Años,
                    Hombre2a5Años = modeloVM.DetalleSolicitud.Hombre2a5Años,
                    Mujer2a5Años = modeloVM.DetalleSolicitud.Mujer2a5Años,
                    Hombre6a17Años = modeloVM.DetalleSolicitud.Hombre6a17Años,
                    Mujer6a17Años = modeloVM.DetalleSolicitud.Mujer6a17Años,
                    TipoPersona = modeloVM.DetalleSolicitud.TipoPersona,
                    AreaMaiz = modeloVM.DetalleSolicitud.AreaMaiz,
                    QuintalesMaiz = modeloVM.DetalleSolicitud.QuintalesMaiz,
                    AreaFrijol = modeloVM.DetalleSolicitud.AreaFrijol,
                    QuintalesFrijol = modeloVM.DetalleSolicitud.QuintalesFrijol,
                    TipoAgriculturaPractica = modeloVM.DetalleSolicitud.TipoAgriculturaPractica,
                    NombreCOCODE = modeloVM.DetalleSolicitud.NombreCOCODE,
                    NombrePersonaRegistro = modeloVM.DetalleSolicitud.NombrePersonaRegistro,
                    InstitucionRepresenta = modeloVM.DetalleSolicitud.InstitucionRepresenta,
                    IdAnulacion = modeloVM.DetalleSolicitud.IdAnulacion,
                    //CantidadBeneficiarios = modeloVM.DetalleSolicitud.CantidadBeneficiarios,
                    //Archivo = modeloVM.Solicitud.Archivo,
                    Restrictiva = modeloVM.DetalleSolicitud.Restrictiva,
                    Estado = modeloVM.DetalleSolicitud.Estado
                };

                if (modelo.IdDetalleSolicitud == 0)
                {
                    var result = await _solicitudService.SaveDetalleSolicitud(modelo, varRetorno);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------
                        //if (file != null && file.Length > 0)
                        {
                            // Obtenemos la extensión del archivo
                            //var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            //var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            //var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdSolicitud}{extension}";

                            //modelo.IdDetalleSolicitud = result.Data.IdDetalleSolicitud;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            //modelo.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes", nuevoNombre);

                            /*using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }*/

                            //await _solicitudService.UpdateDetalleSolicitud(modelo);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (rol == "Admin" && depto == "DAPA")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { dSolicitud = idSolicitud, k = "2" });
                        }
                        else if (rol == "Admin" && depto == "DAAN")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { dSolicitud = idSolicitud, k = "1" });
                        }
                        else
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud });
                        }*/

                        if (varRetorno == "Programacion" || varRetorno == "Ejecucion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idSolicitud, k = "1" });
                        }
                        else if (varRetorno == "Evaluacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Evaluacion", new { idEvaluacion = idSolicitud, k = "1" });
                        }

                        TempData["Mensaje"] = result.Message;
                        //return View(modeloVM);
                        return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "1" });
                    }
                    else
                    {
                        TempData["Mensaje"] = result.Message;
                        return View(modelo);
                    }

                }
                else
                {

                    var result = await _solicitudService.UpdateDetalleSolicitud(modelo, varRetorno);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------                        

                        //if (file != null && file.Length > 0)
                        {
                            //----------------------------Renombrar archivo viejo---------------------------------
                            //var pathActual = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes");
                            //string oldArchivo = modelo.Archivo.Insert(modelo.Archivo.IndexOf("."), "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                            //if (System.IO.File.Exists(pathActual + "/" + modelo.Archivo))
                            //System.IO.File.Move(pathActual + "/" + modelo.Archivo, pathActual + "/" + oldArchivo);
                            //--------------------------------------------------------------------------------------

                            // Obtenemos la extensión del archivo
                            //var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            //var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            //var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdSolicitud}{extension}";

                            modelo.IdDetalleSolicitud = result.Data.IdDetalleSolicitud;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            //modelo.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes", nuevoNombre);

                            /*using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }*/

                            await _solicitudService.UpdateDetalleSolicitud(modelo, varRetorno);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (rol == "Admin" && depto == "DAPA")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "2" });
                        }
                        else if (rol == "Admin" && depto == "DAAN")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "1" });
                        }
                        else
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud });
                        }*/

                        if (varRetorno == "Programacion" || varRetorno == "Ejecucion")
                        {
                            TempData["Mensaje"] = result.Message;
                            ViewBag.RC = varRetorno;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idSolicitud, k = "1", returnController = varRetorno });
                        }
                        else if (varRetorno == "Evaluacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Evaluacion", new { idEvaluacion = idSolicitud, k = "1" });
                        }

                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "1" });
                    }
                }

                return View(modelo);
            }
            else if (condicion == "2")
            {
                DetalleSolicitudAPA modelo = new DetalleSolicitudAPA()
                {
                    IdSolicitud = idSolicitud, //modeloVM.DetalleSolicitud.IdSolicitud, //Convert.ToInt64(ViewBag.solID.ToString()),
                    IdDetalleSolicitud = idDetalle, //modeloVM.DetalleSolicitud.IdDetalleSolicitud,
                                                    //IdIntervencion = modeloVM.DetalleSolicitud.IdIntervencion,
                                                    ////IdDepartamento = modeloVM.DetalleSolicitudAPA.IdDepartamento,
                                                    ////IdMunicipio = modeloVM.DetalleSolicitudAPA.IdMunicipio,
                                                    ////Comunidad = modeloVM.DetalleSolicitudAPA.Comunidad,
                                                    ////Nombre1Esposo = modeloVM.DetalleSolicitudAPA.Nombre1Esposo,
                                                    ////Nombre2Esposo = modeloVM.DetalleSolicitudAPA.Nombre2Esposo,
                                                    ////Nombre3Esposo = modeloVM.DetalleSolicitudAPA.Nombre3Esposo,
                                                    ////Apellido1Esposo = modeloVM.DetalleSolicitudAPA.Apellido1Esposo,
                                                    ////Apellido2Esposo = modeloVM.DetalleSolicitudAPA.Apellido2Esposo,
                                                    ////CuiEsposo = modeloVM.DetalleSolicitudAPA.CuiEsposo,
                                                    ////FNacimientoEsposo = modeloVM.DetalleSolicitudAPA.FNacimientoEsposo,
                    Nombre1Esposa = modeloVM.DetalleSolicitudAPA.Nombre1Esposa,
                    Nombre2Esposa = modeloVM.DetalleSolicitudAPA.Nombre2Esposa,
                    Nombre3Esposa = modeloVM.DetalleSolicitudAPA.Nombre3Esposa,
                    Apellido1Esposa = modeloVM.DetalleSolicitudAPA.Apellido1Esposa,
                    Apellido2Esposa = modeloVM.DetalleSolicitudAPA.Apellido2Esposa,
                    ApellidoCasada = modeloVM.DetalleSolicitudAPA.ApellidoCasada,
                    CuiEsposa = modeloVM.DetalleSolicitudAPA.CuiEsposa,
                    ////FNacimientoEsposa = modeloVM.DetalleSolicitudAPA.FNacimientoEsposa,
                    ////EstadoCivil = modeloVM.DetalleSolicitudAPA.EstadoCivil,
                    ////Embarazada = modeloVM.DetalleSolicitudAPA.Embarazada,
                    ////NiñoDA = modeloVM.DetalleSolicitudAPA.NiñoDA,
                    Genero = modeloVM.DetalleSolicitudAPA.Genero,
                    GrupoEtnico = modeloVM.DetalleSolicitudAPA.GrupoEtnico,
                    ////Hombre0a2Años = modeloVM.DetalleSolicitudAPA.Hombre0a2Años,
                    ////Mujer0a2Años = modeloVM.DetalleSolicitudAPA.Mujer0a2Años,
                    ////Hombre2a5Años = modeloVM.DetalleSolicitudAPA.Hombre2a5Años,
                    ////Mujer2a5Años = modeloVM.DetalleSolicitudAPA.Mujer2a5Años,
                    ////Hombre6a17Años = modeloVM.DetalleSolicitudAPA.Hombre6a17Años,
                    ////Mujer6a17Años = modeloVM.DetalleSolicitudAPA.Mujer6a17Años,
                    ////TipoPersona = modeloVM.DetalleSolicitudAPA.TipoPersona,
                    ////AreaMaiz = modeloVM.DetalleSolicitudAPA.AreaMaiz,
                    ////QuintalesMaiz = modeloVM.DetalleSolicitudAPA.QuintalesMaiz,
                    ////AreaFrijol = modeloVM.DetalleSolicitudAPA.AreaFrijol,
                    ////QuintalesFrijol = modeloVM.DetalleSolicitudAPA.QuintalesFrijol,
                    ////TipoAgriculturaPractica = modeloVM.DetalleSolicitudAPA.TipoAgriculturaPractica,
                    ////NombreCOCODE = modeloVM.DetalleSolicitudAPA.NombreCOCODE,
                    ////NombrePersonaRegistro = modeloVM.DetalleSolicitudAPA.NombrePersonaRegistro,
                    ////InstitucionRepresenta = modeloVM.DetalleSolicitudAPA.InstitucionRepresenta,
                    IdAnulacion = modeloVM.DetalleSolicitudAPA.IdAnulacion,
                    //CantidadBeneficiarios = modeloVM.DetalleSolicitud.CantidadBeneficiarios,
                    //Archivo = modeloVM.Solicitud.Archivo,
                    Restrictiva = modeloVM.DetalleSolicitudAPA.Restrictiva,
                    Estado = modeloVM.DetalleSolicitudAPA.Estado
                };

                if (modelo.IdDetalleSolicitud == 0)
                {
                    var cuentaRegistros = _solicitudService.GetAllDetalleSolicitudAPA(modelo.IdSolicitud).Result.Count();

                    if (Convert.ToInt32(cuentaRegistros) + 1 > 250)
                    {
                        if (!ModelState.IsValid)
                        {
                            //ViewBag.ModalState = "show"; // Mantener la ventana modal abierta
                            //ViewBag.ErrorMessage = "Por favor, verifica los datos ingresados.";
                            TempData["MsgError"] = "Cantidad de beneficiarios sobrepasa el límite aceptado de 250";
                            return RedirectToAction("ListaDetalle", "Evaluacion", new { idEvaluacion = idSolicitud, k = "2" });
                        }
                        /*if (!ModelState.IsValid)
                        {
                            TempData["Mensaje"] = "Error";
                            return BadRequest(TempData["Mensaje"]);
                        } */
                    }
                    else
                    {
                        var result = await _solicitudService.SaveDetalleSolicitudAPA(modelo);

                        if (result.Success && result.Data != null)
                        {
                            //modelo.IdSolicitud=result.Data.IdSolicitud;
                            //Int64 idDevuelto = result.Data.IdSolicitud;
                            //idDevuelto = result.Data.IdSolicitud;
                            //----------------------------
                            //if (file != null && file.Length > 0)
                            {
                                // Obtenemos la extensión del archivo
                                //var extension = Path.GetExtension(file.FileName);
                                // Obtenemos el nombre del archivo sin la extensión
                                //var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                                // Concatenamos el ID al nombre antes de la extensión
                                //var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdSolicitud}{extension}";

                                //modelo.IdDetalleSolicitud = result.Data.IdDetalleSolicitud;
                                //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                                //modelo.Archivo = nuevoNombre;

                                // Ruta donde se guardará el archivo
                                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes", nuevoNombre);

                                /*using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }*/

                                //await _solicitudService.UpdateDetalleSolicitud(modelo);
                                //return RedirectToAction("Index"); // O a la vista que desees redirigir
                            }
                            //----------------------------

                            /*if (rol == "Admin" && depto == "DAPA")
                            {
                                TempData["Mensaje"] = result.Message;
                                return RedirectToAction("IndexDetalle", "Solicitud", new { dSolicitud = idSolicitud, k = "2" });
                            }
                            else if (rol == "Admin" && depto == "DAAN")
                            {
                                TempData["Mensaje"] = result.Message;
                                return RedirectToAction("IndexDetalle", "Solicitud", new { dSolicitud = idSolicitud, k = "1" });
                            }
                            else
                            {
                                TempData["Mensaje"] = result.Message;
                                return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud });
                            }*/

                            if (varRetorno == "Programacion" || varRetorno == "Ejecucion")
                            {
                                TempData["Mensaje"] = result.Message;
                                return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idSolicitud, k = "2" });
                            }
                            else if (varRetorno == "Evaluacion")
                            {
                                TempData["Mensaje"] = result.Message;
                                return RedirectToAction("ListaDetalle", "Evaluacion", new { idEvaluacion = idSolicitud, k = "2" });
                            }

                            TempData["Mensaje"] = result.Message;
                            ////return View(modeloVM);
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "2" });
                        }
                        else
                        {
                            TempData["Mensaje"] = result.Message;
                            return View(modelo);
                        }
                    }



                }
                else
                {

                    var result = await _solicitudService.UpdateDetalleSolicitudAPA(modelo);

                    if (result.Success && result.Data != null)
                    {
                        //modelo.IdSolicitud=result.Data.IdSolicitud;
                        //Int64 idDevuelto = result.Data.IdSolicitud;
                        //idDevuelto = result.Data.IdSolicitud;
                        //----------------------------                        

                        //if (file != null && file.Length > 0)
                        {
                            //----------------------------Renombrar archivo viejo---------------------------------
                            //var pathActual = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes");
                            //string oldArchivo = modelo.Archivo.Insert(modelo.Archivo.IndexOf("."), "_" + DateTime.Now.Date.ToString("dd-MM-yyyy") + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString());

                            //if (System.IO.File.Exists(pathActual + "/" + modelo.Archivo))
                            //System.IO.File.Move(pathActual + "/" + modelo.Archivo, pathActual + "/" + oldArchivo);
                            //--------------------------------------------------------------------------------------

                            // Obtenemos la extensión del archivo
                            //var extension = Path.GetExtension(file.FileName);
                            // Obtenemos el nombre del archivo sin la extensión
                            //var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
                            // Concatenamos el ID al nombre antes de la extensión
                            //var nuevoNombre = $"{nombreSinExtension}_{result.Data.IdSolicitud}{extension}";

                            modelo.IdDetalleSolicitud = result.Data.IdDetalleSolicitud;
                            //modelo.Codigo = DateTime.Now.Year.ToString() + "-" + result.Data.IdSolicitud.ToString();
                            //modelo.Archivo = nuevoNombre;

                            // Ruta donde se guardará el archivo
                            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/solicitudes", nuevoNombre);

                            /*using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }*/

                            await _solicitudService.UpdateDetalleSolicitudAPA(modelo);
                            //return RedirectToAction("Index"); // O a la vista que desees redirigir
                        }
                        //----------------------------

                        /*if (rol == "Admin" && depto == "DAPA")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "2" });
                        }
                        else if (rol == "Admin" && depto == "DAAN")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "1" });
                        }
                        else
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud });
                        }*/

                        if (varRetorno == "Programacion" || varRetorno == "Ejecucion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Programacion", new { idProgramacion = idSolicitud, k = "2" });
                        }
                        else if (varRetorno == "Evaluacion")
                        {
                            TempData["Mensaje"] = result.Message;
                            return RedirectToAction("ListaDetalle", "Evaluacion", new { idEvaluacion = idSolicitud, k = "2" });
                        }

                        TempData["Mensaje"] = result.Message;
                        return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud, k = "2" });
                    }
                }

                return View(modelo);
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }

        }

        public async Task<IActionResult> EliminarSolicitud(Int64 id)
        {
            var result = await _solicitudService.DeleteSolicitud(id);
            await CrearBitacora((int)id, 3, "Se elimina la solicitud");
            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Solicitud");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Solicitud");
            }
        }

        public async Task<IActionResult> EliminarDetalleSolicitud(Int64 idSolicitud, Int64 idDetalle)
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

            if (condicion == "1")
            {
                var result = await _solicitudService.DeleteDetalleSolicitud(idDetalle);
                await CrearBitacora((int)idSolicitud, 3, "Se elimina un beneficiario");

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Solicitud");
                }
            }
            else if (condicion == "2")
            {
                var result = await _solicitudService.DeleteDetalleSolicitudAPA(idDetalle);
                await CrearBitacora((int)idSolicitud, 3, "Se elimina un beneficiario");
                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Solicitud", new { idSolicitud = idSolicitud });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("IndexDetalle", "Solicitud");
                }
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }

        }

        // Método para cargar el archivo Excel
        [HttpPost]
        public IActionResult CargaMasiva(IFormFile fileExcel, Int64 id)
        {
            //SolicitudesVM nuevo = new SolicitudesVM();
            //List<DetalleSolicitud> listDet = new List<DetalleSolicitud>();
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (condicion == "1")
            {
                if (fileExcel != null && fileExcel.Length > 0)
                {
                    var excelData = new List<Dictionary<string, string>>();

                    using (var stream = new MemoryStream())
                    {
                        fileExcel.CopyTo(stream);
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0];
                            var rowCount = worksheet.Dimension.Rows;
                            var colCount = worksheet.Dimension.Columns;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                var rowData = new Dictionary<string, string>();

                                for (int col = 1; col <= colCount; col++) // colCount se mantiene fijo
                                {
                                    rowData.Add($"Columna{col}", worksheet.Cells[row, col]?.Text ?? "");
                                }

                                excelData.Add(rowData);
                            }
                        }
                    }

                    return Json(excelData);
                }

                return BadRequest("Archivo no válido.");
            }
            else if (condicion == "2")
            {
                if (fileExcel != null && fileExcel.Length > 0)
                {
                    // Lista para almacenar los datos leídos
                    var excelData = new List<Models.ExcelRowAPA>();

                    using (var stream = new MemoryStream())
                    {
                        fileExcel.CopyTo(stream);
                        // Establecer el contexto de la licencia antes de usar EPPlus
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        // Usar EPPlus para leer el archivo Excel
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Tomamos la primera hoja
                            var rowCount = worksheet.Dimension.Rows;        // Número de filas
                            var colCount = worksheet.Dimension.Columns;     // Número de columnas

                            List<string> encabezadosEsperados =
                            [
                                "PRIMER NOMBRE",
                                "SEGUNDO NOMBRE",
                                "TERCER NOMBRE",
                                "PRIMER APELLIDO",
                                "SEGUNDO APELLIDO",
                                "APELLIDO DE CASADA",
                                "DPI",
                                "GÉNERO",
                                "ETNIA"
                            ];

                            List<string> encabezadosReales = [];
                            // El ciclo debe ir de la columna 1 hasta el número total de columnas
                            for (int col = 1; col <= colCount; col++)
                            {
                                // Obtiene el valor, lo convierte a string, elimina espacios extra (trim) 
                                // y lo convierte a mayúsculas para asegurar una comparación sin errores de caja.
                                var headerValue = worksheet.Cells[1, col].Value?.ToString().Trim().ToUpperInvariant();

                                if (!string.IsNullOrEmpty(headerValue))
                                {
                                    encabezadosReales.Add(headerValue);
                                }
                            }

                            var esperadosMayusculas = encabezadosEsperados.Select(h => h.Trim().ToUpperInvariant()).ToList();
                            var encabezadosFaltantes = esperadosMayusculas.Except(encabezadosReales).ToList();

                            bool encabezadosCoinciden = esperadosMayusculas.SequenceEqual(encabezadosReales);

                            if (!encabezadosCoinciden)
                            {
                                var mensajeError = new System.Text.StringBuilder("Error en los encabezados del archivo. ");
                                if (encabezadosFaltantes.Count != 0)
                                {
                                    mensajeError.Append($"<br> Encabezados faltantes: <br> {string.Join("<br>-", encabezadosFaltantes)}.");
                                }
                                return BadRequest(mensajeError.ToString());
                            }

                            // Leer filas y columnas de la hoja de cálculo
                            for (int row = 2; row <= rowCount; row++) // Empezar en la fila 2 si hay encabezados
                            {
                                var rowData = new ExcelRowAPA
                                {
                                    Columna1 = worksheet.Cells[row, 1].Text, // Columna 1
                                    Columna2 = worksheet.Cells[row, 2].Text, // Columna 2
                                    Columna3 = worksheet.Cells[row, 3].Text, // Columna 3
                                    Columna4 = worksheet.Cells[row, 4].Text, // Columna 3
                                    Columna5 = worksheet.Cells[row, 5].Text,
                                    Columna6 = worksheet.Cells[row, 6].Text,
                                    Columna7 = worksheet.Cells[row, 7].Text,
                                    Columna8 = worksheet.Cells[row, 8].Text,
                                    Columna9 = worksheet.Cells[row, 9].Text,
                                    Columna10 = worksheet.Cells[row, 10].Text,
                                    Columna11 = worksheet.Cells[row, 11].Text
                                };
                                excelData.Add(rowData);
                            }
                        }
                    }

                    // Pasar los datos al modelo para mostrarlos en la vista
                    return Json(excelData);
                }
                return Ok("Archivo cargado");
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }
        }

        //Carga Masiva Anulacion
        [HttpPost]
        public IActionResult CargaMasivaAnulacion(IFormFile fileExcel, Int64 id)
        {
            //SolicitudesVM nuevo = new SolicitudesVM();
            //List<DetalleSolicitud> listDet = new List<DetalleSolicitud>();
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (condicion == "1")
            {
                if (fileExcel != null && fileExcel.Length > 0)
                {
                    // Lista para almacenar los datos leídos
                    var excelData = new List<Models.ExcelRow>();

                    using (var stream = new MemoryStream())
                    {
                        fileExcel.CopyTo(stream);
                        // Establecer el contexto de la licencia antes de usar EPPlus
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        // Usar EPPlus para leer el archivo Excel
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Tomamos la primera hoja
                            var rowCount = worksheet.Dimension.Rows;        // Número de filas
                            var colCount = worksheet.Dimension.Columns;     // Número de columnas

                            // Leer filas y columnas de la hoja de cálculo
                            for (int row = 2; row <= rowCount; row++) // Empezar en la fila 2 si hay encabezados
                            {
                                DetalleSolicitud detInfo = _solicitudService.GetDetalleSolicitudByCUI(id, worksheet.Cells[row, 1].Text).Result;
                                var rowData = new ExcelRow();

                                try
                                {
                                    rowData = new ExcelRow
                                    {
                                        Columna1 = detInfo.IdDetalleSolicitud.ToString(), // Columna 1                                                                      
                                        Columna2 = detInfo.Nombre1Esposo, //worksheet.Cells[row, 2].Text, // Columna 2
                                        Columna3 = detInfo.Nombre2Esposo, //worksheet.Cells[row, 3].Text, // Columna 3
                                        Columna4 = detInfo.Nombre3Esposo, //worksheet.Cells[row, 4].Text, // Columna 3
                                        Columna5 = detInfo.Apellido1Esposo, //worksheet.Cells[row, 5].Text,
                                        Columna6 = detInfo.Apellido2Esposo, //worksheet.Cells[row, 6].Text,                                    
                                        Columna7 = detInfo.CuiEsposo, // Columna 1|
                                        Columna8 = detInfo.Nombre1Esposa, //worksheet.Cells[row, 2].Text, // Columna 2
                                        Columna9 = detInfo.Nombre2Esposa, //worksheet.Cells[row, 3].Text, // Columna 3
                                        Columna10 = detInfo.Nombre3Esposa, //worksheet.Cells[row, 4].Text, // Columna 3
                                        Columna11 = detInfo.Apellido1Esposa, //worksheet.Cells[row, 5].Text,
                                        Columna12 = detInfo.Apellido2Esposa, //worksheet.Cells[row, 6].Text,
                                        Columna13 = detInfo.CuiEsposa, // Columna 1|
                                        Columna14 = worksheet.Cells[row, 2].Text  //Item anulacion nombre

                                        //Columna39 = worksheet.Cells[row, 39].Text
                                    };
                                    excelData.Add(rowData);

                                }
                                catch
                                {

                                }

                                /*DetalleSolicitud modelo = new DetalleSolicitud()
                                {
                                    IdSolicitud = id, //modeloVM.DetalleSolicitud.IdSolicitud, //Convert.ToInt64(ViewBag.solID.ToString()),
                                                      //IdDetalleSolicitud = idDetalle, //modeloVM.DetalleSolicitud.IdDetalleSolicitud,
                                                      //IdIntervencion = modeloVM.DetalleSolicitud.IdIntervencion,
                                                      //IdDepartamento = modeloVM.DetalleSolicitud.IdDepartamento,
                                                      //IdMunicipio = modeloVM.DetalleSolicitud.IdMunicipio,
                                    Comunidad = rowData.Columna1,
                                    CantidadBeneficiarios = Convert.ToInt32(rowData.Columna2.ToString()),
                                    CantidadRaciones = Convert.ToInt32(rowData.Columna3.ToString())
                                    //Archivo = modeloVM.Solicitud.Archivo,
                                    //Restrictiva = modeloVM.DetalleSolicitud.Restrictiva,
                                    //Estado = modeloVM.DetalleSolicitud.Estado
                                };

                                listDet.Add(modelo);*/
                            }

                            //nuevo.listaDetallesSolicitud=listDet;
                        }
                    }

                    // Pasar los datos al modelo para mostrarlos en la vista
                    return Json(excelData);
                }
                //return RedirectToAction("IndexDetalle", "Solicitud",nuevo);
                //return View("IndexDetalle",listDet);
                //return View();
                //return BadRequest("Archivo no válido");
                return Ok("Archivo cargado");
            }
            else if (condicion == "2")
            {
                if (fileExcel != null && fileExcel.Length > 0)
                {
                    // Lista para almacenar los datos leídos
                    var excelData = new List<Models.ExcelRowAPA>();
                    var errores = new List<object>(); // Lista para almacenar errores
                    var filasConError = new List<int>(); // Para llevar control de filas con error

                    using (var stream = new MemoryStream())
                    {
                        fileExcel.CopyTo(stream);
                        // Establecer el contexto de la licencia antes de usar EPPlus
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        // Usar EPPlus para leer el archivo Excel
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets[0]; // Tomamos la primera hoja
                            var rowCount = worksheet.Dimension.Rows;        // Número de filas
                            var colCount = worksheet.Dimension.Columns;     // Número de columnas

                            // Leer filas y columnas de la hoja de cálculo
                            for (int row = 2; row <= rowCount; row++) // Empezar en la fila 2 si hay encabezados
                            {
                                var cui = worksheet.Cells[row, 1].Text?.Trim();

                                DetalleSolicitudAPA detInfo = _solicitudService.GetDetalleSolicitudByCUIAPA(id, cui).Result;

                                var rowData = new ExcelRowAPA();

                                try
                                {
                                    rowData = new ExcelRowAPA
                                    {
                                        Columna1 = detInfo.IdDetalleSolicitud.ToString(), // Columna 1                                                                      
                                        Columna2 = detInfo.Nombre1Esposa, //worksheet.Cells[row, 2].Text, // Columna 2
                                        Columna3 = detInfo.Nombre2Esposa, //worksheet.Cells[row, 3].Text, // Columna 3
                                        Columna4 = detInfo.Nombre3Esposa, //worksheet.Cells[row, 4].Text, // Columna 3
                                        Columna5 = detInfo.Apellido1Esposa, //worksheet.Cells[row, 5].Text,
                                        Columna6 = detInfo.Apellido2Esposa, //worksheet.Cells[row, 6].Text,                                    
                                        Columna7 = detInfo.ApellidoCasada, //worksheet.Cells[row, 6].Text,                                    
                                        Columna8 = detInfo.CuiEsposa, // Columna 1
                                        Columna9 = _catalogoService.GetCatalogo(detInfo.GrupoEtnico).Result.Descripcion, // Columna 1                                    
                                        Columna10 = worksheet.Cells[row, 2].Text
                                    };
                                    excelData.Add(rowData);
                                }
                                catch
                                {

                                }

                                /*DetalleSolicitud modelo = new DetalleSolicitud()
                                {
                                    IdSolicitud = id, //modeloVM.DetalleSolicitud.IdSolicitud, //Convert.ToInt64(ViewBag.solID.ToString()),
                                                      //IdDetalleSolicitud = idDetalle, //modeloVM.DetalleSolicitud.IdDetalleSolicitud,
                                                      //IdIntervencion = modeloVM.DetalleSolicitud.IdIntervencion,
                                                      //IdDepartamento = modeloVM.DetalleSolicitud.IdDepartamento,
                                                      //IdMunicipio = modeloVM.DetalleSolicitud.IdMunicipio,
                                    Comunidad = rowData.Columna1,
                                    CantidadBeneficiarios = Convert.ToInt32(rowData.Columna2.ToString()),
                                    CantidadRaciones = Convert.ToInt32(rowData.Columna3.ToString())
                                    //Archivo = modeloVM.Solicitud.Archivo,
                                    //Restrictiva = modeloVM.DetalleSolicitud.Restrictiva,
                                    //Estado = modeloVM.DetalleSolicitud.Estado
                                };

                                listDet.Add(modelo);*/
                            }

                            //nuevo.listaDetallesSolicitud=listDet;
                        }
                    }

                    // Pasar los datos al modelo para mostrarlos en la vista
                    return Json(excelData);
                }
                //return RedirectToAction("IndexDetalle", "Solicitud",nuevo);
                //return View("IndexDetalle",listDet);
                //return View();
                //return BadRequest("Archivo no válido");
                return Ok("Archivo cargado");
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }
        }

        public class CargaMasivaRequest
        {
            public List<ExcelRowDinamic> InfoData { get; set; }
            public List<ExcelRowAPA> InfoDat { get; set; }

        }

        [HttpPost]
        public async Task<IActionResult> GuardarCargaMasiva([FromBody] CargaMasivaRequest request, int idInteraccion)
        {
            //SolicitudesVM nuevo = new SolicitudesVM();
            //List<DetalleSolicitud> listDet = new List<DetalleSolicitud>();
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (condicion == "1")
            {
                if (request.InfoData != null && request.InfoData.Count > 0)
                {

                    var response = await _solicitudService.SaveListaDetalleSolicitudxIntervencion(request.InfoData, idInteraccion);

                    TempData["Mensaje"] = response.Message;

                }
                //return RedirectToAction("IndexDetalle", "Solicitud",nuevo);
                //return View("IndexDetalle",listDet);
                //return View();
                //return RedirectToAction("IndexDetalle", "Solicitud", new { Response=infodata });
                return Ok(TempData["Mensaje"]);
                //return BadRequest("Archivo no válido");
            }
            else if (condicion == "2")
            {
                if (request.InfoDat != null && request.InfoDat.Count > 0)
                {
                    if (request.InfoDat.Count <= 250)
                    {
                        var response = await _solicitudService.SaveListaDetalleSolicitudAPA(request.InfoDat);

                        TempData["Mensaje"] = response.Message;

                        return Json(new 
                        { 
                            Success = response.Success, 
                            Message = response.Message,
                            Data = response.Data,
                        });
                    }
                    else
                    {
                        if (!ModelState.IsValid)
                        {
                            TempData["Mensaje"] = "Cantidad de beneficiarios sobrepasa el límite aceptado de 250";
                            return BadRequest("....." + TempData["Mensaje"]);
                        }

                    }
                }

                return Ok(TempData["Mensaje"]);
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }
        }

        public async Task<IActionResult> ActualizarAnulacionCargaMasiva([FromBody] CargaMasivaRequest request)
        {
            //SolicitudesVM nuevo = new SolicitudesVM();
            //List<DetalleSolicitud> listDet = new List<DetalleSolicitud>();
            var depto = User.FindFirst("depto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);
            var condicion = "";

            if ((depto == "DAAN" && rol != "Admin") || (tieneValorK && valorK == "1" && rol == "Admin"))
                condicion = "1";
            else if ((depto == "DAPA" && rol != "Admin") || (tieneValorK && valorK == "2" && rol == "Admin"))
                condicion = "2";

            if (condicion == "1")
            {
                if (request.InfoData != null && request.InfoData.Count > 0)
                {
                    var response = await _solicitudService.UpdateListaDetalleSolicitud(request.InfoData);

                    TempData["Mensaje"] = response.Message;

                }
                //return RedirectToAction("IndexDetalle", "Solicitud",nuevo);
                //return View("IndexDetalle",listDet);
                //return View();
                //return RedirectToAction("IndexDetalle", "Solicitud", new { Response=infodata });
                return Ok(TempData["Mensaje"]);
                //return BadRequest("Archivo no válido");
            }
            else if (condicion == "2")
            {
                if (request.InfoDat != null && request.InfoDat.Count > 0)
                {
                    if (request.InfoDat.Count <= 250)
                    {
                        var response = await _solicitudService.UpdateListaDetalleSolicitudAPA(request.InfoDat);

                        TempData["Mensaje"] = response.Message;
                    }
                    else
                    {

                        if (!ModelState.IsValid)
                        {
                            TempData["Mensaje"] = "Cantidad de beneficiarios sobrepasa el límite aceptado de 250";
                            return BadRequest("....." + TempData["Mensaje"]);
                        }

                    }
                }
                //return RedirectToAction("IndexDetalle", "Solicitud",nuevo);
                //return View("IndexDetalle",listDet);
                //return View();
                //return RedirectToAction("IndexDetalle", "Solicitud", new { Response=infodata });
                return Ok(TempData["Mensaje"]);
                //return BadRequest("Archivo no válido");
            }
            else
            {
                return Unauthorized(); // Si el claim no existe o el valor no coincide
            }
        }

        [HttpPost]
        public async Task<IActionResult> AsignaTecnico([FromBody] SolicitudesVM modeloVM)
        {
            var depto = User.FindFirst("depto")?.Value;
            var idDepto = User.FindFirst("idDepto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

            var info = HttpContext?.Request.Query["data"];


            var varRetorno = HttpContext?.Request.Query["returnController"].ToString();
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

            //var unidades = await _catalogoService.GetCatalogo("UNIDAD");
            //var unidadDict = unidades.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);
            //ViewBag.Unidades = new SelectList(unidades, "IdCatalogo", "Descripcion");

            //var instituciones = await _catalogoService.GetCatalogo("INSTITUCION");
            //var institucionDict = instituciones.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            AsignaTecnico modelo = new AsignaTecnico()
            {
                //IdAsignaTecnico = _solicitudService.cuenta,
                TecnicoId = modeloVM.AsignaTecnico.TecnicoId,
                SolicitudId = modeloVM.AsignaTecnico.SolicitudId,
                DepartamentoId = modeloVM.AsignaTecnico.DepartamentoId,
                IdMunicipio = modeloVM.AsignaTecnico.IdMunicipio,
                DireccionEntrega = modeloVM.AsignaTecnico.DireccionEntrega,
                Restrictiva = 0
            };

            var result = await _solicitudService.SaveAsignaTecnico(modelo);

            if (result.Success && result.Data != null)
            {
                if (varRetorno == "Programacion")
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Programacion", new { k = condicion });
                }

                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Solicitud", new { k = condicion });
            }

            return View(modelo);
        }

        public async Task<IActionResult> ObtenerDetalleTecnicosSolicitud(Int64 id)
        {
            SolicitudesVM modeloVM = new SolicitudesVM()
            {
                Solicitud = await _solicitudService.GetSolicitud((long)id),
                DetalleTecnicos = await _solicitudService.GetDetalleTecnicosSolicitud((long)id)
            };

            return Json(modeloVM);
        }

        public async Task<IActionResult> EliminarTecnico(Int64 id)
        {
            var depto = User.FindFirst("depto")?.Value;
            var idDepto = User.FindFirst("idDepto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

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

            var result = await _solicitudService.DeleteAsignaTecnico(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                //return RedirectToAction("Index", "Ejecucion");
                return Json(result);
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return Json(result);
                //return RedirectToAction("Index", "Ejecucion");
            }
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
                //Agrupados = await _solicitudService.GetDetalleSolicitudAgrupado((long)id),
                DetalleRaciones = await _solicitudService.GetDetalleRacionesSolicitud((long)id)
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
                DepartamentoId = solicitudVM.AsignaRacion.DepartamentoId,
                Comunidad = solicitudVM.AsignaRacion.Comunidad ?? "",
                RacionAsignada = solicitudVM.AsignaRacion.RacionAsignada,
                RacionSaldo = solicitudVM.AsignaRacion.RacionSaldo,
                Restrictiva = 0
            };

            var result = await _solicitudService.SaveAsignaRacion(modelo);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Ejecucion", new { k = condicion });
            }

            return View(modelo);
        }

        public async Task<IActionResult> EliminarRacion(Int64 id)
        {
            var depto = User.FindFirst("depto")?.Value;
            var idDepto = User.FindFirst("idDepto")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;

            var valorK = HttpContext?.Request.Query["k"].ToString();
            bool tieneValorK = !string.IsNullOrEmpty(valorK);

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

            var result = await _solicitudService.DeleteAsignaRacion(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                //return RedirectToAction("Index", "Ejecucion");
                return Json(result);
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return Json(result);
                //return RedirectToAction("Index", "Ejecucion");
            }
        }

        public async Task<IActionResult> ObtenerDetalleRacionesSolicitud(Int64 id)
        {
            SolicitudesVM solicitudVM = new SolicitudesVM()
            {
                Solicitud = await _solicitudService.GetSolicitud((long)id),
                //AsignaRacion = new AsignaRacion(),
                DetalleRaciones = await _solicitudService.GetDetalleRacionesSolicitud((long)id)
            };

            //var comunidades = _solicitudService.GetComunidades((long)id);
            //ViewBag.Comunidades = new SelectList(comunidades, "Value", "Text");

            return Json(solicitudVM);
        }

        private async Task CrearBitacora(int idSolicitud, int estado, string? comentario)
        {

            var idUsuarioClaim = User.FindFirst("IDuser")?.Value;
            if (string.IsNullOrEmpty(idUsuarioClaim) || !int.TryParse(idUsuarioClaim, out int idUsuario))
            {
                return;
            }

            Bitacora modelBitacora = new Bitacora()
            {
                IdBitacora = 0,
                Accion = estado,
                IdRegistro = idSolicitud,
                NombreTabla = "SOLICITUD",
                IdUsuario = idUsuario,
                FechaHora = DateTime.Now,
                Estado = 0,
                Comentario = comentario
            };

            await _solicitudV2Service.SaveBitacora(modelBitacora);
        }
    }
}
