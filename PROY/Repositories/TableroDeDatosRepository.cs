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

        public List<BodegaProducto> ChartBodegasExistenciaProductos()
        {
            // Usar Database Facade para evitar problemas de EF
            var connection = _dbContext.Database.GetDbConnection();
            var command = connection.CreateCommand();

            command.CommandText = @"
                        SELECT 
                            b.NOMBRE_BODEGA AS NombreBodega,
                            p.NOMBRE_PA AS NombrePA,
                            m.EXISTENCIAS AS Existencias
                        FROM SGI.MOVIMIENTOS_PRODUCTOS m
                        INNER JOIN SGI.BODEGA b ON m.BODEGA_ID = b.ID
                        INNER JOIN SGI.PRODUCTO_ALIMENTICIO p ON m.PRODUCTO_ALIMENTICIO_ID = p.ID
                        WHERE m.ESTADO = 0";

            var result = new List<BodegaProducto>();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new BodegaProducto
                    {
                        NombreBodega = reader["NombreBodega"]?.ToString() ?? "Desconocido",
                        NombrePA = reader["NombrePA"]?.ToString() ?? "Desconocido",
                        Existencias = Convert.ToDecimal(reader["Existencias"])
                    });
                }
            }

            return result;
        }

        public List<Racion> ChartRaciones()
        {
            // Usar Database Facade para evitar problemas de EF
            var connection = _dbContext.Database.GetDbConnection();
            var command = connection.CreateCommand();

            command.CommandText = @"
            SELECT 
                COALESCE(p.NOMBRE_PROGRAMA, '') AS NombrePrograma, 
                COUNT(*) AS TotalRaciones
            FROM SGI.RACION_MAESTRO rm 
            INNER JOIN SGI.PROGRAMAS p ON rm.PROGRAMA_ID = p.ID
            GROUP BY p.NOMBRE_PROGRAMA";

            var result = new List<Racion>();

            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(new Racion
                    {
                        NombrePrograma = reader["NombrePrograma"]?.ToString() ?? "Desconocido",
                        TotalRaciones = Convert.ToDecimal(reader["TotalRaciones"])
                    });
                }
            }

            return result;

        }
    }
}