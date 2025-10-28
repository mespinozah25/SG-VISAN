using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class TableroDeDatosRepository : ITableroDeDatosRepository
    {
        private readonly AppDBContext _dbContext;

        public TableroDeDatosRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<dynamic> ChartBodegasExistenciaProductos()
        {
            var query = (from a in _dbContext.MovimientosProductos
                         join b in _dbContext.Bodegas on a.IdBodega equals (int)b.IdBodega
                         join c in _dbContext.ProductoAlimenticio on a.IdProductoAlimenticio equals (int)c.IdProductoAlimenticio
                         where a.Estado == 0
                         select new
                         {
                             b.NombreBodega,
                             c.NombrePA,
                             a.Existencias,
                         }).ToList<dynamic>();

            return query;
        }

        public List<dynamic> ChartRaciones()
        {
            var query = (from a in _dbContext.RacionMaestro
                         where a.Estado == 0
                         select a).ToList<dynamic>();

            return query;
        }
    }
}
