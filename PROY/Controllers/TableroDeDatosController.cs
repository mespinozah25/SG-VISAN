using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PROY.Models;
using PROY.Services;

namespace PROY.Controllers
{
    public class TableroDeDatosController : Controller
    {
        private readonly ITableroDeDatosService _homeService;
        public TableroDeDatosController(ITableroDeDatosService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region CHARTS

        [HttpGet]
        public async Task<IActionResult> ObtenerDatosGraficaBodegasProductos()
        {
            var datos = _homeService.ChartBodegasExistenciaProductos();

            var resultado = datos
                .GroupBy(d => d.NombreBodega)
                .Select(g => new {
                    NombreBodega = g.Key,
                    Productos = g.Select(p => new { p.NombrePA, p.Existencias }).ToList()
                }).ToList();

            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDatosGraficaRaciones()
        {
            var datos = _homeService.ChartBodegasExistenciaProductos();

            var resultado = datos
                .GroupBy(d => d.NombreBodega)
                .Select(g => new {
                    NombreBodega = g.Key,
                    Productos = g.Select(p => new { p.NombrePA, p.Existencias }).ToList()
                }).ToList();

            return Json(resultado);
        }

        #endregion


    }
}

