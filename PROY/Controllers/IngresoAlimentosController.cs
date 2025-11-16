using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PROY.Enum;
using PROY.Models;
using PROY.Repositories;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class IngresoAlimentosController : Controller
    {
        #region Contructores

        private readonly IIngresoAlimentosService _ingresoAlimentosService;
        private readonly IBodegaService _bodegaService;
        private readonly ICatalogoService _catalogoService;
        private readonly IJuntaDeRecepcionService _juntaDeRecepcionService;
        private readonly IProductoAlimenticioService _productoAlimenticioService;
        private readonly IConvenioService _convenioService;
        private readonly IProgramaService _programaService;
        private readonly IPresentacionPAService _presentacionPAService;
        private readonly IUnidadMedidaService _unidadmedidaService;
        private readonly IEstibaService _estibaService;
        private readonly ITransportistaService _transportistaService;
        private readonly IVehiculoService _vehiculoService;
        private readonly IPilotoService _pilotoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IProveedorService _proveedorService;

        public IngresoAlimentosController(IIngresoAlimentosService ingresoAlimentosService,
                                          IBodegaService bodegaService,
                                          ICatalogoService catalogoService,
                                          IJuntaDeRecepcionService juntaDeRecepcionsService,
                                          IProductoAlimenticioService productoAlimenticio,
                                          IConvenioService convenioService,
                                          IProgramaService programaService,
                                          IPresentacionPAService presentacionPAService,
                                          IUnidadMedidaService unidadMedidaService,
                                          IEstibaService estibaService,
                                          ITransportistaService transportistaService,
                                          IVehiculoService vehiculoService,
                                          IPilotoService pilotoService,
                                          IUsuarioService usuarioService,
                                          IProveedorService proveedorService)
        {
            _ingresoAlimentosService = ingresoAlimentosService;
            _bodegaService = bodegaService;
            _catalogoService = catalogoService;
            _juntaDeRecepcionService = juntaDeRecepcionsService;
            _productoAlimenticioService = productoAlimenticio;
            _convenioService = convenioService;
            _programaService = programaService;
            _presentacionPAService = presentacionPAService;
            _unidadmedidaService = unidadMedidaService;
            _estibaService = estibaService;
            _transportistaService = transportistaService;
            _vehiculoService = vehiculoService;
            _pilotoService = pilotoService;
            _usuarioService = usuarioService;
            _proveedorService = proveedorService;
        }

        #endregion

        #region Index

        public async Task<IActionResult> Index(int? idIngresoAlimentos)
        {
            List<IngresoAlimentos> listado = await _ingresoAlimentosService.GetAllIngresoAlimentos();
            List<Bodega> listadoBodegas = await _bodegaService.GetAll();
            var listBitacoras = await _ingresoAlimentosService.GetAllBitacoraEstado((int)EstadosIngresoEnum.Inicio);

            #region Catálogos

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

            var estadoIngreso = await _ingresoAlimentosService.GetAllEstadoIngresos();
            ViewBag.EstadoIngresos = new SelectList(estadoIngreso, "IdEstadoIngresos", "EstadoIngreso");
            var EstadoIngresosDict = estadoIngreso.ToDictionary(i => i.IdEstadoIngresos, i => i.EstadoIngreso);

            #endregion

            if (idIngresoAlimentos == null || idIngresoAlimentos == 0)
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = new IngresoAlimentos(),
                    listaIngresoAlimentos = listado,
                    listaBitacoraInicio = listBitacoras,
                    ListaBodegas = listadoBodegas,
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoIngresoDict = EstadoIngresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(IngresoAlimentosVM);
            }
            else
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = new IngresoAlimentos(),
                    listaIngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(IngresoAlimentosVM);
            }
        }


        #endregion 

        #region Vista IngresoAlimentos - Estado 1

        public async Task<IActionResult> IngresoAlimentos(int? id, int? type)
        {
            List<IngresoAlimentos> listado = await _ingresoAlimentosService.GetAllIngresoAlimentos();
            IngresoAlimentos? ingreso = new();

            if (id != null && id != decimal.Zero)
                ingreso = await _ingresoAlimentosService.GetIngresoAlimentos((Int64)id);

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

            var juntasUtilizadas = listado.Where(o => o.IdJuntaReceptora != null && o.IdJuntaReceptora != ingreso.IdJuntaReceptora).Select(o => (Int64)o.IdJuntaReceptora.Value).Distinct().ToList();
            var juntaReceptora = await _juntaDeRecepcionService.GetAllJuntaReceptora();
            var juntasDisponibles = juntaReceptora.Where(j => !juntasUtilizadas.Contains(j.IdJuntaReceptora)).ToList();

            ViewBag.JuntaReceptora = new SelectList(juntasDisponibles, "IdJuntaReceptora", "NumeroNombramiento");
            var JuntaReceptoraDict = juntasDisponibles.ToDictionary(i => i.IdJuntaReceptora, i => i.NumeroNombramiento);

            var departamentos = await _catalogoService.GetCatalogo("DEPARTAMENTO");
            ViewBag.Departamentos = new SelectList(departamentos, "IdCatalogo", "Descripcion");
            var departamentoDict = departamentos.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var municipios = await _catalogoService.GetCatalogo("MUNICIPIO");
            ViewBag.Municipios = new SelectList(municipios, "IdCatalogo", "Descripcion");
            var municipioDict = municipios.ToDictionary(i => i.IdCatalogo, i => i.Descripcion);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var estadoIngreso = await _ingresoAlimentosService.GetAllEstadoIngresos();
            ViewBag.EstadoIngresos = new SelectList(estadoIngreso, "IdEstadoIngresos", "EstadoIngreso");
            var EstadoIngresosDict = estadoIngreso.ToDictionary(i => i.IdEstadoIngresos, i => i.EstadoIngreso);

            var PresentacionPA = await _presentacionPAService.GetAll();
            ViewBag.PresentacionPA = new SelectList(PresentacionPA, "IdPresentacionPA", "NombrePresentacionPA");
            var PresentacionPADict = PresentacionPA.ToDictionary(i => i.IdPresentacionPA, i => i.NombrePresentacionPA);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);


            #endregion

            if (id == null || id == 0)
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = new IngresoAlimentos(),
                    listaIngresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    JuntaReceptoraDict = JuntaReceptoraDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoIngresoDict = EstadoIngresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
            else
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = new IngresoAlimentos(),
                    listaIngresoAlimentos = listado,
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    JuntaReceptoraDict = JuntaReceptoraDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoIngresoDict = EstadoIngresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerIngresoAlimentos(int id)
        {
            var ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos(id);
            var productosIngreso = await _ingresoAlimentosService.GetAllProductosIngreso(id);

            if (ingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                ingresoAlimentos = ingresoAlimentos,
                productosIngreso = productosIngreso
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarIngresoAlimentos([FromBody] IngresoAlimentosVM modelo)
        {
            IngresoAlimentos modelIngreso = new IngresoAlimentos()
            {
                IdIngresoAlimentos = modelo.IngresoAlimentos.IdIngresoAlimentos,
                IdPrograma = modelo.IngresoAlimentos.IdPrograma,
                IdConvenio = modelo.IngresoAlimentos.IdConvenio,
                IdBodegaRecibe = modelo.IngresoAlimentos.IdBodegaRecibe,
                IdJuntaReceptora = modelo.IngresoAlimentos.IdJuntaReceptora,
                Estado = 0,
            };

            ServiceResponse<IngresoAlimentos> result = new ServiceResponse<IngresoAlimentos>();
            ServiceResponse<ProductosIngreso> resultDet = new ServiceResponse<ProductosIngreso>();
            ServiceResponse<Bitacora> resultBit = new ServiceResponse<Bitacora>();
            long idIngresoAlimentos = 0;

            // Almacenamiento de IngresoAlimentos
            if (modelIngreso.IdIngresoAlimentos == 0)
            {
                result = await _ingresoAlimentosService.SaveIngresoAlimentosEstado1(modelIngreso);
                idIngresoAlimentos = result.Data.IdIngresoAlimentos;

                // En el primer almacenamiento, se guarda la bitácora
                CrearBitacora((int)idIngresoAlimentos, (int)EstadosIngresoEnum.Inicio);
            }
            else
            {
                result = await _ingresoAlimentosService.UpdateIngresoAlimentosEstado1(modelIngreso);
                idIngresoAlimentos = result.Data.IdIngresoAlimentos;

            }

            // Almacenamiento de los detalles
            foreach (var detalle in modelo.listaProductosIngreso)
            {
                ProductosIngreso nuevoDetalle = new ProductosIngreso
                {
                    IdProductosIngreso = detalle.IdProductosIngreso,
                    IdIngresoAlimentos = (int?)idIngresoAlimentos,
                    IdProductoAlimenticio = detalle.IdProductoAlimenticio,
                    IdPresentacion = detalle.IdPresentacion,
                    CantidadUnidad = detalle.CantidadUnidad,
                    IdUnidadMedida = detalle.IdUnidadMedida,
                    CantidadRecibir = detalle.CantidadRecibir,
                    Estado = 0
                };

                if (nuevoDetalle.IdProductosIngreso == 0)
                    resultDet = await _ingresoAlimentosService.SaveProductosIngreso(nuevoDetalle);
                else
                    resultDet = await _ingresoAlimentosService.UpdateProductosIngreso(nuevoDetalle);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelIngreso);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProductoIngreso(Int64 id)
        {
            var result = await _ingresoAlimentosService.DeleteProductosIngreso(id);

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
        public async Task<IActionResult> EliminarIngresoAlimentos(Int64 id)
        {
            var result = await _ingresoAlimentosService.DeleteIngresoAlimentos(id);

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

        #region Vista Junta Receptora - Estado 1 -- VISTA A ELIMINAR
        /*
        public async Task<IActionResult> JuntaReceptora(int id)
        {
            IngresoAlimentos ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos((Int64)id);
            JuntaReceptora? juntaReceptora = await _ingresoAlimentosService.GetJuntaReceptoraDeIngreso((Int64)id);
            List<DetalleJuntaReceptora>? listDetalle = await _ingresoAlimentosService.GetAllDetalleJuntaReceptora(juntaReceptora.IdJuntaReceptora);

            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var Convenio = await _convenioService.GetAll();
            ViewBag.Convenios = new SelectList(Convenio, "IdConvenio", "NombreConvenio");
            var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

            var bodega = await _bodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
            var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var tipoNombramiento = await _ingresoAlimentosService.GetAllTipoNombramiento();
            ViewBag.TiposNombramientos = new SelectList(tipoNombramiento, "IdTipoNombramiento", "NombreTipoNombramiento");
            var tipoNombramientoDict = tipoNombramiento.ToDictionary(i => i.IdTipoNombramiento, i => i.NombreTipoNombramiento);

            #endregion

            var IngresoAlimentosVM = new IngresoAlimentosVM()
            {
                IngresoAlimentos = ingresoAlimentos,
                JuntaReceptora = juntaReceptora,
                listaDetalleJuntaReceptora = listDetalle,
                ProgramaDict = ProgramaDict,
                ConvenioDict = ConvenioDict,
                BodegaDict = bodegaDict,
                Rol = User.FindFirst(ClaimTypes.Role)?.Value
            };

            return View(IngresoAlimentosVM);
        }

        public async Task<IActionResult> ObtenerJuntaReceptora(int id)
        {
            var ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos(id);
            var juntaReceptora = await _ingresoAlimentosService.GetJuntaReceptoraDeIngreso(id);
            var listDetalle = await _ingresoAlimentosService.GetAllDetalleJuntaReceptora(juntaReceptora.IdJuntaReceptora);

            if (ingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                ingresoAlimentos = ingresoAlimentos,
                programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == ingresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
                convenio = _convenioService.GetAll().Result.Where(o => o.IdConvenio == ingresoAlimentos.IdConvenio).Select(o => o.NombreConvenio).FirstOrDefault(),
                bodega = _bodegaService.GetAll().Result.Where(o => o.IdBodega == ingresoAlimentos.IdBodegaRecibe).Select(o => o.NombreBodega).FirstOrDefault(),
                juntaReceptora = juntaReceptora ?? new Models.JuntaReceptora(),
                listDetalle = listDetalle ?? new List<DetalleJuntaReceptora>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarJuntaReceptora([FromForm] string modelo, IFormFile file)
        {
            // Deserializar el modelo
            var ingresoAlimentosVM = JsonConvert.DeserializeObject<IngresoAlimentosVM>(modelo);

            JuntaReceptora modelJunta = new()
            {
                IdJuntaReceptora = ingresoAlimentosVM.JuntaReceptora.IdJuntaReceptora,
                IdIngresoAlimentos = ingresoAlimentosVM.JuntaReceptora.IdIngresoAlimentos,
                NumeroNombramiento = ingresoAlimentosVM.JuntaReceptora.NumeroNombramiento,
                ArchivoNombramiento = ingresoAlimentosVM.JuntaReceptora.ArchivoNombramiento,
                Estado = 0,
            };

            ServiceResponse<JuntaReceptora> result = new ServiceResponse<JuntaReceptora>();
            ServiceResponse<DetalleJuntaReceptora> resultDet = new ServiceResponse<DetalleJuntaReceptora>();
            long idJuntaReceptora = 0;

            string? nombreArchivo = null;

            // Almacenamiento de JuntaReceptora
            if (modelJunta.IdJuntaReceptora == 0)
            {
                result = await _ingresoAlimentosService.SaveJuntaReceptora(modelJunta);
                modelJunta.IdJuntaReceptora = result.Data.IdJuntaReceptora;
                idJuntaReceptora = result.Data.IdJuntaReceptora;
            }
            else
            {
                result = await _ingresoAlimentosService.UpdateJuntaReceptora(modelJunta);
                idJuntaReceptora = result.Data.IdJuntaReceptora;

            }

            // Almacenamiento de los detalles
            foreach (var detalle in ingresoAlimentosVM.listaDetalleJuntaReceptora)
            {
                DetalleJuntaReceptora nuevoDetalle = new DetalleJuntaReceptora
                {
                    IdDetalleJuntaReceptora = detalle.IdDetalleJuntaReceptora,
                    IdJuntaReceptora = (int)idJuntaReceptora,
                    NombreCompleto = detalle.NombreCompleto,
                    DPI = detalle.DPI,
                    IdTipoNombramiento = detalle.IdTipoNombramiento,
                    Estado = 0
                };

                if (nuevoDetalle.IdDetalleJuntaReceptora == 0)
                    resultDet = await _ingresoAlimentosService.SaveDetalleJuntaReceptora(nuevoDetalle);
                else
                    resultDet = await _ingresoAlimentosService.UpdateDetalleJuntaReceptora(nuevoDetalle);
            }


            // Almacenamiento del archivo
            if (file != null && file.Length > 0)
            {
                nombreArchivo = AlmacenamientoDocumentos(file, (int)idJuntaReceptora, "nombramientos");
                modelJunta.ArchivoNombramiento = nombreArchivo;
                result = await _ingresoAlimentosService.UpdateArchivoJuntaReceptora(modelJunta);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelJunta);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalleJuntaReceptora(Int64 id)
        {
            var result = await _ingresoAlimentosService.DeleteDetalleJuntaReceptora(id);

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
        */
        #endregion

        #region Vista Boleta Análisis -- VISTA A ELIMINAR

        //public async Task<IActionResult> BoletaAnalisis(int id)
        //{
        //    IngresoAlimentos ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos((Int64)id);

        //    #region Catálogos

        //    var Programa = await _programaService.GetAll();
        //    ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
        //    var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

        //    var Convenio = await _convenioService.GetAll();
        //    ViewBag.Convenios = new SelectList(Convenio, "IdConvenio", "NombreConvenio");
        //    var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

        //    var bodega = await _bodegaService.GetAll();
        //    ViewBag.Bodegas = new SelectList(bodega, "IdBodega", "NombreBodega");
        //    var bodegaDict = bodega.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

        //    var transportista = await _transportistaService.GetAll();
        //    ViewBag.Transportistas = new SelectList(transportista, "IdTransportista", "NombreTransportista");
        //    var transportistaDict = transportista.ToDictionary(i => i.IdTransportista, i => i.NombreTransportista);

        //    var vehiculo = await _vehiculoService.GetAll();
        //    ViewBag.Vehiculos = new SelectList(vehiculo, "IdVehiculo", "PlacaCabezal");
        //    var vehiculoDict = vehiculo.ToDictionary(i => i.IdVehiculo, i => i.PlacaCabezal);

        //    var piloto = await _pilotoService.GetAll();
        //    ViewBag.Pilotos = new SelectList(piloto, "IdPiloto", "NombresPiloto");
        //    var pilotoDict = piloto.ToDictionary(i => i.IdPiloto, i => i.NombresPiloto);

        //    #endregion

        //    var IngresoAlimentosVM = new IngresoAlimentosVM()
        //    {
        //        IngresoAlimentos = ingresoAlimentos,
        //        ProgramaDict = ProgramaDict,
        //        ConvenioDict = ConvenioDict,
        //        BodegaDict = bodegaDict,
        //        TransportistaDict = transportistaDict,
        //        VehiculoDict = vehiculoDict,
        //        PilotoDict = pilotoDict,
        //        Rol = User.FindFirst(ClaimTypes.Role)?.Value
        //    };

        //    return View(IngresoAlimentosVM);
        //}

        //public async Task<IActionResult> ObtenerBoletaAnalisis(int id)
        //{
        //    var ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos(id);

        //    if (ingresoAlimentos == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(new
        //    {
        //        ingresoAlimentos = ingresoAlimentos,
        //        programa = _programaService.GetAll().Result.Where(o => o.IdPrograma == ingresoAlimentos.IdPrograma).Select(o => o.NombrePrograma).FirstOrDefault(),
        //        convenio = _convenioService.GetAll().Result.Where(o => o.IdConvenio == ingresoAlimentos.IdConvenio).Select(o => o.NombreConvenio).FirstOrDefault(),
        //        bodega = _bodegaService.GetAll().Result.Where(o => o.IdBodega == ingresoAlimentos.IdBodegaRecibe).Select(o => o.NombreBodega).FirstOrDefault(),
        //    });
        //}

        //public async Task<IActionResult> ObtenerDatosPiloto(int id)
        //{
        //    var piloto = await _pilotoService.GetPiloto(id);

        //    if (piloto == null)
        //    {
        //        return NotFound();
        //    }

        //    return Json(new
        //    {
        //        piloto = piloto
        //    });
        //}   

        //[HttpPost]
        //public async Task<IActionResult> GuardarBoletaAnalisis([FromForm] string modelo, IFormFile file)
        //{
        //    // Deserializar el modelo
        //    var ingresoAlimentosVM = JsonConvert.DeserializeObject<IngresoAlimentosVM>(modelo);

        //    IngresoAlimentos modelIngreso = new IngresoAlimentos()
        //    {
        //        IdIngresoAlimentos = ingresoAlimentosVM.IngresoAlimentos.IdIngresoAlimentos,
        //        NoBoletaAnalisis = ingresoAlimentosVM.IngresoAlimentos.NoBoletaAnalisis,
        //        NombreResponsableIndeca = ingresoAlimentosVM.IngresoAlimentos.NombreResponsableIndeca,
        //        IdTransportista = ingresoAlimentosVM.IngresoAlimentos.IdTransportista,
        //        IdVehiculo = ingresoAlimentosVM.IngresoAlimentos.IdVehiculo,
        //        IdPiloto = ingresoAlimentosVM.IngresoAlimentos.IdPiloto,
        //        ObservacionesBoletaAnalisis = ingresoAlimentosVM.IngresoAlimentos.ObservacionesBoletaAnalisis,
        //        SeAceptaBoletaAnalisis = ingresoAlimentosVM.IngresoAlimentos.SeAceptaBoletaAnalisis,
        //        Estado = 0,
        //    };

        //    var nombreDoc = "";

        //    // Almacenamiento del archivo
        //    if (file != null && file.Length > 0)
        //    {
        //        // Obtenemos la extensión del archivo
        //        var extension = Path.GetExtension(file.FileName);
        //        // Obtenemos el nombre del archivo sin la extensión
        //        var nombreSinExtension = Path.GetFileNameWithoutExtension(file.FileName);
        //        // Concatenamos el ID al nombre antes de la extensión
        //        var nuevoNombre = $"{nombreSinExtension}_{modelIngreso.IdIngresoAlimentos}{extension}";

        //        nombreDoc = nuevoNombre;

        //        // Ruta donde se guardará el archivo
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/boletaAnalisis", nuevoNombre);

        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //    }

        //    modelIngreso.ArchivoBoletaAnalisis = nombreDoc;

        //    // Se actualizan los nuevos campos en la tabla INGRESO_ALIMENTOS
        //    var result = await _ingresoAlimentosService.UpdateIngresoAlimentosEstado2(modelIngreso);

        //    // Se crea la nueva Bitácora
        //    CrearBitacora((int)modelIngreso.IdIngresoAlimentos, (int)EstadosIngresoEnum.EnRecepcionDeAlimentos);

        //    // Se finaliza cambiando el estado 
        //    await CambioEstado((int)modelIngreso.IdIngresoAlimentos, (int)EstadosIngresoEnum.Registrar1H);

        //    if (result.Success && result.Data != null)
        //    {
        //        TempData["Mensaje"] = result.Message;
        //        return RedirectToAction("Index", "IngresoAlimentos");
        //    }
        //    else
        //    {
        //        TempData["Mensaje"] = result.Message;
        //        return View(modelIngreso);
        //    }
        //}

        #endregion

        #region Vista Recepción de Alimentos en Bodega - Estado 2

        public async Task<IActionResult> RecepcionAlimentos(int id)
        {
            IngresoAlimentos listado = await _ingresoAlimentosService.GetIngresoAlimentos((Int64)id);

            #region Catálogos

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

            var tipoNombramiento = await _ingresoAlimentosService.GetAllTipoNombramiento();
            ViewBag.TiposNombramientos = new SelectList(tipoNombramiento, "IdTipoNombramiento", "NombreTipoNombramiento");
            var tipoNombramientoDict = tipoNombramiento.ToDictionary(i => i.IdTipoNombramiento, i => i.NombreTipoNombramiento);

            #endregion

            if (id == 0)
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
            else
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerDetallesWaybill(int id)
        {
            var salidas = await _ingresoAlimentosService.GetDetalleWaybillDeIngreso(id);

            var listDetalles = new List<ProductosIngreso>();

            // Se agregan todos los detalles con los que cuenta para calcular las cantidades de los productos que se deben de agregar para la salida
            if (salidas.Count() > 0)
            {
                foreach (var salida in salidas)
                {
                    var detalle = _ingresoAlimentosService.GetAllProductosIngreso(id).Result.Where(o => o.IdBoletaWaybill == salida.IdDetalleWaybill).ToList();

                    foreach (var det in detalle)
                    {
                        listDetalles.Add(det);
                    }
                }
            }

            if (salidas == null)
            {
                return NotFound();
            }

            return Json(new
            {
                listaWaybill = salidas,
                listaDetalles = listDetalles
            });
        }

        public async Task<IActionResult> ObtenerDetallesPorductosWaybill(int id)
        {
            var salidas = await _ingresoAlimentosService.GetDetalleWaybill(id);

            var listDetalles = _ingresoAlimentosService.GetAllProductosIngreso((Int64)salidas.IdIngresoAlimentos).Result.Where(o => o.IdBoletaWaybill == id).ToList();


            return Json(new
            {
                detalleWaybill = salidas,
                detalleProductos = listDetalles
            });
        }

        public async Task<IActionResult> ObtenerRecepcionAlimentosBodega(int id)
        {
            var ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos(id);
            var registroBoleta = await _ingresoAlimentosService.GetAllDetalleBoletaAnalisis(id);
            var juntaReceptora = await _juntaDeRecepcionService.GetJuntaReceptora((Int64)ingresoAlimentos.IdJuntaReceptora);
            var detalleJuntaReceptora = await _juntaDeRecepcionService.GetAllDetalleJuntaReceptora(juntaReceptora.IdJuntaReceptora);
            var productosIngreso = await _ingresoAlimentosService.GetAllProductosIngreso(id);
            var estibas = await _estibaService.GetAll();
            var detalleBoletaAnalisis = await _ingresoAlimentosService.GetAllDetalleBoletaAnalisis(id);

            if (ingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                ingresoAlimentos = ingresoAlimentos,
                registroBoleta = registroBoleta,
                detalleJuntaReceptora = detalleJuntaReceptora,
                productosIngreso = productosIngreso,
                estibas = estibas,
                detalleBoletaAnalisis = detalleBoletaAnalisis
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarDetallesWaybill([FromBody] IngresoAlimentosVM modelo)
        {
            DetalleWaybill modelWaybill = new()
            {
                IdDetalleWaybill = 0,
                IdIngresoAlimentos = modelo.DetalleWaybill.IdIngresoAlimentos,
                NoBoleta = modelo.DetalleWaybill.NoBoleta,
                FechaBoleta = modelo.DetalleWaybill.FechaBoleta,
                Estado = 0
            };

            ServiceResponse<DetalleWaybill> result = new();
            ServiceResponse<ProductosIngreso> resultDet = new();
            long idDetalle = 0;

            // Almacenamiento de DetalleWaybill
            result = await _ingresoAlimentosService.SaveDetalleWaybill(modelWaybill);
            idDetalle = result.Data.IdDetalleWaybill;


            // Almacenamiento de los detalles
            foreach (var detalle in modelo.listaProductosIngreso)
            {
                ProductosIngreso nuevoDetalle = new()
                {
                    IdProductosIngreso = detalle.IdProductosIngreso,
                    IdBoletaWaybill = (int?)idDetalle
                };

                resultDet = await _ingresoAlimentosService.UpdateProductosIngresoWaybillEstado2(nuevoDetalle);
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
        public async Task<IActionResult> GuardarRecepcionAlimentosBodega([FromForm] string modelo, IFormFile fileWaibil, IFormFile fileActa)
        {
            // Deserializar el modelo
            var ingresoAlimentosVM = JsonConvert.DeserializeObject<IngresoAlimentosVM>(modelo);

            IngresoAlimentos modelIngreso = new IngresoAlimentos()
            {
                IdIngresoAlimentos = ingresoAlimentosVM.IngresoAlimentos.IdIngresoAlimentos,
                NoWaybill = ingresoAlimentosVM.IngresoAlimentos.NoWaybill,
                FechaWaybill = ingresoAlimentosVM.IngresoAlimentos.FechaWaybill,
                NoLti = ingresoAlimentosVM.IngresoAlimentos.NoLti,
                FirmaSelloEncargadoWaybill = ingresoAlimentosVM.IngresoAlimentos.FirmaSelloEncargadoWaybill,
                NombreResponsableIndeca = ingresoAlimentosVM.IngresoAlimentos.NombreResponsableIndeca,
                NoActa = ingresoAlimentosVM.IngresoAlimentos.NoActa,
                FolioInicialActa = ingresoAlimentosVM.IngresoAlimentos.FolioInicialActa,
                FolioFinalActa = ingresoAlimentosVM.IngresoAlimentos.FolioFinalActa,
                FirmaMiembrosJuntaActa = ingresoAlimentosVM.IngresoAlimentos.FirmaMiembrosJuntaActa,
                FirmaEncargadoBodegaActa = ingresoAlimentosVM.IngresoAlimentos.FirmaEncargadoBodegaActa
            };

            ServiceResponse<IngresoAlimentos> result = new ServiceResponse<IngresoAlimentos>();
            ServiceResponse<DetalleBoletaAnalisis> resultDetBoleta = new ServiceResponse<DetalleBoletaAnalisis>();
            ServiceResponse<DetalleJuntaReceptora> resultDetJunta = new ServiceResponse<DetalleJuntaReceptora>();
            ServiceResponse<ProductosIngreso> resultDet = new ServiceResponse<ProductosIngreso>();
            ServiceResponse<Bitacora> resultBit = new ServiceResponse<Bitacora>();
            long idIngresoAlimentos = 0;

            // Almacenamiento de IngresoAlimentos
            string? nombreArchivoWaibil = null;
            string? nombreActaRecepcion = null;

            if (fileWaibil != null && fileWaibil.Length > 0)
            {
                nombreArchivoWaibil = await AlmacenamientoDocumentos(fileWaibil, (int)modelIngreso.IdIngresoAlimentos, "archivoWaibil");
            }
            if (fileActa != null && fileActa.Length > 0)
            {
                nombreActaRecepcion = await AlmacenamientoDocumentos(fileActa, (int)modelIngreso.IdIngresoAlimentos, "actaRecepcion");
            }

            modelIngreso.ArchivoWaybill = nombreArchivoWaibil;
            modelIngreso.DocumentoActa = nombreActaRecepcion;

            result = await _ingresoAlimentosService.UpdateIngresoAlimentosEstado2(modelIngreso);
            idIngresoAlimentos = result.Data.IdIngresoAlimentos;

            // Almacenamiento de los detalles de Boleta de Análisis
            foreach (var detalle in ingresoAlimentosVM.listaDetalleBoletaAnalisis)
            {
                DetalleBoletaAnalisis nuevoDetalle = new()
                {
                    IdDetalleBoletaAnalisis = detalle.IdDetalleBoletaAnalisis,
                    IdIngresoAlimentos = (int)idIngresoAlimentos,
                    NoBoletaAnalisis = detalle.NoBoletaAnalisis,
                };

                if (detalle.IdDetalleBoletaAnalisis == 0)
                    resultDetBoleta = await _ingresoAlimentosService.SaveDetalleBoletaAnalisis(nuevoDetalle);
                else
                    resultDetBoleta = await _ingresoAlimentosService.UpdateDetalleBoletaAnalisis(nuevoDetalle);
            }

            // Almacenamiento de los detalles de Junta Receptora
            foreach (var detalle in ingresoAlimentosVM.listaDetalleJuntaReceptora)
            {
                DetalleJuntaReceptora nuevoDetalle = new()
                {
                    IdDetalleJuntaReceptora = detalle.IdDetalleJuntaReceptora,
                    EstaPresente = detalle.EstaPresente,
                };

                resultDetJunta = await _ingresoAlimentosService.UpdateDetalleJuntaReceptoraEstado2(nuevoDetalle);
            }


            // Almacenamiento de los detalles de productos
            foreach (var detalle in ingresoAlimentosVM.listaProductosIngreso)
            {
                ProductosIngreso nuevoDetalle = new ProductosIngreso
                {
                    IdProductosIngreso = detalle.IdProductosIngreso,
                    Estiba = detalle.Estiba.ToUpper().Trim(),
                    FechaIngreso = detalle.FechaIngreso,
                    FechaVencimiento = detalle.FechaVencimiento,
                };

                resultDet = await _ingresoAlimentosService.UpdateProductosIngresoEstado2(nuevoDetalle);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelIngreso);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalleWaybill(Int64 id)
        {
            var result = await _ingresoAlimentosService.DeleteDetalleWaybill(id);

            if (result.Success && result.Data != null)
            {
                return Json(new
                {
                    mensaje = result.Message,
                    codigo = result.Success,
                    listaDetallesWaybill = _ingresoAlimentosService.GetAllProductosIngreso(id).Result.Where(o => o.IdBoletaWaybill == id).ToList()
                });
            }
            else
            {
                return Json(new
                {
                    mensaje = result.Message,
                    codigo = result.Success,
                    listaDetallesWaybill = _ingresoAlimentosService.GetAllProductosIngreso(id).Result.Where(o => o.IdBoletaWaybill == id).ToList()
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalleBoletaAnalisis(Int64 id)
        {
            var result = await _ingresoAlimentosService.DeleteDetalleBoletaAnalisis(id);

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

        #region Vista de Registro de Forma 1-H - Estado 3

        public async Task<IActionResult> RegistroForma1H(int id)
        {
            IngresoAlimentos listado = await _ingresoAlimentosService.GetIngresoAlimentos((Int64)id);

            #region Catálogos

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

            var Proveedor = await _proveedorService.GetAll();
            ViewBag.Proveedores = new SelectList(Proveedor, "IdProveedor", "NombreProveedor");
            var ProveedorDict = Proveedor.ToDictionary(i => i.IdProveedor, i => i.NombreProveedor);

            #endregion

            if (id == 0)
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
            else
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerRregistroForma1H(int id)
        {
            var ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos(id);
            var productosIngreso = await _ingresoAlimentosService.GetAllProductosIngreso(id);

            if (ingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                ingresoAlimentos = ingresoAlimentos,
                productosIngreso = productosIngreso
            });
        }

        public IActionResult ObtenerUsuariosJefeBodega()
        {
            var idRol = _usuarioService.GetAllRoles().Result.Where(o => o.Nombre == "Jefe de Bodega").Select(s => s.IdRol).First();
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
        public async Task<IActionResult> GuardarForma1H([FromForm] string modelo)
        {
            // Deserializar el modelo

            var ingresoAlimentosVM = JsonConvert.DeserializeObject<IngresoAlimentosVM>(modelo);


            IngresoAlimentos modelIngreso = new IngresoAlimentos()
            {
                IdIngresoAlimentos = ingresoAlimentosVM.IngresoAlimentos.IdIngresoAlimentos,
                NoForma1h = ingresoAlimentosVM.IngresoAlimentos.NoForma1h,
                FechaForma1h = ingresoAlimentosVM.IngresoAlimentos.FechaForma1h,
                IdProveedor1H = ingresoAlimentosVM.IngresoAlimentos.IdProveedor1H,
            };

            ServiceResponse<IngresoAlimentos> result = new ServiceResponse<IngresoAlimentos>();
            ServiceResponse<ProductosIngreso> resultDet = new ServiceResponse<ProductosIngreso>();
            ServiceResponse<Bitacora> resultBit = new ServiceResponse<Bitacora>();
            long idIngresoAlimentos = 0;

            // Almacenamiento de IngresoAlimentos

            result = await _ingresoAlimentosService.UpdateIngresoAlimentosEstado3(modelIngreso);
            idIngresoAlimentos = result.Data.IdIngresoAlimentos;

            // Almacenamiento de los detalles
            foreach (var detalle in ingresoAlimentosVM.listaProductosIngreso)
            {
                ProductosIngreso nuevoDetalle = new ProductosIngreso
                {
                    IdProductosIngreso = detalle.IdProductosIngreso,
                    FolioLibroAlmacen = detalle.FolioLibroAlmacen,
                    PrecioUnidad = detalle.PrecioUnidad,
                    ValorTotal = detalle.ValorTotal,
                };

                resultDet = await _ingresoAlimentosService.UpdateProductosIngresoEstado3(nuevoDetalle);
            }


            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelIngreso);
            }
        }

        #endregion

        #region Vista Copias Firmadas - Estado 4

        public async Task<IActionResult> CopiasFirmadas(int id)
        {
            IngresoAlimentos listado = await _ingresoAlimentosService.GetIngresoAlimentos((Int64)id);

            #region Catálogos


            #endregion

            if (id == 0)
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
            else
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = listado,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(IngresoAlimentosVM);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GuardarCopiasFirmadas([FromForm] string modelo, IFormFile fileWaibil, IFormFile fileActa, IFormFile fileForma1H, IFormFile fileBoleta)
        {
            // Deserializar el modelo
            var ingresoAlimentosVM = JsonConvert.DeserializeObject<IngresoAlimentosVM>(modelo);

            IngresoAlimentos modelIngreso = new IngresoAlimentos()
            {
                IdIngresoAlimentos = ingresoAlimentosVM.IngresoAlimentos.IdIngresoAlimentos
            };

            ServiceResponse<IngresoAlimentos> result = new ServiceResponse<IngresoAlimentos>();
            ServiceResponse<Bitacora> resultBit = new ServiceResponse<Bitacora>();
            long idIngresoAlimentos = 0;

            // Almacenamiento de IngresoAlimentos
            string? nombreArchivoWaibil = null;
            string? nombreActaRecepcion = null;
            string? nombreForma1H = null;
            string? nombreBoleta = null;

            if (fileWaibil != null && fileWaibil.Length > 0)
            {
                nombreArchivoWaibil = await AlmacenamientoDocumentos(fileWaibil, (int)modelIngreso.IdIngresoAlimentos, "archivoWaibil-Firmado");
            }
            if (fileActa != null && fileActa.Length > 0)
            {
                nombreActaRecepcion = await AlmacenamientoDocumentos(fileActa, (int)modelIngreso.IdIngresoAlimentos, "actaRecepcion-Firmado");
            }
            if (fileForma1H != null && fileForma1H.Length > 0)
            {
                nombreForma1H = await AlmacenamientoDocumentos(fileForma1H, (int)modelIngreso.IdIngresoAlimentos, "Forma1H-Firmado");
            }
            if (fileBoleta != null && fileBoleta.Length > 0)
            {
                nombreBoleta = await AlmacenamientoDocumentos(fileForma1H, (int)modelIngreso.IdIngresoAlimentos, "boletaAnalisis");
            }

            modelIngreso.ArchivoWaybillFinanciero = nombreArchivoWaibil;
            modelIngreso.ArchivoActaFinanciero = nombreActaRecepcion;
            modelIngreso.ArchivoForma1hFinanciero = nombreForma1H;
            modelIngreso.ArchivoBoletaAnalisis = nombreBoleta;

            result = await _ingresoAlimentosService.UpdateIngresoAlimentosEstado4(modelIngreso);
            idIngresoAlimentos = result.Data.IdIngresoAlimentos;

            // Se finaliza cambiando el estado 
            await CambioEstado((int)modelIngreso.IdIngresoAlimentos, (int)EstadosIngresoEnum.IngresoFinalizado, null);


            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelIngreso);
            }
        }

        #endregion

        #region vista Finalización Ingreso

        public async Task<IActionResult> IngresoAlimentosFinalizado(int id)
        {
            IngresoAlimentos ingreso = await _ingresoAlimentosService.GetIngresoAlimentos(id);
            List<Bodega> listadoBodegas = await _bodegaService.GetAll();
            var listBitacoras = await _ingresoAlimentosService.GetAllBitacora(id);
            var juntaReceptora = await _juntaDeRecepcionService.GetJuntaReceptora((Int64)ingreso.IdJuntaReceptora);
            var detalleJuntaReceptora = await _juntaDeRecepcionService.GetAllDetalleJuntaReceptora(juntaReceptora.IdJuntaReceptora);

            #region Catálogos

            var Programa = await _programaService.GetAll();
            ViewBag.Programas = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

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

            var tipoNombramiento = await _ingresoAlimentosService.GetAllTipoNombramiento();
            ViewBag.TiposNombramientos = new SelectList(tipoNombramiento, "IdTipoNombramiento", "NombreTipoNombramiento");
            var tipoNombramientoDict = tipoNombramiento.ToDictionary(i => i.IdTipoNombramiento, i => i.NombreTipoNombramiento);

            var estadoIngreso = await _ingresoAlimentosService.GetAllEstadoIngresos();
            ViewBag.EstadoIngresos = new SelectList(estadoIngreso, "IdEstadoIngresos", "EstadoIngreso");
            var EstadoIngresosDict = estadoIngreso.ToDictionary(i => i.IdEstadoIngresos, i => i.EstadoIngreso);

            var usuarios = _usuarioService.GetAll().Result.Select(o => new { o.IdUsuario, o.Nombre, o.Apellidos }).ToList();
            ViewBag.Usuarios = new SelectList(usuarios, "IdUsuario", "Nombre");

            #endregion

            if (id == 0)
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = ingreso,
                    listaBitacoraIngreso = listBitacoras,
                    JuntaReceptora = juntaReceptora,
                    listaDetalleJuntaReceptora = detalleJuntaReceptora,
                    ListaBodegas = listadoBodegas,
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoIngresoDict = EstadoIngresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(IngresoAlimentosVM);
            }
            else
            {
                var IngresoAlimentosVM = new IngresoAlimentosVM()
                {
                    IngresoAlimentos = ingreso,
                    listaBitacoraIngreso = listBitacoras,
                    JuntaReceptora = juntaReceptora,
                    listaDetalleJuntaReceptora = detalleJuntaReceptora,
                    ListaBodegas = listadoBodegas,
                    ProgramaDict = ProgramaDict,
                    ConvenioDict = ConvenioDict,
                    DepartamentoDict = departamentoDict,
                    MunicipioDict = municipioDict,
                    BodegaDict = bodegaDict,
                    EstadoIngresoDict = EstadoIngresosDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value,
                    IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                };

                return View(IngresoAlimentosVM);
            }
        }

        public async Task<IActionResult> ObtenerIngresoAlimentosFinalizado(int id)
        {
            var ingresoAlimentos = await _ingresoAlimentosService.GetIngresoAlimentos(id);
            var bodegaRecibe = await _bodegaService.GetBodega((int)ingresoAlimentos.IdBodegaRecibe);
            var registroBoleta = await _ingresoAlimentosService.GetAllDetalleBoletaAnalisis(id);
            var juntaReceptora = await _juntaDeRecepcionService.GetJuntaReceptora((Int64)ingresoAlimentos.IdJuntaReceptora);
            var detalleJuntaReceptora = await _juntaDeRecepcionService.GetAllDetalleJuntaReceptora(juntaReceptora.IdJuntaReceptora);
            var productosIngreso = await _ingresoAlimentosService.GetAllProductosIngreso(id);
            var waybill = await _ingresoAlimentosService.GetDetalleWaybillDeIngreso(id);
            var estibas = await _estibaService.GetAll();
            var detalleBoletaAnalisis = await _ingresoAlimentosService.GetAllDetalleBoletaAnalisis(id);
            var BitacoraEstado1 = _ingresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosIngresoEnum.Inicio).FirstOrDefault();
            var usuarioEstado1 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado1.IdUsuario).FirstOrDefault();

            var BitacoraEstado2 = _ingresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosIngresoEnum.EnRecepcionDeAlimentos).FirstOrDefault();
            var usuarioEstado2 = new Usuario();
            if (BitacoraEstado2 != null)
                usuarioEstado2 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado2.IdUsuario).FirstOrDefault();

            var BitacoraEstado3 = _ingresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosIngresoEnum.Registrar1H).FirstOrDefault();
            var usuarioEstado3 = new Usuario();
            if (BitacoraEstado3 != null)
                usuarioEstado3 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado3.IdUsuario).FirstOrDefault();

            var BitacoraEstado4 = _ingresoAlimentosService.GetAllBitacora(id).Result.Where(o => o.Accion == (int)EstadosIngresoEnum.PendienteDeFinalizar).FirstOrDefault();
            var usuarioEstado4 = new Usuario();
            if (BitacoraEstado4 != null)
                usuarioEstado4 = _usuarioService.GetAll().Result.Where(o => o.IdUsuario == BitacoraEstado4.IdUsuario).FirstOrDefault();

            if (ingresoAlimentos == null)
            {
                return NotFound();
            }

            return Json(new
            {
                ingresoAlimentos = ingresoAlimentos,
                bodegaRecibe = bodegaRecibe,
                registroBoleta = registroBoleta,
                juntaReceptora = juntaReceptora,
                detalleJuntaReceptora = detalleJuntaReceptora,
                productosIngreso = productosIngreso,
                waybill = waybill,
                estibas = estibas,
                detalleBoletaAnalisis = detalleBoletaAnalisis,
                bitacoraEstado1 = BitacoraEstado1,
                usuarioEstado1 = usuarioEstado1,
                bitacoraEstado2 = BitacoraEstado2,
                usuarioEstado2 = usuarioEstado2,
                bitacoraEstado3 = BitacoraEstado3,
                usuarioEstado3 = usuarioEstado3,
                bitacoraEstado4 = BitacoraEstado4,
                usuarioEstado4 = usuarioEstado4
            });
        }

        #endregion

        #region || Cambio de Estados || 

        public async Task<IActionResult> CambioEstado(int id, int estado, int? idUsuario)
        {
            switch (estado)
            {
                case (int)EstadosIngresoEnum.EnRecepcionDeAlimentos:
                    /*
                    var juntaReceptora = await _ingresoAlimentosService.GetJuntaReceptoraDeIngreso(id);

                    if (juntaReceptora.IdJuntaReceptora == null || juntaReceptora.IdJuntaReceptora == 0)
                    {
                        TempData["Mensaje"] = "No se Permite la transición a Bodega - No se ha ingresado la Junta Receptora";
                        return RedirectToAction("Index");
                    }*/

                    break;
                case (int)EstadosIngresoEnum.Registrar1H:

                    // Se crea la nueva Bitácora
                    CrearBitacora(id, (int)EstadosIngresoEnum.EnRecepcionDeAlimentos);

                    break;

                case (int)EstadosIngresoEnum.PendienteDeFinalizar:

                    // Se asigna la el ingreso al usuario seleccionado
                    //var respAsig = await _ingresoAlimentosService.AsignacionUsuarioPendienteFinalizar(id, (int)idUsuario);

                    // Se crea la nueva Bitácora
                    CrearBitacora(id, (int)EstadosIngresoEnum.Registrar1H);

                    break;
                case (int)EstadosIngresoEnum.IngresoFinalizado:

                    // Se crea la nueva Bitácora
                    CrearBitacora(id, (int)EstadosIngresoEnum.PendienteDeFinalizar);

                    // Se crea copia exacta de todos los tados ingresados de la tabla PRODUCTOS_INGRESO hacia MOVIMIENTOS_PRODUCTOS
                    var resp = await _ingresoAlimentosService.MoverProductosIngresoAMovimientos(id);

                    // Se crea copia exacta de todos los tados ingresados de la tabla PRODUCTOS_INGRESO hacia KARDEX
                    var resp2 = await _ingresoAlimentosService.MoverProductosIngresoAKardex(id);

                    break;
            }


            var result = await _ingresoAlimentosService.CambioEsadoIngresoAlimentos(id, estado);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "IngresoAlimentos");
            }
        }

        #endregion

        #region || Creación de Bitácora ||  

        public async void CrearBitacora(int idIngresoAlimentos, int estadoIngreso)
        {
            Bitacora modelBitacota = new Bitacora()
            {
                IdBitacora = 0,
                Accion = estadoIngreso,
                IdRegistro = idIngresoAlimentos,
                NombreTabla = "INGRESO_ALIMENTOS",
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                FechaHora = DateTime.Now,
                Estado = 0
            };

            var resultBit = await _ingresoAlimentosService.SaveBitacora(modelBitacota);
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

        #region Métodos para Edición de Productos en Modal

        [HttpPost]
        public async Task<IActionResult> ActualizarProductoIngreso([FromBody] ProductosIngreso modelo)
        {
            try
            {
                // Validar que el modelo sea válido
                if (modelo == null)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                // Llamar al servicio para actualizar
                var result = await _ingresoAlimentosService.UpdateProductosIngreso(modelo);

                if (result.Success && result.Data != null)
                {
                    return Json(new
                    {
                        success = true,
                        message = result.Message,
                        producto = new
                        {
                            IdProductosIngreso = result.Data.IdProductosIngreso,
                            IdProductoAlimenticio = result.Data.IdProductoAlimenticio,
                            IdPresentacion = result.Data.IdPresentacion,
                            CantidadUnidad = result.Data.CantidadUnidad,
                            IdUnidadMedida = result.Data.IdUnidadMedida,
                            CantidadRecibir = result.Data.CantidadRecibir
                        }
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = result.Message ?? "Error al actualizar el producto"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error interno: {ex.Message}"
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductoIngreso(int id)
        {
            try
            {
                var producto = await _ingresoAlimentosService.GetProductosIngreso(id);

                if (producto == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado" });
                }

                return Json(new
                {
                    success = true,
                    producto = new
                    {
                        IdProductosIngreso = producto.IdProductosIngreso,
                        IdProductoAlimenticio = producto.IdProductoAlimenticio,
                        IdPresentacion = producto.IdPresentacion,
                        CantidadUnidad = producto.CantidadUnidad,
                        IdUnidadMedida = producto.IdUnidadMedida,
                        CantidadRecibir = producto.CantidadRecibir
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error al obtener el producto: {ex.Message}"
                });
            }
        }

        #endregion
    }
}
