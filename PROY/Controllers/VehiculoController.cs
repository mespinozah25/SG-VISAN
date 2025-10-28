using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly IVehiculoService _vehiculoService;
        private readonly ITransportistaService _transportistaService;

        public VehiculoController(IVehiculoService vehiculoService, ITransportistaService transportistaService)
        {
            _vehiculoService = vehiculoService;
            _transportistaService = transportistaService;
        }

        public async Task<IActionResult> Index(int? idData)
        {
            List<Vehiculo> listado = await _vehiculoService.GetAll();

            #region Catálogos  

            var transportista = await _transportistaService.GetAll();
            ViewBag.Transportistas = new SelectList(transportista, "IdTransportista", "NombreTransportista");
            var transportistaDict = transportista.ToDictionary(i => i.IdTransportista, i => i.NombreTransportista);

            #endregion

            if (idData == null || idData == 0)
            {
                var dataVM = new VehiculoVM()
                {
                    Vehiculo = new Vehiculo(),
                    listaVehiculos = listado,
                    TransportistaDict = transportistaDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO VEHICULO";
                return View(dataVM);
            }
            else
            {
                var dataVM = new VehiculoVM()
                {
                    Vehiculo = await _vehiculoService.GetVehiculo((int)idData),
                    listaVehiculos = listado,
                    TransportistaDict = transportistaDict
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR VEHICULO";
                return View(dataVM);
            }
        }

        public async Task<IActionResult> ObtenerVehiculo(int id)
        {
            var dataResp = await _vehiculoService.GetVehiculo(id);

            if (dataResp == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idVehiculo = dataResp.IdVehiculo,
                idTransportista = dataResp.IdTransportista,
                placaCabezal = dataResp.PlacaCabezal,
                plazaPlataforma = dataResp.PlacaPlataforma,
                estado = dataResp.Estado
            });

        }

        public async Task<IActionResult> GuardarVehiculo(VehiculoVM modelo)
        {
            Vehiculo modelData = new Vehiculo()
            {
                IdVehiculo = modelo.Vehiculo.IdVehiculo,
                IdTransportista = modelo.Vehiculo.IdTransportista,
                PlacaCabezal = modelo.Vehiculo.PlacaCabezal,
                PlacaPlataforma = modelo.Vehiculo.PlacaPlataforma,
                Estado = 0,
            };

            if (modelData.IdVehiculo == 0)
            {
                var result = await _vehiculoService.SaveVehiculo(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Vehiculo");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
            else
            {
                var result = await _vehiculoService.UpdateVehiculo(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Vehiculo");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _vehiculoService.DeleteVehiculo(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Vehiculo");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
