using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PROY.Models;
using PROY.Services;
using PROY.ViewModels;

namespace PROY.Controllers
{
    public class JuntaDeRecepcionController : Controller
    {
        #region Constructores

        private readonly IJuntaDeRecepcionService _juntaDeRecepcionService;

        public JuntaDeRecepcionController(IJuntaDeRecepcionService juntaDeRecepcionsService)
        {
            _juntaDeRecepcionService = juntaDeRecepcionsService;
        }

        #endregion

        #region Jyunta Receptora

        public async Task<IActionResult> Index(int? id)
        {
            List<JuntaReceptora>? lista = await _juntaDeRecepcionService.GetAllJuntaReceptora();

            #region Catálogos


            #endregion

            var juntaVM = new JuntaDeRecepcionVM()
            {
                listaJuntaReceptora = lista,
                Rol = User.FindFirst(ClaimTypes.Role)?.Value
            };

            return View(juntaVM);
        }

        public async Task<IActionResult> JuntaReceptora(int? id)
        {
            JuntaReceptora? junta = await _juntaDeRecepcionService.GetJuntaReceptora((Int64)id);
            var listDetalle = await _juntaDeRecepcionService.GetAllDetalleJuntaReceptora((Int64)id);

            #region Catálogos

            var tipoNombramiento = await _juntaDeRecepcionService.GetAllTipoNombramiento();
            ViewBag.TiposNombramientos = new SelectList(tipoNombramiento, "IdTipoNombramiento", "NombreTipoNombramiento");
            var tipoNombramientoDict = tipoNombramiento.ToDictionary(i => i.IdTipoNombramiento, i => i.NombreTipoNombramiento);

            #endregion



            if (id == 0)
            {
                var juntaVM = new JuntaDeRecepcionVM()
                {
                    JuntaReceptora = junta,
                    listaDetalleJuntaReceptora = listDetalle,
                    TipoNombramientoDict = tipoNombramientoDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(juntaVM);
            }
            else
            {
                var juntaVM = new JuntaDeRecepcionVM()
                {
                    JuntaReceptora = junta,
                    listaDetalleJuntaReceptora = listDetalle,
                    TipoNombramientoDict = tipoNombramientoDict,
                    Rol = User.FindFirst(ClaimTypes.Role)?.Value
                };

                return View(juntaVM);
            }
        }

        public async Task<IActionResult> ObtenerJuntaReceptora(int id)
        {
            var juntaReceptora = await _juntaDeRecepcionService.GetJuntaReceptora(id);
            var listDetalle = await _juntaDeRecepcionService.GetAllDetalleJuntaReceptora(id);

            if (juntaReceptora == null)
            {
                return NotFound();
            }

            return Json(new
            {
                juntaReceptora = juntaReceptora ?? new Models.JuntaReceptora(),
                listDetalle = listDetalle ?? new List<DetalleJuntaReceptora>()
            });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarJuntaReceptora([FromForm] string modelo, IFormFile file)
        {
            // Deserializar el modelo
            var juntaVM = JsonConvert.DeserializeObject<JuntaDeRecepcionVM>(modelo);

            JuntaReceptora modelJunta = new()
            {
                IdJuntaReceptora = juntaVM.JuntaReceptora.IdJuntaReceptora,
                NumeroNombramiento = juntaVM.JuntaReceptora.NumeroNombramiento,
                ArchivoNombramiento = juntaVM.JuntaReceptora.ArchivoNombramiento,
                Estado = 0,
            };

            ServiceResponse<JuntaReceptora> result = new ServiceResponse<JuntaReceptora>();
            ServiceResponse<DetalleJuntaReceptora> resultDet = new ServiceResponse<DetalleJuntaReceptora>();
            long idJuntaReceptora = 0;

            string? nombreArchivo = null;

            // Almacenamiento de JuntaReceptora
            if (modelJunta.IdJuntaReceptora == 0)
            {
                result = await _juntaDeRecepcionService.SaveJuntaReceptora(modelJunta);
                modelJunta.IdJuntaReceptora = result.Data.IdJuntaReceptora;
                idJuntaReceptora = result.Data.IdJuntaReceptora;
            }
            else
            {
                result = await _juntaDeRecepcionService.UpdateJuntaReceptora(modelJunta);
                idJuntaReceptora = result.Data.IdJuntaReceptora;

            }

            // Almacenamiento de los detalles
            foreach (var detalle in juntaVM.listaDetalleJuntaReceptora)
            {
                DetalleJuntaReceptora nuevoDetalle = new DetalleJuntaReceptora
                {
                    IdDetalleJuntaReceptora = detalle.IdDetalleJuntaReceptora,
                    IdJuntaReceptora = (int)idJuntaReceptora,
                    NombreCompleto = detalle.NombreCompleto.ToUpper().Trim(),
                    DPI = detalle.DPI,
                    IdTipoNombramiento = detalle.IdTipoNombramiento,
                    Estado = 0
                };

                if (nuevoDetalle.IdDetalleJuntaReceptora == 0)
                    resultDet = await _juntaDeRecepcionService.SaveDetalleJuntaReceptora(nuevoDetalle);
                else
                    resultDet = await _juntaDeRecepcionService.UpdateDetalleJuntaReceptora(nuevoDetalle);
            }


            // Almacenamiento del archivo
            if (file != null && file.Length > 0)
            {
                nombreArchivo = await AlmacenamientoDocumentos(file, (int)idJuntaReceptora, "nombramientos");
                modelJunta.ArchivoNombramiento = nombreArchivo;
                result = await _juntaDeRecepcionService.UpdateArchivoJuntaReceptora(modelJunta);
            }

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "JuntaDeRecepcion");
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
            var result = await _juntaDeRecepcionService.DeleteDetalleJuntaReceptora(id);

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

        [Authorize]
        public async Task<IActionResult> Eliminar(Int64 id)
        {
            var result = await _juntaDeRecepcionService.DeleteJuntaReceptora(id);

            if (result.Success && result.Data != null)
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index", "JuntaDeRecepcion");
            }
            else
            {
                TempData["Mensaje"] = result.Message;
                return RedirectToAction("Index");
            }
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
