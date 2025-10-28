using System.Collections.Generic;
using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class JuntaDeRecepcionService : IJuntaDeRecepcionService
    {
        private readonly IJuntaDeRecepcionRepository _juntaDeRecepcionRepository;

        public JuntaDeRecepcionService(IJuntaDeRecepcionRepository juntaDeRecepcionRepository)
        {
            _juntaDeRecepcionRepository = juntaDeRecepcionRepository;
        }

        #region Consultas

        public async Task<List<TipoNombramiento>> GetAllTipoNombramiento()
        {
            List<TipoNombramiento> listado = await _juntaDeRecepcionRepository.GetAllTipoNombramiento();

            return listado;
        }

        #endregion

        #region CRUD - JUNTA_RECEPTORA

        public async Task<List<JuntaReceptora>>? GetAllJuntaReceptora()
        {
            List<JuntaReceptora>? modelo = await _juntaDeRecepcionRepository.GetAllJuntaReceptora();

            return modelo;
        }

        public async Task<JuntaReceptora> GetJuntaReceptora(Int64 id)
        {
            JuntaReceptora modelo = await _juntaDeRecepcionRepository.GetJuntaReceptora(id);

            return modelo;
        }

        public async Task<ServiceResponse<JuntaReceptora>> SaveJuntaReceptora(JuntaReceptora modelo)
        {
            var response = new ServiceResponse<JuntaReceptora>();

            try
            {
                // Crear objeto
                JuntaReceptora objDB = new JuntaReceptora()
                {
                    IdJuntaReceptora = await _juntaDeRecepcionRepository.CuentaJuntaReceptora(),
                    NumeroNombramiento = modelo.NumeroNombramiento,
                    ArchivoNombramiento = modelo.ArchivoNombramiento,
                    Estado = 0
                };

                // Guardar la junta receptora en la base de datos
                var desc = await _juntaDeRecepcionRepository.SaveJuntaReceptora(objDB);


                response.Data = objDB;
                response.Message = "Junta Receptora creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la junta receptora: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<JuntaReceptora>> UpdateJuntaReceptora(JuntaReceptora modelo)
        {
            var response = new ServiceResponse<JuntaReceptora>();
            JuntaReceptora objDB = await _juntaDeRecepcionRepository.GetJuntaReceptora(modelo.IdJuntaReceptora);

            try
            {
                objDB.IdJuntaReceptora = modelo.IdJuntaReceptora;
                objDB.NumeroNombramiento = modelo.NumeroNombramiento;
                objDB.ArchivoNombramiento = modelo.ArchivoNombramiento ?? objDB.ArchivoNombramiento;

                var desc = await _juntaDeRecepcionRepository.UpdateJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Junta Receptora actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la junta receptora: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<JuntaReceptora>> UpdateArchivoJuntaReceptora(JuntaReceptora modelo)
        {
            var response = new ServiceResponse<JuntaReceptora>();
            JuntaReceptora objDB = await _juntaDeRecepcionRepository.GetJuntaReceptora(modelo.IdJuntaReceptora);

            try
            {
                objDB.ArchivoNombramiento = modelo.ArchivoNombramiento;

                var desc = await _juntaDeRecepcionRepository.UpdateJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Junta Receptora actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la junta receptora: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<JuntaReceptora>> DeleteJuntaReceptora(Int64 id)
        {
            var response = new ServiceResponse<JuntaReceptora>();
            JuntaReceptora objDB = await _juntaDeRecepcionRepository.GetJuntaReceptora(id);

            try
            {
                objDB.Estado = 1;

                _juntaDeRecepcionRepository.UpdateJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Junta Receptora eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la junta receptora: {ex.Message}";
            }

            return response;
        }


        #endregion

        #region CRUD - DETALLE_JUNTA_RECEPTORA

        /// <summary>
        /// Retorna listado de los detalles de junta receptora de un registro de JUNTA_RECEPTORA
        /// </summary>
        /// <param name="id">JUNTA_RECEPTORA_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleJuntaReceptora>> GetAllDetalleJuntaReceptora(Int64 id)
        {
            List<DetalleJuntaReceptora> listado = await _juntaDeRecepcionRepository.GetAllDetalleJuntaReceptora(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de DETALLE_JUNTA_RECEPTORA
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(Int64 id)
        {
            DetalleJuntaReceptora modelo = await _juntaDeRecepcionRepository.GetDetalleJuntaReceptora(id);

            return modelo;
        }

        public async Task<ServiceResponse<DetalleJuntaReceptora>> SaveDetalleJuntaReceptora(DetalleJuntaReceptora modelo)
        {
            var response = new ServiceResponse<DetalleJuntaReceptora>();

            try
            {
                // Crear objeto
                DetalleJuntaReceptora objDB = new DetalleJuntaReceptora()
                {
                    IdDetalleJuntaReceptora = await _juntaDeRecepcionRepository.CuentaDetalleJuntaReceptora(),
                    IdJuntaReceptora = modelo.IdJuntaReceptora,
                    NombreCompleto = modelo.NombreCompleto,
                    DPI = modelo.DPI,
                    IdTipoNombramiento = modelo.IdTipoNombramiento,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                await _juntaDeRecepcionRepository.SaveDetalleJuntaReceptora(objDB);


                response.Data = objDB;
                response.Message = "Racion creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleJuntaReceptora>> UpdateDetalleJuntaReceptora(DetalleJuntaReceptora modelo)
        {
            var response = new ServiceResponse<DetalleJuntaReceptora>();
            DetalleJuntaReceptora objDB = await _juntaDeRecepcionRepository.GetDetalleJuntaReceptora(modelo.IdDetalleJuntaReceptora);

            try
            {
                objDB.IdJuntaReceptora = modelo.IdJuntaReceptora;
                objDB.NombreCompleto = modelo.NombreCompleto;
                objDB.DPI = modelo.DPI;
                objDB.IdTipoNombramiento = modelo.IdTipoNombramiento;

                _juntaDeRecepcionRepository.UpdateDetalleJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Detalle de junta receptora actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle de junta receptora: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleJuntaReceptora>> UpdateDetalleJuntaReceptoraEstado2(DetalleJuntaReceptora modelo)
        {
            var response = new ServiceResponse<DetalleJuntaReceptora>();
            DetalleJuntaReceptora objDB = await _juntaDeRecepcionRepository.GetDetalleJuntaReceptora(modelo.IdDetalleJuntaReceptora);

            try
            {
                objDB.IdDetalleJuntaReceptora = modelo.IdDetalleJuntaReceptora;
                objDB.EstaPresente = modelo.EstaPresente;

                _juntaDeRecepcionRepository.UpdateDetalleJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Detalle de junta receptora actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle de junta receptora: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleJuntaReceptora>> DeleteDetalleJuntaReceptora(Int64 id)
        {
            var response = new ServiceResponse<DetalleJuntaReceptora>();
            DetalleJuntaReceptora objDB = await _juntaDeRecepcionRepository.GetDetalleJuntaReceptora(id);

            try
            {
                objDB.Estado = 1;

                _juntaDeRecepcionRepository.UpdateDetalleJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Detalle de junta receptora eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle de junta receptora: {ex.Message}";
            }

            return response;
        }


        #endregion
    }
}
