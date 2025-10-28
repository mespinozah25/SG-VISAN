using System.ComponentModel.DataAnnotations.Schema;

namespace PROY.Models
{
    public class DetalleEgresoAlimentos
    {
        public Int64 IdDetalleEgresoAlimentos { get; set; }
        public int? IdEgresoAlimentos { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public int? IdPresentacion { get; set; }
        public int? CantidadUnidad { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? NoUnidades { get; set; }
        public string? EstibaEgreso { get; set; }
        public string? EstibaTraslado { get; set; }
        public int Estado { get; set; }

        // Check de Validación utilizado en la venta de DespachoAlmacen
        [NotMapped]
        public int? CheckRow { get; set; }
    }
}
