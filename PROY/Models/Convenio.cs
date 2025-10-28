using System.ComponentModel.DataAnnotations;

namespace PROY.Models
{
    public class Convenio
    {
        public Int64 IdConvenio { get; set; }
        public string? NombreConvenio { get; set; }
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "La Vida Util es obligatoria.")]
        [Range(6, int.MaxValue, ErrorMessage = "La Vida Util (Meses) debe ser de 6 o superior.")]
        public int? VidaUtil { get; set; }
        public int? TotalRaciones { get; set; }
        public int Estado { get; set; }
    }
}
