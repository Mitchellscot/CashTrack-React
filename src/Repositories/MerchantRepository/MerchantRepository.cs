using AutoMapper;
using CashTrack.Data;
using CashTrack.Models.MerchantModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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

        public async Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request)
        {
            if (request.SearchTerm != null)
            {
                try
                {
                    var merchants = await _context.Merchants
                        .Where(x => x.name.ToLower().Contains(request.SearchTerm.ToLower()))
                        .OrderBy(x => x.name)
                        .Skip((request.PageNumber - 1) * request.PageSize)
                        .Take(request.PageSize)
                        .ToArrayAsync();
                    var response = new MerchantModels.Response
                    {
                        TotalPages = await GetTotalPagesForAllMerchantSearch(request.PageSize, request.SearchTerm),
                        PageNumber = request.PageNumber,
                        Merchants = _mapper.Map<MerchantModels.Merchant[]>(merchants)
                    };
                    return response;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            try
            {
                var merchants = await _context.Merchants
                    .OrderBy(x => x.name)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToArrayAsync();
                var response = new MerchantModels.Response
                {
                    TotalPages = await GetTotalPagesForAllMerchants(request.PageSize),
                    PageNumber = request.PageNumber,
                    Merchants = _mapper.Map<MerchantModels.Merchant[]>(merchants)
                };         
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForAllMerchantSearch(int pageSize, string searchTerm)
        {
            var query = await _context.Merchants.Where(x => x.name.ToLower().Contains(searchTerm.ToLower())).ToArrayAsync();
            var totalNumberOfRecords = (decimal)query.Count();
            var totalPages = Math.Ceiling(totalNumberOfRecords / pageSize);
            return (int)totalPages;
        }
        private async Task<int> GetTotalPagesForAllMerchants(int pageSize)
        { 
            var query = await _context.Merchants.ToArrayAsync();
            var totalNumberOfRecords = (decimal)query.Count();
            var totalPages = Math.Ceiling(totalNumberOfRecords / pageSize);
            return (int)totalPages;
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
        //ideas on merchant stats...
        //Total all time, total this year, average all time, average this year, number of times shopped there, 
        //and then per year... so min max av per year (advanced stats I suppose) 
    }
}
