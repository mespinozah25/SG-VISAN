using PROY.Enum;
using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class ReingresoAlimentosService : IReingresoAlimentosService
    {
        private readonly IReingresoAlimentosRepository _reingresoAlimentosRepository;

        public ReingresoAlimentosService(IReingresoAlimentosRepository reingresoAlimentosRepository)
        {
            _reingresoAlimentosRepository = reingresoAlimentosRepository;
        }

        #region Consultas

        public async Task<List<EstadoReingresos>> GetAllEstadoReingresos()
        {
            List<EstadoReingresos> listado = await _reingresoAlimentosRepository.GetAllEstadoReingresos();

            return listado;
        }

        public async Task<List<object>> GetEgresosLike(string term, int limit)
        {
            List<object> listado = await _reingresoAlimentosRepository.GetEgresosLike(term, limit);

            return listado;
        }

        #endregion

        #region CRUD - REINGRESO_ALIMENTOS

        public async Task<List<ReingresoAlimentos>> GetAllReingresoAlimentos()
        {
            List<ReingresoAlimentos> listado = await _reingresoAlimentosRepository.GetAllReingresoAlimentos();

            return listado;
        }

        public async Task<ReingresoAlimentos> GetReingresoAlimentos(Int64 id)
        {
            ReingresoAlimentos modelo = await _reingresoAlimentosRepository.GetReingresoAlimentos(id);

            return modelo;
        }
        public async Task<ServiceResponse<ReingresoAlimentos>> SaveReingresoAlimentosEstado1(ReingresoAlimentos modelo)
        {
            var response = new ServiceResponse<ReingresoAlimentos>();

            try
            {
                // Crear objeto
                ReingresoAlimentos objDb = new ReingresoAlimentos()
                {
                    IdReingresoAlimentos = await _reingresoAlimentosRepository.CuentaReingresoAlimentos(),
                    NoOficioReingreso = modelo.NoOficioReingreso,
                    IdPrograma = modelo.IdPrograma,
                    FechaSolicitud = modelo.FechaSolicitud,
                    ArchivoOficioReingreso = modelo.ArchivoOficioReingreso,
                    IdEgresoAlimentos = modelo.IdEgresoAlimentos,
                    ObservacionFisicaIndeca = modelo.ObservacionFisicaIndeca,
                    ArchivoDespachoAlmacenReingreso = modelo.ArchivoDespachoAlmacenReingreso,
                    IdEstadoReingreso = (int)EstadosReingresoEnum.Inicio, // La primera creación del registro, se inicia con estado 1: Iniciada
                    Estado = 0
                };

                // Guardar el Reingreso de Alimentos en la base de datos
                _reingresoAlimentosRepository.SaveReingresoAlimentos(objDb);


                response.Data = objDb;
                response.Message = "Reingreso de Alimentos creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Reingreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ReingresoAlimentos>> UpdateReingresoAlimentosEstado1(ReingresoAlimentos modelo)
        {
            var response = new ServiceResponse<ReingresoAlimentos>();
            ReingresoAlimentos objDb = await _reingresoAlimentosRepository.GetReingresoAlimentos(modelo.IdReingresoAlimentos);

            try
            {
                objDb.IdReingresoAlimentos = modelo.IdReingresoAlimentos;
                objDb.NoOficioReingreso = modelo.NoOficioReingreso;
                objDb.IdPrograma = modelo.IdPrograma;
                objDb.FechaSolicitud = modelo.FechaSolicitud;
                objDb.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDb.ObservacionFisicaIndeca = modelo.ObservacionFisicaIndeca;
                objDb.ArchivoOficioReingreso = modelo.ArchivoOficioReingreso;

                _reingresoAlimentosRepository.UpdateReingresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Reingreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Reingreso de alimentos: {ex.Message}";
            }

            return response;
        }


        public async Task<ServiceResponse<ReingresoAlimentos>> UpdateReingresoAlimentosEstado2(ReingresoAlimentos modelo)
        {
            var response = new ServiceResponse<ReingresoAlimentos>();
            ReingresoAlimentos objDb = await _reingresoAlimentosRepository.GetReingresoAlimentos(modelo.IdReingresoAlimentos);

            try
            {
                objDb.IdEgresoAlimentos = modelo.IdEgresoAlimentos;
                objDb.ObservacionFisicaIndeca = modelo.ObservacionFisicaIndeca;
                objDb.ArchivoDespachoAlmacenReingreso = modelo.ArchivoDespachoAlmacenReingreso ?? objDb.ArchivoDespachoAlmacenReingreso;

                _reingresoAlimentosRepository.UpdateReingresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Reingreso de alimentos actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el Reingreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ReingresoAlimentos>> AsignacionUsuarioEncargado(int idReingreso, int idUsuario)
        {
            var response = new ServiceResponse<ReingresoAlimentos>();
            ReingresoAlimentos objDb = await _reingresoAlimentosRepository.GetReingresoAlimentos(idReingreso);

            try
            {
                objDb.IdUsuarioEncargado = idUsuario;

                _reingresoAlimentosRepository.UpdateReingresoAlimentos(objDb);

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

        public async Task<ServiceResponse<ReingresoAlimentos>> DeleteReingresoAlimentos(Int64 id)
        {
            var response = new ServiceResponse<ReingresoAlimentos>();
            ReingresoAlimentos objDb = await _reingresoAlimentosRepository.GetReingresoAlimentos(id);

            try
            {
                objDb.Estado = 1;

                _reingresoAlimentosRepository.UpdateReingresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Reingreso de Alimentos eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Reingreso de Alimentos: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Método que realiza el cambio de estado de un Ingreso de Alimentos
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="estado">ESTADO_EGRESO</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ReingresoAlimentos>> CambioEstadoReingresoAlimentos(Int64 id, int estado)
        {
            var response = new ServiceResponse<ReingresoAlimentos>();
            ReingresoAlimentos objDb = await _reingresoAlimentosRepository.GetReingresoAlimentos(id);

            try
            {
                objDb.IdEstadoReingreso = estado;

                _reingresoAlimentosRepository.UpdateReingresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Reingreso de Alimentos Enviado a Bodega";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar enviar el Reingreso de Alimentos a Bodega : {ex.Message}";
            }

            return response;
        }

        #endregion

        #region CRUD - DETALLE_REINGRESO_ALIMENTOS

        /// <summary>
        /// Retorna listado de los detalles de un registro de REINGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">REINGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleReingresoAlimentos>> GetAllDetalleReingresoAlimentos(Int64 id)
        {
            List<DetalleReingresoAlimentos> listado = await _reingresoAlimentosRepository.GetAllDetalleReingresoAlimentos(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de DTALLE_REINGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<DetalleReingresoAlimentos> GetDetalleReingresoAlimentos(Int64 id)
        {
            DetalleReingresoAlimentos modelo = await _reingresoAlimentosRepository.GetDetalleReingresoAlimentos(id);

            return modelo;
        }

        public async Task<ServiceResponse<DetalleReingresoAlimentos>> SaveDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleReingresoAlimentos>();

            try
            {
                // Crear objeto
                DetalleReingresoAlimentos objDB = new DetalleReingresoAlimentos()
                {
                    IdDetalleReingresoAlimentos = await _reingresoAlimentosRepository.CuentaDetalleReingresoAlimentos(),
                    IdReingresoAlimentos = modelo.IdReingresoAlimentos,
                    IdProductoAlimenticio = modelo.IdProductoAlimenticio,
                    IdPresentacion = modelo.IdPresentacion,
                    CantidadUnidad = modelo.CantidadUnidad,
                    IdUnidadMedida = modelo.IdUnidadMedida,
                    Cantidad = modelo.Cantidad,
                    FechaVencimiento = modelo.FechaVencimiento,
                    Estiba = modelo.Estiba,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                _reingresoAlimentosRepository.SaveDetalleReingresoAlimentos(objDB);


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

        public async Task<ServiceResponse<DetalleReingresoAlimentos>> UpdateDetalleReingresoAlimentos(DetalleReingresoAlimentos modelo)
        {
            var response = new ServiceResponse<DetalleReingresoAlimentos>();
            DetalleReingresoAlimentos objDB = await _reingresoAlimentosRepository.GetDetalleReingresoAlimentos(modelo.IdDetalleReingresoAlimentos);

            try
            {
                objDB.IdDetalleReingresoAlimentos = modelo.IdDetalleReingresoAlimentos;
                objDB.IdReingresoAlimentos = modelo.IdReingresoAlimentos;
                objDB.IdProductoAlimenticio = modelo.IdProductoAlimenticio;
                objDB.IdPresentacion = modelo.IdPresentacion;
                objDB.CantidadUnidad = modelo.CantidadUnidad;
                objDB.IdUnidadMedida = modelo.IdUnidadMedida;
                objDB.Cantidad = modelo.Cantidad;
                objDB.FechaVencimiento = modelo.FechaVencimiento;
                objDB.Estiba = modelo.Estiba;

                _reingresoAlimentosRepository.UpdateDetalleReingresoAlimentos(objDB);

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

        public async Task<ServiceResponse<DetalleReingresoAlimentos>> DeleteDetalleReingresoAlimentos(Int64 id)
        {
            var response = new ServiceResponse<DetalleReingresoAlimentos>();
            DetalleReingresoAlimentos objDB = await _reingresoAlimentosRepository.GetDetalleReingresoAlimentos(id);

            try
            {
                objDB.Estado = 1;

                _reingresoAlimentosRepository.UpdateDetalleReingresoAlimentos(objDB);

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

        #region Insert - MOVIMIENTOS_PRODUCTOS

        public async Task<ServiceResponse<int>> MoverProductosReingresoAMovimientos(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _reingresoAlimentosRepository.GetAllDetalleReingresoAlimentos(id);
            var reingresoAlimentos = await _reingresoAlimentosRepository.GetReingresoAlimentos(id);
            var egresoAlimentos = await _reingresoAlimentosRepository.GetEgresoAlimentos((Int64)reingresoAlimentos.IdEgresoAlimentos);

            try
            {
                foreach (DetalleReingresoAlimentos obj in objDB)
                {
                    // Se realiza la búsqueda por Bodega, ProductoAlimenticio, Presentacion y UnidadMedida;
                    // - si existe el registro se realiza un Update la existencia
                    // - al no existir, se realiza un Insert
                    MovimientosProductos objMPDB = new();
                    var objMov = await _reingresoAlimentosRepository.GetMovimientosProductos((int)egresoAlimentos.IdBodegaCarga, (int)obj.IdProductoAlimenticio, (int)obj.IdPresentacion, (int)obj.IdUnidadMedida);

                    // El registro nulo, hace insert /* NO SE DEBE DE HACER INSERT, DEBIDO A QUE SIEMPRE DEBE DE HABER EXISTENCIA DEL INGRESO */
                    if (objMov == null)
                    {
                        // Crear objeto

                        objMPDB.IdMovimientosProductos = await _reingresoAlimentosRepository.CuentaMovimientosProductos();
                        objMPDB.IdBodega = egresoAlimentos.IdBodegaCarga;
                        objMPDB.IdProductoAlimenticio = obj.IdProductoAlimenticio;
                        objMPDB.IdPresentacion = obj.IdPresentacion;
                        objMPDB.Estiba = obj.Estiba;
                        objMPDB.CantidadUnidad = obj.CantidadUnidad;
                        objMPDB.IdUnidadMedida = obj.IdUnidadMedida;
                        objMPDB.Existencias = obj.Cantidad;
                        objMPDB.IdRegistro = 0;
                        objMPDB.Tabla = "DETALLE_REINGRESO_ALIMENTOS";
                        objMPDB.FechaVencimiento = null;
                        objMPDB.Estado = 0;

                        await _reingresoAlimentosRepository.SaveMovimientosProductos(objMPDB);
                    }
                    else
                    {
                        // Al encontrar el registro, se actualiza la existencia.
                        var nuevaExistencia = objMov.Existencias + obj.Cantidad;

                        objMov.Existencias = nuevaExistencia;

                        await _reingresoAlimentosRepository.UpdateMovimientosProductos(objMov);
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

        public async Task<ServiceResponse<int>> MoverProductosReingresoAKardex(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _reingresoAlimentosRepository.GetAllDetalleReingresoAlimentos(id);
            var reingresoAlimentos = await _reingresoAlimentosRepository.GetReingresoAlimentos(id);
            var egresoAlimentos = await _reingresoAlimentosRepository.GetEgresoAlimentos((Int64)reingresoAlimentos.IdEgresoAlimentos);

            try
            {
                foreach (DetalleReingresoAlimentos obj in objDB)
                {
                    // Crear objeto
                    Kardex objKdx = new Kardex()
                    {
                        IdKardex = await _reingresoAlimentosRepository.CuentaKardex(),
                        FechaMovimiento = DateTime.Now,
                        IdBodega = egresoAlimentos.IdBodegaCarga,
                        IdProductoAlimenticio = obj.IdProductoAlimenticio,
                        IdPresentacion = obj.IdPresentacion,
                        Estiba = obj.Estiba,
                        CantidadUnidad = obj.CantidadUnidad,
                        IdUnidadMedida = obj.IdUnidadMedida,
                        IdRegistro = (int)obj.IdDetalleReingresoAlimentos,
                        Tabla = "DETALLE_REINGRESO_ALIMENTOS",
                        Ingreso = obj.Cantidad,
                        Saldo = obj.Cantidad * obj.CantidadUnidad,
                        Estado = 0
                    };

                    _reingresoAlimentosRepository.SaveKardex(objKdx);
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
            List<Bitacora> listado = await _reingresoAlimentosRepository.GetAllBitacora(id);

            return listado;
        }

        public async Task<Bitacora> GetBitacoraEstado(int idEstado, int idReingreso)
        {
            Bitacora listado = await _reingresoAlimentosRepository.GetBitacoraEstado(idEstado, idReingreso);

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
                    IdBitacora = await _reingresoAlimentosRepository.CuentaBitacora(),
                    Accion = modelo.Accion,
                    IdRegistro = modelo.IdRegistro,
                    NombreTabla = modelo.NombreTabla,
                    IdUsuario = modelo.IdUsuario,
                    FechaHora = DateTime.Now,
                    Estado = 0
                };

                // Guardar la Bitacora en la base de datos
                _reingresoAlimentosRepository.SaveBitacora(objDB);


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
