using CashTrack.Data.Entities;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MerchantRepository
{
    public interface IMerchantRepository
    {
        Task<bool> CreateMerchant(Merchants merchant);
        Task<bool> UpdateMerchant(Merchants merchant);
        Task<bool> DeleteMerchant(Merchants merchant);
        Task<Merchants> GetMerchantById(int id);
        Task<Merchants[]> GetMerchantsPagination(int pageSize, int pageNumber);
        Task<Merchants[]> GetMerchantsPaginationSearchTerm(string searchTerm, int pageSize, int pageNumber);
        Task<decimal> GetCountOfAllMerchants();
        Task<decimal> GetCountOfAllMerchantsSearch(string searchTerm);
        Task<Merchants[]> GetAllMerchantsNoTracking();
    }
}
