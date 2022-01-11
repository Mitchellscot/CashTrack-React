using AutoMapper;
using CashTrack.Data;
using CashTrack.Models.MerchantModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MerchantRepository
{
    public class MerchantRepository : IMerchantRepository
    {
        private AppDbContext _context;
        private ILogger<MerchantRepository> _logger;
        private IMapper _mapper;

        public MerchantRepository(AppDbContext context, ILogger<MerchantRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<bool> Commit()
        {
            //only return if more than one row was affected
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request)
        {
            throw new System.NotImplementedException();
            //just get all the merchants. Name and Id I suppose. Alphabetical order. 
            //add a search controller too I suppose (search by name)
        }

        public Task<MerchantModels.Merchant> GetMerchantsByIdAsync(int id)
        {
            throw new System.NotImplementedException();
            //It would be cool to get:
            //Merchant details (duh)
            //A list of all purchases at this merchant
            //A total amount spent at this merchant
            //Average spent at this merchant
            //Maybe a stats class that includes total, average, amount spent by year, etc.... do that later.
        }
    }
}
