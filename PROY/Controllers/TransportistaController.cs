using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class TransportistaController : Controller
    {
        private readonly ITransportistaService _transportistaService;

        public TransportistaController(ITransportistaService transportistaService)
        {
            _transportistaService = transportistaService;
        }

        public async Task<IActionResult> Index(int? idData)
        {
            List<Transportista> listado = await _transportistaService.GetAll();

            #region Catálogos  


            #endregion

            if (idData == null || idData == 0)
            {
                var dataVM = new TransportistaVM()
                {
                    Transportista = new Transportista(),
                    listaTransportistas = listado
                };

                ViewBag.AbrirModalEditar = "new";
                ViewBag.ModalTitle = "NUEVO TRANSPORTISTA";
                return View(dataVM);
            }
            else
            {
                var dataVM = new TransportistaVM()
                {
                    Transportista = await _transportistaService.GetTransportista((int)idData),
                    listaTransportistas = await _transportistaService.GetAll()
                };

                ViewBag.AbrirModalEditar = "edit";
                ViewBag.ModalTitle = "MODIFICAR TRANSPORTISTA";
                return View(dataVM);
            }
        }

        public async Task<IActionResult> ObtenerTransportista(int id)
        {
            var dataResp = await _transportistaService.GetTransportista(id);

            if (dataResp == null)
            {
                return NotFound();
            }

            return Json(new
            {
                idTransportista = dataResp.IdTransportista,
                nombreTransportista = dataResp.NombreTransportista,
                telefono = dataResp.Telefono,
                nombreContacto = dataResp.NombreContacto,
                idUsuario = dataResp.IdUsuario,
                estado = dataResp.Estado
            });

        }

        public async Task<IActionResult> GuardarTransportista(TransportistaVM modelo)
        {
            Transportista modelData = new Transportista()
            {
                IdTransportista = modelo.Transportista.IdTransportista,
                NombreTransportista = modelo.Transportista.NombreTransportista,
                Telefono = modelo.Transportista.Telefono,
                NombreContacto = modelo.Transportista.NombreContacto,
                IdUsuario = int.Parse(User.FindFirst("IDuser").Value),
                Estado = 0,
            };

            if (modelData.IdTransportista == 0)
            {
                var result = await _transportistaService.SaveTransportista(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Transportista");
                }
                else
                {
                    TempData["Mensaje"] = result.Message;
                    return View(modelData);
                }
            }
            else
            {
                var result = await _transportistaService.UpdateTransportista(modelData);

                if (result.Success && result.Data != null)
                {
                    TempData["Mensaje"] = result.Message;
                    return RedirectToAction("Index", "Transportista");
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
            var result = await _transportistaService.DeleteTransportista(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "Transportista");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
