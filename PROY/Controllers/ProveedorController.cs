using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;

        public ProveedorController(IProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }


        public async Task<IActionResult> Index(int? idProveedor)
        {
            List<Proveedor> listado = await _proveedorService.GetAll();

            if (idProveedor == null || idProveedor == 0)
            {
                var proveedorVM = new ProveedorVM()
                {
                    Proveedor = new Proveedor(),
                    listaProveedor = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PROVEEDOR";
                return View(proveedorVM);
            }
            else
            {
                var proveedorVM = new ProveedorVM()
                {
                    Proveedor = await _proveedorService.GetProveedor((int)idProveedor),
                    listaProveedor = await _proveedorService.GetAll(),
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PROVEEDOR ";
                return View(proveedorVM);
            }
        }


        public async Task<IActionResult> ObtenerProveedor(int id)
        {
            var proveedor = await _proveedorService.GetProveedor(id);

            if (proveedor == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idProveedor = proveedor.IdProveedor,
                nombre = proveedor.NombreProveedor,
                estado = proveedor.Estado
            });

        }

        public async Task<IActionResult> GuardarProveedor(ProveedorVM modelo)
        {
            Proveedor modelProveedor = new Proveedor()
            {
                IdProveedor = modelo.Proveedor.IdProveedor,
                NombreProveedor = modelo.Proveedor.NombreProveedor,
                Estado = 0,
            };

            if (modelProveedor.IdProveedor == 0)
            {
                var result = await _proveedorService.SaveProveedor(modelProveedor);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Proveedor");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelProveedor);
                }
            }
            else
            {
                var result = await _proveedorService.UpdateProveedor(modelProveedor);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Proveedor");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelProveedor);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _proveedorService.DeleteProveedor(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Proveedor");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
