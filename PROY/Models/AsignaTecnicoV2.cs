namespace PROY.Models
{
    public class AsignaTecnicoV2
    {
        public int IdAsignaTecnico { get; set; }
        public int IdSolicitud { get; set; }
        public long IdTecnico { get; set; }
        public required Tecnico Tecnico { get; set; }
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public required string DireccionEntrega { get; set; }
        public int Restrictiva { get; set; }
    }
}
