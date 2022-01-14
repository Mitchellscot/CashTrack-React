using CashTrack.Data.Entities;
using CashTrack.Models.MerchantModels;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MerchantRepository
{
    public interface IMerchantRepository
    {
        Task<bool> Commit();
        Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request);
        //Returns an ENTITY - Because the mapper is in the controller. Might make it easier to test (less dependencies)
        Task<Merchants> GetMerchantByIdAsync(int id);
    }
}
