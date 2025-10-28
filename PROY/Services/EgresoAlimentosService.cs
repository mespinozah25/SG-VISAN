using PROY.Enum;
using PROY.Models;
using PROY.Repositories;
using PROY.ViewModels;

namespace PROY.Services
{
    public class EgresoAlimentosService : IEgresoAlimentosService
    {
        private readonly IEgresoAlimentosRepository _egresoAlimentosRepository;
        private readonly ITransportistaRepository _transportistaRepository;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IPilotoRepository _pilotoRepository;

        public EgresoAlimentosService(IEgresoAlimentosRepository egresoAlimentosRepository, ITransportistaRepository transportistaRepository, IVehiculoRepository vehiculoRepository, IPilotoRepository pilotoRepository)
        {
            _egresoAlimentosRepository = egresoAlimentosRepository;
            _transportistaRepository = transportistaRepository;
            _vehiculoRepository = vehiculoRepository;
            _pilotoRepository = pilotoRepository;
        }

        #region Consultas

        public async Task<List<EstadoEgresos>> GetAllEstadoEgresos()
        {
            List<EstadoEgresos> listado = await _egresoAlimentosRepository.GetAllEstadoEgresos();

            return listado;
        }

        public async Task<List<EstadoTraslados>> GetAllEstadoTraslados()
        {
            List<EstadoTraslados> listado = await _egresoAlimentosRepository.GetAllEstadoTraslados();

            return listado;
        }

        public async Task<List<dynamic>> GetAllDetallesRacionXConvenio(int idConvenio)
        {
            var listado = await _egresoAlimentosRepository.GetAllDetallesRacionXConvenio(idConvenio);

            return listado;
        }

        #endregion

        #region CRUD - EGRESO_ALIMENTOS

        public async Task<List<EgresoAlimentos>> GetAllEgresoAlimentos()
        {
            List<EgresoAlimentos> listado = await _egresoAlimentosRepository.GetAllEgresoAlimentos();

            return listado;
        }

        public async Task<EgresoAlimentos> GetEgresoAlimentos(Int64 id)
        {
            EgresoAlimentos modelo = await _egresoAlimentosRepository.GetEgresoAlimentos(id);

            return modelo;
        }
        public async Task<ServiceResponse<EgresoAlimentos>> SaveEgresoAlimentosEstado1(EgresoAlimentos modelo)
        {
            var response = new ServiceResponse<EgresoAlimentos>();

            try
            {
                // Crear objeto
                EgresoAlimentos objDb = new EgresoAlimentos()
                {
                    IdEgresoAlimentos = await _egresoAlimentosRepository.CuentaEgresoAlimentos(),
                    IdPrograma = modelo.IdPrograma,
                    IdConvenio = modelo.IdConvenio,
                    FechaSolicitud = modelo.FechaSolicitud,
                    TotalRaciones = modelo.TotalRaciones,
                    IdTipoAsistencia = modelo.IdTipoAsistencia,
                    IdTipoSalidaAlimento = modelo.IdTipoSalidaAlimento,
                    IdEstadoEgreso = (int)EstadosEgresoEnum.Inicio, // La primera creación del registro, se inicia con estado 1: Iniciada
                    Estado = 0
                };

                // Guardar el Egreso de Alimentos en la base de datos
                _egresoAlimentosRepository.SaveEgresoAlimentos(objDb);


                response.Data = objDb;
                response.Message = "Egreso de Alimentos creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado1(EgresoAlimentos modelo)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(modelo.IdEgresoAlimentos);

            try
            {
                objDb.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDb.IdPrograma = modelo.IdPrograma;
                objDb.IdConvenio = modelo.IdConvenio;
                objDb.FechaSolicitud = modelo.FechaSolicitud;
                objDb.TotalRaciones = modelo.TotalRaciones;
                objDb.IdTipoAsistencia = modelo.IdTipoAsistencia;
                objDb.IdTipoSalidaAlimento = modelo.IdTipoSalidaAlimento;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado2(EgresoAlimentos modelo)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(modelo.IdEgresoAlimentos);

            try
            {
                objDb.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDb.NoDespachoAlmacen = modelo.NoDespachoAlmacen;
                objDb.IdDepartamentoCarga = modelo.IdDepartamentoCarga;
                objDb.IdBodegaCarga = modelo.IdBodegaCarga;
                objDb.NoAutorizacion = modelo.NoAutorizacion;
                objDb.IdDepartamentoDestino = modelo.IdDepartamentoDestino;
                objDb.IdMunicipioDestino = modelo.IdMunicipioDestino;
                objDb.IdBodegaDestino = modelo.IdBodegaDestino;
                objDb.DireccionFinal = modelo.DireccionFinal;
                objDb.Comentarios = modelo.Comentarios;
                objDb.FirmaSelloDirectorVisan = modelo.FirmaSelloDirectorVisan;
                objDb.FirmaSelloJefeDepartamento = modelo.FirmaSelloJefeDepartamento;
                objDb.IdUsuarioEncargadoBodega = modelo.IdUsuarioEncargadoBodega;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        
        public async Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado3(EgresoAlimentos modelo)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(modelo.IdEgresoAlimentos);

            try
            {
                objDb.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDb.FirmaSelloJefeDepartamento = modelo.FirmaSelloJefeDepartamento;
                objDb.FirmaSelloDirectorVisan = modelo.FirmaSelloDirectorVisan;
                objDb.IdUsuarioEncargadoBodega = modelo.IdUsuarioEncargadoBodega;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Ingreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Ingreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> UpdateEgresoAlimentosEstado5(EgresoAlimentos modelo)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(modelo.IdEgresoAlimentos);

            try
            {
                objDb.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDb.FirmaSelloJefeBodegas = modelo.FirmaSelloJefeBodegas;
                objDb.ObservCondSanitariaTransp = modelo.ObservCondSanitariaTransp;
                objDb.ArchivoDespachoAlmacen = modelo.ArchivoDespachoAlmacen;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> AsignacionUsuarioLogisticaEgreso(int idEgreso, int idUsuario)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(idEgreso);

            try
            {
                objDb.IdUsuarioLogistica = idUsuario;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> AsignacionUsuarioEncargadoBodegaEgreso(int idEgreso, int idUsuario)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(idEgreso);

            try
            {
                objDb.IdUsuarioEncargadoBodega = idUsuario;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> AsignacionUsuarioRecibeTraslado(int idEgreso, int idUsuario)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(idEgreso);

            try
            {
                objDb.IdUsuarioRecibeTraslado = idUsuario;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Egreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<EgresoAlimentos>> DeleteEgresoAlimentos(Int64 id)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(id);

            try
            {
                objDb.Estado = 1;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de Alimentos eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Egreso de Alimentos: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Método que realiza el cambio de estado de un Egreso de Alimentos
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="estado">ESTADO_EGRESO</param>
        /// <returns></returns>
        public async Task<ServiceResponse<EgresoAlimentos>> CambioEstadoEgresoAlimentos(Int64 id, int estado)
        {
            var response = new ServiceResponse<EgresoAlimentos>();
            EgresoAlimentos objDb = await _egresoAlimentosRepository.GetEgresoAlimentos(id);

            try
            {
                objDb.IdEstadoEgreso = estado;

                _egresoAlimentosRepository.UpdateEgresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Egreso de Alimentos Enviado a la siguiente Etapa";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar enviar el Egreso de Alimentos a la siguiente Etapa : {ex.Message}";
            }

            return response;
        }

        #endregion

        #region CRUD - DETALLE_EGRESO_ALIMENTOS

        /// <summary>
        /// Retorna listado de los detalles de un registro de EGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">EGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<dynamic>>? GetAllDetalleEgresoAlimentos(Int64 id)
        {
            List<dynamic>? listado = await _egresoAlimentosRepository.GetAllDetalleEgresoAlimentos(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de EGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<DetalleEgresoAlimentos> GetDetalleEgresoAlimentos(Int64 id)
        {
            DetalleEgresoAlimentos modelo = await _egresoAlimentosRepository.GetDetalleEgresoAlimentos(id);

            return modelo;
        }

        public async Task<ServiceResponse<DetalleEgresoAlimentos>> SaveDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleEgresoAlimentos>();

            try
            {
                // Crear objeto
                DetalleEgresoAlimentos objDB = new DetalleEgresoAlimentos()
                {
                    IdDetalleEgresoAlimentos = await _egresoAlimentosRepository.CuentaDetalleEgresoAlimentos(),
                    IdEgresoAlimentos = modelo.IdEgresoAlimentos,
                    IdProductoAlimenticio = modelo.IdProductoAlimenticio,
                    IdPresentacion = modelo.IdPresentacion,
                    CantidadUnidad = modelo.CantidadUnidad,
                    IdUnidadMedida = modelo.IdUnidadMedida,
                    NoUnidades = modelo.NoUnidades,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                _egresoAlimentosRepository.SaveDetalleEgresoAlimentos(objDB);


                response.Data = objDB;
                response.Message = "Detalle creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el datelle: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEgresoAlimentos>> UpdateDetalleEgresoAlimentos(DetalleEgresoAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleEgresoAlimentos>();
            DetalleEgresoAlimentos objDB = await _egresoAlimentosRepository.GetDetalleEgresoAlimentos(modelo.IdDetalleEgresoAlimentos);

            try
            {
                objDB.IdDetalleEgresoAlimentos = modelo.IdDetalleEgresoAlimentos;
                objDB.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDB.IdProductoAlimenticio = modelo.IdProductoAlimenticio;
                objDB.IdPresentacion = modelo.IdPresentacion;
                objDB.CantidadUnidad = modelo.CantidadUnidad;
                objDB.IdUnidadMedida = modelo.IdUnidadMedida;
                objDB.NoUnidades = modelo.NoUnidades;

                _egresoAlimentosRepository.UpdateDetalleEgresoAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Detalle actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEgresoAlimentos>> UpdateDetalleEgresoAlimentosEstado5(DetalleEgresoAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleEgresoAlimentos>();
            DetalleEgresoAlimentos objDB = await _egresoAlimentosRepository.GetDetalleEgresoAlimentos(modelo.IdDetalleEgresoAlimentos);

            try
            {
                objDB.IdDetalleEgresoAlimentos = modelo.IdDetalleEgresoAlimentos;
                objDB.EstibaEgreso = modelo.EstibaEgreso;

                _egresoAlimentosRepository.UpdateDetalleEgresoAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Detalle actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEgresoAlimentos>> UpdateDetalleEgresoAlimentosTrasladoEstado1(DetalleEgresoAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleEgresoAlimentos>();
            DetalleEgresoAlimentos objDB = await _egresoAlimentosRepository.GetDetalleEgresoAlimentos(modelo.IdDetalleEgresoAlimentos);

            try
            {
                objDB.IdDetalleEgresoAlimentos = modelo.IdDetalleEgresoAlimentos;
                objDB.EstibaTraslado = modelo.EstibaTraslado;

                _egresoAlimentosRepository.UpdateDetalleEgresoAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Detalle actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleEgresoAlimentos>> DeleteDetalleEgresoAlimentos(Int64 id)
        {
            var response = new ServiceResponse<DetalleEgresoAlimentos>();
            DetalleEgresoAlimentos objDB = await _egresoAlimentosRepository.GetDetalleEgresoAlimentos(id);

            try
            {
                objDB.Estado = 1;

                _egresoAlimentosRepository.UpdateDetalleEgresoAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Detalle eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle: {ex.Message}";
            }

            return response;
        }


        #endregion

        #region CRUD SOLICITUD_SALIDA_ALIMENTOS

        public async Task<List<SolicitudSalidaAlimentos>> GetAllSolicitudSalidaAlimentos(Int64 id)
        {
            List<SolicitudSalidaAlimentos> listado = await _egresoAlimentosRepository.GetAllSolicitudSalidaAlimentos(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de EGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<SolicitudSalidaAlimentos> GetSolicitudSalidaAlimentos(Int64 id)
        {
            SolicitudSalidaAlimentos modelo = await _egresoAlimentosRepository.GetSolicitudSalidaAlimentos(id);

            return modelo;
        }

        public async Task<ServiceResponse<SolicitudSalidaAlimentos>> SaveSolicitudSalidaAlimentos(SolicitudSalidaAlimentos modelo)
        {
            var response = new ServiceResponse<SolicitudSalidaAlimentos>();

            try
            {
                // Validación de creación de Transportista
                var transportista = await _transportistaRepository.GetTransportistaNombre(modelo.NombreTransportista);

                if (transportista == null)
                {
                    Transportista newTransportista= new()
                    {
                        IdTransportista = await _transportistaRepository.Cuenta(),
                        NombreTransportista = modelo.NombreTransportista,
                        Estado = 0
                    };

                    transportista = await _transportistaRepository.SaveTransportista(newTransportista);
                }

                // Validación de creación de Vehículo
                var vehiculo = await _vehiculoRepository.GetVehiculoPlacaCabezal(modelo.PlacasTransporte);

                if (vehiculo == null)
                {
                    Vehiculo newVehiculo = new()
                    {
                        IdVehiculo = await _vehiculoRepository.Cuenta(),
                        PlacaCabezal = modelo.PlacasTransporte,
                        IdTransportista = (int)transportista.IdTransportista,
                        Estado = 0
                    };

                    vehiculo = await _vehiculoRepository.SaveVehiculo(newVehiculo);
                }

                // Validación de creación de Piloto
                var piloto = await _pilotoRepository.GetPilotoNombreYLicencia(modelo.NombrePiloto, modelo.NoLicencia);

                if (piloto == null)
                {
                    Piloto newPiloto = new()
                    {
                        IdPiloto = await _pilotoRepository.Cuenta(),
                        NombresPiloto = modelo.NombrePiloto,
                        NoLicencia = modelo.NoLicencia,
                        Estado = 0
                    };

                    piloto = await _pilotoRepository.SavePiloto(newPiloto);
                }

                // Crear objeto
                SolicitudSalidaAlimentos objDB = new SolicitudSalidaAlimentos()
                {
                    IdSolicitudSalidaAlimentos = await _egresoAlimentosRepository.CuentaSolicitudSalidaAlimentos(),
                    IdEgresoAlimentos = modelo.IdEgresoAlimentos,
                    NoConstancia = modelo.NoConstancia,
                    FechaDespacho = modelo.FechaDespacho,
                    IdPrograma = modelo.IdPrograma,
                    IdTransportista = (int)transportista.IdTransportista,
                    IdPiloto = (int)piloto.IdPiloto,
                    IdVehiculo = (int)vehiculo.IdVehiculo,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                _egresoAlimentosRepository.SaveSolicitudSalidaAlimentos(objDB);


                response.Data = objDB;
                response.Message = "Solicitud creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la Solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<SolicitudSalidaAlimentos>> DeleteSolicitudSalidaAlimentos(Int64 id)
        {
            var response = new ServiceResponse<SolicitudSalidaAlimentos>();
            SolicitudSalidaAlimentos objDB = await _egresoAlimentosRepository.GetSolicitudSalidaAlimentos(id);

            try
            {
                objDB.Estado = 1;

                _egresoAlimentosRepository.UpdateSolicitudSalidaAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Solicitud eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la solicitud: {ex.Message}";
            }

            return response;
        }

        #endregion

        #region CRUD DETALLE_SALIDA_ALIMENTOS

        public async Task<List<DetalleSalidaAlimentos>> GetAllDetalleSalidaAlimentos(Int64 id)
        {
            List<DetalleSalidaAlimentos> listado = await _egresoAlimentosRepository.GetAllDetalleSalidaAlimentos(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de DETALLE_SALIDA_ALIMENTOS
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<DetalleSalidaAlimentos> GetDetalleSalidaAlimentos(Int64 id)
        {
            DetalleSalidaAlimentos modelo = await _egresoAlimentosRepository.GetDetalleSalidaAlimentos(id);

            return modelo;
        }

        public async Task<ServiceResponse<DetalleSalidaAlimentos>> SaveDetalleSalidaAlimentos(DetalleSalidaAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleSalidaAlimentos>();

            try
            {
                // Crear objeto
                DetalleSalidaAlimentos objDB = new DetalleSalidaAlimentos()
                {
                    IdDetalleSalidaAlimentos = await _egresoAlimentosRepository.CuentaDetalleSalidaAlimentos(),
                    IdSolicitudSalidaAlimentos = modelo.IdSolicitudSalidaAlimentos,
                    IdProductoAlimenticio = modelo.IdProductoAlimenticio,
                    IdPresentacion = modelo.IdPresentacion,
                    CantidadUnidad = modelo.CantidadUnidad,
                    IdUnidadMedida = modelo.IdUnidadMedida,
                    CantidadSalida = modelo.CantidadSalida,
                    EstadoProducto = modelo.EstadoProducto,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                _egresoAlimentosRepository.SaveDetalleSalidaAlimentos(objDB);


                response.Data = objDB;
                response.Message = "Detalle creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la Solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSalidaAlimentos>> DeleteDetalleSalidaAlimentos(Int64 id)
        {
            var response = new ServiceResponse<DetalleSalidaAlimentos>();
            DetalleSalidaAlimentos objDB = await _egresoAlimentosRepository.GetDetalleSalidaAlimentos(id);

            try
            {
                objDB.Estado = 1;

                _egresoAlimentosRepository.UpdateDetalleSalidaAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Detalle eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Detalle: {ex.Message}";
            }

            return response;
        }

        #endregion

        #region CRUD - TRASLADO_ALIMENTOS

        public async Task<List<TrasladoAlimentos>> GetAllTrasladoAlimentos()
        {
            List<TrasladoAlimentos> listado = await _egresoAlimentosRepository.GetAllTrasladoAlimentos();

            return listado;
        }

        /// <summary>
        /// Retorna un registro de TRASLADO_ALIMENTOS
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<TrasladoAlimentos> GetTrasladoAlimentos(Int64 id)
        {
            TrasladoAlimentos modelo = await _egresoAlimentosRepository.GetTrasladoAlimentos(id);

            return modelo;
        }

        /// <summary>
        /// Retorna un registro de TRASLADO_ALIMENTOS con la búsqueda de un EGRESO_ALIMENTOS_ID
        /// </summary>
        /// <param name="id">EGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<TrasladoAlimentos> GetTrasladoAlimentosDeEgreso(Int64 id)
        {
            TrasladoAlimentos modelo = await _egresoAlimentosRepository.GetTrasladoAlimentosDeEgreso(id);

            return modelo;
        }

        public async Task<ServiceResponse<TrasladoAlimentos>> SaveTrasladoAlimentos(TrasladoAlimentos modelo)
        {
            var response = new ServiceResponse<TrasladoAlimentos>();

            try
            {
                // Crear objeto
                TrasladoAlimentos objDB = new()
                {
                    IdTrasladoAlimentos = await _egresoAlimentosRepository.CuentaTrasladoAlimentos(),
                    IdEgresoAlimentos = modelo.IdEgresoAlimentos,
                    IdEstadoTraslado = (int)EstadosTrasladoEnum.RecibiendoTraslado,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                _egresoAlimentosRepository.SaveTrasladoAlimentos(objDB);


                response.Data = objDB;
                response.Message = "Traslado creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el traslado: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<TrasladoAlimentos>> UpdateTrasladoAlimentos(TrasladoAlimentos modelo)
        {
            var response = new ServiceResponse<TrasladoAlimentos>();
            TrasladoAlimentos objDB = await _egresoAlimentosRepository.GetTrasladoAlimentos(modelo.IdTrasladoAlimentos);

            try
            {
                objDB.AutorizaRecepcion = modelo.AutorizaRecepcion;
                objDB.ArchivoDespachoAlmacenTraslado = modelo.ArchivoDespachoAlmacenTraslado ?? objDB.ArchivoDespachoAlmacenTraslado;

                _egresoAlimentosRepository.UpdateTrasladoAlimentos(objDB);

                response.Data = objDB;
                response.Message = "Detalle eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Detalle: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<TrasladoAlimentos>> CambioEstadoTrasladoAlimentos(Int64 id, int estado)
        {
            var response = new ServiceResponse<TrasladoAlimentos>();
            TrasladoAlimentos objDb = await _egresoAlimentosRepository.GetTrasladoAlimentos(id);

            try
            {
                objDb.IdEstadoTraslado = estado;

                _egresoAlimentosRepository.UpdateTrasladoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Traslado de Alimentos Enviado a la siguiente Etapa";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar enviar el Traslado de Alimentos a la siguiente Etapa : {ex.Message}";
            }

            return response;
        }

        #endregion

        #region Insert - MOVIMIENTOS_PRODUCTOS

        public async Task<ServiceResponse<int>> MoverProductosEgresoAMovimientos(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _egresoAlimentosRepository.GetAllDetalleEgresoAlimentos(id);
            var egresoAlimentos = await _egresoAlimentosRepository.GetEgresoAlimentos(id);

            try
            {
                foreach (var obj in objDB)
                {
                    // Se realiza la búsqueda por Bodega, ProductoAlimenticio, Presentacion y UnidadMedida;
                    // - si existe el registro se realiza un Update la existencia
                    // - al no existir, se realiza un Insert
                    MovimientosProductos objMPDB = new();
                    var objMov = await _egresoAlimentosRepository.GetMovimientosProductos((int)egresoAlimentos.IdBodegaCarga, (int)obj.IdProductoAlimenticio, (int)obj.IdUnidadMedida);

                    // El registro nulo, hace insert /* NO SE DEBE DE HACER INSERT, DEBIDO A QUE SIEMPRE DEBE DE HABER EXISTENCIA DEL INGRESO */
                    if (objMov == null)
                    {
                        // Crear objeto

                        objMPDB.IdMovimientosProductos = await _egresoAlimentosRepository.CuentaMovimientosProductos();
                        objMPDB.IdBodega = egresoAlimentos.IdBodegaCarga;
                        objMPDB.IdProductoAlimenticio = obj.IdProductoAlimenticio;
                        objMPDB.IdPresentacion = obj.IdPresentacion;
                        objMPDB.IdConvenio = egresoAlimentos.IdConvenio;
                        objMPDB.CantidadUnidad = obj.CantidadUnidad;
                        objMPDB.IdUnidadMedida = obj.IdUnidadMedida;
                        objMPDB.Existencias = obj.NoUnidades * -1;
                        objMPDB.IdRegistro = 0;
                        objMPDB.Tabla = "DETALLE_EGRESO_ALIMENTOS";
                        objMPDB.FechaVencimiento = null;
                        objMPDB.Estado = 0;

                        await _egresoAlimentosRepository.SaveMovimientosProductos(objMPDB);
                    }
                    else
                    {
                        // Al encontrar el registro, se actualiza la existencia.
                        var nuevaExistencia = objMov.Existencias - obj.NoUnidades;

                        objMov.Existencias = nuevaExistencia;

                        await _egresoAlimentosRepository.UpdateMovimientosProductos(objMov);
                    }
                }


                response.Data = (int)id;
                response.Message = "Movimiento de Productos realizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al mover el ingreso de productos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> MoverProductosTrasladoAMovimientosEgreso(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _egresoAlimentosRepository.GetAllDetalleEgresoAlimentos(id);
            var egresoAlimentos = await _egresoAlimentosRepository.GetEgresoAlimentos(id);

            try
            {
                foreach (var obj in objDB)
                {
                    // Se realiza la búsqueda por Bodega, ProductoAlimenticio, Presentacion y UnidadMedida;
                    // - si existe el registro se realiza un Update la existencia
                    // - al no existir, se realiza un Insert
                    MovimientosProductos objMPDB = new();
                    var objMov = await _egresoAlimentosRepository.GetMovimientosProductos((int)egresoAlimentos.IdBodegaCarga, (int)obj.IdProductoAlimenticio, (int)obj.IdUnidadMedida);

                    // El registro nulo, hace insert /* NO SE DEBE DE HACER INSERT, DEBIDO A QUE SIEMPRE DEBE DE HABER EXISTENCIA DEL INGRESO */
                    if (objMov == null)
                    {
                        // Crear objeto

                        objMPDB.IdMovimientosProductos = await _egresoAlimentosRepository.CuentaMovimientosProductos();
                        objMPDB.IdBodega = egresoAlimentos.IdBodegaCarga;
                        objMPDB.IdProductoAlimenticio = obj.IdProductoAlimenticio;
                        objMPDB.IdPresentacion = obj.IdPresentacion;
                        objMPDB.IdConvenio = egresoAlimentos.IdConvenio;
                        objMPDB.CantidadUnidad = obj.CantidadUnidad;
                        objMPDB.IdUnidadMedida = obj.IdUnidadMedida;
                        objMPDB.Existencias = obj.NoUnidades * -1;
                        objMPDB.IdRegistro = 0;
                        objMPDB.Tabla = "DETALLE_EGRESO_ALIMENTOS";
                        objMPDB.FechaVencimiento = null;
                        objMPDB.Estado = 0;

                        await _egresoAlimentosRepository.SaveMovimientosProductos(objMPDB);
                    }
                    else
                    {
                        // Al encontrar el registro, se actualiza la existencia.
                        var nuevaExistencia = objMov.Existencias - obj.NoUnidades;

                        objMov.Existencias = nuevaExistencia;

                        await _egresoAlimentosRepository.UpdateMovimientosProductos(objMov);
                    }
                }


                response.Data = (int)id;
                response.Message = "Movimiento de Productos realizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al mover el ingreso de productos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> MoverProductosTrasladoAMovimientosIngreso(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var traslado = await _egresoAlimentosRepository.GetTrasladoAlimentos(id);
            var objDB = await _egresoAlimentosRepository.GetAllDetalleEgresoAlimentos((Int64)traslado.IdEgresoAlimentos);
            var egresoAlimentos = await _egresoAlimentosRepository.GetEgresoAlimentos((Int64)traslado.IdEgresoAlimentos);

            try
            {
                foreach (var obj in objDB)
                {
                    // Se realiza la búsqueda por Bodega, ProductoAlimenticio, Presentacion y UnidadMedida;
                    // - si existe el registro se realiza un Update la existencia
                    // - al no existir, se realiza un Insert

                    /* La segunda parte consta de la suma de existencias a la bodega que recibe los productos */
                    MovimientosProductos objMPDBAdd = new();
                    var objMovAdd = await _egresoAlimentosRepository.GetMovimientosProductos((int)egresoAlimentos.IdBodegaDestino, (int)obj.IdProductoAlimenticio, (int)obj.IdUnidadMedida);

                    // El registro nulo, hace insert
                    if (objMovAdd == null)
                    {
                        // Crear objeto

                        objMPDBAdd.IdMovimientosProductos = await _egresoAlimentosRepository.CuentaMovimientosProductos();
                        objMPDBAdd.IdBodega = egresoAlimentos.IdBodegaDestino;
                        objMPDBAdd.IdProductoAlimenticio = obj.IdProductoAlimenticio;
                        objMPDBAdd.IdPresentacion = obj.IdPresentacion;
                        objMPDBAdd.IdConvenio = egresoAlimentos.IdConvenio;
                        objMPDBAdd.CantidadUnidad = obj.CantidadUnidad;
                        objMPDBAdd.IdUnidadMedida = obj.IdUnidadMedida;
                        objMPDBAdd.Existencias = obj.NoUnidades;
                        objMPDBAdd.IdRegistro = 0;
                        objMPDBAdd.Tabla = "DETALLE_EGRESO_ALIMENTOS";
                        objMPDBAdd.FechaVencimiento = null;
                        objMPDBAdd.Estado = 0;

                        await _egresoAlimentosRepository.SaveMovimientosProductos(objMPDBAdd);
                    }
                    else
                    {
                        // Al encontrar el registro, se actualiza la existencia.
                        var nuevaExistencia = objMovAdd.Existencias + obj.NoUnidades;

                        objMovAdd.Existencias = nuevaExistencia;

                        await _egresoAlimentosRepository.UpdateMovimientosProductos(objMovAdd);
                    }
                }


                response.Data = (int)id;
                response.Message = "Movimiento de Productos realizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al mover el ingreso de productos: {ex.Message}";
            }

            return response;
        }

        #endregion

        #region Insert - KARDEX

        public async Task<ServiceResponse<int>> MoverProductosEgresoAKardex(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _egresoAlimentosRepository.GetAllDetalleEgresoAlimentos(id);
            var egresoAlimentos = await _egresoAlimentosRepository.GetEgresoAlimentos(id);

            try
            {
                foreach (var obj in objDB)
                {
                    // Crear objeto
                    Kardex objKdx = new Kardex()
                    {
                        IdKardex = await _egresoAlimentosRepository.CuentaKardex(),
                        FechaMovimiento = DateTime.Now,
                        IdBodega = egresoAlimentos.IdBodegaCarga,
                        IdProductoAlimenticio = obj.IdProductoAlimenticio,
                        IdConvenio = egresoAlimentos.IdConvenio,
                        IdPresentacion = obj.IdPresentacion,
                        CantidadUnidad = obj.CantidadUnidad,
                        IdUnidadMedida = obj.IdUnidadMedida,
                        Estiba = obj.EstibaEgreso,
                        IdRegistro = (int)obj.IdDetalleEgresoAlimentos,
                        Tabla = "DETALLE_EGRESO_ALIMENTOS",
                        Egreso = obj.NoUnidades,
                        Saldo = obj.NoUnidades * obj.CantidadUnidad,
                        Estado = 0
                    };

                    await _egresoAlimentosRepository.SaveKardex(objKdx);
                }


                response.Data = (int)id;
                response.Message = "Kardex de Productos realizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al mover el Kardex: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<int>> MoverProductosTrasladoAKardex(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var traslado = await _egresoAlimentosRepository.GetTrasladoAlimentos(id);
            var objDB = await _egresoAlimentosRepository.GetAllDetalleEgresoAlimentos((Int64)traslado.IdEgresoAlimentos);
            var egresoAlimentos = await _egresoAlimentosRepository.GetEgresoAlimentos((Int64)traslado.IdEgresoAlimentos);

            try
            {
                foreach (var obj in objDB)
                {
                    // Crear objeto de Egreso de una Bodega
                    Kardex objKdxEgreso = new Kardex()
                    {
                        IdKardex = await _egresoAlimentosRepository.CuentaKardex(),
                        FechaMovimiento = DateTime.Now,
                        IdBodega = egresoAlimentos.IdBodegaCarga,
                        IdProductoAlimenticio = obj.IdProductoAlimenticio,
                        IdPresentacion = obj.IdPresentacion,
                        IdConvenio = egresoAlimentos.IdConvenio,
                        CantidadUnidad = obj.CantidadUnidad,
                        IdUnidadMedida = obj.IdUnidadMedida,
                        Estiba = obj.EstibaEgreso,
                        IdRegistro = (int)obj.IdDetalleEgresoAlimentos,
                        Tabla = "DETALLE_EGRESO_ALIMENTOS",
                        Egreso = obj.NoUnidades,
                        Saldo = obj.NoUnidades * obj.CantidadUnidad,
                        Estado = 0
                    };

                    await _egresoAlimentosRepository.SaveKardex(objKdxEgreso);

                    // Crear objeto de Ingreso de la otra Bodega
                    Kardex objKdxIngreso = new Kardex()
                    {
                        IdKardex = await _egresoAlimentosRepository.CuentaKardex(),
                        FechaMovimiento = DateTime.Now,
                        IdBodega = egresoAlimentos.IdBodegaDestino,
                        IdProductoAlimenticio = obj.IdProductoAlimenticio,
                        IdPresentacion = obj.IdPresentacion,
                        IdConvenio = egresoAlimentos.IdConvenio,
                        CantidadUnidad = obj.CantidadUnidad,
                        IdUnidadMedida = obj.IdUnidadMedida,
                        Estiba = obj.EstibaTraslado,
                        IdRegistro = (int)obj.IdDetalleEgresoAlimentos,
                        Tabla = "DETALLE_EGRESO_ALIMENTOS",
                        Ingreso = obj.NoUnidades,
                        Estado = 0
                    };

                    await _egresoAlimentosRepository.SaveKardex(objKdxIngreso);
                }


                response.Data = (int)id;
                response.Message = "Kardex de Productos realizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al mover el Kardex: {ex.Message}";
            }

            return response;
        }

        #endregion

        #region CRUD - BITACORA

        public async Task<List<Bitacora>> GetAllBitacora(Int64 id)
        {
            List<Bitacora> listado = await _egresoAlimentosRepository.GetAllBitacora(id);

            return listado;
        }

        public async Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado)
        {
            List<Bitacora> listado = await _egresoAlimentosRepository.GetAllBitacoraEstado(idEstado);

            return listado;
        }
        public async Task<ServiceResponse<Bitacora>> SaveBitacora(Bitacora modelo)
        {
            var response = new ServiceResponse<Bitacora>();

            try
            {
                // Crear objeto
                Bitacora objDB = new Bitacora()
                {
                    IdBitacora = await _egresoAlimentosRepository.CuentaBitacora(),
                    Accion = modelo.Accion,
                    IdRegistro = modelo.IdRegistro,
                    NombreTabla = modelo.NombreTabla,
                    IdUsuario = modelo.IdUsuario,
                    FechaHora = DateTime.Now,
                    Estado = 0
                };

                // Guardar la Bitacora en la base de datos
                _egresoAlimentosRepository.SaveBitacora(objDB);


                response.Data = objDB;
                response.Message = "Bitácora creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la bitácora: {ex.Message}";
            }

            return response;
        }

        #endregion
    }
}
