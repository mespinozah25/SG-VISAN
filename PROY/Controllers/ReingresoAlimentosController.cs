using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PROY.Enum;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class ReingresoAlimentosController : Controller
    {
        #region Contructores

        private readonly IReingresoAlimentosService _reingresoAlimentosService;
        private readonly IEgresoAlimentosService _egresoAlimentosService;
        private readonly IBodegaService _bodegaService;
        private readonly ICatalogoService _catalogoService;
        private readonly IProductoAlimenticioService _productoAlimenticioService;
        private readonly IConvenioService _convenioService;
        private readonly IProgramaService _programaService;
        private readonly IPresentacionPAService _presentacionPAService;
        private readonly IUnidadMedidaService _unidadmedidaService;
        private readonly ITransportistaService _transportistaService;
        private readonly IVehiculoService _vehiculoService;
        private readonly IPilotoService _pilotoService;
        private readonly IEstibaService _estibaService;
        private readonly IUsuarioService _usuarioService;

        public ReingresoAlimentosController(IReingresoAlimentosService reingresoAlimentosService,
                                            IEgresoAlimentosService egresoAlimentosService, 
                                            IBodegaService bodegaService,
                                            ICatalogoService catalogoService,
                                            IProductoAlimenticioService productoAlimenticio,
                                            IProgramaService programaService,
                                            IPresentacionPAService presentacionPAService,
                                            IUnidadMedidaService unidadMedidaService,
                                            IEstibaService estibaService,
                                            IUsuarioService usuarioService)
        {
            _reingresoAlimentosService = reingresoAlimentosService;
            _egresoAlimentosService = egresoAlimentosService;
            _bodegaService = bodegaService;
            _catalogoService = catalogoService;
            _productoAlimenticioService = productoAlimenticio;
            _programaService = programaService;
            _presentacionPAService = presentacionPAService;
            _unidadmedidaService = unidadMedidaService;
            _estibaService = estibaService;
            _usuarioService = usuarioService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index(int? idReingresoAlimentos)
        {
            List<ReingresoAlimentos> listado = await _reingresoAlimentosService.GetAllReingresoAlimentos();

            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var estadoReingreso = await _reingresoAlimentosService.GetAllEstadoReingresos();
            ViewBag.EstadoReingresos = new SelectList(estadoReingreso, "IdEstadoReingresos", "EstadoReingreso");
            var EstadoReingresosDict = estadoReingreso.ToDictionary(i => i.IdEstadoReingresos, i => i.EstadoReingreso);

            #endregion

            if (idReingresoAlimentos == null || idReingresoAlimentos == 0)
            {
                var reingresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = new ReingresoAlimentos(),
                    listaReingresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(reingresoAlimentosVM);
            }
            else
            {
                var reingresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = new ReingresoAlimentos(),
                    listaReingresoAlimentos = listado,
                    EstadoReingresoDict = EstadoReingresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(reingresoAlimentosVM);
            }
        }

        #endregion

        #region Vista ReingresoAlimentos

        public async Task<IActionResult> ReingresoAlimentos(int? id)
        {
            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var estadoReingreso = await _reingresoAlimentosService.GetAllEstadoReingresos();
            ViewBag.EstadoReingresos = new SelectList(estadoReingreso, "IdEstadoReingresos", "EstadoReingreso");
            var EstadoReingresosDict = estadoReingreso.ToDictionary(i => i.IdEstadoReingresos, i => i.EstadoReingreso);

            #endregion

            if (id == null || id == 0)
            {
                var egresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = new ReingresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
            else
            {
                var egresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = await _reingresoAlimentosService.GetReingresoAlimentos((Int64)id),
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerReingresoAlimentos(int id)
        {
            var reingresoAlimentos = await _reingresoAlimentosService.GetReingresoAlimentos(id);
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos((Int64)reingresoAlimentos.IdEgresoAlimentos);

            if (reingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                reingresoAlimentos = reingresoAlimentos,
                egresoAlimentos = egresoAlimentos
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarReingresoAlimentos([FromForm] string modelo, IFormFile file)
        {
            // Deserializar el modelo
            var reingresoAlimentosVM = JsonConvert.DeserializeObject<ReingresoAlimentosVM>(modelo);

            ReingresoAlimentos modelReingreso = new ReingresoAlimentos()
            {
                IdReingresoAlimentos = reingresoAlimentosVM.ReingresoAlimentos.IdReingresoAlimentos,
                NoOficioReingreso = reingresoAlimentosVM.ReingresoAlimentos.NoOficioReingreso,
                IdPrograma = reingresoAlimentosVM.ReingresoAlimentos.IdPrograma,
                IdEgresoAlimentos = reingresoAlimentosVM.ReingresoAlimentos.IdEgresoAlimentos,
                FechaSolicitud = reingresoAlimentosVM.ReingresoAlimentos.FechaSolicitud,
                ArchivoOficioReingreso = reingresoAlimentosVM.ReingresoAlimentos.ArchivoOficioReingreso,
                Estado = 0,
            };

            ServiceResponse<ReingresoAlimentos> result = new ServiceResponse<ReingresoAlimentos>();
            ServiceResponse<DetalleReingresoAlimentos> resultDet = new ServiceResponse<DetalleReingresoAlimentos>();
            long idReingreso = 0;
            bool almacenamientoBitacora = modelReingreso.IdReingresoAlimentos == 0 ? true : false;

            // Almacenamiento de ReingresoAlimentos
            if (modelReingreso.IdReingresoAlimentos == 0)
            {
                result = await _reingresoAlimentosService.SaveReingresoAlimentosEstado1(modelReingreso);
                idReingreso = result.Data.IdReingresoAlimentos;
            }
            else
            {
                result = await _reingresoAlimentosService.UpdateReingresoAlimentosEstado1(modelReingreso);
                idReingreso = result.Data.IdReingresoAlimentos;

            }

            // Almacenamiento del archivo
            if (file != null && file.Length > 0)
            {
                var nuevoNombre = await AlmacenamientoDocumentos(file, (int)result.Data.IdReingresoAlimentos, "oficioReingresoAlimentos");

                modelReingreso.IdReingresoAlimentos = result.Data.IdReingresoAlimentos;
                modelReingreso.ArchivoOficioReingreso = nuevoNombre;

                await _reingresoAlimentosService.UpdateReingresoAlimentosEstado1(modelReingreso);
            }

            if (almacenamientoBitacora) // Se almacena la primer Bitácora
                CrearBitacora((int)idReingreso, (int)EstadosReingresoEnum.Inicio);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "ReingresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelReingreso);
            }
        }

        #endregion

        #region Vista RegistrarReingreso

        public async Task<IActionResult> RegistrarReingreso(int id)
        {

            ReingresoAlimentos reingreso = await _reingresoAlimentosService.GetReingresoAlimentos(id);
            var detalles = await _reingresoAlimentosService.GetAllDetalleReingresoAlimentos(id);

            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var estadoReingreso = await _reingresoAlimentosService.GetAllEstadoReingresos();
            ViewBag.EstadoReingresos = new SelectList(estadoReingreso, "IdEstadoReingresos", "EstadoReingreso");
            var EstadoReingresosDict = estadoReingreso.ToDictionary(i => i.IdEstadoReingresos, i => i.EstadoReingreso);

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);

            var Estiba = await _estibaService.GetAll();
            ViewBag.Estiba = new SelectList(Estiba, "IdEstiba", "CodigoEstiba");
            var EstibaDict = Estiba.ToDictionary(i => i.IdEstiba, i => i.CodigoEstiba);

            #endregion

            if (id == null || id == 0)
            {
                var egresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = new ReingresoAlimentos(),
                    listaDetalleReingresoAlimentos = detalles,
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    EstibaDict = EstibaDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
            else
            {
                var egresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = reingreso,
                    listaDetalleReingresoAlimentos = detalles,
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    EstibaDict = EstibaDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerRegistroReingreso(int id)
        {
            var reingresoAlimentos = await _reingresoAlimentosService.GetReingresoAlimentos(id);
            var detalles = await _reingresoAlimentosService.GetAllDetalleReingresoAlimentos(id);

            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos((Int64)reingresoAlimentos.IdEgresoAlimentos);

            if (reingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                reingresoAlimentos = reingresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == reingresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                detReingresoAlimentos = detalles,
                egresoAlimentos = egresoAlimentos
            });
        }

        [HttpPost]
        public async Task<IActionResult> ObtenerListEgresos([FromBody] dynamic data)
        {
            string term = data.GetProperty("term").GetString();

            var list = await _reingresoAlimentosService.GetEgresosLike(term, 15);

            return Ok(list); // Retornar un JSON válido
        }

        public async Task<IActionResult> ObtenerEgresoAlimentos(int id)
        {
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos(id);

            if (egresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                egresoAlimentos = egresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == egresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                departamentoCarga = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoCarga).Select(o => o.Descripcion).FirstOrDefault(),
                departamentoDestino = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestino = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                bodegaCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.NombreBodega).FirstOrDefault(),
            });
        }

        public IActionResult ObtenerUsuariosEncargado()
        {
            var idRol = _usuarioService.GetAllRoles().Result.Where(o => o.Nombre == "Encargado de Bodega").Select(s => s.IdRol).First();
            var usuarios = _usuarioService.GetAll().Result.Where(o => o.RolId == idRol).Select(s => new { s.IdUsuario, s.Nombre, s.Apellidos }).ToList();

            if (usuarios == null)
            {
                return NotFound();
            }

            return Json(new
            {
                usuarios
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarRegistroReingreso([FromForm] string modelo, IFormFile file)
        {
            // Deserializar el modelo
            var egresoAlimentosVM = JsonConvert.DeserializeObject<ReingresoAlimentosVM>(modelo);

            ReingresoAlimentos modelReingreso = new ReingresoAlimentos()
            {
                IdReingresoAlimentos = egresoAlimentosVM.ReingresoAlimentos.IdReingresoAlimentos,
                IdEgresoAlimentos = egresoAlimentosVM.ReingresoAlimentos.IdEgresoAlimentos,
                ObservacionFisicaIndeca = egresoAlimentosVM.ReingresoAlimentos.ObservacionFisicaIndeca
            };

            ServiceResponse<ReingresoAlimentos> result = new();
            ServiceResponse<DetalleReingresoAlimentos> resultDet = new();
            long idReingresoAlimentos = 0;
            var nombreArtchivo = "";

            if (file != null && file.Length > 0)
            {
                nombreArtchivo = await AlmacenamientoDocumentos(file, (int)modelReingreso.IdReingresoAlimentos, "despachoAlmacenReingreso");

                modelReingreso.ArchivoDespachoAlmacenReingreso = nombreArtchivo;
            }

            // Almacenamiento de ReingresoAlimentos
            result = await _reingresoAlimentosService.UpdateReingresoAlimentosEstado2(modelReingreso);
            idReingresoAlimentos = result.Data.IdReingresoAlimentos;

            // Almacenamiento de los detalles
            foreach (var detalle in egresoAlimentosVM.listaDetalleReingresoAlimentos)
            {
                DetalleReingresoAlimentos nuevoDetalle = new DetalleReingresoAlimentos
                {
                    IdDetalleReingresoAlimentos = detalle.IdDetalleReingresoAlimentos,
                    IdReingresoAlimentos = detalle.IdReingresoAlimentos,
                    IdProductoAlimenticio = detalle.IdProductoAlimenticio,
                    IdPresentacion = detalle.IdPresentacion,
                    CantidadUnidad = detalle.CantidadUnidad,
                    IdUnidadMedida = detalle.IdUnidadMedida,
                    Cantidad = detalle.Cantidad,
                    FechaVencimiento = detalle.FechaVencimiento,
                    Estiba = "Reingreso",
                    Estado = 0
                };

                if (nuevoDetalle.IdDetalleReingresoAlimentos == 0)
                    resultDet = await _reingresoAlimentosService.SaveDetalleReingresoAlimentos(nuevoDetalle);
                else
                    resultDet = await _reingresoAlimentosService.UpdateDetalleReingresoAlimentos(nuevoDetalle);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "ReingresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelReingreso);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalleReingresoAlimentos(Int64 id)
        {
            var result = await _reingresoAlimentosService.DeleteDetalleReingresoAlimentos(id);

            if (result.Success && result.Data != null)
            {
                return Json(new
                {
                    mensaje = result.Message,
                    codigo = result.Success
                });
            }
            else
            {
                return Json(new
                {
                    mensaje = result.Message,
                    codigo = result.Success
                });
            }
        }

        #endregion

        #region Vista ReingresoFinalizad

        public async Task<IActionResult> ReingresoFinalizado(int id)
        {

            ReingresoAlimentos reingreso = await _reingresoAlimentosService.GetReingresoAlimentos(id);
            var detalles = await _reingresoAlimentosService.GetAllDetalleReingresoAlimentos(id);

            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var estadoReingreso = await _reingresoAlimentosService.GetAllEstadoReingresos();
            ViewBag.EstadoReingresos = new SelectList(estadoReingreso, "IdEstadoReingresos", "EstadoReingreso");
            var EstadoReingresosDict = estadoReingreso.ToDictionary(i => i.IdEstadoReingresos, i => i.EstadoReingreso);

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);

            var Estiba = await _estibaService.GetAll();
            ViewBag.Estiba = new SelectList(Estiba, "IdEstiba", "CodigoEstiba");
            var EstibaDict = Estiba.ToDictionary(i => i.IdEstiba, i => i.CodigoEstiba);

            #endregion

            if (id == null || id == 0)
            {
                var egresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = new ReingresoAlimentos(),
                    listaDetalleReingresoAlimentos = detalles,
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    EstibaDict = EstibaDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
            else
            {
                var egresoAlimentosVM = new ReingresoAlimentosVM()
                {
                    ReingresoAlimentos = reingreso,
                    listaDetalleReingresoAlimentos = detalles,
                    ProgramaDict = ProgramaDict,
                    EstadoReingresoDict = EstadoReingresosDict,
                    EstibaDict = EstibaDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerReingresoFinalizado(int id)
        {
            var reingresoAlimentos = await _reingresoAlimentosService.GetReingresoAlimentos(id);
            var detalles = await _reingresoAlimentosService.GetAllDetalleReingresoAlimentos(id);
            var bitacoraEstado1 = await _reingresoAlimentosService.GetBitacoraEstado((int)EstadosReingresoEnum.Inicio, id);
            var bitacoraEstado2 = await _reingresoAlimentosService.GetBitacoraEstado((int)EstadosReingresoEnum.EnBodega, id);

            Usuario? usuarioEstado1 = new();
            if (bitacoraEstado1 != null)
                usuarioEstado1 = await _usuarioService.GetUsuario((Int64)bitacoraEstado1.IdUsuario);

            Usuario? usuarioEstado2 = new();
            if (bitacoraEstado2 != null)
                usuarioEstado2 = await _usuarioService.GetUsuario((Int64)bitacoraEstado2.IdUsuario);

            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos((Int64)reingresoAlimentos.IdEgresoAlimentos);

            if (reingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                reingresoAlimentos = reingresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == reingresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                detReingresoAlimentos = detalles,
                egresoAlimentos = egresoAlimentos,
                bitacoraEstado1 = bitacoraEstado1,
                usuarioEstado1 = usuarioEstado1,
                bitacoraEstado2 = bitacoraEstado2,
                usuarioEstado2 = usuarioEstado2
            });
        }

        #endregion

        #region || Cambio de Estados || 

        public async Task<IActionResult> CambioEstado(int id, int estado, int? idUsuario)
        {
            // estado = refleja el nuevo estado al que se transiciona la solicitud
            switch (estado)
            {
                case (int)EstadosReingresoEnum.EnBodega:

                    // Se asigna el reingreso al usuario seleccionado
                    var respAsig = await _reingresoAlimentosService.AsignacionUsuarioEncargado(id, (int)idUsuario);

                    break;
                case (int)EstadosReingresoEnum.ReingresoFinalizado:

                    var detalles = await _reingresoAlimentosService.GetAllDetalleReingresoAlimentos(id);

                    if (detalles.Count == 0)
                    {
                        TempData["Mensaje"] = "No se Permite la finalización del Reingreso - No se han ingresado los detalles";
                        return RedirectToAction("Index");
                    }

                    // Se crea la Bitácora del estado del que está saliendo
                    CrearBitacora(id, (int)EstadosReingresoEnum.EnBodega);

                    // Se crea copia exacta de todos los tados ingresados de la tabla PRODUCTOS_INGRESO hacia MOVIMIENTOS_PRODUCTOS
                    var resp = await _reingresoAlimentosService.MoverProductosReingresoAMovimientos(id);

                    // Se crea copia exacta de todos los tados ingresados de la tabla PRODUCTOS_INGRESO hacia KARDEX
                    var resp2 = await _reingresoAlimentosService.MoverProductosReingresoAKardex(id);

                    break;

            }

            var result = await _reingresoAlimentosService.CambioEstadoReingresoAlimentos(id, estado);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "ReingresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "ReingresoAlimentos");
            }
        }

        #endregion

        #region || Procesamiento Documentos || 

        public IActionResult DownloadFile(string fileName, string carpeta)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", carpeta, fileName);

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
                case ".pdf":
                    return "application/pdf";
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

        private static async Task<string> AlmacenamientoDocumentos(IFormFile file, int idRegistro, string nombreCarpeta)
        {
            // Almacenamiento del archivo

            // Obtenemos la extensión del archivo
            var extension = Path.GetExtension(file.FileName);
            // Obtenemos el nombre del archivo sin la extensión
            var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
            // Obtenemos la fechaActual y Hora
            var fechaFormateada = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
            // Concatenamos el ID al nombre antes de la extensión
            var nuevoNombre = $"{nombreSinExtension}_{idRegistro}_{fechaFormateada}{extension}";

            var nombreDoc = nuevoNombre;

            // Ruta donde se guardará el archivo
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + nombreCarpeta, nuevoNombre);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return nombreDoc;
        }

        #endregion

        #region || Creación de Bitácora ||  

        public async void CrearBitacora(int idReinngresoAlimentos, int estado)
        {
            Bitacora modelBitacota = new Bitacora()
            {
                IdBitacora = 0,
                Accion = estado,
                IdRegistro = idReinngresoAlimentos,
                NombreTabla = "REINGRESO_ALIMENTOS",
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                FechaHora = DateTime.Now,
                Estado = 0
            };

            var resultBit = await _reingresoAlimentosService.SaveBitacora(modelBitacota);
        }

        #endregion
    }
}
