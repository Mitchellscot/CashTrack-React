using CashTrack.Data.Entities;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MerchantRepository
{
    public interface IMerchantRepository
    {
        Task<bool> CreateMerchant(Merchants merchant);
        Task<bool> UpdateMerchant(Merchants merchant);
        Task<bool> DeleteMerchant(Merchants merchant);
        Task<Merchants> GetMerchantByIdAsync(int id);
        Task<Merchants[]> GetMerchantsPaginationAsync(int pageSize, int pageNumber);
        Task<Merchants[]> GetAllMerchantsAsync();
        Task<Merchants[]> GetMerchantsBySearchTermAsync(string searchTerm, int pageSize, int pageNumber);
        //these might go in the Expense repo...
        Task<int> GetNumberOfExpensesForMerchant(int id);
        Task<Expenses[]> GetExpensesAndCategoriesByMerchantIdAsync(int id);
    }
}
