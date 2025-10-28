namespace PROY.Models
{
    public class Catalogo
    {
        public int IdCatalogo { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public int Orden { get; set; }
        public int Restrictiva { get; set; }
        //public List<DetalleSolicitud> DetallesSolicitud { get; set; }
    }
}
