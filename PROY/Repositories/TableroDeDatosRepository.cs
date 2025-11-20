using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models.TableroDeDatos;

namespace PROY.Repositories
{
    public class TableroDeDatosRepository : ITableroDeDatosRepository
    {
        private readonly AppDBContext _dbContext;

        public TableroDeDatosRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<TableroConvenio> ChartRacionesDisponibles()
        {
            return _dbContext.Convenio
                .Where(c => c.Estado == 0)
                .GroupBy(c => c.Descripcion)
                .Select(g => new TableroConvenio
                {
                    Convenio = g.Key,
                    Descripcion = string.Join(", ", g.Select(x => string.Concat("CONV ", x.NombreConvenio))),
                    Raciones = Convert.ToDecimal(g.Sum(c => c.TotalRaciones)),
                    Tipo = g.Key == "RESERVAS ESTRATÉGICAS" ? 1 : 2
                })
                .ToList();
        }

        public List<TableroConvenioAvance> ChartRacionesAvance()
        {
            var result = (from c in _dbContext.Convenio
                          join e in _dbContext.EgresoAlimentos on (int)c.IdConvenio equals e.IdConvenio into egresosGroup
                          where c.Estado == 0
                          let totalRacionesEgreso = egresosGroup.Sum(e => e.TotalRaciones ?? 0)
                          select new TableroConvenioAvance
                          {
                              Convenio = "Convenio " + c.NombreConvenio,
                              Descripcion = c.Descripcion,
                              TotalConvenio = c.TotalRaciones ?? 0,
                              RacionesRecibidas = c.TotalRaciones ?? 0,
                              RacionesPendientesRecibir = 0,
                              RacionesEntregadas = totalRacionesEgreso,
                              RacionesPendientesEntregar = (c.TotalRaciones ?? 0) - totalRacionesEgreso,
                              PorcentajeEjecucion = (c.TotalRaciones ?? 0) > 0 ? (totalRacionesEgreso * 100) / (c.TotalRaciones ?? 0) : 0
                          }).ToList();

            return result;
        }

        public List<TableroBodegaProducto> ChartBodegasExistenciaProductos()
        {
            var result = (from m in _dbContext.MovimientosProductos
                          join b in _dbContext.Bodegas on m.IdBodega equals (int)b.IdBodega
                          join p in _dbContext.ProductoAlimenticio on m.IdProductoAlimenticio equals (int)p.IdProductoAlimenticio
                          where m.Estado == 0
                          select new TableroBodegaProducto
                          {
                              NombreBodega = b.NombreBodega,
                              NombrePA = p.NombrePA,
                              Existencias = Convert.ToDecimal(m.Existencias)
                          }).ToList();

            return result;
        }

        public List<TableroRacion> ChartRaciones()
        {
            // Usar Database Facade para evitar problemas de EF
            var result = (from rm in _dbContext.EgresoAlimentos
                          join p in _dbContext.Programa on rm.IdPrograma equals (int)p.IdPrograma
                          group rm by new { p.NombrePrograma } into g
                          select new TableroRacion
                          {
                              NombrePrograma = g.Key.NombrePrograma,
                              TotalRaciones = Convert.ToDecimal(g.Sum(rm => rm.TotalRaciones))
                          }).ToList();

            return result;

        }
    }
}