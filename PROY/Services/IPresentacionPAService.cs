using PROY.Models;

namespace PROY.Services
{
    public interface IPresentacionPAService
    {
        Task<List<PresentacionPA>> GetAll();
        Task<PresentacionPA> GetPresentacionPA(Int64 id);
        Task<ServiceResponse<PresentacionPA>> SavePresentacionPA(PresentacionPA modelo);
        Task<ServiceResponse<PresentacionPA>> UpdatePresentacionPA(PresentacionPA modelo);
        Task<ServiceResponse<PresentacionPA>> DeletePresentacionPA(Int64 id);
    }
}
