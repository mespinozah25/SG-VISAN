namespace PROY.Models
{
    public class EgresoAlimentos
    {
        public Int64 IdEgresoAlimentos { get; set; }
        public string? NoDespachoAlmacen { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdConvenio { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public decimal? TotalRaciones { get; set; }
        public int? IdTipoAsistencia { get; set; }
        public int? IdTipoSalidaAlimento { get; set; }
        public int? IdDepartamentoCarga { get; set; }
        public int? IdBodegaCarga { get; set; }
        public int? IdBodegaDestino { get; set; }
        public string? NoAutorizacion { get; set; }
        public int? IdDepartamentoDestino { get; set; }
        public int? IdMunicipioDestino { get; set; }
        public string? DireccionFinal { get; set; }
        public string? Comentarios { get; set; }
        public int? FirmaSelloDirectorVisan { get; set; }
        public int? FirmaSelloJefeDepartamento { get; set; }
        public string? ObservacionesAutorizacion { get; set; }
        public string? ObservacionesRechazo { get; set; }
        public int? FirmaSelloJefeBodegas { get; set; }
        public string? ArchivoDespachoAlmacen { get; set; }
        public string? ObservCondSanitariaTransp { get; set; }
        public int? IdUsuarioLogistica { get; set; }
        public int? IdUsuarioEncargadoBodega { get; set; }
        public int? IdUsuarioRecibeTraslado { get; set; }
        public int? IdEstadoEgreso { get; set; }
        public int? Estado { get; set; }
    }
}
