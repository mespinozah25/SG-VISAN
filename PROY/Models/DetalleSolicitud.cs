namespace PROY.Models
{
    public class DetalleSolicitud
    {
        public Int64 IdDetalleSolicitud { get; set; }
        public Int64 IdSolicitud { get; set; }
        public Solicitud Solicitud { get; set; }
        //public DetalleSolicitud Detalles { get; set; }
        public Int64 Correlativo { get; set; }
        //public int IdIntervencion {  get; set; }
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public string Comunidad { get; set; }
        public string ? UbicacionAlbergue { get; set; }
        public decimal ?  CodComunidad { get; set; }
        public string ? Nombre1Esposo { get; set; }
        public string ? Nombre2Esposo { get; set; }
        public string ? Nombre3Esposo { get; set; }
        public string ? Apellido1Esposo { get; set; }
        public string ? Apellido2Esposo { get; set; }
        public string ? CuiEsposo { get; set; }
        public DateTime ? FNacimientoEsposo { get; set; }
        public string ? Nombre1Esposa { get; set; }
        public string ? Nombre2Esposa { get; set; }
        public string ? Nombre3Esposa { get; set; }
        public string ? Apellido1Esposa { get; set; }
        public string ? Apellido2Esposa { get; set; }
        public string ? ApellidoCasada { get; set; }
        public string ? CuiEsposa { get; set; }
        public DateTime ? FNacimientoEsposa { get; set; }
        public int EstadoCivil { get; set; }
        public string ? Embarazada { get; set; }
        public string ? NiñoDA { get; set; }
        public int GrupoEtnico { get; set; }
        public int ? Hombre0a2Años { get; set; }
        public int ? Mujer0a2Años { get; set; }
        public int ? Hombre2a5Años { get; set; }
        public int ? Mujer2a5Años { get; set; }
        public int ? Hombre6a17Años { get; set; }
        public int ? Mujer6a17Años { get; set; }
        public int ? TipoPersona { get; set; }
        public decimal ? AreaMaiz { get; set; }
        public decimal ? QuintalesMaiz { get; set; }
        public decimal ? AreaFrijol { get; set; }
        public decimal ? QuintalesFrijol { get; set; }
        public int ? TipoAgriculturaPractica { get; set; }
        public string ? NombreCOCODE { get; set; }
        public string? EstadoFamilia { get; set; }
        public decimal? CantPersonasHogar { get; set; }
        public decimal? CantHombres { get; set; }
        public decimal? CantMujeres { get; set; }
        public string? DañoFamiliar { get; set; }
        public string? DañoVivienda { get; set; }
        public string? PerdidaCultivo { get; set; }
        public string? PerdidaAnimalesCrianza { get; set; }
        public string ? NombrePersonaRegistro { get; set; }
        public string ? InstitucionRepresenta { get; set; }
        public int ? IdAnulacion { get; set; }
        public string ? Proceso {  get; set; }
        public int Restrictiva { get; set; }
        public string Estado { get; set; }
        public Catalogo Catalogo { get; set; }

        public string? NombreCompletoPersona1 { get; set; }
        public decimal? EdadPersona1 { get; set; }
        public string? GeneroPersona1 { get; set; }
        public string? Nombre1Persona1 { get; set; }
        public string? Nombre2Persona1 { get; set; }
        public string? Nombre3Persona1 { get; set; }
        public string? Apellido1Persona1 { get; set; }
        public string? Apellido2Persona1 { get; set; }
        public string? ApellidoCasadaPersona1 { get; set; }
        public string? CuiPersona1 { get; set; }
        public DateTime? FNacimientoPersona1 { get; set; }
        public string? NombreCompletoPersona2 { get; set; }
        public decimal? EdadPersona2 { get; set; }
        public string? GeneroPersona2 { get; set; }
        public string? Nombre1Persona2 { get; set; }
        public string? Nombre2Persona2 { get; set; }
        public string? Nombre3Persona2 { get; set; }
        public string? Apellido1Persona2 { get; set; }
        public string? Apellido2Persona2 { get; set; }
        public string? ApellidoCasadaPersona2 { get; set; }
        public string? CuiPersona2 { get; set; }
        public DateTime? FNacimientoPersona2 { get; set; }
        public string? CodigoNiño { get; set; }
        public string? Nombre1Niño { get; set; }
        public string? Nombre2Niño { get; set; }
        public string? Nombre3Niño { get; set; }
        public string? Apellido1Niño { get; set; }
        public string? Apellido2Niño { get; set; }
        public string? CuiNiño { get; set; }
        public string? Sexo { get; set; }
        public DateTime? FNacimientoNiño { get; set; }
        public Decimal? EdadNiño { get; set; }
        public string? Diagnostico { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Talla { get; set; }
        public string? Parentesco { get; set; }
        public decimal? CantNiños { get; set; }
        public decimal? CantNiñas { get; set; }

        public List<Nino>? Ninos { get; set; }


    }
}
