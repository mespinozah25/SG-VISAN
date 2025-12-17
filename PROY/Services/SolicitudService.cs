using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using PROY.Data;
using PROY.Helpers;
using PROY.Models;
using PROY.Repositories;
using System.Diagnostics;
using static PROY.Repositories.SolicitudRepository;
//using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PROY.Services
{
    public class SolicitudService : ISolicitudService
    {
        private readonly ISolicitudRepository _solicitudRepository;
        private readonly ICatalogoRepository _catalogoRepository;
        private readonly AppDBContext _dBContext;
        //private readonly IHttpContextAccessor _contextAccessor;

        public SolicitudService(ISolicitudRepository solicitudRepository, ICatalogoRepository catalogoRepository, AppDBContext dBContext)
        {
            _solicitudRepository = solicitudRepository;
            _catalogoRepository = catalogoRepository;
            //_contextAccessor = contextAccessor;
            _dBContext = dBContext;
        }

        public async Task<List<Solicitud>> GetAllSolicitudes()
        {
            List<Solicitud> listado = await _solicitudRepository.GetAllSolicitudes();

            return listado;
        }

        public async Task<List<Solicitud>> GetAllSolicitudesAPA()
        {
            List<Solicitud> listado = await _solicitudRepository.GetAllSolicitudesAPA();

            return listado;
        }

        public async Task<List<Solicitud>> GetSolicitudesProgramacion()
        {
            try
            {

                return await _dBContext.Solicitudes
                    .Where(s => s.Estado == "PROGRAMACION" && s.Restrictiva == 0 && s.EstadoEgreso == 0)
                    .Select(s => new Solicitud
                    {
                        IdSolicitud = s.IdSolicitud,
                        Descripcion = s.Correlativo,
                        FechaIngresoDespacho = s.FechaIngresoDespacho,
                        Depto = s.Depto,
                        IdIntervencion = s.IdIntervencion,
                        Raciones = s.Raciones
                    })
                    .OrderByDescending(s => s.FechaIngresoDespacho)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("No se pudieron obtener las solicitudes en programación", ex);
            }
        }

        public async Task<Solicitud> GetSolicitudByIdAsync(long id)
        {
            return await _dBContext.Solicitudes
                .FirstOrDefaultAsync(s => s.IdSolicitud == id);
        }

        public async Task ActualizarEstadoEgreso(long idSolicitud, int nuevoEstado)
        {
            var solicitud = await _dBContext.Solicitudes.FindAsync(idSolicitud);
            if (solicitud != null)
            {
                solicitud.EstadoEgreso = nuevoEstado;
                await _dBContext.SaveChangesAsync();
            }
        }

        //-----------------------------------------------------------------
        /*public JsonResult GetComunidades(Int64 id)
        {
            var soli = _solicitudRepository.GetAllDetalleSolicitud(id);

            var agrupado = soli.Result
                .GroupBy(s => new { s.Comunidad }) // Agrupar por Departamento y Municipio
                .Select(g => new
                {
                    //Departamento = g.Key.Departamento,
                    //Municipio = g.Key.Municipio,
                    //Comunidad = g.First().Comunidad, // Puedes ajustar según necesidad
                    //Beneficiarios = g.Sum(s => s.Beneficiarios), // Sumar Beneficiarios
                    //Racion = g.Sum(s => s.Racion) // Sumar Racion
                    Comunidad = g.Key.Comunidad
                }).ToList();

            return new JsonResult(agrupado);
        }*/

        public List<SelectListItem> GetComunidades(Int64 id)
        {
            //var soli = _solicitudService.GetComunidades(id);
            var soli = _solicitudRepository.GetAllDetalleSolicitud(id);

            var agrupado = soli.Result
                .GroupBy(s => new { s.Comunidad })
                .Select(g => new SelectListItem
                {
                    //Value = $"{g.Key.Departamento}-{g.Key.Municipio}", // ID único combinando depto y municipio
                    //Text = $"{g.Key.Departamento} - {g.Key.Municipio}" // Nombre que se mostrará en el select
                    Value = g.Key.Comunidad,
                    Text = g.Key.Comunidad
                }).ToList();

            return agrupado;
        }

        public List<SelectListItem> GetComunidadesAPA(Int64 id)
        {
            //var soli = _solicitudService.GetComunidades(id);
            //var soli = _solicitudRepository.GetAllDetalleSolicitudAPA(id);
            var soli = _solicitudRepository.GetSolicitud(id);

            /*var agrupado = soli.Result
                .GroupBy(s => new { s.Comunidad })
                .Select(g => new SelectListItem
                {
                    //Value = $"{g.Key.Departamento}-{g.Key.Municipio}", // ID único combinando depto y municipio
                    //Text = $"{g.Key.Departamento} - {g.Key.Municipio}" // Nombre que se mostrará en el select
                    Value = g.Key.Comunidad,
                    Text = g.Key.Comunidad
                }).ToList();*/

            var agrupado = new List<SelectListItem>
            {
                new SelectListItem { Value = soli.Result.Comunidad, Text = soli.Result.Comunidad }
            };

            return agrupado;
        }

        //-----------------------------------------------------------------

        public async Task<List<DetalleSolicitud>> GetAllDetalleSolicitud(long id)
        {
            List<DetalleSolicitud> listado = await _solicitudRepository.GetAllDetalleSolicitud(id);

            return listado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudAPA(long id)
        {
            List<DetalleSolicitudAPA> listado = await _solicitudRepository.GetAllDetalleSolicitudAPA(id);

            return listado;
        }

        public async Task<List<DetalleSolicitudAPA>> GetAllDetalleSolicitudAPACodigo(long id)
        {
            List<DetalleSolicitudAPA> listado = await _solicitudRepository.GetAllDetalleSolicitudAPACodigo(id);

            return listado;
        }

        public async Task<Solicitud> GetSolicitud(long id)
        {
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(id);

            return solicitud_encontrada;
        }

        public async Task<DetalleSolicitud> GetDetalleSolicitud(long id)
        {
            //Busca por id de solicitud
            DetalleSolicitud detalle_encontrado = await _solicitudRepository.GetDetalleSolicitud(id);

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitud> GetDetalleSolicitudByCUI(long id, string paramCUI)
        {
            //Busca por id de solicitud
            DetalleSolicitud detalle_encontrado = await _solicitudRepository.GetDetalleSolicitudByCUI(id, paramCUI);

            return detalle_encontrado;
        }

        public async Task<DetalleSolicitudAPA> GetDetalleSolicitudByCUIAPA(long id, string paramCUI)
        {
            //Busca por CUI y id de solicitud
            DetalleSolicitudAPA detalle_encontrado = await _solicitudRepository.GetDetalleSolicitudByCUIAPA(id, paramCUI);

            return detalle_encontrado;
        }
        public async Task<DetalleSolicitudAPA> GetDetalleSolicitudAPA(long id)
        {
            //Busca por id de solicitud
            DetalleSolicitudAPA detalle_encontrado = await _solicitudRepository.GetDetalleSolicitudAPA(id);

            return detalle_encontrado;
        }

        public async Task<ServiceResponse<Solicitud>> SaveSolicitud(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();

            try
            {
                // Crear objeto Usuario
                Solicitud solicitud = new Solicitud()
                {
                    IdSolicitud = await _solicitudRepository.CuentaSolicitud(),
                    Correlativo = modelo.Correlativo,
                    //Codigo = DateTime.Now.Year + "-" + modelo.Correlativo,
                    Depto = modelo.Depto,
                    IdInstitucion = modelo.IdInstitucion,
                    FechaIngresoDespacho = Convert.ToDateTime(modelo.FechaIngresoDespacho).Date,
                    FechaIngresoDireccion = Convert.ToDateTime(modelo.FechaIngresoDireccion).Date,
                    FechaIngresoUnidad = Convert.ToDateTime(modelo.FechaIngresoUnidad).Date,
                    IdUnidad = modelo.IdUnidad,
                    IdIntervencion = modelo.IdIntervencion,
                    Archivo = modelo.Archivo,
                    Descripcion = modelo.Descripcion,
                    Raciones = modelo.Raciones,
                    Restrictiva = 0,
                    Fallecidos = null,
                    Duplicados = null,
                    SinDatos = null,
                    Gobierno = null,
                    FInicioEntrega = null,
                    FFinEntrega = null,
                    TecnicoObservaciones = null,
                    Estado = "CREADO",
                    FechaRegistro = modelo.FechaRegistro,
                    UsuarioRegistro = modelo.UsuarioRegistro,
                    IdProyecto = modelo.Depto == "2" ? modelo.IdProyecto : null,
                    Departamento = modelo.Depto == "2" ? modelo.Departamento : null,
                    Municipio = modelo.Depto == "2" ? modelo.Municipio : null,
                    Comunidad = modelo.Depto == "2" ? modelo.Comunidad : null,
                    NombreCocode = modelo.Depto == "2" ? modelo.NombreCocode : null,
                    ComunidadCocode = modelo.Depto == "2" ? modelo.ComunidadCocode : null,
                    TelefonoCocode = modelo.Depto == "2" ? modelo.TelefonoCocode : null,
                };

                // Guardar solicitud en la base de datos
                await _solicitudRepository.SaveSolicitud(solicitud);

                response.Data = solicitud;
                response.Success = true;
                response.Message = "Solicitud creada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitud>> SaveDetalleSolicitud(DetalleSolicitud modelo, string proceso)
        {
            var response = new ServiceResponse<DetalleSolicitud>();
            //var depto = _contextAccessor.HttpContext.User.FindFirst("depto")?.Value;
            try
            {
                // Crear objeto Usuario
                DetalleSolicitud detSolicitud = new DetalleSolicitud()
                {
                    IdDetalleSolicitud = await _solicitudRepository.CuentaDetalle(),
                    IdSolicitud = modelo.IdSolicitud,
                    //IdDetalleSolicitud=modelo.IdDetalleSolicitud,
                    //Correlativo = modelo.Correlativo,
                    //Codigo = DateTime.Now.Year + "-" + modelo.Correlativo,
                    ///IdIntervencion = modelo.IdIntervencion,
                    IdDepartamento = modelo.IdDepartamento,
                    IdMunicipio = modelo.IdMunicipio,
                    Comunidad = modelo.Comunidad,
                    Nombre1Esposo = modelo.Nombre1Esposo,
                    Nombre2Esposo = modelo.Nombre2Esposo,
                    Nombre3Esposo = modelo.Nombre3Esposo,
                    Apellido1Esposo = modelo.Apellido1Esposo,
                    Apellido2Esposo = modelo.Apellido2Esposo,
                    CuiEsposo = modelo.CuiEsposo,
                    FNacimientoEsposo = modelo.FNacimientoEsposo,
                    Nombre1Esposa = modelo.Nombre1Esposa,
                    Nombre2Esposa = modelo.Nombre2Esposa,
                    Nombre3Esposa = modelo.Nombre3Esposa,
                    Apellido1Esposa = modelo.Apellido1Esposa,
                    Apellido2Esposa = modelo.Apellido2Esposa,
                    ApellidoCasada = modelo.ApellidoCasada,
                    CuiEsposa = modelo.CuiEsposa,
                    FNacimientoEsposa = modelo.FNacimientoEsposa,
                    EstadoCivil = modelo.EstadoCivil,
                    Embarazada = modelo.Embarazada,
                    NiñoDA = modelo.NiñoDA,
                    GrupoEtnico = modelo.GrupoEtnico,
                    Hombre0a2Años = modelo.Hombre0a2Años,
                    Mujer0a2Años = modelo.Mujer0a2Años,
                    Hombre2a5Años = modelo.Hombre2a5Años,
                    Mujer2a5Años = modelo.Mujer2a5Años,
                    Hombre6a17Años = modelo.Hombre6a17Años,
                    Mujer6a17Años = modelo.Mujer6a17Años,
                    TipoPersona = modelo.TipoPersona,
                    AreaMaiz = modelo.AreaMaiz,
                    QuintalesMaiz = modelo.QuintalesMaiz,
                    AreaFrijol = modelo.AreaFrijol,
                    QuintalesFrijol = modelo.QuintalesFrijol,
                    TipoAgriculturaPractica = modelo.TipoAgriculturaPractica,
                    NombreCOCODE = modelo.NombreCOCODE,
                    NombrePersonaRegistro = modelo.NombrePersonaRegistro,
                    InstitucionRepresenta = modelo.InstitucionRepresenta,
                    //CantidadBeneficiarios = modelo.CantidadBeneficiarios,
                    //Archivo = modelo.Archivo,
                    Restrictiva = 0,
                    Estado = "CREADO"
                };

                if (proceso == "Programacion")
                {
                    if (detSolicitud.IdAnulacion != null && detSolicitud.IdAnulacion != 0)
                        detSolicitud.Proceso = "PROGRAMACION";
                }
                else if (proceso == "Evaluacion")
                {
                    if (detSolicitud.IdAnulacion != null && detSolicitud.IdAnulacion != 0)
                        detSolicitud.Proceso = "EVALUACION";
                }



                // Guardar solicitud en la base de datos
                await _solicitudRepository.SaveDetalleSolicitud(detSolicitud);

                response.Data = detSolicitud;
                response.Success = true;
                response.Message = "Detalle Solicitud creado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitudAPA>> SaveDetalleSolicitudAPA(DetalleSolicitudAPA modelo)
        {
            var response = new ServiceResponse<DetalleSolicitudAPA>();

            try
            {
                // Crear objeto Usuario
                DetalleSolicitudAPA detSolicitud = new DetalleSolicitudAPA()
                {
                    IdDetalleSolicitud = await _solicitudRepository.CuentaDetalleAPA(),
                    IdSolicitud = modelo.IdSolicitud,
                    //IdDetalleSolicitud=modelo.IdDetalleSolicitud,
                    //Correlativo = modelo.Correlativo,
                    //Codigo = DateTime.Now.Year + "-" + modelo.Correlativo,
                    ///IdIntervencion = modelo.IdIntervencion,
                    ////IdDepartamento = modelo.IdDepartamento,
                    ////IdMunicipio = modelo.IdMunicipio,
                    ////Comunidad = modelo.Comunidad,
                    ////Nombre1Esposo = modelo.Nombre1Esposo,
                    ////Nombre2Esposo = modelo.Nombre2Esposo,
                    ////Nombre3Esposo = modelo.Nombre3Esposo,
                    ////Apellido1Esposo = modelo.Apellido1Esposo,
                    ////Apellido2Esposo = modelo.Apellido2Esposo,
                    ////CuiEsposo = modelo.CuiEsposo,
                    ////FNacimientoEsposo = modelo.FNacimientoEsposo,
                    Nombre1Esposa = modelo.Nombre1Esposa,
                    Nombre2Esposa = modelo.Nombre2Esposa,
                    Nombre3Esposa = modelo.Nombre3Esposa,
                    Apellido1Esposa = modelo.Apellido1Esposa,
                    Apellido2Esposa = modelo.Apellido2Esposa,
                    ApellidoCasada = modelo.ApellidoCasada,
                    CuiEsposa = modelo.CuiEsposa,
                    ////FNacimientoEsposa = modelo.FNacimientoEsposa,
                    ////EstadoCivil = modelo.EstadoCivil,
                    ////Embarazada = modelo.Embarazada,
                    ////NiñoDA = modelo.NiñoDA,
                    GrupoEtnico = modelo.GrupoEtnico,
                    Genero = modelo.Genero,
                    ////Hombre0a2Años = modelo.Hombre0a2Años,
                    ////Mujer0a2Años = modelo.Mujer0a2Años,
                    ////Hombre2a5Años = modelo.Hombre2a5Años,
                    ////Mujer2a5Años = modelo.Mujer2a5Años,
                    ////Hombre6a17Años = modelo.Hombre6a17Años,
                    ////Mujer6a17Años = modelo.Mujer6a17Años,
                    ////TipoPersona = modelo.TipoPersona,
                    ////AreaMaiz = modelo.AreaMaiz,
                    ////QuintalesMaiz = modelo.QuintalesMaiz,
                    ////AreaFrijol = modelo.AreaFrijol,
                    ////QuintalesFrijol = modelo.QuintalesFrijol,
                    ////TipoAgriculturaPractica = modelo.TipoAgriculturaPractica,
                    ////NombreCOCODE = modelo.NombreCOCODE,
                    ////NombrePersonaRegistro = modelo.NombrePersonaRegistro,
                    ////InstitucionRepresenta = modelo.InstitucionRepresenta,
                    Restrictiva = 0,
                    Estado = "CREADO"
                };


                // Guardar solicitud en la base de datos
                await _solicitudRepository.SaveDetalleSolicitudAPA(detSolicitud);

                response.Data = detSolicitud;
                response.Success = true;
                response.Message = "Detalle Solicitud creado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitud>> SaveListaDetalleSolicitud(List<Models.ExcelRow> datos)
        {
            var response = new ServiceResponse<DetalleSolicitud>();

            //DetalleSolicitud detSolicitud;

            try
            {
                foreach (var dato in datos)
                {
                    //var departamentoID = await _catalogoRepository.GetDepartamento(dato.Columna1);
                    // Crear objeto Usuario
                    DetalleSolicitud detSolicitud = new DetalleSolicitud()
                    {
                        IdDetalleSolicitud = await _solicitudRepository.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1), //modelo.IdSolicitud,
                        //IdDetalleSolicitud=modelo.IdDetalleSolicitud,
                        //Correlativo = modelo.Correlativo,
                        //Codigo = DateTime.Now.Year + "-" + modelo.Correlativo,
                        ///IdIntervencion = modelo.IdIntervencion,
                        IdDepartamento = await _catalogoRepository.GetDepartamento(dato.Columna2),
                        IdMunicipio = await _catalogoRepository.GetMunicipio(dato.Columna2, dato.Columna3),
                        Comunidad = dato.Columna4,

                        Nombre1Esposo = dato.Columna5,
                        Nombre2Esposo = dato.Columna6,
                        Nombre3Esposo = dato.Columna7,
                        Apellido1Esposo = dato.Columna8,
                        Apellido2Esposo = dato.Columna9,
                        CuiEsposo = dato.Columna10,
                        FNacimientoEsposo = DateTime.TryParse(dato.Columna11, out DateTime fechaNacimientoEsposo) ? fechaNacimientoEsposo.Date : (DateTime?)null,
                        Nombre1Esposa = dato.Columna12,
                        Nombre2Esposa = dato.Columna13,
                        Nombre3Esposa = dato.Columna14,
                        Apellido1Esposa = dato.Columna15,
                        Apellido2Esposa = dato.Columna16,
                        ApellidoCasada = dato.Columna17,
                        CuiEsposa = dato.Columna18,
                        FNacimientoEsposa = DateTime.TryParse(dato.Columna19, out DateTime fechaNacimientoEsposa) ? fechaNacimientoEsposa.Date : (DateTime?)null,
                        EstadoCivil = await _catalogoRepository.GetItemCatalogo(dato.Columna20, "ESTADO CIVIL"),//Int16.TryParse(dato.Columna20, out Int16 estadocivil) ? estadocivil: 0,
                        //EstadoCivil = Int16.TryParse(dato.Columna20, out Int16 estadocivil) ? estadocivil : 0,
                        Embarazada = dato.Columna21,
                        NiñoDA = dato.Columna22,
                        GrupoEtnico = await _catalogoRepository.GetItemCatalogo(dato.Columna23, "GRUPO ETNICO"),
                        //GrupoEtnico = Int16.TryParse(dato.Columna23, out Int16 grupoetnico) ? grupoetnico : 0,
                        Hombre0a2Años = Int16.TryParse(dato.Columna24, out Int16 hombre0a2años) ? hombre0a2años : 0,
                        Mujer0a2Años = Int16.TryParse(dato.Columna25, out Int16 mujer0a2años) ? mujer0a2años : 0,
                        Hombre2a5Años = Int16.TryParse(dato.Columna26, out Int16 hombre2a5años) ? hombre2a5años : 0,
                        Mujer2a5Años = Int16.TryParse(dato.Columna27, out Int16 mujer2a5años) ? mujer2a5años : 0,
                        Hombre6a17Años = Int16.TryParse(dato.Columna28, out Int16 hombre6a17años) ? hombre6a17años : 0,
                        Mujer6a17Años = Int16.TryParse(dato.Columna29, out Int16 mujer6a17años) ? mujer6a17años : 0,
                        TipoPersona = Int16.TryParse(dato.Columna30, out Int16 tipopersona) ? tipopersona : 0,
                        AreaMaiz = Decimal.TryParse(dato.Columna31, out Decimal areamaiz) ? areamaiz : 0,
                        QuintalesMaiz = Decimal.TryParse(dato.Columna32, out Decimal quintalesmaiz) ? quintalesmaiz : 0,
                        AreaFrijol = Decimal.TryParse(dato.Columna33, out Decimal areafrijol) ? areafrijol : 0,
                        QuintalesFrijol = Decimal.TryParse(dato.Columna34, out Decimal quintalesfrijol) ? quintalesfrijol : 0,
                        TipoAgriculturaPractica = Int16.TryParse(dato.Columna35, out Int16 tipoagriculturapractica) ? tipoagriculturapractica : 0,
                        NombreCOCODE = dato.Columna36,
                        NombrePersonaRegistro = dato.Columna37,
                        InstitucionRepresenta = dato.Columna38,
                        //CantidadBeneficiarios = Convert.ToInt32(dato.Columna5), //dato.CantidadBeneficiarios,
                        //Archivo = modelo.Archivo,
                        Restrictiva = 0,
                        Estado = "CREADO"
                    };

                    await _solicitudRepository.SaveDetalleSolicitud(detSolicitud);

                    response.Data = detSolicitud;
                    response.Success = true;
                    response.Message = "Detalle Solicitud creado exitosamente";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitud>> SaveListaDetalleSolicitudxIntervencion(List<ExcelRowDinamic> datos,int IdInteraccion)
        {
            var response = new ServiceResponse<DetalleSolicitud>();

            //DetalleSolicitud detSolicitud;

            try
            {

                foreach (var dato in datos)
                {
                    var detSolicitud = await MapToDetalleSolicitud(dato, IdInteraccion, _solicitudRepository, _catalogoRepository);


                    await _solicitudRepository.SaveDetalleSolicitud(detSolicitud);
                    
                    response.Data = detSolicitud;
                    response.Success = true;
                    response.Message = "Detalle Solicitud creado exitosamente";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public static async Task<DetalleSolicitud> MapToDetalleSolicitud(ExcelRowDinamic dato, int idIntervencion, ISolicitudRepository repo, ICatalogoRepository catalogo)
        {
            DetalleSolicitud detalle;
            List<Nino> listaNinos = new List<Nino>();

            switch (idIntervencion)
            {
                case 21:
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),
                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna2),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna2, dato.Columna3),
                        Comunidad = dato.Columna4,
                        Nombre1Esposo = dato.Columna5,
                        Nombre2Esposo = dato.Columna6,
                        Nombre3Esposo = dato.Columna7,
                        Apellido1Esposo = dato.Columna8,
                        Apellido2Esposo = dato.Columna9,
                        CuiEsposo = dato.Columna10,
                        FNacimientoEsposo = DateTime.TryParse(dato.Columna11, out var fne) ? fne : (DateTime?)null,
                        Nombre1Esposa = dato.Columna12,
                        Nombre2Esposa = dato.Columna13,
                        Nombre3Esposa = dato.Columna14,
                        Apellido1Esposa = dato.Columna15,
                        Apellido2Esposa = dato.Columna16,
                        ApellidoCasada = dato.Columna17,
                        CuiEsposa = dato.Columna18,
                        FNacimientoEsposa = DateTime.TryParse(dato.Columna19, out var fna) ? fna : (DateTime?)null,
                        EstadoCivil = await catalogo.GetItemCatalogo(dato.Columna20, "ESTADO CIVIL"),
                        Embarazada = dato.Columna21,
                        NiñoDA = dato.Columna22,
                        GrupoEtnico = await catalogo.GetItemCatalogo(dato.Columna23, "GRUPO ETNICO"),
                        Hombre0a2Años = short.TryParse(dato.Columna24, out var h02) ? h02 : 0,
                        Mujer0a2Años = short.TryParse(dato.Columna25, out var m02) ? m02 : 0,
                        Hombre2a5Años = short.TryParse(dato.Columna26, out var h25) ? h25 : 0,
                        Mujer2a5Años = short.TryParse(dato.Columna27, out var m25) ? m25 : 0,
                        Hombre6a17Años = short.TryParse(dato.Columna28, out var h617) ? h617 : 0,
                        Mujer6a17Años = short.TryParse(dato.Columna29, out var m617) ? m617 : 0,
                        TipoPersona = short.TryParse(dato.Columna30, out var tipo) ? tipo : 0,
                        AreaMaiz = decimal.TryParse(dato.Columna31, out var am) ? am : 0,
                        QuintalesMaiz = decimal.TryParse(dato.Columna32, out var qm) ? qm : 0,
                        AreaFrijol = decimal.TryParse(dato.Columna33, out var af) ? af : 0,
                        QuintalesFrijol = decimal.TryParse(dato.Columna34, out var qf) ? qf : 0,
                        TipoAgriculturaPractica = short.TryParse(dato.Columna35, out var tap) ? tap : 0,
                        NombreCOCODE = dato.Columna36,
                        NombrePersonaRegistro = dato.Columna37,
                        InstitucionRepresenta = dato.Columna38,
                        Estado = "CREADO",
                        Restrictiva = 0
                    };
                    break;

                case 3:
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),
                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna2),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna2, dato.Columna3),
                        Comunidad = dato.Columna4,
                        UbicacionAlbergue = dato.Columna5,
                        Nombre1Esposo = dato.Columna6,
                        Nombre2Esposo = dato.Columna7,
                        Nombre3Esposo = dato.Columna8,
                        Apellido1Esposo = dato.Columna9,
                        Apellido2Esposo = dato.Columna10,
                        CuiEsposo = dato.Columna11,
                        FNacimientoEsposo = DateTime.TryParse(dato.Columna12, out var fne2) ? fne2 : (DateTime?)null,
                        Nombre1Esposa = dato.Columna13,
                        Nombre2Esposa = dato.Columna14,
                        Nombre3Esposa = dato.Columna15,
                        Apellido1Esposa = dato.Columna16,
                        Apellido2Esposa = dato.Columna17,
                        ApellidoCasada = dato.Columna18,
                        CuiEsposa = dato.Columna19,
                        FNacimientoEsposa = DateTime.TryParse(dato.Columna20, out var fna2) ? fna2 : (DateTime?)null,
                        GrupoEtnico = await catalogo.GetItemCatalogo(dato.Columna21, "GRUPO ETNICO"),
                        EstadoFamilia = dato.Columna22,
                        CantPersonasHogar = short.TryParse(dato.Columna23, out var cph) ? cph : 0,
                        CantHombres = short.TryParse(dato.Columna24, out var ch) ? ch : 0,
                        CantMujeres = short.TryParse(dato.Columna25, out var cm) ? cm : 0,
                        DañoFamiliar = dato.Columna26,
                        DañoVivienda = dato.Columna27,
                        PerdidaCultivo = dato.Columna28,
                        PerdidaAnimalesCrianza = dato.Columna29,
                        NombrePersonaRegistro = dato.Columna30,
                        InstitucionRepresenta = dato.Columna31,
                        Estado = "CREADO",
                        Restrictiva = 0
                    };
                    break;

                case 22:
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),
                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna2),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna2, dato.Columna3),
                        Comunidad = dato.Columna4,
                        CodComunidad = short.TryParse(dato.Columna5, out var cc) ? cc : 0,

                        // Familia
                        Nombre1Persona1 = dato.Columna6,
                        Nombre2Persona1 = dato.Columna7,
                        Nombre3Persona1 = dato.Columna8,
                        Apellido1Persona1 = dato.Columna9,
                        Apellido2Persona1 = dato.Columna10,
                        ApellidoCasadaPersona1 = dato.Columna11,
                        CuiPersona1 = dato.Columna12,
                        FNacimientoPersona1 = DateTime.TryParse(dato.Columna13, out var fnf) ? fnf : (DateTime?)null,

                        // Jefe de hogar
                        Parentesco = dato.Columna14,
                        Nombre1Persona2 = dato.Columna15,
                        Nombre2Persona2 = dato.Columna16,
                        Nombre3Persona2 = dato.Columna17,
                        Apellido1Persona2 = dato.Columna18,
                        Apellido2Persona2 = dato.Columna19,
                        ApellidoCasadaPersona2 = dato.Columna20,
                        CuiPersona2 = dato.Columna21,
                        FNacimientoPersona2 = DateTime.TryParse(dato.Columna22, out var fnjh) ? fnjh : (DateTime?)null,

                        // Niño
                        CodigoNiño = dato.Columna23,
                        Nombre1Niño = dato.Columna24,
                        Nombre2Niño = dato.Columna25,
                        Nombre3Niño = dato.Columna26,
                        Apellido1Niño = dato.Columna27,
                        Apellido2Niño = dato.Columna28,
                        CuiNiño = dato.Columna29,
                        Sexo = dato.Columna30,
                        FNacimientoNiño = DateTime.TryParse(dato.Columna31, out var fnn) ? fnn : (DateTime?)null,
                        EdadNiño = short.TryParse(dato.Columna32, out var edadn) ? edadn : (short?)null,
                        GrupoEtnico = await catalogo.GetItemCatalogo(dato.Columna33, "GRUPO ETNICO"),
                        Diagnostico = dato.Columna34,
                        Peso = decimal.TryParse(dato.Columna35, out var peso) ? peso : (decimal?)null,
                        Talla = decimal.TryParse(dato.Columna36, out var talla) ? talla : (decimal?)null,

                        Estado = "CREADO",
                        Restrictiva = 0
                    };
                    break;

                case 6:
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),
                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna2),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna2, dato.Columna3),
                        Comunidad = dato.Columna4,

                        Nombre1Esposo = dato.Columna5,
                        Nombre2Esposo = dato.Columna6,
                        Nombre3Esposo = dato.Columna7,
                        Apellido1Esposo = dato.Columna8,
                        Apellido2Esposo = dato.Columna9,
                        CuiEsposo = dato.Columna10,
                        FNacimientoEsposo = DateTime.TryParse(dato.Columna11, out var fn) ? fn : (DateTime?)null,

                        Nombre1Esposa = dato.Columna12,
                        Nombre2Esposa = dato.Columna13,
                        Nombre3Esposa = dato.Columna14,
                        Apellido1Esposa = dato.Columna15,
                        Apellido2Esposa = dato.Columna16,
                        ApellidoCasada = dato.Columna17,
                        CuiEsposa = dato.Columna18,
                        FNacimientoEsposa = DateTime.TryParse(dato.Columna19, out var fnsa) ? fnsa : (DateTime?)null,

                        Nombre1Niño = dato.Columna20,
                        Nombre2Niño = dato.Columna21,
                        Nombre3Niño = dato.Columna22,
                        Apellido1Niño = dato.Columna23,
                        Apellido2Niño = dato.Columna24,
                        CuiNiño = dato.Columna25,
                        FNacimientoNiño = DateTime.TryParse(dato.Columna26, out var fnno) ? fnno : (DateTime?)null,

                        GrupoEtnico = await catalogo.GetItemCatalogo(dato.Columna27, "GRUPO ETNICO"),
                        EstadoFamilia = dato.Columna28,
                        CantHombres = short.TryParse(dato.Columna29, out var chs) ? chs : 0,
                        CantMujeres = short.TryParse(dato.Columna30, out var cms) ? cms : 0,
                        CantNiños = short.TryParse(dato.Columna31, out var cn) ? cn : 0,
                        CantNiñas = short.TryParse(dato.Columna32, out var cna) ? cna : 0,

                        NombrePersonaRegistro = dato.Columna33,
                        InstitucionRepresenta = dato.Columna34,

                        Estado = "CREADO",
                        Restrictiva = 0
                    };
                    break;

                case 8:
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),

                        NombreCompletoPersona1 = dato.Columna2,
                        CuiPersona1 = dato.Columna3,
                        EdadPersona1 = short.TryParse(dato.Columna4, out var ep) ? ep : 0,
                        GeneroPersona1 = dato.Columna5,

                        NombreCompletoPersona2 = dato.Columna6,
                        CuiPersona2 = dato.Columna7,
                        EdadPersona2 = short.TryParse(dato.Columna8, out var ep2) ? ep2 : 0,
                        
                        GeneroPersona2 = dato.Columna9,

                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna10),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna10, dato.Columna11),
                        Comunidad = dato.Columna12,

                        Estado = "CREADO",
                        Restrictiva = 0
                    };
                    break;

                case 4:
                    // Crear la solicitud primero
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),
                        NombreCompletoPersona1 = dato.Columna2,
                        CuiPersona1 = dato.Columna3,
                        EdadPersona1 = short.TryParse(dato.Columna4, out var edad) ? edad : (short)0,
                        // Al final se insertan estas (aunque estén después en el Excel)
                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna35),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna35, dato.Columna36),
                        Comunidad = dato.Columna37,
                        Estado = "CREADO",
                        Restrictiva = 0
                    };



                    for (int i = 0; i < 10; i++) // de Niño1 a Niño10
                    {
                        int baseCol = 5 + i * 3; // Comienza en Columna5 (nombre niño 1)

                        string nombre = dato.GetType().GetProperty($"Columna{baseCol}")?.GetValue(dato)?.ToString();
                        string cui = dato.GetType().GetProperty($"Columna{baseCol + 1}")?.GetValue(dato)?.ToString();
                        string edadStr = dato.GetType().GetProperty($"Columna{baseCol + 2}")?.GetValue(dato)?.ToString();
                        int? edadniño = int.TryParse(edadStr, out var edadFinal) ? edadFinal : (int?)null;

                        if (!string.IsNullOrWhiteSpace(nombre))
                        {
                            listaNinos.Add(new Nino
                            {
                                IdDetalleSolicitud = detalle.IdDetalleSolicitud,
                                NombreCompleto = nombre,
                                CUI = cui,
                                Edad = edadniño
                            });
                        }
                    }

                    detalle.Ninos = listaNinos;

                    break;

                case 5:
                    detalle = new DetalleSolicitud
                    {
                        IdDetalleSolicitud = await repo.CuentaDetalle(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1),

                        // Ubicación
                        IdDepartamento = await catalogo.GetDepartamento(dato.Columna2),
                        IdMunicipio = await catalogo.GetMunicipio(dato.Columna2, dato.Columna3),
                        Comunidad = dato.Columna4,

                        // Datos del esposo
                        Nombre1Esposo = dato.Columna5,
                        Nombre2Esposo = dato.Columna6,
                        Nombre3Esposo = dato.Columna7,
                        Apellido1Esposo = dato.Columna8,
                        Apellido2Esposo = dato.Columna9,
                        CuiEsposo = dato.Columna10,
                        FNacimientoEsposo = DateTime.TryParse(dato.Columna11, out var fno) ? fno : (DateTime?)null,

                        // Datos de la esposa
                        Nombre1Esposa = dato.Columna12,
                        Nombre2Esposa = dato.Columna13,
                        Nombre3Esposa = dato.Columna14,
                        Apellido1Esposa = dato.Columna15,
                        Apellido2Esposa = dato.Columna16,
                        ApellidoCasada = dato.Columna17,
                        CuiEsposa = dato.Columna18,
                        FNacimientoEsposa = DateTime.TryParse(dato.Columna19, out var fnac) ? fnac : (DateTime?)null,

                        // Datos adicionales
                        GrupoEtnico = await catalogo.GetItemCatalogo(dato.Columna90, "GRUPO ÉTNICO"),
                        EstadoFamilia = dato.Columna91,
                        CantHombres = short.TryParse(dato.Columna92, out var chos) ? chos : (short)0,
                        CantMujeres = short.TryParse(dato.Columna93, out var cmus) ? cmus : (short)0,
                        CantNiños = short.TryParse(dato.Columna94, out var cnos) ? cnos : (short)0,
                        CantNiñas = short.TryParse(dato.Columna95, out var cnas) ? cnas : (short)0,
                        NombrePersonaRegistro = dato.Columna96,
                        InstitucionRepresenta = dato.Columna97,

                        Estado = "CREADO",
                        Restrictiva = 0
                    };



                    for (int i = 0; i < 10; i++)
                    {
                        int baseCol = 20 + i * 7; // Desde la columna 20 inician los hijos

                        string nombre1 = dato.GetType().GetProperty($"Columna{baseCol}")?.GetValue(dato)?.ToString();
                        string nombre2 = dato.GetType().GetProperty($"Columna{baseCol + 1}")?.GetValue(dato)?.ToString();
                        string nombre3 = dato.GetType().GetProperty($"Columna{baseCol + 2}")?.GetValue(dato)?.ToString();
                        string apellido1 = dato.GetType().GetProperty($"Columna{baseCol + 3}")?.GetValue(dato)?.ToString();
                        string apellido2 = dato.GetType().GetProperty($"Columna{baseCol + 4}")?.GetValue(dato)?.ToString();
                        string cui = dato.GetType().GetProperty($"Columna{baseCol + 5}")?.GetValue(dato)?.ToString();
                        string fechaNacimiento = dato.GetType().GetProperty($"Columna{baseCol + 6}")?.GetValue(dato)?.ToString();
                        
                        if (!string.IsNullOrWhiteSpace(nombre1) || !string.IsNullOrWhiteSpace(apellido1))
                        {
                            listaNinos.Add(new Nino
                            {
                                IdDetalleSolicitud = detalle.IdDetalleSolicitud,
                                Nombre1 = nombre1,
                                Nombre2 = nombre2,
                                Nombre3 = nombre3,
                                Apellido1 = apellido1,
                                Apellido2 = apellido2,
                                CUI = cui,
                                FechaNacimiento = fechaNacimiento
                            });
                        }
                    }

                    detalle.Ninos = listaNinos;
                    break;



                default:
                    throw new Exception($"No se ha definido mapeo para la intervención {idIntervencion}");
            }

            return detalle;
        }
        public async Task<ServiceResponse<DetalleSolicitud>> UpdateListaDetalleSolicitud(List<ExcelRowDinamic> datos)
        {
            var response = new ServiceResponse<DetalleSolicitud>();

            //DetalleSolicitud detSolicitud;

            try
            {
                foreach (var dato in datos)
                {
                    //var departamentoID = await _catalogoRepository.GetDepartamento(dato.Columna1);
                    // Crear objeto Usuario

                    var detSolicitud = await _solicitudRepository.GetDetalleSolicitudID(Convert.ToInt64(dato.Columna1));

                    detSolicitud.IdAnulacion = await _catalogoRepository.GetItemCatalogo(dato.Columna14, "ANULACION");

                    await _solicitudRepository.UpdateDetalleSolicitud(detSolicitud);

                    response.Data = detSolicitud;
                    response.Success = true;
                    response.Message = "Detalle Solicitud creado exitosamente";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitudAPA>> SaveListaDetalleSolicitudAPA(List<Models.ExcelRowAPA> datos)
        {
            var response = new ServiceResponse<DetalleSolicitudAPA>();

            //DetalleSolicitud detSolicitud;

            try
            {
                foreach (var dato in datos)
                {
                    //var departamentoID = await _catalogoRepository.GetDepartamento(dato.Columna1);
                    // Crear objeto Usuario
                    DetalleSolicitudAPA detSolicitud = new DetalleSolicitudAPA()
                    {
                        IdDetalleSolicitud = await _solicitudRepository.CuentaDetalleAPA(),
                        IdSolicitud = Convert.ToInt32(dato.Columna1), //modelo.IdSolicitud,
                        //IdDetalleSolicitud=modelo.IdDetalleSolicitud,
                        //Correlativo = modelo.Correlativo,
                        //Codigo = DateTime.Now.Year + "-" + modelo.Correlativo,
                        ///IdIntervencion = modelo.IdIntervencion,
                        ///IdDepartamento = await _catalogoRepository.GetDepartamento(dato.Columna2),
                        ///IdMunicipio = await _catalogoRepository.GetMunicipio(dato.Columna2, dato.Columna3),
                        ///Comunidad = dato.Columna4,

                        ///Nombre1Esposo = dato.Columna5,
                        ///Nombre2Esposo = dato.Columna6,
                        ///Nombre3Esposo = dato.Columna7,
                        ///Apellido1Esposo = dato.Columna8,
                        ///Apellido2Esposo = dato.Columna9,
                        ///CuiEsposo = dato.Columna10,
                        ///FNacimientoEsposo = DateTime.TryParse(dato.Columna11, out DateTime fechaNacimientoEsposo) ? fechaNacimientoEsposo.Date : (DateTime?)null,
                        Nombre1Esposa = dato.Columna2,
                        Nombre2Esposa = dato.Columna3,
                        Nombre3Esposa = dato.Columna4,
                        Apellido1Esposa = dato.Columna5,
                        Apellido2Esposa = dato.Columna6,
                        ApellidoCasada = dato.Columna7,
                        CuiEsposa = dato.Columna8,

                        ///FNacimientoEsposa = DateTime.TryParse(dato.Columna19, out DateTime fechaNacimientoEsposa) ? fechaNacimientoEsposa.Date : (DateTime?)null,
                        ///EstadoCivil = await _catalogoRepository.GetItemCatalogo(dato.Columna20, "ESTADO CIVIL"),//Int16.TryParse(dato.Columna20, out Int16 estadocivil) ? estadocivil: 0,
                        //EstadoCivil = Int16.TryParse(dato.Columna20, out Int16 estadocivil) ? estadocivil : 0,
                        ///Embarazada = dato.Columna21,
                        ///NiñoDA = dato.Columna22,
                        Genero = dato.Columna9,
                        GrupoEtnico = await _catalogoRepository.GetItemCatalogo(dato.Columna10, "GRUPO ETNICO"),

                        //GrupoEtnico = Int16.TryParse(dato.Columna23, out Int16 grupoetnico) ? grupoetnico : 0,
                        ///Hombre0a2Años = Int16.TryParse(dato.Columna24, out Int16 hombre0a2años) ? hombre0a2años : 0,
                        ///Mujer0a2Años = Int16.TryParse(dato.Columna25, out Int16 mujer0a2años) ? mujer0a2años : 0,
                        ///Hombre2a5Años = Int16.TryParse(dato.Columna26, out Int16 hombre2a5años) ? hombre2a5años : 0,
                        ///Mujer2a5Años = Int16.TryParse(dato.Columna27, out Int16 mujer2a5años) ? mujer2a5años : 0,
                        ///Hombre6a17Años = Int16.TryParse(dato.Columna28, out Int16 hombre6a17años) ? hombre6a17años : 0,
                        ///Mujer6a17Años = Int16.TryParse(dato.Columna29, out Int16 mujer6a17años) ? mujer6a17años : 0,
                        ///TipoPersona = Int16.TryParse(dato.Columna30, out Int16 tipopersona) ? tipopersona : 0,
                        ///AreaMaiz = Decimal.TryParse(dato.Columna31, out Decimal areamaiz) ? areamaiz : 0,
                        ///QuintalesMaiz = Decimal.TryParse(dato.Columna32, out Decimal quintalesmaiz) ? quintalesmaiz : 0,
                        ///AreaFrijol = Decimal.TryParse(dato.Columna33, out Decimal areafrijol) ? areafrijol : 0,
                        ///QuintalesFrijol = Decimal.TryParse(dato.Columna34, out Decimal quintalesfrijol) ? quintalesfrijol : 0,
                        ///TipoAgriculturaPractica = Int16.TryParse(dato.Columna35, out Int16 tipoagriculturapractica) ? tipoagriculturapractica : 0,
                        ///NombreCOCODE = dato.Columna36,
                        ///NombrePersonaRegistro = dato.Columna37,
                        ///InstitucionRepresenta = dato.Columna38,
                        Restrictiva = 0,
                        Estado = "CREADO"
                    };

                    await _solicitudRepository.SaveDetalleSolicitudAPA(detSolicitud);

                    response.Data = detSolicitud;
                    response.Success = true;
                    response.Message = "Detalle Solicitud creado exitosamente";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitudAPA>> UpdateListaDetalleSolicitudAPA(List<Models.ExcelRowAPA> datos)
        {
            var response = new ServiceResponse<DetalleSolicitudAPA>();

            //DetalleSolicitud detSolicitud;

            try
            {
                foreach (var dato in datos)
                {
                    //var departamentoID = await _catalogoRepository.GetDepartamento(dato.Columna1);
                    // Crear objeto Usuario

                    var detSolicitud = await _solicitudRepository.GetDetalleSolicitudIDAPA(Convert.ToInt64(dato.Columna1));

                    detSolicitud.IdAnulacion = await _catalogoRepository.GetItemCatalogo(dato.Columna10, "ANULACION");

                    await _solicitudRepository.UpdateDetalleSolicitudAPA(detSolicitud);

                    response.Data = detSolicitud;
                    response.Success = true;
                    response.Message = "Detalle Solicitud creado exitosamente";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al crear el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Solicitud>> UpdateSolicitud(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.IdSolicitud = modelo.IdSolicitud;
                solicitud_encontrada.Correlativo = !string.IsNullOrEmpty(modelo.Correlativo) ? modelo.Correlativo : solicitud_encontrada.Correlativo;
                solicitud_encontrada.Codigo = !string.IsNullOrEmpty(modelo.Codigo) ? modelo.Codigo : solicitud_encontrada.Codigo;
                solicitud_encontrada.Depto = modelo.Depto ?? solicitud_encontrada.Depto;
                //solicitud_encontrada.IdInstitucion = modelo.IdInstitucion!=0 ? modelo.IdInstitucion : solicitud_encontrada.IdInstitucion;
                solicitud_encontrada.IdInstitucion = (!string.IsNullOrEmpty(modelo.IdInstitucion.ToString()) && modelo.IdInstitucion != 0) ? modelo.IdInstitucion : solicitud_encontrada.IdInstitucion;
                solicitud_encontrada.FechaIngresoDespacho = (modelo.FechaIngresoDespacho != null && modelo.FechaIngresoDespacho != DateTime.Parse("01/01/0001")) ? modelo.FechaIngresoDespacho : solicitud_encontrada.FechaIngresoDespacho;
                //solicitud_encontrada.FechaIngresoDespacho = modelo.FechaIngresoDespacho != null ? modelo.FechaIngresoDespacho : solicitud_encontrada.FechaIngresoDespacho;
                //solicitud_encontrada.FechaIngresoUnidad = modelo.FechaIngresoUnidad;
                //solicitud_encontrada.FechaIngresoUnidad = !string.IsNullOrEmpty(modelo.FechaIngresoUnidad) ? modelo.FechaIngresoUnidad : solicitud_encontrada.FechaIngresoUnidad;
                solicitud_encontrada.FechaIngresoDireccion = (modelo.FechaIngresoDireccion != null && modelo.FechaIngresoDireccion != DateTime.Parse("01/01/0001")) ? modelo.FechaIngresoDireccion : solicitud_encontrada.FechaIngresoDireccion;
                solicitud_encontrada.FechaIngresoUnidad = (modelo.FechaIngresoUnidad != null && modelo.FechaIngresoUnidad != DateTime.Parse("01/01/0001")) ? modelo.FechaIngresoUnidad : solicitud_encontrada.FechaIngresoUnidad;
                solicitud_encontrada.IdUnidad = !string.IsNullOrEmpty(modelo.IdUnidad.ToString()) ? modelo.IdUnidad : solicitud_encontrada.IdUnidad;
                solicitud_encontrada.IdIntervencion = (!string.IsNullOrEmpty(modelo.IdIntervencion.ToString()) && modelo.IdIntervencion != 0) ? modelo.IdIntervencion : solicitud_encontrada.IdIntervencion;
                solicitud_encontrada.Archivo = modelo.Archivo ?? solicitud_encontrada.Archivo;
                solicitud_encontrada.Raciones = modelo.Raciones != 0 ? modelo.Raciones : solicitud_encontrada.Raciones;

                solicitud_encontrada.Fallecidos = modelo.Fallecidos ?? solicitud_encontrada.Fallecidos;
                solicitud_encontrada.Duplicados = modelo.Duplicados ?? solicitud_encontrada.Duplicados;
                solicitud_encontrada.SinDatos = modelo.SinDatos ?? solicitud_encontrada.SinDatos;
                solicitud_encontrada.Gobierno = modelo.Gobierno ?? solicitud_encontrada.Gobierno;
                solicitud_encontrada.Descripcion = modelo.Descripcion ?? solicitud_encontrada.Descripcion;

                solicitud_encontrada.FInicioEntrega = modelo.FInicioEntrega;
                solicitud_encontrada.FFinEntrega = modelo.FFinEntrega;
                //solicitud_encontrada.DireccionEntrega = modelo.DireccionEntrega ?? solicitud_encontrada.DireccionEntrega;
                //solicitud_encontrada.TecnicoNombre = modelo.TecnicoNombre ?? solicitud_encontrada.TecnicoNombre;
                //solicitud_encontrada.TecnicoTelefono = modelo.TecnicoTelefono ?? solicitud_encontrada.TecnicoTelefono;
                solicitud_encontrada.TecnicoObservaciones = modelo.TecnicoObservaciones ?? solicitud_encontrada.TecnicoObservaciones;

                //solicitud_encontrada.IdProyecto = modelo.Depto == "2" ? solicitud_encontrada.IdProyecto : modelo.IdProyecto;
                solicitud_encontrada.IdProyecto = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.IdProyecto.ToString()) ? modelo.IdProyecto : solicitud_encontrada.IdProyecto : null;
                //solicitud_encontrada.Departamento = modelo.Depto == "2" ? solicitud_encontrada.Departamento :  modelo.Departamento;
                solicitud_encontrada.Departamento = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.Departamento.ToString()) ? modelo.Departamento : solicitud_encontrada.Departamento : null;
                //solicitud_encontrada.Municipio = modelo.Depto == "2" ? solicitud_encontrada.Municipio : modelo.Municipio;
                solicitud_encontrada.Municipio = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.Municipio.ToString()) ? modelo.Municipio : solicitud_encontrada.Municipio : null;
                //solicitud_encontrada.Comunidad = modelo.Depto == "2" ? solicitud_encontrada.Comunidad : modelo.Comunidad;
                solicitud_encontrada.Comunidad = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.Comunidad.ToString()) ? modelo.Comunidad : solicitud_encontrada.Comunidad : null;
                solicitud_encontrada.NombreCocode = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.NombreCocode.ToString()) ? modelo.NombreCocode : solicitud_encontrada.NombreCocode : null;
                solicitud_encontrada.ComunidadCocode = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.ComunidadCocode.ToString()) ? modelo.ComunidadCocode : solicitud_encontrada.ComunidadCocode : null;
                solicitud_encontrada.TelefonoCocode = modelo.Depto == "2" ? !string.IsNullOrEmpty(modelo.TelefonoCocode.ToString()) ? modelo.TelefonoCocode : solicitud_encontrada.TelefonoCocode : null;
                await _solicitudRepository.UpdateSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Solicitud actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Solicitud>> UpdateEstado(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.Estado = "EVALUACION";

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

        public async Task<ServiceResponse<Solicitud>> UpdateEstadoARevAdministrativa(Solicitud modelo)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(modelo.IdSolicitud);

            try
            {
                solicitud_encontrada.Estado = "REV ADMINISTRATIVA";

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

        public async Task<ServiceResponse<DetalleSolicitud>> UpdateDetalleSolicitud(DetalleSolicitud modelo, string proceso)
        {
            var response = new ServiceResponse<DetalleSolicitud>();
            DetalleSolicitud solicitud_encontrada = await _solicitudRepository.GetDetalleSolicitud(modelo.IdDetalleSolicitud);

            try
            {
                solicitud_encontrada.IdDetalleSolicitud = modelo.IdDetalleSolicitud;
                ///solicitud_encontrada.IdIntervencion = modelo.IdIntervencion;
                solicitud_encontrada.IdDepartamento = modelo.IdDepartamento;
                solicitud_encontrada.IdMunicipio = modelo.IdMunicipio;
                solicitud_encontrada.Comunidad = modelo.Comunidad;
                solicitud_encontrada.Nombre1Esposo = modelo.Nombre1Esposo;
                solicitud_encontrada.Nombre2Esposo = modelo.Nombre2Esposo;
                solicitud_encontrada.Nombre3Esposo = modelo.Nombre3Esposo;
                solicitud_encontrada.Apellido1Esposo = modelo.Apellido1Esposo;
                solicitud_encontrada.Apellido2Esposo = modelo.Apellido2Esposo;
                solicitud_encontrada.CuiEsposo = modelo.CuiEsposo;
                solicitud_encontrada.FNacimientoEsposo = modelo.FNacimientoEsposo;
                solicitud_encontrada.Nombre1Esposa = modelo.Nombre1Esposa;
                solicitud_encontrada.Nombre2Esposa = modelo.Nombre2Esposa;
                solicitud_encontrada.Nombre3Esposa = modelo.Nombre3Esposa;
                solicitud_encontrada.Apellido1Esposa = modelo.Apellido1Esposa;
                solicitud_encontrada.Apellido2Esposa = modelo.Apellido2Esposa;
                solicitud_encontrada.ApellidoCasada = modelo.ApellidoCasada;
                solicitud_encontrada.CuiEsposa = modelo.CuiEsposa;
                solicitud_encontrada.FNacimientoEsposa = modelo.FNacimientoEsposa;

                solicitud_encontrada.EstadoCivil = modelo.EstadoCivil;
                solicitud_encontrada.Embarazada = modelo.Embarazada;
                solicitud_encontrada.NiñoDA = modelo.NiñoDA;
                solicitud_encontrada.GrupoEtnico = modelo.GrupoEtnico;
                solicitud_encontrada.Hombre0a2Años = modelo.Hombre0a2Años;
                solicitud_encontrada.Mujer0a2Años = modelo.Mujer0a2Años;
                solicitud_encontrada.Hombre2a5Años = modelo.Hombre2a5Años;
                solicitud_encontrada.Mujer2a5Años = modelo.Mujer2a5Años;
                solicitud_encontrada.Hombre6a17Años = modelo.Hombre6a17Años;
                solicitud_encontrada.Mujer6a17Años = modelo.Mujer6a17Años;
                solicitud_encontrada.TipoPersona = modelo.TipoPersona;
                solicitud_encontrada.AreaMaiz = modelo.AreaMaiz;
                solicitud_encontrada.QuintalesMaiz = modelo.QuintalesMaiz;
                solicitud_encontrada.AreaFrijol = modelo.AreaFrijol;
                solicitud_encontrada.QuintalesFrijol = modelo.QuintalesFrijol;
                solicitud_encontrada.TipoAgriculturaPractica = modelo.TipoAgriculturaPractica;
                solicitud_encontrada.NombreCOCODE = modelo.NombreCOCODE;
                solicitud_encontrada.NombrePersonaRegistro = modelo.NombrePersonaRegistro;
                solicitud_encontrada.InstitucionRepresenta = modelo.InstitucionRepresenta;
                solicitud_encontrada.IdAnulacion = modelo.IdAnulacion;
                //solicitud_encontrada.CantidadBeneficiarios = modelo.CantidadBeneficiarios;
                //solicitud_encontrada.Archivo = modelo.Archivo;

                if (proceso.ToString() == "Programacion")
                {
                    if (solicitud_encontrada.IdAnulacion != null && solicitud_encontrada.IdAnulacion != 0)
                        solicitud_encontrada.Proceso = "PROGRAMACION";
                }
                else if (proceso.ToString() == "Evaluacion")
                {
                    if (solicitud_encontrada.IdAnulacion != null && solicitud_encontrada.IdAnulacion != 0)
                        solicitud_encontrada.Proceso = "EVALUACION";
                }

                await _solicitudRepository.UpdateDetalleSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Detalle Solicitud actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitudAPA>> UpdateDetalleSolicitudAPA(DetalleSolicitudAPA modelo)
        {
            var response = new ServiceResponse<DetalleSolicitudAPA>();
            DetalleSolicitudAPA solicitud_encontrada = await _solicitudRepository.GetDetalleSolicitudAPA(modelo.IdDetalleSolicitud);

            try
            {
                solicitud_encontrada.IdDetalleSolicitud = modelo.IdDetalleSolicitud;
                ///solicitud_encontrada.IdIntervencion = modelo.IdIntervencion;
                //solicitud_encontrada.IdDepartamento = modelo.IdDepartamento;
                //solicitud_encontrada.IdMunicipio = modelo.IdMunicipio;
                //solicitud_encontrada.Comunidad = modelo.Comunidad;
                //solicitud_encontrada.Nombre1Esposo = modelo.Nombre1Esposo;
                //solicitud_encontrada.Nombre2Esposo = modelo.Nombre2Esposo;
                //solicitud_encontrada.Nombre3Esposo = modelo.Nombre3Esposo;
                //solicitud_encontrada.Apellido1Esposo = modelo.Apellido1Esposo;
                //solicitud_encontrada.Apellido2Esposo = modelo.Apellido2Esposo;
                //solicitud_encontrada.CuiEsposo = modelo.CuiEsposo;
                //solicitud_encontrada.FNacimientoEsposo = modelo.FNacimientoEsposo;
                solicitud_encontrada.Nombre1Esposa = modelo.Nombre1Esposa;
                solicitud_encontrada.Nombre2Esposa = modelo.Nombre2Esposa;
                solicitud_encontrada.Nombre3Esposa = modelo.Nombre3Esposa;
                solicitud_encontrada.Apellido1Esposa = modelo.Apellido1Esposa;
                solicitud_encontrada.Apellido2Esposa = modelo.Apellido2Esposa;
                solicitud_encontrada.ApellidoCasada = modelo.ApellidoCasada;
                solicitud_encontrada.CuiEsposa = modelo.CuiEsposa;
                //solicitud_encontrada.FNacimientoEsposa = modelo.FNacimientoEsposa;

                //solicitud_encontrada.EstadoCivil = modelo.EstadoCivil;
                //solicitud_encontrada.Embarazada = modelo.Embarazada;
                //solicitud_encontrada.NiñoDA = modelo.NiñoDA;
                solicitud_encontrada.Genero = modelo.Genero;
                solicitud_encontrada.GrupoEtnico = modelo.GrupoEtnico;
                //solicitud_encontrada.Hombre0a2Años = modelo.Hombre0a2Años;
                //solicitud_encontrada.Mujer0a2Años = modelo.Mujer0a2Años;
                //solicitud_encontrada.Hombre2a5Años = modelo.Hombre2a5Años;
                //solicitud_encontrada.Mujer2a5Años = modelo.Mujer2a5Años;
                //solicitud_encontrada.Hombre6a17Años = modelo.Hombre6a17Años;
                //solicitud_encontrada.Mujer6a17Años = modelo.Mujer6a17Años;
                //solicitud_encontrada.TipoPersona = modelo.TipoPersona;
                //solicitud_encontrada.AreaMaiz = modelo.AreaMaiz;
                //solicitud_encontrada.QuintalesMaiz = modelo.QuintalesMaiz;
                //solicitud_encontrada.AreaFrijol = modelo.AreaFrijol;
                //solicitud_encontrada.QuintalesFrijol = modelo.QuintalesFrijol;
                //solicitud_encontrada.TipoAgriculturaPractica = modelo.TipoAgriculturaPractica;
                //solicitud_encontrada.NombreCOCODE = modelo.NombreCOCODE;
                //solicitud_encontrada.NombrePersonaRegistro = modelo.NombrePersonaRegistro;
                //solicitud_encontrada.InstitucionRepresenta = modelo.InstitucionRepresenta;
                solicitud_encontrada.IdAnulacion = modelo.IdAnulacion;

                await _solicitudRepository.UpdateDetalleSolicitudAPA(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Success = true;
                response.Message = "Detalle Solicitud actualizada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al actualizar el detalle solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<Solicitud>> DeleteSolicitud(long id)
        {
            var response = new ServiceResponse<Solicitud>();
            Solicitud solicitud_encontrada = await _solicitudRepository.GetSolicitud(id);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _solicitudRepository.UpdateSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitud>> DeleteDetalleSolicitud(long idDetalle)
        {
            var response = new ServiceResponse<DetalleSolicitud>();
            DetalleSolicitud solicitud_encontrada = await _solicitudRepository.GetDetalleSolicitudID(idDetalle);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _solicitudRepository.UpdateDetalleSolicitud(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Detalle de Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle de la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<DetalleSolicitudAPA>> DeleteDetalleSolicitudAPA(long idDetalle)
        {
            var response = new ServiceResponse<DetalleSolicitudAPA>();
            DetalleSolicitudAPA solicitud_encontrada = await _solicitudRepository.GetDetalleSolicitudIDAPA(idDetalle);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _solicitudRepository.UpdateDetalleSolicitudAPA(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Detalle de Solicitud eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar el detalle de la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<AsignaTecnico>> SaveAsignaTecnico(AsignaTecnico modelo)
        {
            var response = new ServiceResponse<AsignaTecnico>();

            try
            {
                // Crear objeto Usuario
                AsignaTecnico asignaTecnico = new AsignaTecnico()
                {
                    IdAsignaTecnico = await _solicitudRepository.CuentaAsignaTecnico(),
                    TecnicoId = modelo.TecnicoId,
                    SolicitudId = modelo.SolicitudId,
                    DepartamentoId = modelo.DepartamentoId,
                    IdMunicipio = modelo.IdMunicipio,
                    DireccionEntrega = modelo.DireccionEntrega,
                    Restrictiva = 0
                };

                // Guardar solicitud en la base de datos
                await _solicitudRepository.SaveAsignaTecnico(asignaTecnico);

                response.Data = asignaTecnico;
                response.Success = true;
                response.Message = "Tecnico asignado exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al asignar al técnico: {ex.Message}";
            }

            return response;
        }

        //public async Task<List<AsignaTecnico>> GetDetalleTecnicosSolicitud(long id)
        public async Task<List<dynamic>> GetDetalleTecnicosSolicitud(long id)
        {
            //Busca por id de solicitud
            //List<AsignaTecnico> detalle_encontrado = await _solicitudRepository.GetDetalleTecnicosSolicitud(id);
            var detalle_encontrado = await _solicitudRepository.GetDetalleTecnicosSolicitud(id);
            return detalle_encontrado;
        }

        public async Task<ServiceResponse<AsignaTecnico>> DeleteAsignaTecnico(long idAsignaTecnico)
        {
            var response = new ServiceResponse<AsignaTecnico>();
            AsignaTecnico solicitud_encontrada = await _solicitudRepository.GetAsignaTecnico(idAsignaTecnico);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _solicitudRepository.UpdateAsignaTecnico(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Asignación de tecnico eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la asignación de técnico: {ex.Message}";
            }

            return response;
        }

        public async Task<List<Tecnico>> GetAllTecnicos()
        {

            List<Tecnico> listado = await _solicitudRepository.GetAllTecnicos();

            return listado;
        }

        public async Task<Boolean> GetTotalRacionesAsignadas(Int64 id)
        {
            //Obtener encabezado de solicitud
            var sol = await _solicitudRepository.GetSolicitud(id);

            var total = 0;

            if (sol.Depto == "1")
            {
                //Cuenta total de beneficiarios que posee la solicitud
                total = await _dBContext.DetalleSolicitud
                             .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id
                                    && (ds.IdAnulacion == null || ds.IdAnulacion == 67))
                             .CountAsync();
            }
            else if (sol.Depto == "2")
            {
                //No se toma en cuenta el municipio ya que viene desde el encabezado entonces todo el detalle le pertenece
                total = await _dBContext.DetalleSolicitudAPA
                             .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == id
                                    //&& Convert.ToInt32(modelo.IdMunicipio) == ds.IdMunicipio
                                    && (ds.IdAnulacion == null || ds.IdAnulacion == 67))
                             .CountAsync();
            }

            //Calcula total de raciones
            var totalRaciones = total * sol.Raciones;

            //Verifica total de raciones asignadas
            var totalAsignado = await _dBContext.AsignaRaciones
                             .Where(ar => ar.SolicitudId == id && ar.Restrictiva == 0)
                             .SumAsync(ar => ar.RacionAsignada);

            Boolean resultado = false;

            //Verifica si se a asignado el total de raciones disponibles
            if (totalRaciones == totalAsignado)
                resultado = true;

            return resultado;
        }

        public async Task<ServiceResponse<AsignaRacion>> SaveAsignaRacion(AsignaRacion modelo)
        {
            var response = new ServiceResponse<AsignaRacion>();

            var sol = await _solicitudRepository.GetSolicitud(modelo.SolicitudId);

            var total = 0;

            if (sol.Depto == "1")
            {
                total = await _dBContext.DetalleSolicitud
                             .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == modelo.SolicitudId
                                    && Convert.ToInt32(modelo.IdMunicipio) == ds.IdMunicipio
                                    && Convert.ToInt32(modelo.DepartamentoId) == ds.IdDepartamento
                                    && modelo.Comunidad == ds.Comunidad
                                    && (ds.IdAnulacion == null || ds.IdAnulacion == 67)
                                    )
                             .CountAsync();
            }
            else if (sol.Depto == "2")
            {
                //No se toma en cuenta el municipio ya que viene desde el encabezado entonces todo el detalle le pertenece
                total = await _dBContext.DetalleSolicitudAPA
                             .Where(ds => ds.Restrictiva == 0 && ds.IdSolicitud == modelo.SolicitudId
                                    //&& Convert.ToInt32(modelo.IdMunicipio) == ds.IdMunicipio
                                    && (ds.IdAnulacion == null || ds.IdAnulacion == 67))
                             .CountAsync();
            }


            var asignado = await _dBContext.AsignaRaciones
                             .Where(a => a.IdMunicipio == modelo.IdMunicipio
                                    && a.DepartamentoId == modelo.DepartamentoId
                                    && a.SolicitudId == modelo.SolicitudId
                                    && a.Comunidad == modelo.Comunidad
                                    && a.Restrictiva == 0)
                             .SumAsync(a => a.RacionAsignada);

            try
            {
                // Crear objeto Usuario
                AsignaRacion asignaRacion = new AsignaRacion()
                {
                    IdAsignaRacion = await _solicitudRepository.CuentaAsignaRacion(),
                    SolicitudId = modelo.SolicitudId,
                    IdMunicipio = modelo.IdMunicipio,
                    DepartamentoId = modelo.DepartamentoId,
                    Comunidad = modelo.Comunidad,
                    RacionAsignada = modelo.RacionAsignada,
                    RacionSaldo = (total * sol.Raciones) - asignado - modelo.RacionAsignada,
                    Restrictiva = 0
                };

                // Guardar solicitud en la base de datos
                await _solicitudRepository.SaveAsignaRacion(asignaRacion);

                response.Data = asignaRacion;
                response.Success = true;
                response.Message = "Racion asignada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al asignar la racion: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<AsignaRacion>> DeleteAsignaRacion(long idRacion)
        {
            var response = new ServiceResponse<AsignaRacion>();
            AsignaRacion solicitud_encontrada = await _solicitudRepository.GetAsignaRacion(idRacion);

            try
            {
                solicitud_encontrada.Restrictiva = 1;

                await _solicitudRepository.UpdateAsignaRacion(solicitud_encontrada);

                response.Data = solicitud_encontrada;
                response.Message = "Racion eliminada exitosamente";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error al intentar eliminar la ración de la solicitud: {ex.Message}";
            }

            return response;
        }

        public async Task<List<AsignaRacion>> GetDetalleRacionesSolicitud(long id)
        {
            //Busca por id de solicitud
            List<AsignaRacion> detalle_encontrado = await _solicitudRepository.GetDetalleRacionesSolicitud(id);

            return detalle_encontrado;
        }

        public async Task<int?> GetIntervencionBySolicitudId(Int64? idSolicitud)
        {
            var idintervencion = await _solicitudRepository.GetIntervencionBySolicitudId(idSolicitud);
        return idintervencion;
        }

        //public Task<ServiceResponse<DetalleSolicitud>> SaveListaDetalleSolicitud(List<Services.ExcelRow> datos)
        //{
        //    throw new NotImplementedException();
        //}

        // Clase para recibir los datos desde el cliente
        //public class ExcelRow
        //{
        //    public string Columna1 { get; set; }
        //    public string Columna2 { get; set; }
        //    public string Columna3 { get; set; }
        //}

        // Ejemplo de entidad que se guarda en la BD
        /*public class EntidadDeBD
        {
            public int Id { get; set; }
            public string Columna1 { get; set; }
            public string Columna2 { get; set; }
            public string Columna3 { get; set; }
        }*/
    }
}
