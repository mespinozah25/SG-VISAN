using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class ProductoAlimenticioController : Controller
    {
        private readonly IProductoAlimenticioService _productoalimenticioService;

        public ProductoAlimenticioController(IProductoAlimenticioService productoalimenticioService)
        {
            _productoalimenticioService = productoalimenticioService;
        }


        public async Task<IActionResult> Index(int? idProductoAlimenticio)
        {
            List<ProductoAlimenticio> listado = await _productoalimenticioService.GetAll();

            if (idProductoAlimenticio == null || idProductoAlimenticio == 0)
            {
                var productoalimenticioVM = new ProductoAlimenticioVM()
                {
                    ProductoAlimenticio = new ProductoAlimenticio(),
                    listaProductoAlimenticio = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PRODUCTO ALIMENTICIO";
                return View(productoalimenticioVM);
            }
            else
            {
                var productoalimenticioVM = new ProductoAlimenticioVM()
                {
                    ProductoAlimenticio = await _productoalimenticioService.GetProductoAlimenticio((int)idProductoAlimenticio),
                    listaProductoAlimenticio = await _productoalimenticioService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PRODUCTO ALIMENTICIO ";
                return View(productoalimenticioVM);
            }
        }


        public async Task<IActionResult> ObtenerProductoAlimenticio(int id)
        {
            var productoalimenticio = await _productoalimenticioService.GetProductoAlimenticio(id);

            if (productoalimenticio == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idProductoAlimenticio = productoalimenticio.IdProductoAlimenticio,
                nombre = productoalimenticio.NombrePA,
                estado = productoalimenticio.Estado
            });

        }

        public async Task<IActionResult> GuardarProductoAlimenticio(ProductoAlimenticioVM modelo)
        {
            ProductoAlimenticio modelProductoAlimenticio = new ProductoAlimenticio()
            {
                IdProductoAlimenticio = modelo.ProductoAlimenticio.IdProductoAlimenticio,
                NombrePA = modelo.ProductoAlimenticio.NombrePA,
                Estado = 0,
            };

            if (modelProductoAlimenticio.IdProductoAlimenticio == 0)
            {
                var result = await _productoalimenticioService.SaveProductoAlimenticio(modelProductoAlimenticio);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "ProductoAlimenticio");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelProductoAlimenticio);
                }
            }
            else
            {
                var result = await _productoalimenticioService.UpdateProductoAlimenticio(modelProductoAlimenticio);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "ProductoAlimenticio");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelProductoAlimenticio);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _productoalimenticioService.DeleteProductoAlimenticio(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "ProductoAlimenticio");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
