namespace PROY.Models.TableroDeDatos
{
    public class BodegaProducto
    {
        public string NombreBodega { get; set; }
        public string NombrePA { get; set; }
        public decimal Existencias { get; set; }
    }

    public class Racion
    {
        public string NombrePrograma { get; set; }
        public decimal TotalRaciones { get; set; }
    }
}