using PROY.Models;

namespace PROY.ViewModels
{
    public class HistorialSolicitudVM
    {
        public Solicitud? Solicitud { get; set; }
        public List<Bitacora>? Historial { get; set; } = new List<Bitacora>();

        // Método para obtener el texto de la acción
    }
}