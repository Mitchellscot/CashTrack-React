using CashTrack.Data.Entities;
using CashTrack.Models.MerchantModels;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MerchantRepository
{
    public interface IMerchantRepository
    {
        Task<bool> Commit();
        Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request);
        Task<MerchantDetail> GetMerchantDetailAsync(int id);
        Task<Merchants> CreateMerchant(AddEditMerchant request);
    }
}
