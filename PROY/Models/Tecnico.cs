using System.Text.Json.Serialization;

namespace PROY.Models
{
    public class Tecnico
    {
        public Int64 IdTecnico { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }               
        public int Restrictiva { get; set; }
        //public AsignaTecnico AsignaTecnico { get; set; }
        //[JsonIgnore]
        //public List<AsignaTecnico> AsignaTecnicos { get; set; }
        //public List<Solicitud> Solicitudes { get; set; }
    }
}
