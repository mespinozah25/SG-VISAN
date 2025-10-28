namespace PROY.Models
{
    public class AsignaTecnico
    {
        public Int64 IdAsignaTecnico { get; set; }
        public Int64 TecnicoId { get; set; }
        public Tecnico Tecnico { get; set; }
        public Int64 SolicitudId { get; set; }
        public Solicitud Solicitud { get; set; }
        public string DepartamentoId { get; set; }
        public string IdMunicipio { get; set; }
        public string ? DireccionEntrega { get; set; }
        public int Restrictiva { get; set; }
    }
}
