using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class EstibaController : Controller
    {
        private readonly IEstibaService _estibaService;
        private readonly IBodegaService _BodegaService;
        private readonly ISubBodegaService _SubBodegaService;
        private readonly IProductoAlimenticioService _productoAlimenticioService;

        public EstibaController(IEstibaService estibaService, IBodegaService bodegaService, ISubBodegaService subBodegaService, IProductoAlimenticioService productoAlimenticioService)
        {
            _estibaService = estibaService;
            _BodegaService = bodegaService;
            _SubBodegaService = subBodegaService;
            _productoAlimenticioService = productoAlimenticioService;
        }

        public async Task<IActionResult> Index(int idBodega, int idSubBodega, int? idData)
        {
            List<Estiba> listado = await _estibaService.ObtenerSegunBodegaYSubBodega(idBodega, idSubBodega);

            #region Catálogos  

            var bodegas = await _BodegaService.GetAll();
            ViewBag.Bodegas = new SelectList(bodegas, "IdBodega", "NombreBodega");
            var bodegasDict = bodegas.ToDictionary(i => i.IdBodega, i => i.NombreBodega);

            var subBodegas = await _SubBodegaService.GetAll();
            ViewBag.SubBodegas = new SelectList(subBodegas, "IdSubBodega", "NombreSubBodega");
            var subBodegasDict = subBodegas.ToDictionary(i => i.IdSubBodega, i => i.NombreSubBodega);

            var productos = await _productoAlimenticioService.GetAll();
            ViewBag.Productos = new SelectList(productos, "IdProductoAlimenticio", "NombrePA");
            var productosDict = productos.ToDictionary(i => i.IdProductoAlimenticio, i => i.NombrePA);

            #endregion

            if (idData == null || idData == 0)
            {
                var dataVM = new EstibaVM()
                {
                    Estiba = new Estiba(idBodega, idSubBodega),
                    listaEstibas = listado,
                    BodegaDict = bodegasDict,
                    SubBodegaDict = subBodegasDict,
                    ProductoDict = productosDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVA ESTIBA";
                return View(dataVM);
            }
            else
            {
                var dataVM = new EstibaVM()
                {
                    Estiba = await _estibaService.GetEstiba((int)idData),
                    listaEstibas = listado,
                    BodegaDict = bodegasDict,
                    SubBodegaDict = subBodegasDict,
                    ProductoDict = productosDict
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR ESTIBA";
                return View(dataVM);
            }
        }

        public async Task<IActionResult> ObtenerEstiba(int id)
        {
            var dataResp = await _estibaService.GetEstiba(id);

            if (dataResp == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idEstiba = dataResp.IdEstiba,
                idBodega = dataResp.IdBodega,
                idSubBodega = dataResp.IdSubBodega,
                codigo = dataResp.CodigoEstiba,
                descripcionEstiba = dataResp.DescripcionEstiba,
                idProducto = dataResp.IdProducto,
                metrosCuadrados = dataResp.MetrosCuadrados,
                idUsuario = dataResp.IdUsuario,
                estado = dataResp.Estado
            });

        }

        public async Task<IActionResult> GuardarEstiba(EstibaVM modelo)
        {
            Estiba modelData = new Estiba()
            {
                IdEstiba = modelo.Estiba.IdEstiba,
                IdBodega = modelo.Estiba.IdBodega,
                IdSubBodega = modelo.Estiba.IdSubBodega,
                CodigoEstiba = modelo.Estiba.CodigoEstiba,
                DescripcionEstiba = modelo.Estiba.DescripcionEstiba,
                IdProducto = modelo.Estiba.IdProducto,
                MetrosCuadrados = modelo.Estiba.MetrosCuadrados,
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                Estado = 0,
            };

            if (modelData.IdEstiba == 0)
            {
                var result = await _estibaService.SaveEstiba(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Estiba", new { idBodega = modelo.Estiba.IdBodega, idSubBodega = modelo.Estiba.IdSubBodega });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
            else
            {
                var result = await _estibaService.UpdateEstiba(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Estiba", new { idBodega = modelo.Estiba.IdBodega, idSubBodega = modelo.Estiba.IdSubBodega });
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id, int idBodega, int idSubBodega)
        {
            var result = await _estibaService.DeleteEstiba(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Estiba", new { idBodega, idSubBodega });
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
