namespace PROY.Models
{
    public class Estiba
    {
        public Estiba() { }

        public Estiba(int IdBodega, int IdSubBodega)
        {
            this.IdBodega = IdBodega;
            this.IdSubBodega = IdSubBodega;
        }

        public Int64 IdEstiba { get; set; }
        public int? IdBodega { get; set; }
        public int? IdSubBodega { get; set; }
        public string? CodigoEstiba { get; set; }
        public string? DescripcionEstiba { get; set; }
        public int? IdProducto { get; set; }
        public decimal? MetrosCuadrados { get; set; }
        public int? IdUsuario { get; set; }
        public int? Estado { get; set; }
    }
}
