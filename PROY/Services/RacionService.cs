using PROY.Models;
using PROY.Repositories;

namespace PROY.Services
{
    public class RacionService : IRacionService
    {
        private readonly IRacionRepository _racionRepository;

        public RacionService(IRacionRepository racionRepository)
        {
            _racionRepository = racionRepository;
        }

        #region RACION_MAESTRO
        public async Task<List<RacionMaestro>> GetAllRacionMaestro()
        {
            List<RacionMaestro> listado = await _racionRepository.GetAllRacionMaestro();

            return listado;
        }

        public async Task<RacionMaestro> GetRacionMaestro(Int64 id)
        {
            RacionMaestro racion_encontrada = await _racionRepository.GetRacionMaestro(id);

            return racion_encontrada;
        }

        public async Task<ServiceResponse<RacionMaestro>> SaveRacionMaestro(RacionMaestro modelo)
        {
            var response = new ServiceResponse<RacionMaestro>();

            try
            {
                // Crear objeto
                RacionMaestro racion = new RacionMaestro()
                {
                    IdRacionMaestro = await _racionRepository.CuentaRacionMaestro(),
                    IdPrograma = modelo.IdPrograma,
                    IdConvenio = modelo.IdConvenio,
                    TotalKilocalorias = modelo.TotalKilocalorias,
                    TotalCarbohidratos = modelo.TotalCarbohidratos,
                    TotalGrasas = modelo.TotalGrasas,
                    TotalProteinas = modelo.TotalProteinas,
                    PorcentajeKilocalorias = modelo.PorcentajeKilocalorias,
                    PorcentajeCarbohidratos = modelo.PorcentajeCarbohidratos,
                    PorcentajeGrasas = modelo.PorcentajeGrasas,
                    PorcentajeProteinas = modelo.PorcentajeProteinas,
                    IdUsuario = modelo.IdUsuario,
                    Estado = 0
                };

                // Guardar la ración maestro en la base de datos
                _racionRepository.SaveRacionMaestro(racion);


                response.Data = racion;
                response.Message = "Racion creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<RacionMaestro>> UpdateRacionMaestro(RacionMaestro modelo)
        {
            var response = new ServiceResponse<RacionMaestro>();
            RacionMaestro racion_encontrada = await _racionRepository.GetRacionMaestro(modelo.IdRacionMaestro);

            try
            {
                racion_encontrada.IdRacionMaestro = modelo.IdRacionMaestro;
                racion_encontrada.IdPrograma = modelo.IdPrograma;
                racion_encontrada.IdConvenio = modelo.IdConvenio;
                racion_encontrada.TotalKilocalorias = modelo.TotalKilocalorias;
                racion_encontrada.TotalCarbohidratos = modelo.TotalCarbohidratos;
                racion_encontrada.TotalProteinas = modelo.TotalProteinas;
                racion_encontrada.TotalGrasas = modelo.TotalGrasas;
                racion_encontrada.PorcentajeKilocalorias = modelo.PorcentajeKilocalorias;
                racion_encontrada.PorcentajeCarbohidratos = modelo.PorcentajeCarbohidratos;
                racion_encontrada.PorcentajeProteinas = modelo.PorcentajeProteinas;
                racion_encontrada.PorcentajeGrasas = modelo.PorcentajeGrasas;
                racion_encontrada.IdUsuario = modelo.IdUsuario;

                _racionRepository.UpdateRacionMaestro(racion_encontrada);

                response.Data = racion_encontrada;
                response.Message = "Racion actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<RacionMaestro>> DeleteRacionMaestro(Int64 id)
        {
            var response = new ServiceResponse<RacionMaestro>();
            RacionMaestro racion_encontrada = await _racionRepository.GetRacionMaestro(id);

            try
            {
                racion_encontrada.Estado = 1;

                _racionRepository.UpdateRacionMaestro(racion_encontrada);

                response.Data = racion_encontrada;
                response.Message = "Ración eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la ración: {ex.Message}";
            }

            return response;
        }

        #endregion

        #region RACION_DETALLE

        public async Task<List<RacionDetalle>> GetAllRacionDetalle(Int64 id)
        {
            List<RacionDetalle> listado = await _racionRepository.GetAllRacionDetalle(id);

            return listado;
        }

        public async Task<RacionDetalle> GetRacionDetalle(Int64 id)
        {
            RacionDetalle racion_encontrada = await _racionRepository.GetRacionDetalle(id);

            return racion_encontrada;
        }

        public async Task<ServiceResponse<RacionDetalle>> SaveRacionDetalle(RacionDetalle modelo)
        {
            var response = new ServiceResponse<RacionDetalle>();

            try
            {
                // Crear objeto
                RacionDetalle racion = new RacionDetalle()
                {
                    IdRacionDetalle = await _racionRepository.CuentaRacionDetalle(),
                    IdRacionMaestro = modelo.IdRacionMaestro,
                    IdProductoAlimenticio = modelo.IdProductoAlimenticio,
                    CantidadMedida = modelo.CantidadMedida,
                    IdUnidadMedida = modelo.IdUnidadMedida,
                    Kilocalorias = modelo.Kilocalorias,
                    Carbohidratos = modelo.Carbohidratos,
                    Proteinas = modelo.Proteinas,
                    Grasas = modelo.Grasas,
                    Estado = 0
                };

                // Guardar la ración detalle en la base de datos
                _racionRepository.SaveRacionDetalle(racion);


                response.Data = racion;
                response.Message = "Racion creada con éxito";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<RacionDetalle>> UpdateRacionDetalle(RacionDetalle modelo)
        {
            var response = new ServiceResponse<RacionDetalle>();
            RacionDetalle racion_encontrada = await _racionRepository.GetRacionDetalle(modelo.IdRacionDetalle);

            try
            {
                racion_encontrada.IdRacionDetalle = modelo.IdRacionDetalle;
                racion_encontrada.IdRacionMaestro = modelo.IdRacionMaestro;
                racion_encontrada.IdProductoAlimenticio = modelo.IdProductoAlimenticio;
                racion_encontrada.CantidadMedida = modelo.CantidadMedida;
                racion_encontrada.IdUnidadMedida = modelo.IdUnidadMedida;
                racion_encontrada.Kilocalorias = modelo.Kilocalorias;
                racion_encontrada.Carbohidratos = modelo.Carbohidratos;
                racion_encontrada.Proteinas = modelo.Proteinas;
                racion_encontrada.Grasas = modelo.Grasas;

                _racionRepository.UpdateRacionDetalle(racion_encontrada);

                response.Data = racion_encontrada;
                response.Message = "Racion actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<RacionDetalle>> DeleteRacionDetalle(Int64 id)
        {
            var response = new ServiceResponse<RacionDetalle>();
            RacionDetalle racion_encontrada = await _racionRepository.GetRacionDetalle(id);

            try
            {
                racion_encontrada.Estado = 1;

                _racionRepository.UpdateRacionDetalle(racion_encontrada);

                response.Data = racion_encontrada;
                response.Message = "Ración eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la ración: {ex.Message}";
            }

            return response;
        }

        #endregion

        
    }
}
