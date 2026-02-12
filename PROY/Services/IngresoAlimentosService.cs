using PROY.Enum;
using PROY.Models;
using PROY.Repositories;
using PROY.ViewModels;

namespace PROY.Services
{
    public class IngresoAlimentosService : IIngresoAlimentosService
    {
        private readonly IIngresoAlimentosRepository _ingresoAlimentosRepository;
        private readonly IJuntaDeRecepcionRepository _juntaDeRecepcionRepository;

        public IngresoAlimentosService(IIngresoAlimentosRepository ingresoAlimentosRepository, IJuntaDeRecepcionRepository juntaDeRecepcionRepository)
        {
            _ingresoAlimentosRepository = ingresoAlimentosRepository;
            _juntaDeRecepcionRepository = juntaDeRecepcionRepository;
        }

        #region Consultas



        public async Task<List<EstadoIngresos>> GetAllEstadoIngresos()
        {
            List<EstadoIngresos> listado = await _ingresoAlimentosRepository.GetAllEstadoIngresos();

            return listado;
        }

        public async Task<List<TipoNombramiento>> GetAllTipoNombramiento()
        {
            List<TipoNombramiento> listado = await _ingresoAlimentosRepository.GetAllTipoNombramiento();

            return listado;
        }

        #endregion

        #region CRUD - INGRESO_ALIMENTOS

        public async Task<List<IngresoAlimentos>> GetAllIngresoAlimentos()
        {
            List<IngresoAlimentos> listado = await _ingresoAlimentosRepository.GetAllIngresoAlimentos();

            return listado;
        }

        public async Task<IngresoAlimentos> GetIngresoAlimentos(Int64 id)
        {
            IngresoAlimentos modelo = await _ingresoAlimentosRepository.GetIngresoAlimentos(id);

            return modelo;
        }
        public async Task<ServiceResponse<IngresoAlimentos>> SaveIngresoAlimentosEstado1(IngresoAlimentos modelo)
        {
            var response = new ServiceResponse<IngresoAlimentos>();

            try
            {
                // Crear objeto
                IngresoAlimentos objDb = new IngresoAlimentos()
                {
                    IdIngresoAlimentos = await _ingresoAlimentosRepository.CuentaIngresoAlimentos(),
                    IdPrograma = modelo.IdPrograma,
                    IdConvenio = modelo.IdConvenio,
                    IdBodegaRecibe = modelo.IdBodegaRecibe,
                    IdJuntaReceptora = modelo.IdJuntaReceptora,
                    IdEstadoIngreso = 1, // La primera creación del registro, se inicia con estado 1: Iniciada
                    Estado = 0
                };

                // Guardar el Ingreso de Alimentos en la base de datos
                _ingresoAlimentosRepository.SaveIngresoAlimentos(objDb);


                response.Data = objDb;
                response.Message = "Ingreso de Alimentos creado con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Ingreso de alimentos: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado1(IngresoAlimentos modelo)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(modelo.IdIngresoAlimentos);

            try
            {
                objDb.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDb.IdPrograma = modelo.IdPrograma;
                objDb.IdConvenio = modelo.IdConvenio;
                objDb.IdBodegaRecibe = modelo.IdBodegaRecibe;
                objDb.IdJuntaReceptora = modelo.IdJuntaReceptora;
                objDb.IdEstadoIngreso = 1;

                _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

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

        public async Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado2(IngresoAlimentos modelo)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(modelo.IdIngresoAlimentos);

            try
            {
                objDb.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDb.NoWaybill = modelo.NoWaybill;
                objDb.FechaWaybill = modelo.FechaWaybill;
                objDb.NoLti = modelo.NoLti;
                objDb.ArchivoWaybill = modelo.ArchivoWaybill ?? objDb.ArchivoWaybill;
                objDb.FirmaSelloEncargadoWaybill = modelo.FirmaSelloEncargadoWaybill;
                objDb.NombreResponsableIndeca = modelo.NombreResponsableIndeca;
                objDb.NoActa = modelo.NoActa;
                objDb.FolioInicialActa = modelo.FolioInicialActa;
                objDb.FolioFinalActa = modelo.FolioFinalActa;
                objDb.DocumentoActa = modelo.DocumentoActa ?? objDb.DocumentoActa;
                objDb.FirmaMiembrosJuntaActa = modelo.FirmaMiembrosJuntaActa;
                objDb.FirmaEncargadoBodegaActa = modelo.FirmaEncargadoBodegaActa;

                _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

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

        public async Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado3(IngresoAlimentos modelo)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(modelo.IdIngresoAlimentos);

            try
            {
                objDb.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDb.NoForma1h = modelo.NoForma1h;
                objDb.FechaForma1h = modelo.FechaForma1h;
                objDb.IdProveedor1H = modelo.IdProveedor1H;

                _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

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

        public async Task<ServiceResponse<IngresoAlimentos>> UpdateIngresoAlimentosEstado4(IngresoAlimentos modelo)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(modelo.IdIngresoAlimentos);

            try
            {
                objDb.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDb.ArchivoWaybillFinanciero = modelo.ArchivoWaybillFinanciero ?? objDb.ArchivoWaybillFinanciero;
                objDb.ArchivoActaFinanciero = modelo.ArchivoActaFinanciero ?? objDb.ArchivoActaFinanciero;
                objDb.ArchivoForma1hFinanciero = modelo.ArchivoForma1hFinanciero ?? objDb.ArchivoForma1hFinanciero;
                objDb.ArchivoBoletaAnalisis = modelo.ArchivoBoletaAnalisis ?? objDb.ArchivoBoletaAnalisis;

                _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

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

        public async Task<ServiceResponse<IngresoAlimentos>> AsignacionUsuarioPendienteFinalizar(int idEgreso, int idUsuario)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(idEgreso);

            try
            {
                objDb.IdUsuarioJefeBodega = idUsuario;

                var resp = await _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

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

        public async Task<ServiceResponse<IngresoAlimentos>> DeleteIngresoAlimentos(Int64 id)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(id);

            try
            {
                objDb.Estado = 1;

                _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Ingreso de Alimentos eliminado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Ingreso de Alimentos: {ex.Message}";
            }

            return response;
        }

        /// <summary>
        /// Método que realiza el cambio de estado de un Ingreso de Alimentos
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="estado">ESTADO_INGRESO</param>
        /// <returns></returns>
        public async Task<ServiceResponse<IngresoAlimentos>> CambioEsadoIngresoAlimentos(Int64 id, int estado)
        {
            var response = new ServiceResponse<IngresoAlimentos>();
            IngresoAlimentos objDb = await _ingresoAlimentosRepository.GetIngresoAlimentos(id);

            try
            {
                objDb.IdEstadoIngreso = estado;

                _ingresoAlimentosRepository.UpdateIngresoAlimentos(objDb);

                response.Data = objDb;
                response.Message = "Ingreso de Alimentos Enviado a la siguiente Etapa";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar enviar el Ingreso de Alimentos a la siguiente Etapa : {ex.Message}";
            }

            return response;
        }

        #endregion

        #region CRUD - PRODUCTOS_INGRESO

        /// <summary>
        /// Retorna listado de los Productos Ingreso de un registro de INGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<ProductosIngreso>> GetAllProductosIngreso(Int64 id)
        {
            List<ProductosIngreso> listado = await _ingresoAlimentosRepository.GetAllProductosIngreso(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de PRODUCTOS_INGRESO
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<ProductosIngreso> GetProductosIngreso(Int64 id)
        {
            ProductosIngreso modelo = await _ingresoAlimentosRepository.GetProductosIngreso(id);

            return modelo;
        }

        public async Task<ServiceResponse<ProductosIngreso>> SaveProductosIngreso(ProductosIngreso modelo)
        {
            var response = new ServiceResponse<ProductosIngreso>();

            try
            {
                // Crear objeto
                ProductosIngreso objDB = new ProductosIngreso()
                {
                    IdProductosIngreso = await _ingresoAlimentosRepository.CuentaProductosIngreso(),
                    IdIngresoAlimentos = modelo.IdIngresoAlimentos,
                    IdProductoAlimenticio = modelo.IdProductoAlimenticio,
                    Estiba = modelo.Estiba,
                    IdPresentacion = modelo.IdPresentacion,
                    CantidadUnidad = modelo.CantidadUnidad,
                    IdUnidadMedida = modelo.IdUnidadMedida,
                    CantidadRecibir = modelo.CantidadRecibir,
                    FechaIngreso = modelo.FechaIngreso,
                    FechaVencimiento = modelo.FechaVencimiento,
                    FolioLibroAlmacen = modelo.FolioLibroAlmacen,
                    PrecioUnidad = modelo.PrecioUnidad,
                    ValorTotal = modelo.ValorTotal,
                    Estado = 0
                };

                // Guardar la ración detalle en la base de datos
                _ingresoAlimentosRepository.SaveProductosIngreso(objDB);


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

        public async Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngreso(ProductosIngreso modelo)
        {
            var response = new ServiceResponse<ProductosIngreso>();
            ProductosIngreso objDB = await _ingresoAlimentosRepository.GetProductosIngreso(modelo.IdProductosIngreso);

            try
            {
                objDB.IdProductosIngreso = modelo.IdProductosIngreso;
                objDB.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDB.IdProductoAlimenticio = modelo.IdProductoAlimenticio;
                objDB.Estiba = modelo.Estiba;
                objDB.IdPresentacion = modelo.IdPresentacion;
                objDB.CantidadUnidad = modelo.CantidadUnidad;
                objDB.IdUnidadMedida = modelo.IdUnidadMedida;
                objDB.CantidadRecibir = modelo.CantidadRecibir;
                objDB.FechaIngreso = modelo.FechaIngreso;
                objDB.FechaVencimiento = modelo.FechaVencimiento;
                objDB.FolioLibroAlmacen = modelo.FolioLibroAlmacen;
                objDB.PrecioUnidad = modelo.PrecioUnidad;
                objDB.ValorTotal = modelo.ValorTotal;

                _ingresoAlimentosRepository.UpdateProductosIngreso(objDB);

                response.Data = objDB;
                response.Message = "Productos Ingreso actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el producto de ingreso: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngresoWaybillEstado2(ProductosIngreso modelo)
        {
            var response = new ServiceResponse<ProductosIngreso>();
            ProductosIngreso objDB = await _ingresoAlimentosRepository.GetProductosIngreso(modelo.IdProductosIngreso);

            try
            {
                objDB.IdBoletaWaybill = modelo.IdBoletaWaybill;

                _ingresoAlimentosRepository.UpdateProductosIngreso(objDB);

                response.Data = objDB;
                response.Message = "Productos Ingreso actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el producto de ingreso: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngresoEstado2(ProductosIngreso modelo)
        {
            var response = new ServiceResponse<ProductosIngreso>();
            ProductosIngreso objDB = await _ingresoAlimentosRepository.GetProductosIngreso(modelo.IdProductosIngreso);

            try
            {
                objDB.Estiba = modelo.Estiba;
                objDB.FechaIngreso = modelo.FechaIngreso;
                objDB.FechaVencimiento = modelo.FechaVencimiento;

                _ingresoAlimentosRepository.UpdateProductosIngreso(objDB);

                response.Data = objDB;
                response.Message = "Productos Ingreso actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el producto de ingreso: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductosIngreso>> UpdateProductosIngresoEstado3(ProductosIngreso modelo)
        {
            var response = new ServiceResponse<ProductosIngreso>();
            ProductosIngreso objDB = await _ingresoAlimentosRepository.GetProductosIngreso(modelo.IdProductosIngreso);

            try
            {
                objDB.FolioLibroAlmacen = modelo.FolioLibroAlmacen; // Representa en la vista: No. Kardex
                objDB.PrecioUnidad = modelo.PrecioUnidad;
                objDB.ValorTotal = modelo.ValorTotal;

                _ingresoAlimentosRepository.UpdateProductosIngreso(objDB);

                response.Data = objDB;
                response.Message = "Productos Ingreso actualizado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el producto de ingreso: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<ProductosIngreso>> DeleteProductosIngreso(Int64 id)
        {
            var response = new ServiceResponse<ProductosIngreso>();
            ProductosIngreso objDB = await _ingresoAlimentosRepository.GetProductosIngreso(id);

            try
            {
                objDB.Estado = 1;

                _ingresoAlimentosRepository.UpdateProductosIngreso(objDB);

                response.Data = objDB;
                response.Message = "Productos de Ingresos eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar los productos de ingreso: {ex.Message}";
            }

            return response;
        }


        #endregion

        #region CRUD - DETALLE_WAYBILL

        /// <summary>
        /// Retorna un registro proveniente de INGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleWaybill>> GetDetalleWaybillDeIngreso(Int64 id)
        {
            List<DetalleWaybill> modelo = await _ingresoAlimentosRepository.GetDetalleWaybillDeIngreso(id);

            return modelo;
        }

        public async Task<DetalleWaybill> GetDetalleWaybill(Int64 id)
        {
            DetalleWaybill modelo = await _ingresoAlimentosRepository.GetDetalleWaybill(id);

            return modelo;
        }

        public async Task<ServiceResponse<DetalleWaybill>> SaveDetalleWaybill(DetalleWaybill modelo)
        {
            var response = new ServiceResponse<DetalleWaybill>();

            try
            {
                // Crear objeto
                DetalleWaybill objDB = new DetalleWaybill()
                {
                    IdDetalleWaybill = await _ingresoAlimentosRepository.CuentaDetalleWaybill(),
                    IdIngresoAlimentos = modelo.IdIngresoAlimentos,
                    NoBoleta = modelo.NoBoleta,
                    FechaBoleta = modelo.FechaBoleta,
                    Estado = 0
                };

                // Guardar la junta receptora en la base de datos
                var desc = await _ingresoAlimentosRepository.SaveDetalleWaybill(objDB);


                response.Data = objDB;
                response.Message = "Detalle Waybill creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el Detalle Waybill: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleWaybill>> DeleteDetalleWaybill(Int64 id)
        {
            var response = new ServiceResponse<DetalleWaybill>();
            DetalleWaybill objDB = await _ingresoAlimentosRepository.GetDetalleWaybill(id);

            try
            {
                objDB.Estado = 1;

                _ingresoAlimentosRepository.UpdateDetalleWaybill(objDB);

                response.Data = objDB;
                response.Message = "Detalle Waybill eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el Detalle Waybill: {ex.Message}";
            }

            return response;
        }


        #endregion

        #region CRUD - JUNTA_RECEPTORA
        /*
        /// <summary>
        /// Retorna un registro proveniente de INGRESO_ALIMENTOS
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<JuntaReceptora> GetJuntaReceptoraDeIngreso(Int64 id)
        {
            JuntaReceptora modelo = await _ingresoAlimentosRepository.GetJuntaReceptoraDeIngreso(id);

            return modelo;
        }

        public async Task<JuntaReceptora> GetJuntaReceptora(Int64 id)
        {
            JuntaReceptora modelo = await _ingresoAlimentosRepository.GetJuntaReceptora(id);

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
                    IdJuntaReceptora = await _ingresoAlimentosRepository.CuentaJuntaReceptora(),
                    IdIngresoAlimentos = modelo.IdIngresoAlimentos,
                    NumeroNombramiento = modelo.NumeroNombramiento,
                    ArchivoNombramiento = modelo.ArchivoNombramiento,
                    Estado = 0
                };

                // Guardar la junta receptora en la base de datos
                var desc = await _ingresoAlimentosRepository.SaveJuntaReceptora(objDB);


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
            JuntaReceptora objDB = await _ingresoAlimentosRepository.GetJuntaReceptora(modelo.IdJuntaReceptora);

            try
            {
                objDB.IdJuntaReceptora = modelo.IdJuntaReceptora;
                objDB.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDB.NumeroNombramiento = modelo.NumeroNombramiento;
                objDB.ArchivoNombramiento = modelo.ArchivoNombramiento ?? objDB.ArchivoNombramiento;

                var desc = await _ingresoAlimentosRepository.UpdateJuntaReceptora(objDB);

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
            JuntaReceptora objDB = await _ingresoAlimentosRepository.GetJuntaReceptora(modelo.IdJuntaReceptora);

            try
            {
                objDB.ArchivoNombramiento = modelo.ArchivoNombramiento;

                var desc = await _ingresoAlimentosRepository.UpdateJuntaReceptora(objDB);

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
            JuntaReceptora objDB = await _ingresoAlimentosRepository.GetJuntaReceptora(id);

            try
            {
                objDB.Estado = 1;

                _ingresoAlimentosRepository.UpdateJuntaReceptora(objDB);

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
        */

        #endregion

        #region CRUD - DETALLE_JUNTA_RECEPTORA
        /*
        /// <summary>
        /// Retorna listado de los detalles de junta receptora de un registro de JUNTA_RECEPTORA
        /// </summary>
        /// <param name="id">JUNTA_RECEPTORA_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleJuntaReceptora>> GetAllDetalleJuntaReceptora(Int64 id)
        {
            List<DetalleJuntaReceptora> listado = await _ingresoAlimentosRepository.GetAllDetalleJuntaReceptora(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de DETALLE_JUNTA_RECEPTORA
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<DetalleJuntaReceptora> GetDetalleJuntaReceptora(Int64 id)
        {
            DetalleJuntaReceptora modelo = await _ingresoAlimentosRepository.GetDetalleJuntaReceptora(id);

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
                    IdDetalleJuntaReceptora = await _ingresoAlimentosRepository.CuentaDetalleJuntaReceptora(),
                    IdJuntaReceptora = modelo.IdJuntaReceptora,
                    NombreCompleto = modelo.NombreCompleto,
                    DPI = modelo.DPI,
                    IdTipoNombramiento = modelo.IdTipoNombramiento,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                await _ingresoAlimentosRepository.SaveDetalleJuntaReceptora(objDB);


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
            DetalleJuntaReceptora objDB = await _ingresoAlimentosRepository.GetDetalleJuntaReceptora(modelo.IdDetalleJuntaReceptora);

            try
            {
                objDB.IdJuntaReceptora = modelo.IdJuntaReceptora;
                objDB.NombreCompleto = modelo.NombreCompleto;
                objDB.DPI = modelo.DPI;
                objDB.IdTipoNombramiento = modelo.IdTipoNombramiento;

                _ingresoAlimentosRepository.UpdateDetalleJuntaReceptora(objDB);

                response.Data = objDB;
                response.Message = "Detalle de junta receptora actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle de junta receptora: {ex.Message}";
            }

            return response;
        }*/

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

        /*
        public async Task<ServiceResponse<DetalleJuntaReceptora>> DeleteDetalleJuntaReceptora(Int64 id)
        {
            var response = new ServiceResponse<DetalleJuntaReceptora>();
            DetalleJuntaReceptora objDB = await _ingresoAlimentosRepository.GetDetalleJuntaReceptora(id);

            try
            {
                objDB.Estado = 1;

                _ingresoAlimentosRepository.UpdateDetalleJuntaReceptora(objDB);

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
        */

        #endregion

        #region CRUD - DETALLE_BOLETA_ANALISIS

        /// <summary>
        /// Retorna listado de los detalles de junta receptora de un registro de JUNTA_RECEPTORA
        /// </summary>
        /// <param name="id">INGRESO_ALIMENTOS_ID</param>
        /// <returns></returns>
        public async Task<List<DetalleBoletaAnalisis>>? GetAllDetalleBoletaAnalisis(Int64 id)
        {
            List<DetalleBoletaAnalisis>? listado = await _ingresoAlimentosRepository.GetAllDetalleBoletaAnalisis(id);

            return listado;
        }

        /// <summary>
        /// Retorna un registro de DETALLE_BOLETA_ANALISIS
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public async Task<DetalleBoletaAnalisis> GetDetalleBoletaAnalisis(Int64 id)
        {
            DetalleBoletaAnalisis modelo = await _ingresoAlimentosRepository.GetDetalleBoletaAnalisis(id);

            return modelo;
        }

        public async Task<ServiceResponse<DetalleBoletaAnalisis>> SaveDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo)
        {
            var response = new ServiceResponse<DetalleBoletaAnalisis>();

            try
            {
                // Crear objeto
                DetalleBoletaAnalisis objDB = new DetalleBoletaAnalisis()
                {
                    IdDetalleBoletaAnalisis = await _ingresoAlimentosRepository.CuentaDetalleBoletaAnalisis(),
                    IdIngresoAlimentos = modelo.IdIngresoAlimentos,
                    NoBoletaAnalisis = modelo.NoBoletaAnalisis,
                    Estado = 0
                };

                // Guardar el detalle en la base de datos
                await _ingresoAlimentosRepository.SaveDetalleBoletaAnalisis(objDB);


                response.Data = objDB;
                response.Message = "Detalle Boleta creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle de boleta: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleBoletaAnalisis>> UpdateDetalleBoletaAnalisis(DetalleBoletaAnalisis modelo)
        {
            var response = new ServiceResponse<DetalleBoletaAnalisis>();
            DetalleBoletaAnalisis objDB = await _ingresoAlimentosRepository.GetDetalleBoletaAnalisis(modelo.IdDetalleBoletaAnalisis);

            try
            {
                objDB.IdIngresoAlimentos = modelo.IdIngresoAlimentos;
                objDB.NoBoletaAnalisis = modelo.NoBoletaAnalisis;

                _ingresoAlimentosRepository.UpdateDetalleBoletaAnalisis(objDB);

                response.Data = objDB;
                response.Message = "Detalle de Boleta actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle de Boleta: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleBoletaAnalisis>> DeleteDetalleBoletaAnalisis(Int64 id)
        {
            var response = new ServiceResponse<DetalleBoletaAnalisis>();
            DetalleBoletaAnalisis objDB = await _ingresoAlimentosRepository.GetDetalleBoletaAnalisis(id);

            try
            {
                objDB.Estado = 1;

                _ingresoAlimentosRepository.UpdateDetalleBoletaAnalisis(objDB);

                response.Data = objDB;
                response.Message = "Detalle de Boleta eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle de Boleta: {ex.Message}";
            }

            return response;
        }

        #endregion

        #region Insert - MOVIMIENTOS_PRODUCTOS

        public async Task<ServiceResponse<int>> MoverProductosIngresoAMovimientos(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _ingresoAlimentosRepository.GetAllProductosIngreso(id);
            var ingresoAlimentos = await _ingresoAlimentosRepository.GetIngresoAlimentos(id);

            try
            {
                foreach (ProductosIngreso obj in objDB)
                {
                    // Se realiza la búsqueda por Bodega, ProductoAlimenticio, Presentacion y UnidadMedida;
                    // - si existe el registro se realiza un Update la existencia
                    // - al no existir, se realiza un Insert
                    MovimientosProductos objMPDB = new();
                    var objMov = await _ingresoAlimentosRepository.GetMovimientosProductos((int)ingresoAlimentos.IdBodegaRecibe, (int)obj.IdProductoAlimenticio, (int)obj.IdPresentacion, (int)obj.IdUnidadMedida);

                    // El registro nulo, hace insert
                    if (objMov == null)
                    {
                        // Crear objeto

                        objMPDB.IdMovimientosProductos = await _ingresoAlimentosRepository.CuentaMovimientosProductos();
                        objMPDB.IdBodega = ingresoAlimentos.IdBodegaRecibe;
                        objMPDB.IdProductoAlimenticio = obj.IdProductoAlimenticio;
                        objMPDB.IdPresentacion = obj.IdPresentacion;
                        objMPDB.Estiba = obj.Estiba;
                        objMPDB.IdConvenio = ingresoAlimentos.IdConvenio;
                        objMPDB.CantidadUnidad = obj.CantidadUnidad;
                        objMPDB.IdUnidadMedida = obj.IdUnidadMedida;
                        objMPDB.Existencias = obj.CantidadRecibir;
                        objMPDB.FechaVencimiento = obj.FechaVencimiento;
                        objMPDB.IdRegistro = obj.IdIngresoAlimentos;
                        objMPDB.Tabla = "PRODUCTOS_INGRESO";
                        objMPDB.Estado = 0;

                        await _ingresoAlimentosRepository.SaveMovimientosProductos(objMPDB);
                    }
                    else
                    {
                        // Al encontrar el registro, se actualiza la existencia.
                        var nuevaExistencia = objMov.Existencias + obj.CantidadRecibir;

                        objMov.Existencias = nuevaExistencia;

                        await _ingresoAlimentosRepository.UpdateMovimientosProductos(objMov);
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

        public async Task<ServiceResponse<int>> MoverProductosIngresoAKardex(Int64 id)
        {
            var response = new ServiceResponse<int>();
            var objDB = await _ingresoAlimentosRepository.GetAllProductosIngreso(id);
            var ingresoAlimentos = await _ingresoAlimentosRepository.GetIngresoAlimentos(id);

            try
            {
                foreach (ProductosIngreso obj in objDB)
                {
                    // Crear objeto
                    Kardex objKdx = new Kardex()
                    {
                        IdKardex = await _ingresoAlimentosRepository.CuentaKardex(),
                        FechaMovimiento = DateTime.Now,
                        IdBodega = ingresoAlimentos.IdBodegaRecibe,
                        IdProductoAlimenticio = obj.IdProductoAlimenticio,
                        IdPresentacion = obj.IdPresentacion,
                        Estiba = obj.Estiba,
                        IdConvenio = ingresoAlimentos.IdConvenio,
                        CantidadUnidad = obj.CantidadUnidad,
                        IdUnidadMedida = obj.IdUnidadMedida,
                        IdRegistro = (int)obj.IdProductosIngreso,
                        Tabla = "PRODUCTOS_INGRESO",
                        Ingreso = obj.CantidadRecibir,
                        Estado = 0
                    };

                    _ingresoAlimentosRepository.SaveKardex(objKdx);
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
            List<Bitacora> listado = await _ingresoAlimentosRepository.GetAllBitacora(id);

            return listado;
        }

        public async Task<List<Bitacora>> GetAllBitacoraEstado(int idEstado)
        {
            List<Bitacora> listado = await _ingresoAlimentosRepository.GetAllBitacoraEstado(idEstado);

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
                    IdBitacora = await _ingresoAlimentosRepository.CuentaBitacora(),
                    Accion = modelo.Accion,
                    IdRegistro = modelo.IdRegistro,
                    NombreTabla = modelo.NombreTabla,
                    IdUsuario = modelo.IdUsuario,
                    FechaHora = DateTime.Now,
                    Estado = 0
                };

                // Guardar la Bitacora en la base de datos
                _ingresoAlimentosRepository.SaveBitacora(objDB);


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

        #endregion

    }
}
