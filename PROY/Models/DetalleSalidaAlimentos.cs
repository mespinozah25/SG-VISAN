namespace PROY.Models
{
    public class DetalleSalidaAlimentos
    {
        public Int64 IdDetalleSalidaAlimentos { get; set; }
        public int? IdSolicitudSalidaAlimentos { get; set; }
        public int? IdProductoAlimenticio { get; set; }
        public int? IdPresentacion { get; set; }
        public int? CantidadUnidad { get; set; }
        public int? IdUnidadMedida { get; set; }
        public int? CantidadSalida { get; set; }
        public string? EstadoProducto { get; set; }
        public int Estado { get; set; }
    }
}
