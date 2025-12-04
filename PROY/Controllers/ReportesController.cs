using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;
using System.IO;
using ExcelRow = PROY.Models.ExcelRow;

namespace PROY.Controllers
{
    public class ReportesController : Controller
    {
        #region Constructores

        private readonly IReporteService _reporteService;
        private readonly IBodegaService _bodegaService;
        private readonly ITipoBodegaService _tipoBodegaService;
        private readonly ICatalogoService _catalogoService;
        private readonly IProductoAlimenticioService _productoAlimenticioService;
        private readonly IConvenioService _convenioService;
        private readonly IProgramaService _programaService;
        private readonly IPresentacionPAService _presentacionPAService;
        private readonly IUnidadMedidaService _unidadmedidaService;
        private readonly ITransportistaService _transportistaService;
        private readonly IVehiculoService _vehiculoService;
        private readonly IPilotoService _pilotoService;
        private readonly IPersonalBodegaService _personalBodegaService;
        private readonly IRenglonPresupuestarioService _renglonPresupuestarioService;
        private readonly IPuestoService _puestoService;
        private readonly IRacionService _racionService;
        private readonly IEgresoAlimentosService _egresoAlimentosService;
        private readonly ISolicitudV2Service _solicitudV2Service;
        private readonly ISolicitudService _solicitudService;

        public ReportesController(IReporteService reporteService,
                                  IBodegaService bodegaService,
                                  ICatalogoService catalogoService,
                                  ITipoBodegaService tipoBodegaService,
                                  IProductoAlimenticioService productoAlimenticio,
                                  IConvenioService convenioService,
                                  IProgramaService programaService,
                                  IPresentacionPAService presentacionPAService,
                                  IUnidadMedidaService unidadMedidaService,
                                  ITransportistaService transportistaService,
                                  IVehiculoService vehiculoService,
                                  IPilotoService pilotoService,
                                  IPersonalBodegaService personalBodegaService,
                                  IRenglonPresupuestarioService renglonPresupuestarioService,
                                  IPuestoService puestoService,
                                  IRacionService racionService,
                                  IEgresoAlimentosService egresoAlimentosService,
                                  ISolicitudV2Service solicitudV2Service,
                                  ISolicitudService solicitudService)
        {
            _reporteService = reporteService;
            _bodegaService = bodegaService;
            _catalogoService = catalogoService;
            _tipoBodegaService = tipoBodegaService;
            _productoAlimenticioService = productoAlimenticio;
            _convenioService = convenioService;
            _programaService = programaService;
            _presentacionPAService = presentacionPAService;
            _unidadmedidaService = unidadMedidaService;
            _transportistaService = transportistaService;
            _vehiculoService = vehiculoService;
            _pilotoService = pilotoService;
            _personalBodegaService = personalBodegaService;
            _renglonPresupuestarioService = renglonPresupuestarioService;
            _puestoService = puestoService;
            _racionService = racionService;
            _egresoAlimentosService = egresoAlimentosService;
            _solicitudV2Service = solicitudV2Service;
            _solicitudService = solicitudService;
        }

        #endregion

        #region Catálogos

        public async Task<(Dictionary<long, string> ProductoAlimenticioDict, Dictionary<long, string> ProgramaDict, Dictionary<long, string> ConvenioDict, Dictionary<long, string> bodegaDict, Dictionary<long, string> PresentacionPADict,
                           Dictionary<long, string> UnidadMedidaDict, Dictionary<int, string> departamentoDict, Dictionary<int, string> municipioDict, Dictionary<long, string> tipoBodegasDict,
                           Dictionary<long, string> renglonPresupuestarioDict, Dictionary<long, string> puestoDict, Dictionary<long, string> TransportistaDict, Dictionary<long, string> pilotoDict)> CompletarCatálogosAsync()
        {
            var ProductoAlimenticio = await _productoAlimenticioService.GetAll();
            var listaConSeleccionPA = new List<ProductoAlimenticio> { new ProductoAlimenticio { IdProductoAlimenticio = 0, NombrePA = "-- Seleccione --" } };
            listaConSeleccionPA.AddRange(ProductoAlimenticio);
            ViewBag.ProductoAlimenticio = new SelectList(listaConSeleccionPA, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Programa = await _programaService.GetAll();
            var listaConSeleccionPro = new List<Programa> { new Programa { IdPrograma = 0, NombrePrograma = "-- Seleccione --" } };
            listaConSeleccionPro.AddRange(Programa);
            ViewBag.Programas = new SelectList(listaConSeleccionPro, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var Convenio = await _convenioService.GetAll();
            var listaConSeleccionConv = new List<Convenio> { new Convenio { IdConvenio = 0, NombreConvenio = "-- Seleccione --" } };
            listaConSeleccionConv.AddRange(Convenio);
            ViewBag.Convenios = new SelectList(listaConSeleccionConv, "IdConvenio", "NombreConvenio");
            var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

            var bodega = await _bodegaService.GetAll();
            var listaConSeleccionBo = new List<Bodega> { new Bodega { IdBodega = 0, NombreBodega = "-- Seleccione --" } };
            listaConSeleccionBo.AddRange(bodega);
            ViewBag.Bodegas = new SelectList(listaConSeleccionBo, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var PresentacionPA = await _presentacionPAService.GetAll();
            var listaConSeleccionPre = new List<PresentacionPA> { new PresentacionPA { IdPresentacionPA = 0, NombrePresentacionPA = "-- Seleccione --" } };
            listaConSeleccionPre.AddRange(PresentacionPA);
            ViewBag.PresentacionPA = new SelectList(listaConSeleccionPre, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            var listaConSeleccionUni = new List<UnidadMedida> { new UnidadMedida { IdUnidadMedida = 0, NombreUnidadMedida = "-- Seleccione --" } };
            listaConSeleccionUni.AddRange(UnidadMedida);
            ViewBag.UnidadMedida = new SelectList(listaConSeleccionUni, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            var listaConSeleccionDep = new List<Catalogo> { new Catalogo { IdCatalogo = 0, Descripcion = "-- Seleccione --" } };
            listaConSeleccionDep.AddRange(departamentos);
            ViewBag.Departamentos = new SelectList(listaConSeleccionDep, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            var listaConSeleccionMun = new List<Catalogo> { new Catalogo { IdCatalogo = 0, Descripcion = "-- Seleccione --" } };
            listaConSeleccionMun.AddRange(municipios);
            ViewBag.Municipios = new SelectList(listaConSeleccionMun, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var tipoBodegas = await _tipoBodegaService.GetAll();
            var listaConSeleccionTipBod = new List<TipoBodega> { new TipoBodega { IdTipoBodega = 0, NombreTipoBodega = "-- Seleccione --" } };
            listaConSeleccionTipBod.AddRange(tipoBodegas);
            ViewBag.tipoBodegas = new SelectList(listaConSeleccionTipBod, "IdTipoBodega", "NombreTipoBodega");
            var tipoBodegasDict = tipoBodegas.ToDictionary(i => i.IdTipoBodega, i => i.NombreTipoBodega);

            var renglonPresupuestario = await _renglonPresupuestarioService.GetAll();
            var listaConSeleccionRenPres = new List<RenglonPresupuestario> { new RenglonPresupuestario { IdRenglonPresupuestario = 0, Renglon = "-- Seleccione --" } };
            listaConSeleccionRenPres.AddRange(renglonPresupuestario);
            ViewBag.renglonPresupuestario = new SelectList(listaConSeleccionRenPres, "IdRenglonPresupuestario", "Renglon");
            var renglonPresupuestarioDict = renglonPresupuestario.ToDictionary(i => i.IdRenglonPresupuestario, i => i.Renglon);

            var puestos = await _puestoService.GetAll();
            var listaConSeleccionPuesto = new List<Puesto> { new Puesto { IdPuesto = 0, NombrePuesto = "-- Seleccione --" } };
            listaConSeleccionPuesto.AddRange(puestos);
            ViewBag.puestos = new SelectList(listaConSeleccionPuesto, "IdPuesto", "NombrePuesto");
            var puestoDict = puestos.ToDictionary(i => i.IdPuesto, i => i.NombrePuesto);

            var transportista = await _transportistaService.GetAll();
            var listaConSeleccionTrans = new List<Transportista> { new Transportista { IdTransportista = 0, NombreTransportista = "-- Seleccione --" } };
            listaConSeleccionTrans.AddRange(transportista);
            ViewBag.transportistas = new SelectList(listaConSeleccionTrans, "IdTransportista", "NombreTransportista");
            var transportistasDict = transportista.ToDictionary(i => i.IdTransportista, i => i.NombreTransportista);

            var piloto = await _pilotoService.GetAll();
            var listaConSeleccionPiloto = new List<Piloto> { new Piloto { IdPiloto = 0, NombresPiloto = "-- Seleccione --" } };
            listaConSeleccionPiloto.AddRange(piloto);
            ViewBag.pilotos = new SelectList(listaConSeleccionPiloto, "IdPiloto", "NombresPiloto");
            var pilotoDict = piloto.ToDictionary(i => i.IdPiloto, i => i.NombresPiloto);

            return (ProductoAlimenticioDict, ProgramaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, puestoDict, renglonPresupuestarioDict, transportistasDict, pilotoDict);
        }

        #endregion

        #region Vistas

        public async Task<IActionResult> Bodegas(int? idBodega)
        {
            List<Bodega> listado = await _bodegaService.GetAll();

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (idBodega == null || idBodega == 0)
            {
                var bodegaVM = new BodegaVM()
                {
                    Bodega = new Bodega(),
                    listaBodegas = listado,
                    TipoBodegaDict = tipoBodegasDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict
                };

                return View(bodegaVM);
            }
            else
            {
                var bodegaVM = new BodegaVM()
                {
                    Bodega = await _bodegaService.GetBodega((int)idBodega),
                    listaBodegas = await _bodegaService.GetAll(),
                };

                return View(bodegaVM);
            }
        }

        public async Task<IActionResult> MovimientoProductos(int? id)
        {
            List<MovimientosProductos> listado = await _reporteService.GetAllMovimientosProductos();

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (id == null || id == 0)
            {
                var movVM = new MovimientoProductosVM()
                {
                    MovimientosProductos = new MovimientosProductos(),
                    listaMovimientosProductos = listado,
                    BodegaDict = bodegaDict,
                    ProductoAlimenticioDict = productoDict,
                    PresentacionDict = PresentacionPADict,
                    UnidadMedidaDict = UnidadMedidaDict,
                };

                return View(movVM);
            }
            else
            {
                var movVM = new MovimientoProductosVM()
                {
                    MovimientosProductos = new MovimientosProductos(),
                    listaMovimientosProductos = listado,
                    BodegaDict = bodegaDict,
                    ProductoAlimenticioDict = productoDict,
                    PresentacionDict = PresentacionPADict,
                    UnidadMedidaDict = UnidadMedidaDict,
                };

                return View(movVM);
            }
        }


        public async Task<IActionResult> PersonalBodegas(int? idPersonalBodega)
        {
            List<PersonalBodega> listado = await _personalBodegaService.GetAll();

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (idPersonalBodega == null || idPersonalBodega == 0)
            {
                var personalBodegaVM = new PersonalBodegaVM()
                {
                    PersonalBodega = new PersonalBodega(),
                    listaPersonalBodegas = listado,
                    PuestoDict = puestoDict,
                    RenglonPreDict = renglonPresupuestarioDict,
                    BodegaDict = bodegaDict,
                };

                return View(personalBodegaVM);
            }
            else
            {
                var personalBodegaVM = new PersonalBodegaVM()
                {
                    PersonalBodega = await _personalBodegaService.GetPersonalBodega((int)idPersonalBodega),
                    listaPersonalBodegas = await _personalBodegaService.GetAll(),
                };

                return View(personalBodegaVM);
            }
        }


        public async Task<IActionResult> ExistenciasProducto(int? id)
        {
            FiltrosReportesDTO filtrosReportesDTO = new FiltrosReportesDTO();
            filtrosReportesDTO.IdBodega = 0;
            filtrosReportesDTO.IdProductoAlimenticio = 0;
            filtrosReportesDTO.IdUnidadMedida = 0;
            filtrosReportesDTO.IdConvenio = 0;

            List<dynamic> listado = _reporteService.ReporteExistenciasProducto(filtrosReportesDTO);

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (id == null || id == 0)
            {
                var kardexVM = new KardexVM()
                {
                    Kardex = new Kardex(),
                    listaKardex = listado,
                    BodegaDict = bodegaDict,
                    ProductoAlimenticioDict = productoDict,
                    PresentacionDict = PresentacionPADict,
                    UnidadMedidaDict = UnidadMedidaDict,
                };

                return View(kardexVM);
            }
            else
            {
                var kardexVM = new KardexVM()
                {
                    Kardex = new Kardex(),
                    listaKardex = listado,
                    BodegaDict = bodegaDict,
                    ProductoAlimenticioDict = productoDict,
                    PresentacionDict = PresentacionPADict,
                    UnidadMedidaDict = UnidadMedidaDict,
                };

                return View(kardexVM);
            }
        }

        public async Task<IActionResult> Convenios(int? idConvenio)
        {
            List<Convenio> listado = await _convenioService.GetAll();

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (idConvenio == null || idConvenio == 0)
            {
                var convenioVM = new ConvenioVM()
                {
                    Convenio = new Convenio(),
                    listaConvenios = listado,
                };

                return View(convenioVM);
            }
            else
            {
                var convenioVM = new ConvenioVM()
                {
                    Convenio = await _convenioService.GetConvenio((int)idConvenio),
                    listaConvenios = await _convenioService.GetAll(),
                };

                return View(convenioVM);
            }
        }


        public async Task<IActionResult> Raciones(int? idRacion)
        {
            FiltrosReportesDTO filtrosReportesDTO = new FiltrosReportesDTO();
            filtrosReportesDTO.IdConvenio = 0;

            List<dynamic> listado = _reporteService.ReporteRaciones(filtrosReportesDTO);

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (idRacion == null || idRacion == 0)
            {
                var racionVM = new RacionVM()
                {
                    Racion = new RacionMaestro(),
                    listaRacionReporte = listado,
                    UnidadMedidaDict = UnidadMedidaDict,
                    ProgramaDict = programaDict,
                    ProductoAlimenticioDict = productoDict,
                    ConvenioDict = ConvenioDict
                };

                return View(racionVM);
            }
            else
            {
                var racionVM = new RacionVM()
                {
                    Racion = await _racionService.GetRacionMaestro((int)idRacion),
                    listaRacion = await _racionService.GetAllRacionMaestro(),
                };

                return View(racionVM);
            }
        }

        public async Task<IActionResult> InventarioAlimentos(int? id)
        {
            List<MovimientosProductos> listado = await _reporteService.GetAllMovimientosProductos();

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (id == null || id == 0)
            {
                var movVM = new MovimientoProductosVM()
                {
                    MovimientosProductos = new MovimientosProductos(),
                    listaMovimientosProductos = listado,
                    BodegaDict = bodegaDict,
                    ProductoAlimenticioDict = productoDict,
                    PresentacionDict = PresentacionPADict,
                    UnidadMedidaDict = UnidadMedidaDict,
                    ConvenioDict = ConvenioDict,
                };

                return View(movVM);
            }
            else
            {
                var movVM = new MovimientoProductosVM()
                {
                    MovimientosProductos = new MovimientosProductos(),
                    listaMovimientosProductos = listado,
                    BodegaDict = bodegaDict,
                    ProductoAlimenticioDict = productoDict,
                    PresentacionDict = PresentacionPADict,
                    UnidadMedidaDict = UnidadMedidaDict,
                    ConvenioDict = ConvenioDict,
                };

                return View(movVM);
            }
        }

        public async Task<IActionResult> ConstanciaSalidaAlimentos(int? id)
        {
            List<SolicitudSalidaAlimentos> listado = await _reporteService.GetAllSolicitudSalidaAlimentos();

            #region Catálogos

            var (productoDict, programaDict, ConvenioDict, bodegaDict, PresentacionPADict, UnidadMedidaDict, departamentoDict, municipioDict, tipoBodegasDict, renglonPresupuestarioDict, puestoDict, transportistasDict, pilotoDict) = await CompletarCatálogosAsync();

            #endregion

            if (id == null || id == 0)
            {
                var movVM = new SolicitudSalidaAlimentosVM()
                {
                    SolicitudSalidaAlimentos = new SolicitudSalidaAlimentos(),
                    listaSolicitudSalidaAlimentos = listado,
                    TransportistaDict = transportistasDict,
                    PilotoDict = pilotoDict,
                    ProgramaDict = programaDict,
                };

                return View(movVM);
            }
            else
            {
                var movVM = new SolicitudSalidaAlimentosVM()
                {
                    SolicitudSalidaAlimentos = new SolicitudSalidaAlimentos(),
                    listaSolicitudSalidaAlimentos = listado,
                    TransportistaDict = transportistasDict,
                    PilotoDict = pilotoDict,
                    ProgramaDict = programaDict,
                };

                return View(movVM);
            }
        }

        #endregion

        #region Métodos de Búsqueda 

        [HttpPost]
        public Task<IActionResult> BusquedaMovimientoProductos([FromForm] string modelo)
        {
            // Deserializar el modelo
            var jObject = JObject.Parse(modelo);
            var filtrosReportesDTO = jObject["FiltrosReportesDTO"]?.ToObject<FiltrosReportesDTO>();

            var listado = _reporteService.ReporteMovimientoProductos(filtrosReportesDTO);

            return Task.FromResult<IActionResult>(Json(new
            {
                tabla = listado
            }));
        }

        public Task<IActionResult> BusquedaExistenciasProducto([FromForm] string modelo)
        {
            // Deserializar el modelo
            var jObject = JObject.Parse(modelo);
            var filtrosReportesDTO = jObject["FiltrosReportesDTO"]?.ToObject<FiltrosReportesDTO>();

            var listado = _reporteService.ReporteExistenciasProducto(filtrosReportesDTO);

            return Task.FromResult<IActionResult>(Json(new
            {
                tabla = listado
            }));
        }

        public Task<IActionResult> BusquedaRaciones([FromForm] string modelo)
        {
            // Deserializar el modelo
            var jObject = JObject.Parse(modelo);
            var filtrosReportesDTO = jObject["FiltrosReportesDTO"]?.ToObject<FiltrosReportesDTO>();

            var listado = _reporteService.ReporteRaciones(filtrosReportesDTO);

            return Task.FromResult<IActionResult>(Json(new
            {
                tabla = listado
            }));
        }

        public Task<IActionResult> BusquedaInventarioAlimentos([FromForm] string modelo)
        {
            // Deserializar el modelo
            var jObject = JObject.Parse(modelo);
            var filtrosReportesDTO = jObject["FiltrosReportesDTO"]?.ToObject<FiltrosReportesDTO>();

            var listado = _reporteService.ReporteInventarioAlimentos(filtrosReportesDTO);


            return Task.FromResult<IActionResult>(Json(new
            {
                tabla = listado
            }));
        }

        public Task<IActionResult> BusquedaConstanciaSalida([FromForm] string modelo)
        {
            // Deserializar el modelo
            var jObject = JObject.Parse(modelo);
            var filtrosReportesDTO = jObject["FiltrosReportesDTO"]?.ToObject<FiltrosReportesDTO>();

            var listado = _reporteService.GetAllSolicitudSalidaAlimentos().Result.ToList();

            if (filtrosReportesDTO.NoConstancia != null && filtrosReportesDTO.NoConstancia != "")
                listado = listado.Where(o => o.NoConstancia.ToUpper() == filtrosReportesDTO.NoConstancia.ToUpper()).ToList();


            return Task.FromResult<IActionResult>(Json(new
            {
                tabla = listado
            }));
        }

        #endregion

        #region Generación de Documento

        public async Task<IActionResult> GeneraReporte(int id, [FromQuery] FiltrosReportesDTO filtros)
        {
            var reporte = await _reporteService.GenerarReporte(id, filtros);

            if (reporte == null)
            {
                return NotFound();
            }

            return Json(new
            {
                reporte = reporte
            });

        }

        #endregion


        public async Task<IActionResult> ProgramacionesAPA()
        {

            // Obtener catálogos
            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");

            // Preparar SelectLists para la vista
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");

            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarProgramacionesAPA([FromBody] FiltrosReportesDTO filtros)
        {
            try
            {
                var configuration = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var id_estado_str = configuration["EtapasSolicitud:APA:PROGRAMACION"];

                if (!int.TryParse(id_estado_str, out int id_estado))
                {
                    id_estado = 0;
                }

                // Aquí implementas la lógica de búsqueda con los filtros
                var resultados = await _solicitudV2Service.ObtenerSolicitudesConFiltros(
                    idEstado: id_estado,
                    idDepto: 2, // Este es el parámetro fijo
                    idDepartamento: filtros.IdDepartamento, // Este es el filtro opcional
                    idMunicipio: filtros.IdMunicipio,
                    fechaInicio: filtros.FechaInicio,
                    fechaFin: filtros.FechaFin,
                    rechazada: 0
                );

                // Obtener catálogos para mapear IDs a nombres
                var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
                var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");

                // Crear objetos anónimos con nombres en lugar de IDs
                var resultadosConNombres = resultados.Select(s => new
                {
                    s.IdSolicitud,
                    s.Codigo,
                    s.Correlativo,
                    DepartamentoNombre = departamentos.FirstOrDefault(d => d.IdCatalogo == s.Departamento)?.Descripcion ?? "No especificado",
                    MunicipioNombre = municipios.FirstOrDefault(m => m.IdCatalogo == s.Municipio)?.Descripcion ?? "No especificado",
                    s.Comunidad,
                    FechaRegistro = s.FechaRegistro.ToString("dd/MM/yyyy"),
                    s.Raciones,
                    s.Descripcion,
                    s.Estado,
                    // Mantener los IDs originales por si los necesitas
                    DepartamentoId = s.Departamento,
                    MunicipioId = s.Municipio
                }).ToList();


                return Json(new { success = true, data = resultadosConNombres });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> EjecucionesAPA()
        {
            // Obtener catálogos
            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");

            // Preparar SelectLists para la vista
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");

            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarEjecucionesAPA([FromBody] FiltrosReportesDTO filtros)
        {

            try
            {

                // Aquí implementas la lógica de búsqueda con los filtros
                var resultados = await _solicitudV2Service.ObtenerEjecucionesAPAConFiltros(
                    idDepartamento: filtros.IdDepartamento, // Este es el filtro opcional
                    idMunicipio: filtros.IdMunicipio,
                    fechaInicio: filtros.FechaInicio,
                    fechaFin: filtros.FechaFin
                );

                return Json(new { success = true, data = resultados });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> DespachosAPA()
        {
            ViewBag.Departamentos = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarDespachosAPA([FromBody] FiltrosReportesDTO filtros)
        {

            try
            {

                // Aquí implementas la lógica de búsqueda con los filtros
                var resultados = await _solicitudV2Service.ObtenerDespachosAPAConFiltros(
                    idDepartamento: filtros.IdDepartamento, // Este es el filtro opcional
                    idMunicipio: filtros.IdMunicipio,
                    fechaInicio: filtros.FechaInicio,
                    fechaFin: filtros.FechaFin
                );

                return Json(new { success = true, data = resultados });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> BeneficiariosConveniosAPA()
        {
            ViewBag.Departamentos = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            return View();
        }

        public async Task<IActionResult> ProcesosAPA()
        {
            ViewBag.Departamentos = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            return View();
        }

        public async Task<IActionResult> RenapAPA()
        {
            ViewBag.Departamentos = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarRenapAPA([FromBody] FiltrosReportesDTO filtros)
        {
            Solicitud solicitud = new Solicitud();
            solicitud = await _solicitudService.GetSolicitud((long)filtros.IdSolicitud);

            var datos = (await _solicitudService.GetAllDetalleSolicitudAPA((long)filtros.IdSolicitud))
                        .Where(d => d.IdAnulacion == 67 || d.IdAnulacion == 0 || d.IdAnulacion is null);

            // Configuración para EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var hoja = package.Workbook.Worksheets.Add("CONSULTAS");

                // Encabezados                                
                hoja.Cells[1, 1].Value = "CUI";
                hoja.Cells[1, 2].Value = "Referencia";

                // Aplicar estilo al encabezado
                using (var rango = hoja.Cells[1, 1, 1, 2])
                {
                    rango.Style.Font.Bold = true;
                    rango.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rango.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                }

                // Llenar datos
                int fila = 2;
                foreach (var ii in datos)
                {
                    //var paramDepto = await _catalogoService.GetCatalogo(ii.IdDepartamento);
                    //var paramMuni = await _catalogoService.GetCatalogo(ii.IdMunicipio);
                    //var paramEstadoCivil = await _catalogoService.GetCatalogo(ii.EstadoCivil);
                    var paramGrupoEtnico = await _catalogoService.GetCatalogo(ii.GrupoEtnico);

                    hoja.Cells[fila, 1].Value = ii.CuiEsposa;
                    hoja.Cells[fila, 2].Value = "EXP-APA-" + solicitud.Codigo;

                    fila++;
                }

                // Autoajustar columnas
                hoja.Cells[hoja.Dimension.Address].AutoFitColumns();

                // Generar archivo Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Descargar archivo
                var nombreArchivo = "ConsultaCUIRenap.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }

        public async Task<IActionResult> GuatenominasAPA()
        {
            ViewBag.Departamentos = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            return View();
        }

        public async Task<IActionResult> ExportarGuatenominas([FromBody] FiltrosReportesDTO filtros)
        {
            // Datos de ejemplo
            var datos = (await _solicitudService.GetAllDetalleSolicitudAPA((long)filtros.IdSolicitud))
                        .Where(d => d.IdAnulacion == 67 || d.IdAnulacion == 0 || d.IdAnulacion is null);

            // Crear contenido del archivo TXT
            var contenidoTxt = new StringBuilder();

            // Llenar datos
            foreach (var ii in datos)
            {
                var paramGrupoEtnico = await _catalogoService.GetCatalogo(ii.GrupoEtnico);

                contenidoTxt.AppendLine($"{ii.CuiEsposa}|\t");
            }

            // Convertir a array de bytes
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            await writer.WriteAsync(contenidoTxt.ToString());
            await writer.FlushAsync();
            stream.Position = 0;

            // Descargar archivo
            var nombreArchivo = "ConsultaCUIGuatenominas.txt";
            return File(stream, "text/plain", nombreArchivo);
        }

        public async Task<IActionResult> PlanillaAPA()
        {
            ViewBag.Departamentos = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            ViewBag.Municipios = new SelectList(new List<object>(), "IdCatalogo", "Descripcion");
            return View();
        }

    }
}
