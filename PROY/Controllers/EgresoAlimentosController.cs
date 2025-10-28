using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PROY.Enum;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class EgresoAlimentosController : Controller
    {
        #region Contructores

        private readonly IEgresoAlimentosService _egresoAlimentosService;
        private readonly IIngresoAlimentosService _ingresoAlimentosService;
        private readonly IBodegaService _bodegaService;
        private readonly ICatalogoService _catalogoService;
        private readonly IProductoAlimenticioService _productoAlimenticioService;
        private readonly IRacionService _raciongaService;
        private readonly IConvenioService _convenioService;
        private readonly IProgramaService _programaService;
        private readonly IPresentacionPAService _presentacionPAService;
        private readonly IUnidadMedidaService _unidadmedidaService;
        private readonly ITransportistaService _transportistaService;
        private readonly IVehiculoService _vehiculoService;
        private readonly IPilotoService _pilotoService;
        private readonly ITipoAsistenciaService _tipoAsistenciaService;
        private readonly IUsuarioService _usuarioService;
        private readonly ISolicitudService _solicitudService;


        public EgresoAlimentosController(IEgresoAlimentosService egresoAlimentosService,
                                         IIngresoAlimentosService ingresoAlimentosService,
                                         IBodegaService bodegaService,
                                         ICatalogoService catalogoService,
                                         IProductoAlimenticioService productoAlimenticio,
                                         IConvenioService convenioService,
                                         IProgramaService programaService,
                                         IPresentacionPAService presentacionPAService,
                                         IUnidadMedidaService unidadMedidaService,
                                         ITransportistaService transportistaService,
                                         IVehiculoService vehiculoService,
                                         IPilotoService pilotoService,
                                         ITipoAsistenciaService tipoAsistenciaService,
                                         IUsuarioService usuarioService,
                                         IRacionService raciongaService,
                                         ISolicitudService solicitudService)
        {
            _egresoAlimentosService = egresoAlimentosService;
            _ingresoAlimentosService = ingresoAlimentosService;
            _bodegaService = bodegaService;
            _catalogoService = catalogoService;
            _productoAlimenticioService = productoAlimenticio;
            _convenioService = convenioService;
            _programaService = programaService;
            _presentacionPAService = presentacionPAService;
            _unidadmedidaService = unidadMedidaService;
            _transportistaService = transportistaService;
            _vehiculoService = vehiculoService;
            _pilotoService = pilotoService;
            _tipoAsistenciaService = tipoAsistenciaService;
            _usuarioService = usuarioService;
            _raciongaService = raciongaService;
            _solicitudService = solicitudService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index(int? idEgresoAlimentos)
        {
            List<EgresoAlimentos> listado = await _egresoAlimentosService.GetAllEgresoAlimentos();
            List<TrasladoAlimentos> listadoTraslados = await _egresoAlimentosService.GetAllTrasladoAlimentos();
            var listBitacoras = await _egresoAlimentosService.GetAllBitacoraEstado((int)EstadosIngresoEnum.Inicio);

            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var Convenio = await _convenioService.GetAll();
            ViewBag.Convenios = new SelectList(Convenio, "IdConvenio", "NombreConvenio");
            var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var estadoTraslado = await _egresoAlimentosService.GetAllEstadoTraslados();
            ViewBag.EstadoTraslados = new SelectList(estadoTraslado, "IdEstadoTraslados", "EstadoTraslado");
            var EstadoTrasladosDict = estadoTraslado.ToDictionary(i => i.IdEstadoTraslados, i => i.EstadoTraslado);

            var tipoAsistencia = await _tipoAsistenciaService.GetAll();
            ViewBag.TipoAsistencias = new SelectList(tipoAsistencia, "IdTipoAsistencia", "NombreTipoAsistencia");
            var TipoAsistenciaDict = tipoAsistencia.ToDictionary(i => i.IdTipoAsistencia, i => i.NombreTipoAsistencia);

            var solicitudes = await _solicitudService.GetSolicitudesProgramacion();
            ViewBag.Solicitudes = new SelectList(solicitudes, "IdSolicitud", "Descripcion");
            var SolicitudesDict = solicitudes.ToDictionary(i => i.IdSolicitud, i => i.Descripcion);

            var idRolLogistica = _usuarioService.GetAllRoles().Result.Where(o => o.Nombre == "Operador de Logistica").Select(s => s.IdRol).First();
            var usuariosLogistica = _usuarioService.GetAll().Result.Where(o => o.RolId == idRolLogistica);
            ViewBag.Usuarios = new SelectList(usuariosLogistica, "IdUsuario", "EstadoTraslado");
            var UsuarioDict = usuariosLogistica.ToDictionary(i => i.IdUsuario, i => i.Nombre);

            #endregion

            if (idEgresoAlimentos == null || idEgresoAlimentos == 0)
            {
                var egresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    listaEgresoAlimentos = listado,
                    listaTrasladoAlimentos = listadoTraslados,
                    listaBitacoraInicio = listBitacoras,
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    SolicitudesDict = SolicitudesDict,
                    TipoAsistenciaDict = TipoAsistenciaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    EstadoTrasladoDict = EstadoTrasladosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(egresoAlimentosVM);
            }
            else
            {
                var egresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    listaEgresoAlimentos = listado,
                    listaTrasladoAlimentos = listadoTraslados,
                    listaBitacoraInicio = listBitacoras,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(egresoAlimentosVM);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSolicitudesProgramacion()
        {
            try
            {
                var solicitudes = await _solicitudService.GetSolicitudesProgramacion();

                // Aca pasamos los datos en lsita de la base de datos
                var resultado = solicitudes.Select(s => new {
                    idSolicitud = s.IdSolicitud,
                    descripcion = s.Descripcion,
                    fechaIngresoDespacho = s.FechaIngresoDespacho.ToString("yyyy-MM-dd"),
                    depto = s.Depto,
                    idIntervencion = s.IdIntervencion,
                    raciones = s.Raciones
                }).ToList();

                return Json(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al obtener solicitudes",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetDatosSolicitud(long id)
        {
            try
            {
                var solicitud = await _solicitudService.GetSolicitud(id);

                if (solicitud == null || solicitud.Estado != "PROGRAMACION" || solicitud.Restrictiva != 0 || solicitud.EstadoEgreso != 0)
                    return Json(new { success = false, message = "Solicitud no disponible" });

                return Json(new
                {
                    success = true,
                    fechaIngresoDespacho = solicitud.FechaIngresoDespacho.ToString("yyyy-MM-dd"),
                    depto = solicitud.Depto,
                    raciones = solicitud.Raciones,
                    idIntervencion = solicitud.IdIntervencion,
                    descripcion = solicitud.Descripcion
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error al obtener la solicitud",
                    error = ex.Message
                });
            }
        }

        public IActionResult ObtenerUsuariosLogistica()
        {
            var idRolLogistica = _usuarioService.GetAllRoles().Result.Where(o => o.Nombre == "Operador de Logística").Select(s => s.IdRol).First();
            var usuariosLogistica = _usuarioService.GetAll().Result.Where(o => o.RolId == idRolLogistica).Select(s => new { s.IdUsuario, s.Nombre, s.Apellidos }).ToList();

            if (usuariosLogistica == null)
            {
                return NotFound();
            }

            return Json(new
            {
                usuariosLogistica
            });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarEgresoAlimentos(Int64 id)
        {
            var result = await _egresoAlimentosService.DeleteEgresoAlimentos(id);

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

        #region Vista EgresoAlimentos

        public async Task<IActionResult> EgresoAlimentos(int? idEgresoAlimentos)
        {
            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var Convenio = await _convenioService.GetAll();
            ViewBag.Convenios = new SelectList(Convenio, "IdConvenio", "NombreConvenio");
            var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var tipoAsistencia = await _tipoAsistenciaService.GetAll();
            ViewBag.TipoAsistencias = new SelectList(tipoAsistencia, "IdTipoAsistencia", "NombreTipoAsistencia");
            var TipoAsistenciaDict = tipoAsistencia.ToDictionary(i => i.IdTipoAsistencia, i => i.NombreTipoAsistencia);

            #endregion

            if (idEgresoAlimentos == null || idEgresoAlimentos == 0)
            {
                var egresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    TipoAsistenciaDict = TipoAsistenciaDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
            else
            {
                var egresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos((Int64)idEgresoAlimentos),
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    TipoAsistenciaDict = TipoAsistenciaDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(egresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerEgresoAlimentos(int id)
        {
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos(id);
            var detallesRacion = await _egresoAlimentosService.GetAllDetallesRacionXConvenio((int)egresoAlimentos.IdConvenio);

            if (egresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                egresoAlimentos = egresoAlimentos,
                detallesRacion = detallesRacion
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarEgresoAlimentos([FromBody] EgresoAlimentosVM modelo)
        {
            var tipoAsistenciaMap = new Dictionary<int, int>
            {
                { 7, 21 },   // INSAN
                { 2, 3 },    // INSAN EMERGENCIA
                { 11, 4 },   // MEDIDA JUDICIAL
                { 3, 5 },    // MEDIDA CAUTELAR
                { 1, 6 },    // MEDIDA TRANSITORIA
                { 12, 8 },   // POLÍTICA PÚBLICA
                { 4, 22 },   // NDA
                { 10, 9 },   // ALIMENTOS POR ACCIONES
                { 13, 7 }    // RESERVA ESTRATÉGICA
            };

            int idCatalogo = (int)modelo.EgresoAlimentos.IdTipoAsistencia;

            int tipoAsistenciaFinal = tipoAsistenciaMap.ContainsKey(idCatalogo)
            ? tipoAsistenciaMap[idCatalogo]
            : idCatalogo;  // si no está en el diccionario, agrega el valor del catalogo

            EgresoAlimentos modelEgreso = new EgresoAlimentos()
            {
                IdEgresoAlimentos = modelo.EgresoAlimentos.IdEgresoAlimentos,
                IdPrograma = modelo.EgresoAlimentos.IdPrograma,
                IdConvenio = modelo.EgresoAlimentos.IdConvenio,
                FechaSolicitud = modelo.EgresoAlimentos.FechaSolicitud,
                TotalRaciones = modelo.EgresoAlimentos.TotalRaciones,
                //IdTipoAsistencia = modelo.EgresoAlimentos.IdTipoAsistencia,
                IdTipoAsistencia = tipoAsistenciaFinal,
                IdTipoSalidaAlimento = modelo.EgresoAlimentos.IdTipoSalidaAlimento,
                Estado = 0
            };

            ServiceResponse<EgresoAlimentos> result = new ServiceResponse<EgresoAlimentos>();
            ServiceResponse<Bitacora> resultBit = new ServiceResponse<Bitacora>();
            long idIngresoAlimentos = 0;

            // Almacenamiento de IngresoAlimentos
            if (modelEgreso.IdEgresoAlimentos == 0)
            {
                result = await _egresoAlimentosService.SaveEgresoAlimentosEstado1(modelEgreso);
                idIngresoAlimentos = result.Data.IdEgresoAlimentos;

                // En el primer almacenamiento, se guarda la bitácora
                CrearBitacora((int)idIngresoAlimentos, (int)EstadosEgresoEnum.Inicio);
            }
            else
            {
                result = await _egresoAlimentosService.UpdateEgresoAlimentosEstado1(modelEgreso);
                idIngresoAlimentos = result.Data.IdEgresoAlimentos;

            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelEgreso);
            }
        }

        [HttpPost]
        public IActionResult ActualizarEstadoSolicitud(int idSolicitud)
        {
            try
            {
                _solicitudService.ActualizarEstadoEgreso(idSolicitud, 1); // Cambiar estado a 1 (egresado)
                return Json(new { success = true, mensaje = "Estado actualizado." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, mensaje = "Error: " + ex.Message });
            }
        }

        #endregion

        #region Vista Despacho de Almacén

        public async Task<IActionResult> DespachoAlmacen(int? id, int? type)
        {
            EgresoAlimentos listado = await _egresoAlimentosService.GetEgresoAlimentos((int)id);

            #region Catálogos

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);


            #endregion

            if (id == null || id == 0)
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
            else
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerDespachoAlmacen(int id)
        {
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos(id);
            var detalles = await _egresoAlimentosService.GetAllDetalleEgresoAlimentos(id);
            var detallesRacion = await _egresoAlimentosService.GetAllDetallesRacionXConvenio((int)egresoAlimentos.IdConvenio);
            var idDepartamentoCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.IdDepartamento).FirstOrDefault();
            var idMunicipioDestino = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaDestino).Select(o => o.IdMunicipio).FirstOrDefault();


            if (egresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            { 
                egresoAlimentos = egresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == egresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                convenio = _convenioService.GetAll().Result.Where(o => o.IdConvenio == egresoAlimentos.IdConvenio).Select(o => o.NombreConvenio).FirstOrDefault(),
                departamentoCarga = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == idDepartamentoCarga).Select(o => o.Descripcion).FirstOrDefault(),
                departamentoDestino = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestinoTraslado = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == idMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestinoEgreso = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                bodegaDestino = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaDestino).Select(o => o.NombreBodega).FirstOrDefault(),
                bodegaCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.NombreBodega).FirstOrDefault(),
                tipoAsistencia = _tipoAsistenciaService.GetAll().Result.Where(o => o.IdTipoAsistencia == egresoAlimentos.IdTipoAsistencia).Select(o => o.NombreTipoAsistencia).FirstOrDefault(),
                detEgresoAlimentos = detalles,
                detRacion = detallesRacion
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarDespachoAlmacen([FromBody] EgresoAlimentosVM modelo)
        {
            var estadoActual = modelo.EgresoAlimentos.IdEstadoEgreso;

            EgresoAlimentos modelEgreso = new EgresoAlimentos()
            {
                IdEgresoAlimentos = modelo.EgresoAlimentos.IdEgresoAlimentos,
                NoDespachoAlmacen = modelo.EgresoAlimentos.NoDespachoAlmacen,
                IdDepartamentoCarga = null,
                IdBodegaCarga = modelo.EgresoAlimentos.IdBodegaCarga,
                NoAutorizacion = modelo.EgresoAlimentos.NoAutorizacion,
                IdDepartamentoDestino = modelo.EgresoAlimentos.IdDepartamentoDestino,
                IdMunicipioDestino = modelo.EgresoAlimentos.IdMunicipioDestino,
                IdBodegaDestino = modelo.EgresoAlimentos.IdBodegaDestino,
                DireccionFinal = modelo.EgresoAlimentos.DireccionFinal,
                Comentarios = modelo.EgresoAlimentos.Comentarios,
                FirmaSelloDirectorVisan = modelo.EgresoAlimentos.FirmaSelloDirectorVisan,
                FirmaSelloJefeDepartamento = modelo.EgresoAlimentos.FirmaSelloJefeDepartamento,
                IdUsuarioEncargadoBodega = modelo.EgresoAlimentos.IdUsuarioEncargadoBodega,
                Estado = 0,
            };

            ServiceResponse<EgresoAlimentos> result = new ServiceResponse<EgresoAlimentos>();
            ServiceResponse<DetalleEgresoAlimentos> resultDet = new ServiceResponse<DetalleEgresoAlimentos>();
            long idEgresoAlimentos = 0;

            // Almacenamiento de EgresoAlimentos
            result = await _egresoAlimentosService.UpdateEgresoAlimentosEstado2(modelEgreso);
            idEgresoAlimentos = result.Data.IdEgresoAlimentos;

            // En el segundo almacenamiento, se cambia el estado a 3
            //CambioEstado((int)idEgresoAlimentos, (int)EstadosEgresoEnum.EnBodega, null, null);

            

            // Almacenamiento de los detalles
            foreach (var detalle in modelo.listaDetalleEgresoAlimentos)
            {
                DetalleEgresoAlimentos nuevoDetalle = new DetalleEgresoAlimentos
                {
                    IdDetalleEgresoAlimentos = detalle.IdDetalleEgresoAlimentos,
                    IdEgresoAlimentos = (int?)idEgresoAlimentos,
                    IdProductoAlimenticio = detalle.IdProductoAlimenticio,
                    IdPresentacion = null,
                    CantidadUnidad = detalle.CantidadUnidad,
                    IdUnidadMedida = detalle.IdUnidadMedida,
                    NoUnidades = detalle.NoUnidades,
                    Estado = 0
                };

                if (nuevoDetalle.IdDetalleEgresoAlimentos == 0 && detalle.CheckRow == 0) // ID de egreso 0 y check seleccionado -- Almacena
                    resultDet = await _egresoAlimentosService.SaveDetalleEgresoAlimentos(nuevoDetalle);

                if (nuevoDetalle.IdDetalleEgresoAlimentos != 0 && detalle.CheckRow == 0) // ID egreso con valor y check seleccionado -- Update
                    resultDet = await _egresoAlimentosService.UpdateDetalleEgresoAlimentos(nuevoDetalle);

                if (nuevoDetalle.IdDetalleEgresoAlimentos != 0 && detalle.CheckRow == 1) // ID egreso con valor y check NO seleccionado -- Delete
                    resultDet = await _egresoAlimentosService.DeleteDetalleEgresoAlimentos(nuevoDetalle.IdDetalleEgresoAlimentos);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelEgreso);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalleEgresoAlimentos(Int64 id)
        {
            var result = await _egresoAlimentosService.DeleteDetalleEgresoAlimentos(id);

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

        #region Vista Revison Despacho Almacén

        public async Task<IActionResult> RevisionDespachoAlmacen(int? id, int? type)
        {
            EgresoAlimentos listado = await _egresoAlimentosService.GetEgresoAlimentos((int)id);

            #region Catálogos

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);


            #endregion

            if (id == null || id == 0)
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
            else
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
        }

        public IActionResult ObtenerUsuariosEncargadosBodega()
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
        public async Task<IActionResult> GuardarRevisionDespachoAlmacen([FromBody] EgresoAlimentosVM modelo)
        {
            EgresoAlimentos modelEgreso = new EgresoAlimentos()
            {
                IdEgresoAlimentos = modelo.EgresoAlimentos.IdEgresoAlimentos,
                FirmaSelloJefeDepartamento = modelo.EgresoAlimentos.FirmaSelloJefeDepartamento,
                FirmaSelloDirectorVisan = modelo.EgresoAlimentos.FirmaSelloDirectorVisan,
                IdUsuarioEncargadoBodega = modelo.EgresoAlimentos.IdUsuarioEncargadoBodega,
                Estado = 0,
            };

            ServiceResponse<EgresoAlimentos> result = new ServiceResponse<EgresoAlimentos>();
            long idEgresoAlimentos = 0;
            
            // Almacenamiento de EgresoAlimentos
            result = await _egresoAlimentosService.UpdateEgresoAlimentosEstado3(modelEgreso);
            idEgresoAlimentos = result.Data.IdEgresoAlimentos;


            // Indicador si se aprueba o rechaza la solicitud
            if (modelo.ApruebaRechazo)
                CambioEstado((int)idEgresoAlimentos, (int)EstadosEgresoEnum.ApruebaEgresoDeAlimentos, null, null); // rechazado se cambia el estado a 4
            else
                CambioEstado((int)idEgresoAlimentos, (int)EstadosEgresoEnum.RechazoEgresoDeAlimentos, null, null); // aceptado se cambia el estado a 5


            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelEgreso);
            }
        }

        #endregion

        #region Vista Revison Egreso

        public async Task<IActionResult> RevisionEgreso(int? id, int? type)
        {
            EgresoAlimentos listado = await _egresoAlimentosService.GetEgresoAlimentos((int)id);

            #region Catálogos

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);


            #endregion

            if (id == null || id == 0)
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
            else
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarRevisionEgreso([FromForm] string modelo, IFormFile file, bool condicionSanitaria)
        {
            // Deserializar el modelo
            var egresoAlimentosVM = JsonConvert.DeserializeObject<EgresoAlimentosVM>(modelo);

            EgresoAlimentos modelEgreso = new EgresoAlimentos()
            {
                IdEgresoAlimentos = egresoAlimentosVM.EgresoAlimentos.IdEgresoAlimentos,
                FirmaSelloJefeBodegas = egresoAlimentosVM.EgresoAlimentos.FirmaSelloJefeBodegas,
                ObservCondSanitariaTransp = egresoAlimentosVM.EgresoAlimentos.ObservCondSanitariaTransp
            };

            ServiceResponse<EgresoAlimentos> result = new ServiceResponse<EgresoAlimentos>();
            long idEgresoAlimentos = 0;
            var nombreArtchivo = "";

            if (file != null && file.Length > 0)
            {
                nombreArtchivo = await AlmacenamientoDocumentos(file, (int)modelEgreso.IdEgresoAlimentos, "despachoAlmacen");
            }

            modelEgreso.ArchivoDespachoAlmacen = nombreArtchivo;

            // Almacenamiento de EgresoAlimentos
            result = await _egresoAlimentosService.UpdateEgresoAlimentosEstado5(modelEgreso);
            idEgresoAlimentos = result.Data.IdEgresoAlimentos;


            // En el escenario en el que sea true, se cambia a estado 7, si es false se rechaza a estado 6
            if (condicionSanitaria)
                CambioEstado((int)idEgresoAlimentos, (int)EstadosEgresoEnum.ApruebaEgresoDeAlimentos, null, null); // aprobado se cambia el estado a 7
            else
                CambioEstado((int)idEgresoAlimentos, (int)EstadosEgresoEnum.RechazoEgresoDeAlimentos, null, null); // rechazado se cambia el estado a 6


            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelEgreso);
            }
        }

        #endregion

        #region Vista Finalización Egreso

        public async Task<IActionResult> FinalizacionDespacho(int? id, int? type)
        {
            EgresoAlimentos listado = await _egresoAlimentosService.GetEgresoAlimentos((int)id);

            #region Catálogos

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var pilotos = await _pilotoService.GetAll();
            ViewBag.Pilotos = new SelectList(pilotos, "IdPiloto", "NombresPiloto");
            var PilotosDict = pilotos.ToDictionary(i => i.IdPiloto, i => i.NombresPiloto );

            var transportista = await _transportistaService.GetAll();
            ViewBag.Transportistas = new SelectList(transportista, "IdTransportista", "NombreTransportista");
            var TransportistaDict = transportista.ToDictionary(i => i.IdTransportista, i => i.NombreTransportista);

            var vehiculos = await _vehiculoService.GetAll();
            ViewBag.Vehiculos = new SelectList(vehiculos, "IdVehiculo", "PlacaPlataforma");
            var VehiculosDict = vehiculos.ToDictionary(i => i.IdVehiculo, i => i.PlacaPlataforma);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);


            #endregion

            if (id == null || id == 0)
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    TransportistaDict = TransportistaDict,
                    VehiculoDict = VehiculosDict,
                    PilotoDict = PilotosDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
            else
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    TransportistaDict = TransportistaDict,
                    VehiculoDict = VehiculosDict,
                    PilotoDict = PilotosDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerFinalizacionDespacho(int id)
        {
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos(id);
            var detalles = await _egresoAlimentosService.GetAllDetalleEgresoAlimentos(id);
            var idDepartamentoCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.IdDepartamento).FirstOrDefault();

            if (egresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                egresoAlimentos = egresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == egresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                departamentoCarga = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == idDepartamentoCarga).Select(o => o.Descripcion).FirstOrDefault(),
                departamentoDestino = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestino = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                bodegaDestino = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaDestino).Select(o => o.NombreBodega).FirstOrDefault(),
                bodegaCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.NombreBodega).FirstOrDefault(),
                detEgresoAlimentos = detalles
            });
        }

        public async Task<IActionResult> ObtenerSolicitudesSalida(int id)
        {
            var salidas = await _egresoAlimentosService.GetAllSolicitudSalidaAlimentos(id);

            var listDetallesSalidas = new List<DetalleSalidaAlimentos>();

            // Se agregan todos los detalles con los que cuenta para calcular las cantidades de los productos que se deben de agregar para la salida
            if (salidas.Count() > 0)
            {
                foreach (var salida in salidas)
                {
                    var detalle = await _egresoAlimentosService.GetAllDetalleSalidaAlimentos(salida.IdSolicitudSalidaAlimentos);

                    foreach(var det in detalle)
                    {
                        listDetallesSalidas.Add(det);
                    }
                }
            }

            if (salidas == null)
            {
                return NotFound();
            }

            return Json(new
            {
                listaSolicitudSalidaAlimentos = salidas,
                listaDetalleSalidaAlimentos = listDetallesSalidas
            });
        }

        public async Task<IActionResult> ObtenerDetallesSolicitudSalida(int id)
        {
            var solicitud = await _egresoAlimentosService.GetSolicitudSalidaAlimentos(id);
            var detalles = await _egresoAlimentosService.GetAllDetalleSalidaAlimentos(solicitud.IdSolicitudSalidaAlimentos);

            if (solicitud == null)
            {
                return NotFound();
            }

            return Json(new
            {
                solicitudSalida = solicitud,
                listaSolicitudSalidaAlimentos = detalles,
                placas = _vehiculoService.GetAll().Result.Where(o => o.IdVehiculo == solicitud.IdVehiculo).Select(o => o.PlacaPlataforma),
                nombrePiloto = _pilotoService.GetAll().Result.Where(o => o.IdPiloto == solicitud.IdPiloto).Select(o => o.NombresPiloto),
            });
        }

        public async Task<IActionResult> ObtenerDatosPiloto(int id)
        {
            var piloto = await _pilotoService.GetPiloto(id);

            if (piloto == null)
            {
                return NotFound();
            }

            return Json(new
            {
                piloto = piloto
            });
        }

        public async Task<IActionResult> ObtenerDatosVehiculo(int id)
        {
            var piloto = await _vehiculoService.GetVehiculo(id);

            if (piloto == null)
            {
                return NotFound();
            }

            return Json(new
            {
                vehiculo = piloto
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarConstanciaSalida([FromBody] EgresoAlimentosVM modelo)
        {
            SolicitudSalidaAlimentos modelSalida = new SolicitudSalidaAlimentos()
            {
                IdSolicitudSalidaAlimentos = 0,
                IdEgresoAlimentos = modelo.SolicitudSalidaAlimentos.IdEgresoAlimentos,
                NoConstancia = modelo.SolicitudSalidaAlimentos.NoConstancia,
                FechaDespacho = modelo.SolicitudSalidaAlimentos.FechaDespacho,
                IdPrograma = modelo.SolicitudSalidaAlimentos.IdPrograma,
                IdTransportista = modelo.SolicitudSalidaAlimentos.IdTransportista,
                IdPiloto = modelo.SolicitudSalidaAlimentos.IdPiloto,
                IdVehiculo = modelo.SolicitudSalidaAlimentos.IdVehiculo,
                Estado = 0,

                // Nombres
                NombreTransportista = modelo.SolicitudSalidaAlimentos.NombreTransportista.ToUpper().Trim(),
                PlacasTransporte = modelo.SolicitudSalidaAlimentos.PlacasTransporte.ToUpper().Trim(),
                NoTrailer = modelo.SolicitudSalidaAlimentos.NoTrailer.ToUpper().Trim(),
                NombrePiloto = modelo.SolicitudSalidaAlimentos.NombrePiloto.ToUpper().Trim(),
                NoLicencia = modelo.SolicitudSalidaAlimentos.NoLicencia.ToUpper().Trim(),
            };

            ServiceResponse<SolicitudSalidaAlimentos> result = new();
            ServiceResponse<DetalleSalidaAlimentos> resultDet = new();
            long idSolicitud = 0;

            // Almacenamiento de EgresoAlimentos
            result = await _egresoAlimentosService.SaveSolicitudSalidaAlimentos(modelSalida);
            idSolicitud = result.Data.IdSolicitudSalidaAlimentos;


            // Almacenamiento de los detalles
            foreach (var detalle in modelo.listaDetalleSalidaAlimentos)
            {
                DetalleSalidaAlimentos nuevoDetalle = new DetalleSalidaAlimentos
                {
                    IdDetalleSalidaAlimentos = detalle.IdDetalleSalidaAlimentos,
                    IdSolicitudSalidaAlimentos = (int?)idSolicitud,
                    IdProductoAlimenticio = detalle.IdProductoAlimenticio,
                    IdPresentacion = null,
                    CantidadUnidad = detalle.CantidadUnidad,
                    IdUnidadMedida = detalle.IdUnidadMedida,
                    CantidadSalida = detalle.CantidadSalida,
                    EstadoProducto = detalle.EstadoProducto,
                    Estado = 0
                };

                if (nuevoDetalle.IdDetalleSalidaAlimentos == 0)
                    resultDet = await _egresoAlimentosService.SaveDetalleSalidaAlimentos(nuevoDetalle);
            }

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

        [HttpPost]
        public async Task<IActionResult> GuardarEstibaDetallesEgresos([FromBody] EgresoAlimentosVM modelo)
        {
            
            ServiceResponse<DetalleEgresoAlimentos> resultDet = new ServiceResponse<DetalleEgresoAlimentos>();
            

            // Almacenamiento de los detalles
            foreach (var detalle in modelo.listaDetalleEgresoAlimentos)
            {
                DetalleEgresoAlimentos nuevoDetalle = new DetalleEgresoAlimentos
                {
                    IdDetalleEgresoAlimentos = detalle.IdDetalleEgresoAlimentos,
                    EstibaEgreso = detalle.EstibaEgreso,
                };

                resultDet = await _egresoAlimentosService.UpdateDetalleEgresoAlimentosEstado5(nuevoDetalle);
            }

            if (resultDet.Success && resultDet.Data != null)
            {
                TempData["Mensaje"] = resultDet.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = resultDet.Message;
                return View(resultDet);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarSolicitudSalida(Int64 id)
        {
            var result = await _egresoAlimentosService.DeleteSolicitudSalidaAlimentos(id);

            if (result.Success && result.Data != null)
            {
                return Json(new
                {
                    mensaje = result.Message,
                    codigo = result.Success,
                    listaDetalleSalidaAlimentos = await _egresoAlimentosService.GetAllDetalleSalidaAlimentos(id)
            });
            }
            else
            {
                return Json(new
                {
                    mensaje = result.Message,
                    codigo = result.Success,
                    listaDetalleSalidaAlimentos = await _egresoAlimentosService.GetAllDetalleSalidaAlimentos(id)
                });
            }
        }

        #endregion

        #region Vista Egreso Alimentos Finalizado

        public async Task<IActionResult> EgresoAlimentosFinalizado(int? id, int? estado)
        {
            EgresoAlimentos listado = await _egresoAlimentosService.GetEgresoAlimentos((int)id);
            var traslado = _egresoAlimentosService.GetAllTrasladoAlimentos().Result.Where(o => o.IdEgresoAlimentos == id).FirstOrDefault();

            #region Catálogos

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var Convenio = await _convenioService.GetAll();
            ViewBag.Convenios = new SelectList(Convenio, "IdConvenio", "NombreConvenio");
            var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var estadoTraslados = await _egresoAlimentosService.GetAllEstadoTraslados();
            ViewBag.EstadoTraslados = new SelectList(estadoTraslados, "IdEstadoTraslados", "EstadoTraslado");
            var EstadoTrasladosDict = estadoTraslados.ToDictionary(i => i.IdEstadoTraslados, i => i.EstadoTraslado);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);

            var pilotos = await _pilotoService.GetAll();
            ViewBag.Pilotos = new SelectList(pilotos, "IdPiloto", "NombresPiloto");
            var PilotosDict = pilotos.ToDictionary(i => i.IdPiloto, i => i.NombresPiloto);

            var transportista = await _transportistaService.GetAll();
            ViewBag.Transportistas = new SelectList(transportista, "IdTransportista", "NombreTransportista");
            var TransportistaDict = transportista.ToDictionary(i => i.IdTransportista, i => i.NombreTransportista);

            var vehiculos = await _vehiculoService.GetAll();
            ViewBag.Vehiculos = new SelectList(vehiculos, "IdVehiculo", "PlacaPlataforma");
            var VehiculosDict = vehiculos.ToDictionary(i => i.IdVehiculo, i => i.PlacaPlataforma);

            var usuarios = _usuarioService.GetAll().Result.Select(o => new { o.IdUsuario, o.Nombre, o.Apellidos }).ToList();
            ViewBag.Usuarios = new SelectList(usuarios, "IdUsuario", "Nombre");

            #endregion

            if (id == null || id == 0)
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
            else
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = listado,
                    TrasladoAlimentos = traslado ?? new TrasladoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerEgresoAlimentosFinalizado(int id)
        {
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos(id);
            var detalles = await _egresoAlimentosService.GetAllDetalleEgresoAlimentos(id);
            var detallesRacion = await _egresoAlimentosService.GetAllDetallesRacionXConvenio((int)egresoAlimentos.IdConvenio);
            var idDepartamentoCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.IdDepartamento).FirstOrDefault();
            var idMunicipioDestino = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaDestino).Select(o => o.IdMunicipio).FirstOrDefault();

            var BitacoraEstado1 = _egresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosEgresoEnum.Inicio).FirstOrDefault();
            var usuarioEstado1 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado1.IdUsuario).FirstOrDefault();

            var BitacoraEstado2 = _egresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosEgresoEnum.CreacionDeDespacho).FirstOrDefault();
            var usuarioEstado2 = new Usuario();
            if (BitacoraEstado2 != null)
                usuarioEstado2 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado2.IdUsuario).FirstOrDefault();

            var BitacoraEstado3 = _egresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosEgresoEnum.EnBodega).OrderByDescending(a => a.IdBitacora).FirstOrDefault();
            var usuarioEstado3 = new Usuario();
            if (BitacoraEstado3 != null)
                usuarioEstado3 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado3.IdUsuario).FirstOrDefault();

            var BitacoraEstado4 = _egresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosEgresoEnum.RechazoEgresoDeAlimentos).FirstOrDefault();
            var usuarioEstado4 = new Usuario();
            if (BitacoraEstado4 != null)
                usuarioEstado4 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado4.IdUsuario).FirstOrDefault();

            var BitacoraEstado5 = _egresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosEgresoEnum.ApruebaEgresoDeAlimentos).FirstOrDefault();
            var usuarioEstado5 = new Usuario();
            if (BitacoraEstado5 != null)
                usuarioEstado5 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado5.IdUsuario).FirstOrDefault();

            var BitacoraEstado6 = _egresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosEgresoEnum.EgresoFinalizado).FirstOrDefault();
            var usuarioEstado6 = new Usuario();
            if (BitacoraEstado6 != null)
                usuarioEstado6 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado6.IdUsuario).FirstOrDefault();

            // Métodos del Traslado

            var traslado = _egresoAlimentosService.GetAllTrasladoAlimentos().Result.Where(o => o.IdEgresoAlimentos == id).FirstOrDefault();

            var BitacoraEstado7 = new Bitacora();
            var usuarioEstado7 = new Usuario();
            var BitacoraEstado8 = new Bitacora();
            var usuarioEstado8 = new Usuario();
            var usuarioRecibeTraslado = new Usuario();
            if (traslado != null)
            {
                usuarioRecibeTraslado = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == egresoAlimentos.IdUsuarioRecibeTraslado).FirstOrDefault();

                BitacoraEstado7 = _egresoAlimentosService.GetAllBitacora(traslado.IdTrasladoAlimentos).Result.Where(o => o.Accion == (int)EstadosTrasladoEnum.RecibiendoTraslado).FirstOrDefault();
                if (BitacoraEstado7 != null)
                    usuarioEstado7 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado7.IdUsuario).FirstOrDefault();

                BitacoraEstado8 = _egresoAlimentosService.GetAllBitacora(traslado.IdTrasladoAlimentos).Result.Where(o => o.Accion == (int)EstadosTrasladoEnum.TrasladoFinalizado).FirstOrDefault();
                if (BitacoraEstado8 != null)
                    usuarioEstado8 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado8.IdUsuario).FirstOrDefault();
            }

            if (egresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                egresoAlimentos = egresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == egresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                convenio = _convenioService.GetAll().Result.Where(o => o.IdConvenio == egresoAlimentos.IdConvenio).Select(o => o.NombreConvenio).FirstOrDefault(),
                departamentoCarga = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == idDepartamentoCarga).Select(o => o.Descripcion).FirstOrDefault(),
                departamentoDestino = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestinoTraslado = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == idMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestinoEgreso = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                bodegaDestino = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaDestino).Select(o => o.NombreBodega).FirstOrDefault(),
                bodegaCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.NombreBodega).FirstOrDefault(),
                tipoAsistencia = _tipoAsistenciaService.GetAll().Result.Where(o => o.IdTipoAsistencia == egresoAlimentos.IdTipoAsistencia).Select(o => o.NombreTipoAsistencia).FirstOrDefault(),
                detEgresoAlimentos = detalles,
                detRacion = detallesRacion,
                bitacoraEstado1 = BitacoraEstado1,
                usuarioEstado1 = usuarioEstado1,
                bitacoraEstado2 = BitacoraEstado2,
                usuarioEstado2 = usuarioEstado2,
                bitacoraEstado3 = BitacoraEstado3,
                usuarioEstado3 = usuarioEstado3,
                bitacoraEstado4 = BitacoraEstado4,
                usuarioEstado4 = usuarioEstado4,
                bitacoraEstado5 = BitacoraEstado5,
                usuarioEstado5 = usuarioEstado5,
                bitacoraEstado6 = BitacoraEstado6,
                usuarioEstado6 = usuarioEstado6,
                traslado = traslado,
                bitacoraEstado7 = BitacoraEstado7,
                usuarioEstado7 = usuarioEstado7,
                bitacoraEstado8 = BitacoraEstado8,
                usuarioEstado8 = usuarioEstado8,
                usuarioRecibeTraslado = usuarioRecibeTraslado
            });
        }


        #endregion

            #region TRASLADO DE ALIMENTOS

        public async Task<IActionResult> CrearTraslado(int idEgreso)
        {
            TrasladoAlimentos objDB = new()
            {
                IdTrasladoAlimentos = 0,
                IdEgresoAlimentos = idEgreso
            };

            var result = await _egresoAlimentosService.SaveTrasladoAlimentos(objDB);

            // Se almacena la primer bitácora
            CrearBitacoraTraslado((int)result.Data.IdTrasladoAlimentos, (int)EstadosTrasladoEnum.RecibiendoTraslado);

            return Json(result);
        }

        #endregion

        #region Vista Recepcion Traslado de Alimentos

        public async Task<IActionResult> RecepcionTraslado(int? id, int? type)
        {
            TrasladoAlimentos traslado = await _egresoAlimentosService.GetTrasladoAlimentosDeEgreso((int)id);
            EgresoAlimentos listado = await _egresoAlimentosService.GetEgresoAlimentos((int)traslado.IdEgresoAlimentos);
            

            #region Catálogos

            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoEgreso = await _egresoAlimentosService.GetAllEstadoEgresos();
            ViewBag.EstadoEgresos = new SelectList(estadoEgreso, "IdEstadoEgresos", "EstadoEgreso");
            var EstadoEgresosDict = estadoEgreso.ToDictionary(i => i.IdEstadoEgresos, i => i.EstadoEgreso);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);


            #endregion

            if (id == null || id == 0)
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = new EgresoAlimentos(),
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
            else
            {
                var EgresoAlimentosVM = new EgresoAlimentosVM()
                {
                    EgresoAlimentos = listado,
                    TrasladoAlimentos = traslado,
                    ProgramaDict = ProgramaDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoEgresoDict = EstadoEgresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(EgresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerRecepcionTraslado(int id)
        {
            var traslado = await _egresoAlimentosService.GetTrasladoAlimentosDeEgreso((int)id);
            var egresoAlimentos = await _egresoAlimentosService.GetEgresoAlimentos((int)traslado.IdEgresoAlimentos);
            var detalles = await _egresoAlimentosService.GetAllDetalleEgresoAlimentos(egresoAlimentos.IdEgresoAlimentos);

            if (egresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                egresoAlimentos = egresoAlimentos,
                trasladoAlimentos = traslado,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == egresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                departamentoCarga = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoCarga).Select(o => o.Descripcion).FirstOrDefault(),
                departamentoDestino = _catalogoService.GetCatalogo("DEPARTAMENTO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdDepartamentoDestino).Select(o => o.Descripcion).FirstOrDefault(),
                municipioDestino = _catalogoService.GetCatalogo("MUNICIPIO").Result.Where(o => o.IdCatalogo == egresoAlimentos.IdMunicipioDestino).Select(o => o.Descripcion).FirstOrDefault(),
                bodegaDestino = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaDestino).Select(o => o.NombreBodega).FirstOrDefault(),
                bodegaCarga = _bodegaService.GetAll().Result.Where(o => o.IdBodega == egresoAlimentos.IdBodegaCarga).Select(o => o.NombreBodega).FirstOrDefault(),
                detEgresoAlimentos = detalles
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarRecepcionTraslado([FromForm] string modelo, IFormFile file)
        {
            // Deserializar el modelo
            var egresoAlimentosVM = JsonConvert.DeserializeObject<EgresoAlimentosVM>(modelo);

            TrasladoAlimentos modelTraslado = new()
            {
                IdTrasladoAlimentos = egresoAlimentosVM.TrasladoAlimentos.IdTrasladoAlimentos,
                AutorizaRecepcion = egresoAlimentosVM.TrasladoAlimentos.AutorizaRecepcion
            };

            ServiceResponse<TrasladoAlimentos> result = new ServiceResponse<TrasladoAlimentos>();
            ServiceResponse<DetalleEgresoAlimentos> resultDet = new ServiceResponse<DetalleEgresoAlimentos>();
            long idTrasladoAlimentos = 0;
            var nombreArtchivo = string.Empty;

            if (file != null && file.Length > 0)
            {
                nombreArtchivo = await AlmacenamientoDocumentos(file, (int)modelTraslado.IdTrasladoAlimentos, "despachoAlmacenTraslado");

                modelTraslado.ArchivoDespachoAlmacenTraslado = nombreArtchivo;
            }

            // Almacenamiento de EgresoAlimentos
            result = await _egresoAlimentosService.UpdateTrasladoAlimentos(modelTraslado);
            idTrasladoAlimentos = result.Data.IdTrasladoAlimentos;

            // Almacenamiento de los detalles
            foreach (var detalle in egresoAlimentosVM.listaDetalleEgresoAlimentos)
            {
                DetalleEgresoAlimentos nuevoDetalle = new DetalleEgresoAlimentos
                {
                    IdDetalleEgresoAlimentos = detalle.IdDetalleEgresoAlimentos,
                    EstibaTraslado = detalle.EstibaTraslado,
                };

                resultDet = await _egresoAlimentosService.UpdateDetalleEgresoAlimentosTrasladoEstado1(nuevoDetalle);
            }


            // se cambia el estado a finalizado
            CambioEstadoTraslado((int)idTrasladoAlimentos, (int)EstadosTrasladoEnum.TrasladoFinalizado); 


            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelTraslado);
            }
        }

        #endregion

        #region || Cambio de Estados || 

        public async Task<IActionResult> CambioEstado(int id, int estado, int? idTipoSalidaAlimento, int? idUsuario)
        {
            // estado = refleja el nuevo estado al que se transiciona la solicitud
            switch (estado)
            {
                case (int)EstadosEgresoEnum.CreacionDeDespacho:

                    // Se asigna el egreso al usuario seleccionado
                    //var respAsig = await _egresoAlimentosService.AsignacionUsuarioLogisticaEgreso(id, (int)idUsuario);

                    break;
                case (int)EstadosEgresoEnum.EnBodega:

                    // Se asigna el egreso al usuario seleccionado
                    var respAsig = await _egresoAlimentosService.AsignacionUsuarioEncargadoBodegaEgreso(id, (int)idUsuario);

                    // Se crea la nueva Bitácora del estado anterior
                    CrearBitacora(id, (int)EstadosEgresoEnum.CreacionDeDespacho);

                    break;
                case (int)EstadosEgresoEnum.RechazoEgresoDeAlimentos:

                    // Se crea la nueva Bitácora del estado anterior
                    CrearBitacora(id, (int)EstadosEgresoEnum.EnBodega);

                    break;
                case (int)EstadosEgresoEnum.ApruebaEgresoDeAlimentos:

                    // Se crea la nueva Bitácora del estado 3: RevisionPorJefeDeBodegas
                    CrearBitacora(id, (int)EstadosEgresoEnum.EnBodega);

                    break;
                case (int)EstadosEgresoEnum.EgresoFinalizado:

                    // Se crea la nueva Bitácora del estado 7: ApruebaEgresoDeAlimentos
                    CrearBitacora(id, (int)EstadosEgresoEnum.ApruebaEgresoDeAlimentos);

                    // Al ser un Egreso de alimentos, se ejecutan sus funciones
                    if ((int)idTipoSalidaAlimento == (int)TipoSalidaEnum.EgresoAlimentos)
                    {
                        // Se crea copia exacta de todos los tados ingresados de la tabla DETALLE_EGRESO_ALIMENTOS hacia MOVIMIENTOS_PRODUCTOS
                        var resp = await _egresoAlimentosService.MoverProductosEgresoAMovimientos(id);

                        // Se crea copia exacta de todos los datos ingresados de la tabla DETALLE_EGRESO_ALIMENTOS hacia KARDEX
                        var resp2 = await _egresoAlimentosService.MoverProductosEgresoAKardex(id);
                    }
                    else
                    {
                        // Se realiza el movimiento de Productos, restando las salidas de la bodega 
                        var respM = await _egresoAlimentosService.MoverProductosTrasladoAMovimientosEgreso(id);

                        // Se asigna el usuario que recibe el traslado 
                        var respAsig2 = await _egresoAlimentosService.AsignacionUsuarioRecibeTraslado(id, (int)idUsuario);

                        // Se crea el traslado
                        var resp = CrearTraslado(id);
                    }

                    break;

            }

            var result = await _egresoAlimentosService.CambioEstadoEgresoAlimentos(id, estado);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
        }

        public async Task<IActionResult> CambioEstadoTraslado(int id, int estado)
        {
            // estado = refleja el nuevo estado al que se transiciona la solicitud
            switch (estado)
            {
                case (int)EstadosTrasladoEnum.TrasladoFinalizado:

                    // Bitácora se almacena desde el finalización
                    CrearBitacoraTraslado(id, estado);

                    // Se crea copia exacta de todos los datos ingresados de la tabla DETALLE_EGRESO_ALIMENTOS hacia MOVIMIENTOS_PRODUCTOS
                    var resp = await _egresoAlimentosService.MoverProductosTrasladoAMovimientosIngreso(id);

                    // Se crea copia exacta de todos los datos ingresados de la tabla DETALLE_EGRESO_ALIMENTOS hacia KARDEX
                    var resp2 = await _egresoAlimentosService.MoverProductosTrasladoAKardex(id);

                    break;

            }

            var result = await _egresoAlimentosService.CambioEstadoTrasladoAlimentos(id, estado);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "EgresoAlimentos");
            }
        }

        #endregion

        #region || Creación de Bitácora ||  

        public async void CrearBitacora(int idEgresoAlimentos, int estadoEgreso)
        {
            Bitacora modelBitacota = new Bitacora()
            {
                IdBitacora = 0,
                Accion = estadoEgreso,
                IdRegistro = idEgresoAlimentos,
                NombreTabla = "EGRESO_ALIMENTOS",
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                FechaHora = DateTime.Now,
                Estado = 0
            };

            var resultBit = await _egresoAlimentosService.SaveBitacora(modelBitacota);
        }

        public async void CrearBitacoraTraslado(int idTrasladoAlimentos, int estadoTraslado)
        {
            Bitacora modelBitacota = new Bitacora()
            {
                IdBitacora = 0,
                Accion = estadoTraslado,
                IdRegistro = idTrasladoAlimentos,
                NombreTabla = "TRASLADO_ALIMENTOS",
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                FechaHora = DateTime.Now,
                Estado = 0
            };

            var resultBit = await _egresoAlimentosService.SaveBitacora(modelBitacota);
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
    }
}
