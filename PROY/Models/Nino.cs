namespace PROY.Models
{
    public class Nino
    {
        public int? IdNino { get; set; }
        public Int64? IdDetalleSolicitud { get; set; }
        public string? NombreCompleto { get; set; }
        public string? Nombre1 { get; set; }
        public string? Nombre2 { get; set; }
        public string? Nombre3 { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? FechaNacimiento { get; set; }
        public string? CUI { get; set; }
        public int? Edad { get; set; }
        public DetalleSolicitud? DetalleSolicitud { get; set; }



    }
}
