namespace PROY.Models
{
    public class ReporteEjecucion
    {
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public int CantidadComunidades { get; set; }
        public int TotalRaciones { get; set; }
    }


    public class ReporteDespacho
    {
        public int Despacho { get; set; }
        public string Exp { get; set; }
        public int TotalRaciones { get; set; }
        public string Exp2 { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public string Comunidad { get; set; }
        public string TecnicoResponsable { get; set; }
        public string Convenio { get; set; }
        public string Estatus { get; set; }
        public DateTime FechaOtorgamiento { get; set; }
        public string Alimento { get; set; }
        public string Observaciones { get; set; }
    }
}