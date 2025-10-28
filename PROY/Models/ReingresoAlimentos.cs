namespace PROY.Models
{
    public class ReingresoAlimentos
    {
        public Int64 IdReingresoAlimentos { get; set; }
        public string? NoOficioReingreso { get; set; }
        public int? IdPrograma { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public string? ArchivoOficioReingreso { get; set; }
        public decimal? IdEgresoAlimentos { get; set; }
        public string? ObservacionFisicaIndeca { get; set; }
        public string? ArchivoDespachoAlmacenReingreso { get; set; }
        public int? IdUsuarioEncargado { get; set; }
        public int? IdEstadoReingreso { get; set; }
        public int? Estado { get; set; }
    }
}
