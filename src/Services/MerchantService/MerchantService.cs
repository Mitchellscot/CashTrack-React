using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Aggregators;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.MerchantModels;
using CashTrack.Repositories.MerchantRepository;
using CashTrack.Repositories.SubCategoriesRepository.cs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CashTrack.Services.MerchantService
{
    public class MerchantService : IMerchantService
    {
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepo;
        private readonly ISubCategoryRepository _subCategoryRepository;

        public MerchantService(IMapper mapper, IMerchantRepository merchantRepository, ISubCategoryRepository subCategoryRepository)
        {
            _mapper = mapper;
            _merchantRepo = merchantRepository;
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request)
        {
            if (request.SearchTerm != null)
            {
                var merchants = await _merchantRepo.GetMerchantsPaginationSearchTerm(request.SearchTerm, request.PageSize, request.PageNumber);
                var merchantViewModels = merchants.Select(m => new Merchant
                {
                    Id = m.id,
                    Name = m.name,
                    City = m.city,
                    IsOnline = m.is_online,
                    NumberOfExpenses = _merchantRepo.GetNumberOfExpensesForMerchant(m.id).Result
                }).ToArray();

                var searchTermResponse = new MerchantModels.Response
                {
                    TotalPages = await GetTotalPagesForAllMerchantSearch(request.SearchTerm, request.PageSize),
                    PageNumber = request.PageNumber,
                    Merchants = merchantViewModels
                };
                return searchTermResponse;
            }
            else
            {
                var merchants = await _merchantRepo.GetMerchantsPagination(request.PageSize, request.PageNumber);
                var merchantViewModels = merchants.Select(m => new Merchant
                {
                    Id = m.id,
                    Name = m.name,
                    City = m.city,
                    IsOnline = m.is_online,
                    NumberOfExpenses = _merchantRepo.GetNumberOfExpensesForMerchant(m.id).Result
                }).ToArray();

                var response = new MerchantModels.Response
                {
                    TotalPages = await GetTotalPagesForAllMerchants(request.PageSize),
                    PageNumber = request.PageNumber,
                    Merchants = merchantViewModels
                };
                return response;
            }
        }

        private async Task<int> GetTotalPagesForAllMerchantSearch(string searchTerm, int pageSize)
        {
            var totalNumberOfRecords = await _merchantRepo.GetCountOfAllMerchantsSearch(searchTerm);
            var totalPages = Math.Ceiling(totalNumberOfRecords / pageSize);
            return (int)totalPages;
        }
        private async Task<int> GetTotalPagesForAllMerchants(int pageSize)
        {
            var totalNumberOfRecords = await _merchantRepo.GetCountOfAllMerchants();
            var totalPages = Math.Ceiling(totalNumberOfRecords / pageSize);
            return (int)totalPages;
        }

        public async Task<MerchantDetail> GetMerchantDetailAsync(int id)
        {
            var merchantEntity = await _merchantRepo.GetMerchantById(id);
            //might have to check for exceptions here I don't know, there is a check for them in the repo

            var merchantExpenses = await _merchantRepo.GetExpensesAndCategoriesByMerchantId(id);

            var recentExpenses = merchantExpenses.OrderByDescending(e => e.purchase_date)
                .Take(10)
                .Select(x => new ExpenseQuickView()
                {
                    Id = x.id,
                    PurchaseDate = x.purchase_date.Date.ToShortDateString(),
                    Amount = x.amount,
                    SubCategory = x.category == null ? "none" : x.category.sub_category_name
                }).ToList();

            var expenseTotals = merchantExpenses.Aggregate(new ExpenseTotalsAggregator(),
                    (acc, e) => acc.Accumulate(e),
                    acc => acc.Compute());

            var expenseStatistics = merchantExpenses.GroupBy(e => e.purchase_date.Year)
                    .Select(g =>
                    {
                        var results = g.Aggregate(new ExpenseStatisticsAggregator(),
                            (acc, e) => acc.Accumulate(e),
                            acc => acc.Compute());

                        return new AnnualExpenseStatistics()
                        {
                            Year = g.Key,
                            Average = results.Average,
                            Min = results.Min,
                            Max = results.Max,
                            Total = results.Total,
                            Count = results.Count
                        };
                    }).OrderBy(x => x.Year).ToList();

            var subCategories = await _subCategoryRepository.GetAllSubCategoriesAsync();

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

            var merchantDetail = new MerchantDetail()
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

        public async Task<Merchants> CreateUpdateMerchant(AddEditMerchant request)
        {
            var merchants = await _merchantRepo.GetAllMerchantsNoTracking();
            if (merchants.Any(x => x.name == request.Name))
                throw new DuplicateMerchantNameException(request.Name);

            var merchant = _mapper.Map<Merchants>(request);
            //if the request doesn't have an id, it means it's not in the database yet so we are just updating an existing merchant
            var success = false;
            if (request.Id == null)
            {
                merchant.id = (merchants.OrderBy(x => x.id).LastOrDefault()).id + 1;
                success = await _merchantRepo.CreateMerchant(merchant);
            }
            else
            {
                success = await _merchantRepo.UpdateMerchant(merchant);
            }

            if (!success)
                throw new Exception("Couldn't save merchant");

            return merchant;
        }

        public async Task<bool> DeleteMerchant(int id)
        {
            var merchant = await _merchantRepo.GetMerchantById(id);
            if (merchant == null)
                throw new MerchantNotFoundException(id.ToString());

            var success = await _merchantRepo.DeleteMerchant(merchant);

            if (!success)
                throw new Exception("Unable to delete merchant");

            return success;
        }
    }
}
