namespace PROY.Models
{
    public class PersonalBodega
    {
        public Int64 IdPersonalBodega { get; set; }
        public string? Dpi { get; set; }
        public string? Nit { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public int? IdPuesto { get; set; }
        public int? IdRenglonPresupuestario { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public int? IdBodega { get; set; }
        public int? IdUsuario { get; set; }
        public int Estado { get; set; }
    }
}
