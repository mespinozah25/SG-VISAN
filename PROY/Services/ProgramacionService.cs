using PROY.Models;
using PROY.Repositories;
using static PROY.Repositories.ProgramacionRepository;

namespace PROY.Services
{
    public class ProgramacionService : IProgramacionService
    {
        private readonly IProgramacionRepository _programacionRepository;
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ICatalogoRepository _catalogoRepository;
        public ProgramacionService(IProgramacionRepository programacionRepository, ISolicitudRepository solicitudRepository, ICatalogoRepository catalogoRepository)
        {
            _programacionRepository= programacionRepository;
            _solicitudRepository = solicitudRepository;
            _catalogoRepository = catalogoRepository;
        }

        public async Task<List<Solicitud>> GetAllProgramaciones()
        {
            List<Solicitud> listado = await _programacionRepository.GetAllProgramaciones();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllProgramacionesAPA()
        {
            List<Solicitud> listado = await _programacionRepository.GetAllProgramacionesAPA();

            return listado;
        }
        public async Task<List<DetalleSolicitud>> GetAllDetalleProgramacion(long idProgramacion)
        {
            List<DetalleSolicitud> lista_detalle_encontrado = await _programacionRepository.GetAllDetalleProgramacion(idProgramacion);

            return lista_detalle_encontrado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleProgramacionAPA(long idProgramacion)
        {
            List<DetalleSolicitudAPA> lista_detalle_encontrado = await _programacionRepository.GetAllDetalleProgramacionAPA(idProgramacion);

            return lista_detalle_encontrado;
        }


        public async Task<DetalleSolicitud> GetDetalleProgramacionID(long idDetalleProgramacion)
        {
            DetalleSolicitud detalle_encontrado = await _programacionRepository.GetDetalleProgramacionID(idDetalleProgramacion);

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleProgramacionIDAPA(long idDetalleProgramacion)
        {
            DetalleSolicitudAPA detalle_encontrado = await _programacionRepository.GetDetalleProgramacionIDAPA(idDetalleProgramacion);

            return detalle_encontrado;
        }

        public async Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.Estado = "EJECUCION";

                await _solicitudRepository.UpdateSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Solicitud enviada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al enviar la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<List<Agrupado>> GetDetalleSolicitudAgrupado(long idProgramacion)
        {
            var listado = await _programacionRepository.GetDetalleSolicitudAgrupado(idProgramacion);

            return listado;
        }

        public async Task<List<Catalogo>> GetComparar(long idProgramacion)
        {
            var lista1 = await _programacionRepository.GetDetalleSolicitudAgrupado(idProgramacion);

            var lista2 = await _catalogoRepository.GetCatalogo("DEPARTAMENTO");

            List<Catalogo> lista3 = new List<Catalogo>();
            //List<Catalogo> lista4 = new List<Catalogo>();

            foreach (var todos in lista2)
            {
                foreach(var grupo in lista1)
                {
                    if (grupo.Departamento == todos.IdCatalogo.ToString())
                    {
                        if (!lista3.Any(c => c.IdCatalogo == todos.IdCatalogo))
                        {
                            Catalogo nuevoCatalogo = new Catalogo
                            {
                                IdCatalogo = todos.IdCatalogo,
                                Descripcion = todos.Descripcion,
                                Tipo = todos.Tipo,
                                Orden = todos.Orden,
                                Restrictiva = todos.Restrictiva
                            };

                            lista3.Add(nuevoCatalogo);
                        }
                            
                    }
                }
            }

            return lista3;
        }
    }
}
