using CashTrack.Data.Entities;
using System.Threading.Tasks;
using CashTrack.Repositories;
using System.Linq.Expressions;
using System;

namespace CashTrack.Repositories.MerchantRepository
{
    public interface IMerchantRepository : IRepository<Merchants>
    {
        Task<decimal> GetCountOfMerchants(Expression<Func<Merchants, bool>> predicate);
        Task<Merchants[]> GetAllMerchantsNoTracking();
    }
}
