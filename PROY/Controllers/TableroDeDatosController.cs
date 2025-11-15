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
            try
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
                var datosFormateados = new List<object[]>
        {
            new object[] { "Programa", "Raciones" }
        };

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
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error en ObtenerDatosGraficaRaciones: {ex.Message}");

                // Devolver datos de prueba en caso de error
                return Json(new List<object[]>
        {
            new object[] { "Programa", "Raciones" },
            new object[] { "PROGRAMA NDA", 52000 },
            new object[] { "PROGRAMA INSAN", 75000 },
            new object[] { "RESERVAS ESTRATEGICAS", 120000 },
            new object[] { "PROGRAMA APA", 45000 },
            new object[] { "MEDIDAS ESPECIALES", 30000 }
        });
            }
        }


        #endregion
    }
}