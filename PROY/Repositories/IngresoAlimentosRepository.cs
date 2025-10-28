using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class IngresoAlimentosRepository : IIngresoAlimentosRepository
    {
        private readonly AppDBContext _dbContext;

        public IngresoAlimentosRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Centas (Extracción de llaves Primarias)

        public async Task<long> CuentaIngresoAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.IngresoAlimentos
                .Select(t => t.IdIngresoAlimentos)
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

        public async Task<long> CuentaProductosIngreso()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.ProductosIngreso
                .Select(t => t.IdProductosIngreso)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaDetalleWaybill()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleWaybill
                .Select(t => t.IdDetalleWaybill)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaJuntaReceptora()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.JuntaReceptora
                .Select(t => t.IdJuntaReceptora)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaDetalleJuntaReceptora()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleJuntaReceptora
                .Select(t => t.IdDetalleJuntaReceptora)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaDetalleBoletaAnalisis()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleBoletaAnalisis
                .Select(t => t.IdDetalleBoletaAnalisis)
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
        public async Task<List<EstadoIngresos>> GetAllEstadoIngresos()
        {
            List<EstadoIngresos> listado = await _dbContext.EstadoIngresos.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<TipoNombramiento>> GetAllTipoNombramiento()
        {
            List<TipoNombramiento> listado = await _dbContext.TipoNombramiento.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        #endregion

        #region INGRESO_ALIMENTOS

        public async Task<List<IngresoAlimentos>> GetAllIngresoAlimentos()
        {
            List<IngresoAlimentos> listado = new List<IngresoAlimentos>();
            try
            {
                 listado = await _dbContext.IngresoAlimentos.Where(c => c.Estado == 0).OrderByDescending(o => o.IdIngresoAlimentos).ToListAsync();

            }
            catch(Exception er)
            {
                var error = er;
            }

            return listado;
        }

        public async Task<IngresoAlimentos> GetIngresoAlimentos(long id)
        {
            IngresoAlimentos data = await _dbContext.IngresoAlimentos.FirstAsync(t => t.IdIngresoAlimentos == id);

            return data;
        }

        public async Task<IngresoAlimentos> SaveIngresoAlimentos(IngresoAlimentos modelo)
        {
            try
            {
                _dbContext.IngresoAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<IngresoAlimentos> UpdateIngresoAlimentos(IngresoAlimentos modelo)
        {
            _dbContext.IngresoAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

        #region PRODUCTOS_INGRESO

        /// <summary>
        /// Extrae todos los prodcutos ingresados de un INGRESO_ALIMENTOS_ID
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<ProductosIngreso>> GetAllProductosIngreso(long id)
        {
            List<ProductosIngreso> listado = await _dbContext.ProductosIngreso.Where(c => c.IdIngresoAlimentos == id && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<ProductosIngreso> GetProductosIngreso(long id)
        {
            ProductosIngreso data = await _dbContext.ProductosIngreso.FirstAsync(t => t.IdProductosIngreso == id);

            return data;
        }

        public async Task<ProductosIngreso> SaveProductosIngreso(ProductosIngreso modelo)
        {
            try
            {
                _dbContext.ProductosIngreso.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<ProductosIngreso> UpdateProductosIngreso(ProductosIngreso modelo)
        {
            _dbContext.ProductosIngreso.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }


        #endregion

        #region DETALLE_WAYBILL

        /// <summary>
        /// Devuelve Detalle Waybill de un Ingreso de Alimentos
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleWaybill>> GetDetalleWaybillDeIngreso(long id)
        {
            List<DetalleWaybill>? data = new();
            try
            {
                data = await _dbContext.DetalleWaybill.Where(t => t.IdIngresoAlimentos == id && t.Estado == 0).ToListAsync();
            }
            catch (Exception er)
            {
                var error = er;
            }

            return data;
        }

        public async Task<DetalleWaybill> GetDetalleWaybill(long id)
        {
            DetalleWaybill? data = new();
            try
            {
                data = await _dbContext.DetalleWaybill.FirstAsync(t => t.IdDetalleWaybill == id);
            }
            catch (Exception er)
            {
                var error = er;
            }

            return data;
        }

        public async Task<DetalleWaybill> SaveDetalleWaybill(DetalleWaybill modelo)
        {
            try
            {
                _dbContext.DetalleWaybill.Add(modelo);
                _dbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                var error = exc;
            }

            return modelo;
        }

        public async Task<DetalleWaybill> UpdateDetalleWaybill(DetalleWaybill modelo)
        {
            try
            {
                _dbContext.DetalleWaybill.Update(modelo);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var err = ex;
            }

            return modelo;
        }


        #endregion

        #region JUNTA_RECEPTORA
        /*
        /// <summary>
        /// Devuelve Junta Receptora de un Ingreso de Alimentos
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<JuntaReceptora> GetJuntaReceptoraDeIngreso(long id)
        {
            JuntaReceptora? data = new();
            try
            {
                data = await _dbContext.JuntaReceptora.FirstAsync(t => t.IdIngresoAlimentos == id && t.Estado == 0);
            }
            catch(Exception er)
            {
                var error = er;
            }

            return data;
        }

        public async Task<JuntaReceptora> GetJuntaReceptora(long id)
        {
            JuntaReceptora? data = new();
            try
            {
                data = await _dbContext.JuntaReceptora.FirstAsync(t => t.IdJuntaReceptora == id);
            }
            catch (Exception er)
            {
                var error = er;
            }

            return data;
        }

        public async Task<JuntaReceptora> SaveJuntaReceptora(JuntaReceptora modelo)
        {
            try
            {
                _dbContext.JuntaReceptora.Add(modelo);
                _dbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                var error = exc;
            }

            return modelo;
        }

        public async Task<JuntaReceptora> UpdateJuntaReceptora(JuntaReceptora modelo)
        {
            try
            {
                _dbContext.JuntaReceptora.Update(modelo);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                var err = ex;
            }
            
            return modelo;
        }
        */

        #endregion

        #region DETALLE_JUNTA_RECEPTORA
        /*
        /// <summary>
        /// Lista de Detalles de Junta Receptora provenientes del Maestro: JUNTA_RECEPTORA
        /// </summary>
        /// <param name="id">JUNTA_RECEPTORA_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleJuntaReceptora>> GetAllDetalleJuntaReceptora(long id)
        {
            List<DetalleJuntaReceptora> listado = await _dbContext.DetalleJuntaReceptora.Where(c => c.IdJuntaReceptora == id && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(long id)
        {
            DetalleJuntaReceptora data = await _dbContext.DetalleJuntaReceptora.FirstAsync(t => t.IdDetalleJuntaReceptora == id);

            return data;
        }

        public async Task<DetalleJuntaReceptora> SaveDetalleJuntaReceptora(DetalleJuntaReceptora modelo)
        {
            try
            {
                _dbContext.DetalleJuntaReceptora.Add(modelo);
                _dbContext.SaveChanges();
            }
            catch (Exception exc)
            {
                var error = exc;
            }

            return modelo;
        }

        public async Task<DetalleJuntaReceptora> UpdateDetalleJuntaReceptora(DetalleJuntaReceptora modelo)
        {
            _dbContext.DetalleJuntaReceptora.Update(modelo);
            _dbContext.SaveChanges();
            return modelo;
        }
        */

        #endregion

        #region DETALLE_BOLETA_ANALISIS

        /// <summary>
        /// Lista de Detalles de Junta Receptora provenientes del Maestro: INGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleBoletaAnalisis>>? GetAllDetalleBoletaAnalisis(long id)
        {
            List<DetalleBoletaAnalisis>? listado = new();
            try
            {
                listado = await _dbContext.DetalleBoletaAnalisis.Where(c => c.IdIngresoAlimentos == id && c.Estado == 0).ToListAsync();
            }
            catch(Exception ex)
            {
                var error = ex;
            }
            

            return listado;
        }

        public async Task<DetalleBoletaAnalisis> GetDetalleBoletaAnalisis(long id)
        {
            DetalleBoletaAnalisis data = await _dbContext.DetalleBoletaAnalisis.FirstAsync(t => t.IdDetalleBoletaAnalisis == id);

            return data;
        }

        public async Task<DetalleBoletaAnalisis> SaveDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo)
        {
            try
            {
                _dbContext.DetalleBoletaAnalisis.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }

            return modelo;
        }

        public async Task<DetalleBoletaAnalisis> UpdateDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo)
        {
            _dbContext.DetalleBoletaAnalisis.Update(modelo);
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

        public async Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado)
        {
            List<Bitacora> listado = await _dbContext.Bitacora.Where(c => c.Accion == idEstado && c.NombreTabla.Contains("INGRESO_ALIMENTOS") && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<Bitacora>> GetAllBitacora(long id)
        {
            List<Bitacora> listado = await _dbContext.Bitacora.Where(c => c.IdRegistro == id && c.NombreTabla == "INGRESO_ALIMENTOS" && c.Estado == 0).ToListAsync();

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
