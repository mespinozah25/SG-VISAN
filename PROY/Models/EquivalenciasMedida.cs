namespace PROY.Models
{
    public class EquivalenciasMedida
    {
        public Int64 IdEquivalenciasMedida { get; set; }
        public int? CantidadDesde { get; set; }
        public int? IdUnidadMedidaDesde { get; set; }
        public int? CantidadHasta { get; set; }
        public int? IdCantidadMedidaHasta { get; set; }
        public int Estado { get; set; }
    }
}
