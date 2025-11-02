
namespace PROY.Models
{
    public class AsignaRacion
    {
        public Int64 IdAsignaRacion { get; set; }        
        public Int64 SolicitudId { get; set; }
        public Solicitud Solicitud { get; set; }        
        public string DepartamentoId { get; set; }
        public string? IdMunicipio { get; set; }
        public string Comunidad { get; set; }
        public int RacionAsignada {  get; set; }
        public int RacionSaldo { get; set; }
        public int Restrictiva { get; set; }
    }
}
