using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PROY.Models;
using PROY.Services;
using PROY.Models.TableroDeDatos;

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
            var racionesDisponibles = _homeService.ChartRacionesDisponibles();
            var racionesAvance = _homeService.ChartRacionesAvance();

            var totalConvenio = racionesAvance.Sum(r => r.TotalConvenio);
            var totalRacionesEntregadas = racionesAvance.Sum(r => r.RacionesEntregadas);

            var totalporcentaje = totalConvenio > 0 ? ((totalRacionesEntregadas / totalConvenio) * 100) : 0;

            var tablero = new
            {
                raciones_disponibles = racionesDisponibles,
                total_raciones = racionesDisponibles.Sum(r => r.Raciones),
                raciones_avance = racionesAvance,
                totalConvenio,
                totalRacionesRecibidas = racionesAvance.Sum(r => r.RacionesRecibidas),
                totalRacionesPendientesRecibir = racionesAvance.Sum(r => r.RacionesPendientesRecibir),
                totalRacionesEntregadas,
                totalRacionesPendientesEntregar = racionesAvance.Sum(r => r.RacionesPendientesEntregar),
                totalporcentaje
            };

            return View(tablero);
        }

        #region CHARTS

        [HttpGet]
        public async Task<IActionResult> ObtenerDatosGraficaBodegasProductos()
        {
            var datos = _homeService.ChartBodegasExistenciaProductos();

            // Formato correcto para Google Charts usando dynamic
            var encabezados = new List<object> { "Bodega" };
            var bodegasMap = new Dictionary<string, Dictionary<string, decimal>>();

            // Obtener todos los productos únicos
            var productosUnicos = new List<string>();
            foreach (dynamic dato in datos)
            {
                string producto = dato.NombrePA;
                if (!productosUnicos.Contains(producto))
                {
                    productosUnicos.Add(producto);
                }
            }
            encabezados.AddRange(productosUnicos);

            // Organizar datos por bodega
            foreach (dynamic dato in datos)
            {
                string bodega = dato.NombreBodega;
                string producto = dato.NombrePA;
                decimal existencia = dato.Existencias;

                if (!bodegasMap.ContainsKey(bodega))
                {
                    bodegasMap[bodega] = new Dictionary<string, decimal>();
                }

                bodegasMap[bodega][producto] = existencia;
            }

            // Crear filas de datos
            var filas = new List<object[]>();
            foreach (var bodega in bodegasMap)
            {
                var fila = new List<object> { bodega.Key };

                foreach (var producto in productosUnicos)
                {
                    if (bodega.Value.ContainsKey(producto))
                    {
                        fila.Add(bodega.Value[producto]);
                    }
                    else
                    {
                        fila.Add(0);
                    }
                }

                filas.Add(fila.ToArray());
            }

            var resultado = new
            {
                encabezados = encabezados,
                filas = filas
            };

            return Json(resultado);
        }

        [HttpGet]
        public IActionResult ObtenerDatosGraficaRaciones()
        {
            var datos = _homeService.ChartRaciones();
            // Si no hay datos, devolver estructura vacía
            if (datos == null || !datos.Any())
            {
                return Json(new List<object[]>
                    {
                        new object[] { "Programa", "Raciones" },
                        new object[] { "Sin datos", 0 }
                    });
            }
            // Formato para gráfico de pie
            var datosFormateados = new List<object[]> { new object[] { "Programa", "Raciones" } };
            // Procesar datos - USANDO TIPOS FUERTES en lugar de dynamic
            var programasDict = new Dictionary<string, decimal>();
            foreach (var dato in datos)
            {
                string programa = dato.NombrePrograma;
                decimal raciones = dato.TotalRaciones;

                if (programasDict.ContainsKey(programa))
                {
                    programasDict[programa] += raciones;
                }
                else
                {
                    programasDict[programa] = raciones;
                }
            }
            foreach (var programa in programasDict)
            {
                datosFormateados.Add(new object[] { programa.Key, programa.Value });
            }
            return Json(datosFormateados);
        }

        #endregion
    }
}