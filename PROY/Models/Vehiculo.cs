namespace PROY.Models
{
    public class Vehiculo
    {
        public Int64 IdVehiculo { get; set; }
        public int? IdTransportista { get; set; }
        public string? PlacaCabezal { get; set; }
        public string? PlacaPlataforma { get; set; }
        public int? Estado { get; set; }
    }
}
