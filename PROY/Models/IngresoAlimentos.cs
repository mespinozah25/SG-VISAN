namespace PROY.Models
{
    public class IngresoAlimentos
    {
        public Int64 IdIngresoAlimentos { get; set; }
        public int? IdPrograma { get; set; }
        public int? IdConvenio { get; set; }
        public int? IdBodegaRecibe { get; set; }
        public int? IdJuntaReceptora { get; set; }
        public string? NoWaybill { get; set; }
        public DateTime? FechaWaybill { get; set; }
        public string? NoLti { get; set; }
        public string? ArchivoWaybill { get; set; }
        public int? FirmaSelloEncargadoWaybill { get; set; }
        public string? NombreResponsableIndeca { get; set; }
        public string? NoActa { get; set; }
        public int? FolioInicialActa { get; set; }
        public int? FolioFinalActa { get; set; }
        public string? DocumentoActa { get; set; }
        public int? FirmaMiembrosJuntaActa { get; set; }
        public int? FirmaEncargadoBodegaActa { get; set; }
        public string? NoForma1h { get; set; }
        public DateTime? FechaForma1h { get; set; }
        public int? IdProveedor1H { get; set; }
        public string? ArchivoWaybillFinanciero { get; set; }
        public string? ArchivoActaFinanciero { get; set; }
        public string? ArchivoForma1hFinanciero { get; set; }
        public string? ArchivoBoletaAnalisis { get; set; }
        public int? IdUsuarioJefeBodega { get; set; }
        public int? IdEstadoIngreso { get; set; }
        public int? Estado { get; set; }
    }
}
