using AutoMapper;
using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.ExpenseModels;
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
                        .Select(m => new MerchantModels.Merchant
                        {
                            Id = m.id,
                            Name = m.name,
                            City = m.city,
                            IsOnline = m.is_online,
                            NumberOfExpenses = _context.Expenses.Count(x => x.merchant.id == m.id)
                        })
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
                    .Select(m => new MerchantModels.Merchant
                    {
                        Id = m.id,
                        Name = m.name,
                        City = m.city,
                        IsOnline = m.is_online,
                        NumberOfExpenses = _context.Expenses.Count(x => x.merchant.id == m.id)
                    })
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

        public async Task<MerchantModels.MerchantDetail> GetMerchantByIdAsync(int id)
        {
            var merchantEntity = await _context.Merchants.SingleOrDefaultAsync(x => x.id == id);

            if (merchantEntity == null)
                throw new MerchantNotFoundException(id.ToString());

            var merchantExpenses = await _context.Expenses.Where(e => e.merchant.id == id).Include(x => x.category).ToListAsync();
            var recentExpenses = merchantExpenses.OrderByDescending(e => e.purchase_date)
                .Take(10)
                .Select(x => new ExpenseModels.ExpenseQuickView(){
                    Id = x.id,
                    PurchaseDate = x.purchase_date.Date.ToShortDateString(),
                    Amount = x.amount,
                    //this isn't working right
                    SubCategory =  x.category == null ? "none" : x.category.sub_category_name
                }).ToList();

            var expenseTotals = merchantExpenses.Aggregate(new ExpenseModels.ExpenseTotalsAggregator(),
                    (acc, e) => acc.Accumulate(e),
                    acc => acc.Compute());

            var expenseStatistics = merchantExpenses.GroupBy(e => e.purchase_date.Year)
                    .Select(g =>
                        {
                            var results = g.Aggregate(
                                                new ExpenseModels.ExpenseStatisticsAggregator(),
                                (acc, e) => acc.Accumulate(e),
                                acc => acc.Compute()
                                                 );
                            return new ExpenseModels.AnnualExpenseStatistics()
                            {
                                Year = g.Key,
                                Average = results.Average,
                                Min = results.Min,
                                Max = results.Max,
                                Total = results.Total,
                                Count = results.Count
                            };
                        }).OrderBy(x => x.Year).ToList();

            var subCategories = await _context.ExpenseSubCategories.ToListAsync();

            var merchantExpenseCategories = subCategories.GroupJoin(merchantExpenses,
                c => c.id, e => e.category.id, (c, g) => new
                {
                    Category = c.sub_category_name,
                    Expenses = g
                }).Select(x => new
                {
                    Category = x.Category,
                    Count = x.Expenses.Count()
                }).Where(x => x.Count > 0).OrderByDescending(x => x.Count).ToDictionary(k => k.Category, v => v.Count);

            var merchantExpenseAmounts = subCategories.GroupJoin(merchantExpenses,
                c => c.id, e => e.category.id, (c, g) => new
                {
                    Category = c.sub_category_name,
                    Expenses = g
                }).Select(x => new
                {
                    Category = x.Category,
                    Sum = x.Expenses.Sum(e => e.amount)
                }).Where(x => x.Sum > 0).OrderByDescending(x => x.Sum).ToDictionary(k => k.Category, v => v.Sum);

            var mostUsedCategory = merchantExpenseCategories.FirstOrDefault().Key;

            var merchantDetail = new MerchantModels.MerchantDetail()
            {
                Id = merchantEntity.id,
                Name = merchantEntity.name,
                SuggestOnLookup = merchantEntity.suggest_on_lookup,
                City = merchantEntity.city,
                State = merchantEntity.state,
                Notes = merchantEntity.notes,
                IsOnline = merchantEntity.is_online,
                ExpenseTotals = expenseTotals,
                MostUsedCategory = mostUsedCategory,
                AnnualExpenseStatistics = expenseStatistics,
                PurchaseCategoryOccurances = merchantExpenseCategories,
                PurchaseCategoryTotals = merchantExpenseAmounts,
                RecentExpenses = recentExpenses
            };

            return merchantDetail;
        }

    }
}
