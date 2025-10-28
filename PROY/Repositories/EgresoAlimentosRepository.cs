using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class EgresoAlimentosRepository : IEgresoAlimentosRepository
    {
        private readonly AppDBContext _dbContext;

        public EgresoAlimentosRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Centas (Extracción de llaves Primarias)

        public async Task<long> CuentaEgresoAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.EgresoAlimentos
                .Select(t => t.IdEgresoAlimentos)
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

        public async Task<long> CuentaDetalleEgresoAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleEgresoAlimentos
                .Select(t => t.IdDetalleEgresoAlimentos)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaSolicitudSalidaAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.SolicitudSalidaAlimentos
                .Select(t => t.IdSolicitudSalidaAlimentos)
                .MaxAsync();

                maxID += 1;
            }
            catch
            {
                maxID = 1;
            }

            return maxID;
        }

        public async Task<long> CuentaDetalleSalidaAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.DetalleSalidaAlimentos
                .Select(t => t.IdDetalleSalidaAlimentos)
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

        public async Task<long> CuentaTrasladoAlimentos()
        {
            Int64 maxID = 0;
            try
            {
                maxID = await _dbContext.TrasladoAlimentos
                .Select(t => t.IdTrasladoAlimentos)
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
        public async Task<List<EstadoEgresos>> GetAllEstadoEgresos()
        {
            List<EstadoEgresos> listado = await _dbContext.EstadoEgresos.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<EstadoTraslados>> GetAllEstadoTraslados()
        {
            List<EstadoTraslados> listado = await _dbContext.EstadoTraslados.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<dynamic>> GetAllDetallesRacionXConvenio(int idConvenio)
        {
            var listado = (from a in _dbContext.RacionMaestro
                           join b in _dbContext.RacionDetalle on a.IdRacionMaestro equals (int)b.IdRacionMaestro into bs
                           from b in bs.DefaultIfEmpty()
                           join c in _dbContext.ProductoAlimenticio on b.IdProductoAlimenticio equals (int)c.IdProductoAlimenticio into cs
                           from c in cs.DefaultIfEmpty()
                           join d in _dbContext.UnidadMedida on b.IdUnidadMedida equals (int)d.IdUnidadMedida into ds
                           from d in ds.DefaultIfEmpty()
                           where a.Estado == decimal.Zero &&
                                 b.Estado == decimal.Zero &&
                                 a.IdConvenio == idConvenio
                           orderby b.IdRacionDetalle ascending
                           select new
                           {
                               b.IdRacionDetalle,
                               b.IdRacionMaestro,
                               b.IdProductoAlimenticio,
                               b.IdUnidadMedida,
                               b.CantidadMedida,
                               c.NombrePA,
                               d.NombreUnidadMedida
                           });

            return [.. listado];
        }

        #endregion

        #region EGRESO_ALIMENTOS

        public async Task<List<EgresoAlimentos>> GetAllEgresoAlimentos()
        {
            List<EgresoAlimentos> listado = new List<EgresoAlimentos>();
            try
            {
                listado = await _dbContext.EgresoAlimentos.Where(c => c.Estado == 0).OrderByDescending(i => i.IdEgresoAlimentos).ToListAsync();

            }
            catch (Exception er)
            {
                var error = er;
            }

            return listado;
        }

        public async Task<EgresoAlimentos> GetEgresoAlimentos(long id)
        {
            EgresoAlimentos data = await _dbContext.EgresoAlimentos.FirstAsync(t => t.IdEgresoAlimentos == id);

            return data;
        }

        public async Task<EgresoAlimentos> SaveEgresoAlimentos(EgresoAlimentos modelo)
        {
            try
            {
                _dbContext.EgresoAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<EgresoAlimentos> UpdateEgresoAlimentos(EgresoAlimentos modelo)
        {
            _dbContext.EgresoAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

        #region DETALLE_EGRESO_ALIMENTOS

        /// <summary>
        /// Extrae todos los detalles ingresados de un EGRESO_ALIMENTOS_ID
        /// </summary>
        /// <param name="id">EGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<dynamic>>? GetAllDetalleEgresoAlimentos(long id)
        {
            List<dynamic>? slistado = new();
            try
            {
                slistado = await _dbContext.DetalleEgresoAlimentos.Where(c => c.IdEgresoAlimentos == id && c.Estado == 0)
                        .Select(o => new { 
                            o.IdDetalleEgresoAlimentos,
                            o.IdEgresoAlimentos,
                            o.IdProductoAlimenticio,
                            o.IdPresentacion,
                            o.CantidadUnidad,
                            o.IdUnidadMedida,
                            o.NoUnidades,
                            o.EstibaEgreso,
                            o.EstibaTraslado,
                            o.Estado
                        } ).ToListAsync<dynamic>();

            }
            catch (Exception er)
            {
                var error = er;
            }

            return slistado;
        }

        public async Task<DetalleEgresoAlimentos> GetDetalleEgresoAlimentos(long id)
        {
            DetalleEgresoAlimentos data = await _dbContext.DetalleEgresoAlimentos.FirstAsync(t => t.IdDetalleEgresoAlimentos == id);

            return data;
        }

        public async Task<DetalleEgresoAlimentos> SaveDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo)
        {
            try
            {
                _dbContext.DetalleEgresoAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<DetalleEgresoAlimentos> UpdateDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo)
        {
            _dbContext.DetalleEgresoAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }


        #endregion

        #region SOLICITUD_SALIDA_ALIMENTOS

        /// <summary>
        /// Extrae todos los detalles ingresados de un EGRESO_ALIMENTOS_ID
        /// </summary>
        /// <param name="id">EGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos(long id)
        {
            List<SolicitudSalidaAlimentos> listado = await _dbContext.SolicitudSalidaAlimentos.Where(c => c.IdEgresoAlimentos == id && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<SolicitudSalidaAlimentos> GetSolicitudSalidaAlimentos(long id)
        {
            SolicitudSalidaAlimentos data = await _dbContext.SolicitudSalidaAlimentos.FirstAsync(t => t.IdSolicitudSalidaAlimentos == id);

            return data;
        }

        public async Task<SolicitudSalidaAlimentos> SaveSolicitudSalidaAlimentos(SolicitudSalidaAlimentos modelo)
        {
            try
            {
                _dbContext.SolicitudSalidaAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<SolicitudSalidaAlimentos> UpdateSolicitudSalidaAlimentos(SolicitudSalidaAlimentos modelo)
        {
            _dbContext.SolicitudSalidaAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }

        #endregion

        #region DETALLE_SALIDA_ALIMENTOS

        /// <summary>
        /// Extrae todos los detalles ingresados de un SOLICITUD_SALIDA_ALIMENTOS_ID
        /// </summary>
        /// <param name="id">SOLICITUD_SALIDA_ALIMENTOS_IDC</param>
        /// <returns></returns>
        public async Task<List<DetalleSalidaAlimentos>> GetAllDetalleSalidaAlimentos(long id)
        {
            List<DetalleSalidaAlimentos> listado = await _dbContext.DetalleSalidaAlimentos.Where(c => c.IdSolicitudSalidaAlimentos == id && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<DetalleSalidaAlimentos> GetDetalleSalidaAlimentos(long id)
        {
            DetalleSalidaAlimentos data = await _dbContext.DetalleSalidaAlimentos.FirstAsync(t => t.IdDetalleSalidaAlimentos == id);

            return data;
        }

        public async Task<DetalleSalidaAlimentos> SaveDetalleSalidaAlimentos(DetalleSalidaAlimentos modelo)
        {
            try
            {
                _dbContext.DetalleSalidaAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<DetalleSalidaAlimentos> UpdateDetalleSalidaAlimentos(DetalleSalidaAlimentos modelo)
        {
            _dbContext.DetalleSalidaAlimentos.Update(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }


        #endregion

        #region TRASLADO_ALIMENTOS

        public async Task<List<TrasladoAlimentos>> GetAllTrasladoAlimentos()
        {
            List<TrasladoAlimentos> listado = new List<TrasladoAlimentos>();
            try
            {
                listado = await _dbContext.TrasladoAlimentos.Where(c => c.Estado == 0).OrderByDescending(i => i.IdTrasladoAlimentos).ToListAsync();

            }
            catch (Exception er)
            {
                var error = er;
            }

            return listado;
        }

        public async Task<TrasladoAlimentos> GetTrasladoAlimentos(long id)
        {
            TrasladoAlimentos data = await _dbContext.TrasladoAlimentos.FirstAsync(t => t.IdTrasladoAlimentos == id);

            return data;
        }

        public async Task<TrasladoAlimentos>? GetTrasladoAlimentosDeEgreso(long id)
        {
            TrasladoAlimentos? data = await _dbContext.TrasladoAlimentos.Where(t => t.IdEgresoAlimentos == id).FirstOrDefaultAsync();

            return data;
        }

        public async Task<TrasladoAlimentos> SaveTrasladoAlimentos(TrasladoAlimentos modelo)
        {
            try
            {
                _dbContext.TrasladoAlimentos.Add(modelo);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception exc)
            {
                var error = exc;
            }


            return modelo;
        }

        public async Task<TrasladoAlimentos> UpdateTrasladoAlimentos(TrasladoAlimentos modelo)
        {
            _dbContext.TrasladoAlimentos.Update(modelo);
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
        public async Task<MovimientosProductos>? GetMovimientosProductos(int idBodega, int idProductoAlimenticio, int idUnidadMedida)
        {
            MovimientosProductos? data = await _dbContext.MovimientosProductos.Where(t => t.IdBodega == idBodega &&
                                                                                          t.IdProductoAlimenticio == idProductoAlimenticio &&
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
            List<Bitacora> listado = await _dbContext.Bitacora.Where(c => c.Accion == idEstado && c.NombreTabla == "EGRESO_ALIMENTOS" && c.Estado == 0).ToListAsync();

            return listado;
        }

        public async Task<List<Bitacora>> GetAllBitacora(long id)
        {
            List<Bitacora> listado = await _dbContext.Bitacora.Where(c => c.IdRegistro == id && c.NombreTabla == "EGRESO_ALIMENTOS" && c.Estado == 0).ToListAsync();

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
