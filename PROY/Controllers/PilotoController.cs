using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class PilotoController : Controller
    {
        private readonly IPilotoService _pilotoService;
        private readonly ITransportistaService _transportistaService;

        public PilotoController(IPilotoService pilotoService, ITransportistaService transportistaService)
        {
            _pilotoService = pilotoService;
            _transportistaService = transportistaService;
        }

        public async Task<IActionResult> Index(int? idData)
        {
            List<Piloto> listado = await _pilotoService.GetAll();

            #region Catálogos  

            var transportista = await _transportistaService.GetAll();
            ViewBag.Transportistas = new SelectList(transportista, "IdTransportista", "NombreTransportista");
            var transportistaDict = transportista.ToDictionary(i => i.IdTransportista, i => i.NombreTransportista);

            #endregion

            if (idData == null || idData == 0)
            {
                var dataVM = new PilotoVM()
                {
                    Piloto = new Piloto(),
                    listaPilotos = listado,
                    TransportistaDict = transportistaDict
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO PILOTO";
                return View(dataVM);
            }
            else
            {
                var dataVM = new PilotoVM()
                {
                    Piloto = await _pilotoService.GetPiloto((int)idData),
                    listaPilotos = listado,
                    TransportistaDict = transportistaDict
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR PILOTO";
                return View(dataVM);
            }
        }

        public async Task<IActionResult> ObtenerPiloto(int id)
        {
            var dataResp = await _pilotoService.GetPiloto(id);

            if (dataResp == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idPiloto = dataResp.IdPiloto,
                idTransportista = dataResp.IdTransportista,
                nombresPiloto = dataResp.NombresPiloto,
                apellidosPiloto = dataResp.ApellidosPiloto,
                noLicencia = dataResp.NoLicencia,
                estado = dataResp.Estado
            });

        }

        public async Task<IActionResult> GuardarPiloto(PilotoVM modelo)
        {
            Piloto modelData = new Piloto()
            {
                IdPiloto = modelo.Piloto.IdPiloto,
                IdTransportista = modelo.Piloto.IdTransportista,
                NombresPiloto = modelo.Piloto.NombresPiloto.Trim(),
                ApellidosPiloto = modelo.Piloto.ApellidosPiloto.Trim(),
                NoLicencia = modelo.Piloto.NoLicencia,
                Estado = 0,
            };

            if (modelData.IdPiloto == 0)
            {
                var result = await _pilotoService.SavePiloto(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Piloto");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
            else
            {
                var result = await _pilotoService.UpdatePiloto(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Piloto");
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
            var result = await _pilotoService.DeletePiloto(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Piloto");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
