using PROY.Models;

namespace PROY.Repositories
{
    public interface IPresentacionPARepository
    {
        Task<List<PresentacionPA>> GetAll();
        Task<PresentacionPA> GetPresentacionPA(Int64 id);
        Task<Int64> Cuenta();
        Task<PresentacionPA> SavePresentacionPA(PresentacionPA modelo);
        Task<PresentacionPA> UpdatePresentacionPA(PresentacionPA modelo);
    }
}
