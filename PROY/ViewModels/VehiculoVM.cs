using PROY.Models;

namespace PROY.ViewModels
{
    public class VehiculoVM
    {
        public Vehiculo Vehiculo { get; set; }
        public List<Vehiculo> listaVehiculos { get; set; }
        public Dictionary<Int64, string>? TransportistaDict { get; set; }
    }
}
