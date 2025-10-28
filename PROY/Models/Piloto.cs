namespace PROY.Models
{
    public class Piloto
    {
        public Int64 IdPiloto { get; set; }
        public int? IdTransportista { get; set; }
        public string? NombresPiloto { get; set; }
        public string? ApellidosPiloto { get; set; }
        public string? NoLicencia { get; set; }
        public int? Estado { get; set; }
    }
}
