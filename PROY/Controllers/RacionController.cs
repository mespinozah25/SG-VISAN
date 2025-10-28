using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PROY.Controllers
{
    public class RacionController : Controller
    {
        private readonly IRacionService _racionService;
        private readonly IProductoAlimenticioService _productoalimenticioService;
        private readonly IConvenioService _convenioService;
        private readonly IProgramaService _programaService;
        private readonly IUnidadMedidaService _unidadmedidaService;

        public RacionController(IRacionService racionService, IProductoAlimenticioService productoalimenticioService, IConvenioService convenioService, IProgramaService programaService, IUnidadMedidaService unidadmedidaService)
        {
            _racionService = racionService;
            _productoalimenticioService = productoalimenticioService;
            _convenioService = convenioService;
            _programaService = programaService;
            _unidadmedidaService = unidadmedidaService;
        }


        public async Task<IActionResult> Index(int? idRacion)
        {
            List<RacionMaestro> listado = await _racionService.GetAllRacionMaestro();

            #region Detalle Racion

            for (int i = 0; i < listado.Count; i++)
                listado[i].RacionDetalle = await _racionService.GetAllRacionDetalle(1);

            #endregion

            #region Catálogos

            var ProductoAlimenticio = await _productoalimenticioService.GetAll();
            ViewBag.ProductoAlimenticio = new SelectList(ProductoAlimenticio, "IdProductoAlimenticio", "NombrePA");
            var ProductoAlimenticioDict = ProductoAlimenticio.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            var Convenio = await _convenioService.GetAll();
            ViewBag.Convenio = new SelectList(Convenio, "IdConvenio", "NombreConvenio");
            var ConvenioDict = Convenio.ToDictionary(i => i.IdConvenio, i => i.NombreConvenio);

            var Programa = await _programaService.GetAll();
            ViewBag.Programa = new SelectList(Programa, "IdPrograma", "NombrePrograma");
            var ProgramaDict = Programa.ToDictionary(i => i.IdPrograma, i => i.NombrePrograma);

            var UnidadMedida = await _unidadmedidaService.GetAll();
            ViewBag.UnidadMedida = new SelectList(UnidadMedida, "IdUnidadMedida", "NombreUnidadMedida");
            var UnidadMedidaDict = UnidadMedida.ToDictionary(i => i.IdUnidadMedida, i => i.NombreUnidadMedida);

            #endregion

            if (idRacion == null || idRacion == 0)
            {
                var racionVM = new RacionVM()
                {
                    Racion = new RacionMaestro(),
                    listaRacion = listado,
                    ProductoAlimenticioDict = ProductoAlimenticioDict,
                    ConvenioDict = ConvenioDict,
                    ProgramaDict = ProgramaDict,
                    UnidadMedidaDict = UnidadMedidaDict,
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA RACION";
                return View(racionVM);
            }
            else
            {
                var racionVM = new RacionVM()
                {
                    Racion = await _racionService.GetRacionMaestro((int)idRacion),
                    listaRacion = listado,
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR RACIÓN ";
                return View(racionVM);
            }
        }

        public async Task<IActionResult> ObtenerRacion(int id)
        {
            var racion = await _racionService.GetRacionMaestro(id);
            var racionDetalle = await _racionService.GetAllRacionDetalle(id);

            if (racion == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idRacion = racion.IdRacionMaestro,
                IdPrograma = racion.IdPrograma,
                idConvenio = racion.IdConvenio,
                totalKilocalorias = racion.TotalKilocalorias,
                totalCarbohidratos = racion.TotalCarbohidratos,
                totalProteinas = racion.TotalProteinas,
                totalGrasas = racion.TotalGrasas,
                porcentajeKilocalorias = racion.PorcentajeKilocalorias,
                porcentajeCarbohidratos = racion.PorcentajeCarbohidratos,
                porcentajeProteinas = racion.PorcentajeProteinas,
                porcentajeGrasas = racion.PorcentajeGrasas,
                estado = racion.Estado,
                racionDetalle = racionDetalle
            });

        }

        [HttpPost]
        public async Task<IActionResult> GuardarRacion([FromBody] RacionVM modelo)
        {
            RacionMaestro modelRacion = new RacionMaestro()
            {
                IdRacionMaestro = modelo.Racion.IdRacionMaestro,
                IdConvenio = modelo.Racion.IdConvenio,
                IdPrograma = modelo.Racion.IdPrograma,
                TotalKilocalorias = modelo.Racion.TotalKilocalorias,
                TotalCarbohidratos = modelo.Racion.TotalCarbohidratos,
                TotalProteinas = modelo.Racion.TotalProteinas,
                TotalGrasas = modelo.Racion.TotalGrasas,
                PorcentajeKilocalorias = modelo.Racion.PorcentajeKilocalorias,
                PorcentajeCarbohidratos = modelo.Racion.PorcentajeCarbohidratos,
                PorcentajeProteinas = modelo.Racion.PorcentajeProteinas,
                PorcentajeGrasas = modelo.Racion.PorcentajeGrasas,
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                Estado = 0,
            };

            ServiceResponse<RacionMaestro> result = new ServiceResponse<RacionMaestro>();
            ServiceResponse<RacionDetalle> resultDet = new ServiceResponse<RacionDetalle>();
            long idRacionMaestro = 0;

            // Almacenamiento de Ración Maestro
            if (modelRacion.IdRacionMaestro == 0)
            {
                result = await _racionService.SaveRacionMaestro(modelRacion);
                idRacionMaestro = result.Data.IdRacionMaestro;
                
            }
            else
            {
                result = await _racionService.UpdateRacionMaestro(modelRacion);
                idRacionMaestro = result.Data.IdRacionMaestro;

            }

            // Almacenamiento de los detalles
            foreach (var detalle in modelo.listaRacionDetalle)
            {
                RacionDetalle nuevoDetalle = new RacionDetalle
                {
                    IdRacionDetalle = detalle.IdRacionDetalle, 
                    IdRacionMaestro = (int?)idRacionMaestro,
                    IdProductoAlimenticio = detalle.IdProductoAlimenticio,
                    CantidadMedida = detalle.CantidadMedida,
                    IdUnidadMedida = detalle.IdUnidadMedida,
                    Kilocalorias = detalle.Kilocalorias,
                    Carbohidratos = detalle.Carbohidratos,
                    Proteinas = detalle.Proteinas,
                    Grasas = detalle.Grasas,
                    Estado = 0
                };

                if (nuevoDetalle.IdRacionDetalle == 0)
                    resultDet = await _racionService.SaveRacionDetalle(nuevoDetalle);
                else
                    resultDet = await _racionService.UpdateRacionDetalle(nuevoDetalle);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Racion");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return View(modelRacion);
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _racionService.DeleteRacionMaestro(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Racion");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EliminarDetalle(Int64 id)
        {
            var result = await _racionService.DeleteRacionDetalle(id);

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
    }
}
