using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PROY.Data;
using PROY.Models;

namespace PROY.Repositories
{
    public class JuntaDeRecepcionRepository : IJuntaDeRecepcionRepository
    {
        private readonly AppDBContext _dbContext;

        public JuntaDeRecepcionRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Centas (Extracción de llaves Primarias)

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

        #endregion

        #region Consultas

        public async Task<List<TipoNombramiento>> GetAllTipoNombramiento()
        {
            List<TipoNombramiento> listado = await _dbContext.TipoNombramiento.Where(c => c.Estado == 0).ToListAsync();

            return listado;
        }

        #endregion

        #region JUNTA_RECEPTORA


        public async Task<List<JuntaReceptora>>? GetAllJuntaReceptora()
        {
            List<JuntaReceptora>? data = new();
            try
            {
                data = await _dbContext.JuntaReceptora.Where(t =>  t.Estado == 0).ToListAsync();
            }
            catch (Exception er)
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


        #endregion

        #region DETALLE_JUNTA_RECEPTORA

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


        #endregion
    }
}
