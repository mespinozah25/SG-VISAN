
namespace PROY.Models
{
    public class AsignaRacionV2
    {
        public int IdAsignaRacion { get; set; }        
        public int IdSolicitud { get; set; }
        public int IdDepartamento { get; set; }        
        public int IdMunicipio { get; set; }
        public required string Comunidad { get; set; }
        public int RacionAsignada {  get; set; }
        public int RacionSaldo { get; set; }
        public required string UsuarioRegistro { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Restrictiva { get; set; }
    }
}
