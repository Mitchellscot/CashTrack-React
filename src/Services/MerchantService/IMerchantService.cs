using CashTrack.Data.Entities;
using CashTrack.Models.MerchantModels;
using System.Threading.Tasks;

namespace CashTrack.Services.MerchantService
{
    public interface IMerchantService
    {
        Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request);
        Task<MerchantDetail> GetMerchantDetailAsync(int id);
        Task<Merchants> CreateUpdateMerchant(AddEditMerchant request);
        Task<bool> DeleteMerchant(int id);
    }
}
