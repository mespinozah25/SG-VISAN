namespace PROY.Models
{
    public class SubBodega
    {
        public SubBodega() {}

        public SubBodega(int IdBodega)
        {
            this.IdBodega = IdBodega;
        }

        public Int64 IdSubBodega { get; set; }
        public string? NombreSubBodega { get; set; }
        public int? IdBodega { get; set; }
        public int Estado { get; set; }
    }
}
