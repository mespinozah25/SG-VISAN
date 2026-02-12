using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PROY.Models;
using PROY.Services;
using PROY.Models.TableroDeDatos;

namespace PROY.Controllers
{
    public class TableroDeDatosAPAController : Controller
    {
        private readonly ITableroDeDatosAPAService _homeService;
        public TableroDeDatosAPAController(ITableroDeDatosAPAService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var totalSolicitud = await _homeService.ChartTotalSolicitud();
            var totalBeneficiarios = await _homeService.ChartTotalBeneficiarios();

            var tablero = new
            {
                TotalSolicitudes = totalSolicitud.total_solicitudes,
                TotalBeneficiarios = totalBeneficiarios.total_beneficiarios
            };

            return View(tablero);
        }

        // Nuevos endpoints para los gráficos
        [HttpGet]
        public async Task<IActionResult> ChartSolicitudDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var datos = await _homeService.ChartSolicitudDepartamento(fechaInicio, fechaFin);
            return Json(datos);
        }

        [HttpGet]
        public async Task<IActionResult> ChartSolicitudMunicipio(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var datos = await _homeService.ChartSolicitudMunicipio(fechaInicio, fechaFin);
            return Json(datos);
        }

        [HttpGet]
        public async Task<IActionResult> ChartSolicitudProyectoDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var datos = await _homeService.ChartSolicitudProyectoDepartamento(fechaInicio, fechaFin);
            return Json(datos);
        }

        [HttpGet]
        public async Task<IActionResult> ChartBeneficiarioDepartamento(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var datos = await _homeService.ChartBeneficiarioDepartamento(fechaInicio, fechaFin);
            return Json(datos);
        }

        [HttpGet]
        public async Task<IActionResult> ChartBeneficiarioProyecto(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var datos = await _homeService.ChartBeneficiarioProyecto(fechaInicio, fechaFin);
            return Json(datos);
        }


    }
}