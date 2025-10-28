namespace PROY.Models
{
    public class DetalleSolicitudAPA
    {
        public Int64 IdDetalleSolicitud { get; set; }
        public Int64 IdSolicitud { get; set; }
        public Solicitud Solicitud { get; set; }
        //public DetalleSolicitud Detalles { get; set; }
        public Int64 Correlativo { get; set; }
        //public int IdIntervencion { get; set; }
        public int ? IdDepartamento { get; set; }
        public int ? IdMunicipio { get; set; }
        public string ? Comunidad { get; set; }
        public string? Nombre1Esposo { get; set; }
        public string? Nombre2Esposo { get; set; }
        public string? Nombre3Esposo { get; set; }
        public string? Apellido1Esposo { get; set; }
        public string? Apellido2Esposo { get; set; }
        public string? CuiEsposo { get; set; }
        public DateTime? FNacimientoEsposo { get; set; }
        public string? Nombre1Esposa { get; set; }
        public string? Nombre2Esposa { get; set; }
        public string? Nombre3Esposa { get; set; }
        public string? Apellido1Esposa { get; set; }
        public string? Apellido2Esposa { get; set; }
        public string? ApellidoCasada { get; set; }
        public string? CuiEsposa { get; set; }
        public DateTime? FNacimientoEsposa { get; set; }
        public int ? EstadoCivil { get; set; }
        public string? Embarazada { get; set; }
        public string? NiñoDA { get; set; }
        public int GrupoEtnico { get; set; }
        public int? Hombre0a2Años { get; set; }
        public int? Mujer0a2Años { get; set; }
        public int? Hombre2a5Años { get; set; }
        public int? Mujer2a5Años { get; set; }
        public int? Hombre6a17Años { get; set; }
        public int? Mujer6a17Años { get; set; }
        public int? TipoPersona { get; set; }
        public decimal? AreaMaiz { get; set; }
        public decimal? QuintalesMaiz { get; set; }
        public decimal? AreaFrijol { get; set; }
        public decimal? QuintalesFrijol { get; set; }
        public int? TipoAgriculturaPractica { get; set; }
        public string? NombreCOCODE { get; set; }
        public string? NombrePersonaRegistro { get; set; }
        public string? InstitucionRepresenta { get; set; }
        public string? Genero { get; set; } 
        public int? IdAnulacion { get; set; }
        public string? Proceso { get; set; }
        public int Restrictiva { get; set; }
        public string Estado { get; set; }
        public Catalogo Catalogo { get; set; }
    }
}
