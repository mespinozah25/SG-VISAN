using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class ReingresoAlimentosRepository : IReingresoAlimentosRepository
    {
        private readonly AppDBContext _dbContext;

        public ReingresoAlimentosRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Centas (Extracción de llaves Primarias)

        public async Task<long> CuentaReingresoAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.ReingresoAlimentos
                .Select(t => t.IdReingresoAlimentos)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaBitacora()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Bitacora
                .Select(t => t.IdBitacora)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaDetalleReingresoAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleReingresoAlimentos
                .Select(t => t.IdDetalleReingresoAlimentos)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaMovimientosProductos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.MovimientosProductos
                .Select(t => t.IdMovimientosProductos)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaKardex()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.Kardex
                .Select(t => t.IdKardex)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        #endregion

        #region Consultas
        public async Task<List<EstadoReingresos>> GetAllEstadoReingresos()
        {
            List<EstadoReingresos> listado = await _dbContext.EstadoReingresos.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public Task<List<object>> GetEgresosLike(string term, int limit)
        {

            var termBusq = term.ToUpper();

            var query = from a in _dbContext.EgresoAlimentos
                        orderby a.NoDespachoAlmacen ascending
                        where a.NoDespachoAlmacen.ToUpper().Contains(termBusq)
                        select new
                        {
                            id = a.IdEgresoAlimentos,
                            value = a.NoDespachoAlmacen
                        };

            return Task.FromResult(query.Take(limit).ToList<object>());
        }

        public async Task<EgresoAlimentos> GetEgresoAlimentos(long id)
        {
            EgresoAlimentos data = await _dbContext.EgresoAlimentos.FirstAsync(t => t.IdEgresoAlimentos == id);

            return data;
        }

        #endregion

        #region REINGRESO_ALIMENTOS

        public async Task<List<ReingresoAlimentos>> GetAllReingresoAlimentos()
        {
            List<ReingresoAlimentos> listado = new List<ReingresoAlimentos>();
            try
            {
                listado = await _dbContext.ReingresoAlimentos.Where(c => c.Estado == 0).OrderByDescending(o => o.IdReingresoAlimentos).ToListAsync();

            }
            catch (Exception er)
            {
                var error = er;
            }

            return listado;
        }

        public async Task<ReingresoAlimentos> GetReingresoAlimentos(long id)
        {
            ReingresoAlimentos data = await _dbContext.ReingresoAlimentos.FirstAsync(t => t.IdReingresoAlimentos == id);

            return data;
        }

        public async Task<ReingresoAlimentos> SaveReingresoAlimentos(ReingresoAlimentos modelo)
        {
            try
            {
                _dbContext.ReingresoAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<ReingresoAlimentos> UpdateReingresoAlimentos(ReingresoAlimentos modelo)
        {
            _dbContext.ReingresoAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

        #region DETALLE_EGRESO_ALIMENTOS

        /// <summary>
        /// Extrae todos los detalles ingresados de un REINGRESO_ALIMENTOS_ID
        /// </summary>
        /// <param name="id">REINGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleReingresoAlimentos>> GetAllDetalleReingresoAlimentos(long id)
        {
            List<DetalleReingresoAlimentos> listado = await _dbContext.DetalleReingresoAlimentos.Where(c => c.IdReingresoAlimentos == id && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<DetalleReingresoAlimentos> GetDetalleReingresoAlimentos(long id)
        {
            DetalleReingresoAlimentos data = await _dbContext.DetalleReingresoAlimentos.FirstAsync(t => t.IdDetalleReingresoAlimentos == id);

            return data;
        }

        public async Task<DetalleReingresoAlimentos> SaveDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo)
        {
            try
            {
                _dbContext.DetalleReingresoAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<DetalleReingresoAlimentos> UpdateDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo)
        {
            _dbContext.DetalleReingresoAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }


        #endregion

        #region MOVIMIENTOS_PRODUCTOS

        /// <summary>
        /// Extrae el registro específico del Movimiento Productos
        /// </summary>
        /// <param name="idBodega"></param>
        /// <param name="idProductoAlimenticio"></param>
        /// <param name="idPresentacion"></param>
        /// <param name="idUnidadMedida"></param>
        /// <returns></returns>
        public async Task<MovimientosProductos>? GetMovimientosProductos(int idBodega, int idProductoAlimenticio, int idPresentacion, int idUnidadMedida)
        {
            MovimientosProductos? data = await _dbContext.MovimientosProductos.Where(t => t.IdBodega == idBodega &&
                                                                                          t.IdProductoAlimenticio == idProductoAlimenticio &&
                                                                                          t.IdPresentacion == idPresentacion &&
                                                                                          t.IdUnidadMedida == idUnidadMedida).FirstOrDefaultAsync();

            return data;
        }

        public async Task<MovimientosProductos> SaveMovimientosProductos(MovimientosProductos modelo)
        {
            try
            {
                _dbContext.MovimientosProductos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<MovimientosProductos> UpdateMovimientosProductos(MovimientosProductos modelo)
        {
            _dbContext.MovimientosProductos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

        #region KARDEX

        public async Task<Kardex> SaveKardex(Kardex modelo)
        {
            try
            {
                _dbContext.Kardex.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        #endregion

        #region BITACORA

        public async Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso)
        {
            Bitacora listado = await _dbContext.Bitacora.Where(c => c.IdRegistro == idReingreso && c.Accion == idEstado && c.NombreTabla == "REINGRESO_ALIMENTOS" && c.Estado == 0).FirstOrDefaultAsync();

            return listado;
        }

        public async Task<List<Bitacora>> GetAllBitacora(long id)
        {
            List<Bitacora> listado = await _dbContext.Bitacora.Where(c => c.IdRegistro == id && c.NombreTabla == "REINGRESO_ALIMENTOS" && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<Bitacora> SaveBitacora(Bitacora modelo)
        {
            try
            {
                _dbContext.Bitacora.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }

            return modelo;
        }

        #endregion
    }
}
