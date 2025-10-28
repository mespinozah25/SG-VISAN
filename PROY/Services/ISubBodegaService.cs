using PROY.Models;

namespace PROY.Services
{
    public interface ISubBodegaService
    {
        Task<List<SubBodega>> GetAll();
        Task<List<SubBodega>> ObtenerSegunBodega(int idBodega);
        Task<SubBodega> GetSubBodega(Int64 id);
        Task<ServiceResponse<SubBodega>> SaveSubBodega(SubBodega modelo);
        Task<ServiceResponse<SubBodega>> UpdateSubBodega(SubBodega modelo);
        Task<ServiceResponse<SubBodega>> DeleteSubBodega(Int64 id);
    }
}
