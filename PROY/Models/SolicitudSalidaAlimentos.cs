using System.ComponentModel.DataAnnotations.Schema;

namespace PROY.Models
{
    public class SolicitudSalidaAlimentos
    {
        public Int64 IdSolicitudSalidaAlimentos { get; set; }
        public int? IdEgresoAlimentos { get; set; }
        public string? NoConstancia { get; set; }
        public DateTime? FechaDespacho { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdTransportista { get; set; }
        public int? IdPiloto { get; set; }
        public int? IdVehiculo { get; set; }
        public int Estado { get; set; }

        // Campos de Validación utilizado en la venta de Finalización de Despacho
        [NotMapped]
        public string? NombreTransportista { get; set; }
        [NotMapped]
        public string? PlacasTransporte { get; set; }
        [NotMapped]
        public string? NoTrailer { get; set; }
        [NotMapped]
        public string? NombrePiloto { get; set; }
        [NotMapped]
        public string? NoLicencia { get; set; }
    }
}
