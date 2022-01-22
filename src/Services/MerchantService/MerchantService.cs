﻿using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Aggregators;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.MerchantModels;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.MerchantRepository;
using CashTrack.Repositories.SubCategoriesRepository.cs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.MerchantService
{
    public class MerchantService : IMerchantService
    {
        private readonly IMapper _mapper;
        private readonly IMerchantRepository _merchantRepo;
        private readonly ISubCategoryRepository _subCategoryRepo;
        private readonly IExpenseRepository _expenseRepo;

        public MerchantService(IMapper mapper, IMerchantRepository merchantRepo, ISubCategoryRepository subCategoryRepo, IExpenseRepository expenseRepo)
        {
            _mapper = mapper;
            _merchantRepo = merchantRepo;
            _subCategoryRepo = subCategoryRepo;
            _expenseRepo = expenseRepo;
        }

        public async Task<MerchantModels.Response> GetMerchantsAsync(MerchantModels.Request request)
        {
            Expression<Func<Merchants, bool>> allMerchants = (Merchants m) => true;
            Expression<Func<Merchants, bool>> merchantSearch = (Merchants m) => m.name.ToLower().Contains(request.SearchTerm.ToLower());

            var predicate = request.SearchTerm == null ? allMerchants : merchantSearch;

            var merchantEntities = await _merchantRepo.FindWithPagination(predicate, request.PageNumber, request.PageSize);

            var count = await _merchantRepo.GetCountOfMerchants(predicate);

            var merchantViewModels = merchantEntities.Select(m => new Merchant
            {
                Id = m.id,
                Name = m.name,
                City = m.city,
                IsOnline = m.is_online,
                NumberOfExpenses = (int)_expenseRepo.GetCountOfExpenses(x => x.merchantid == m.id).Result
            }).ToArray();

            var searchTermResponse = new MerchantModels.Response
            {
                TotalPages = (int)Math.Ceiling(count / request.PageSize),
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalMerchants = await _merchantRepo.GetCountOfMerchants(predicate),
                Merchants = merchantViewModels
            };
            return searchTermResponse;

        }

        public async Task<MerchantDetail> GetMerchantDetailAsync(int id)
        {
            var merchantEntity = await _merchantRepo.FindById(id);

            var merchantExpenses = await _expenseRepo.GetExpensesAndCategories(x => x.merchantid == id);

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

            var subCategories = await _subCategoryRepo.GetAllSubCategoriesAsync();

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

        public async Task<Merchants> CreateUpdateMerchantAsync(AddEditMerchant request)
        {
            var merchants = await _merchantRepo.GetAllMerchantsNoTracking();
            if (merchants.Any(x => x.name == request.Name))
                throw new DuplicateMerchantNameException(request.Name);

            var merchant = _mapper.Map<Merchants>(request);
            //if the request doesn't have an id, it means it's not in the database yet so we are just updating an existing merchant
            var success = false;
            if (request.Id == null)
            {
                //I manually set the id here because when I use the test database it messes with the id autogeneration
                merchant.id = (merchants.OrderBy(x => x.id).LastOrDefault()).id + 1;
                success = await _merchantRepo.Create(merchant);
            }
            else
            {
                success = await _merchantRepo.Update(merchant);
            }

            if (!success)
                throw new Exception("Couldn't save merchant to the database");

            return merchant;
        }

        public async Task<bool> DeleteMerchantAsync(int id)
        {
            var merchant = await _merchantRepo.FindById(id);

            return await _merchantRepo.Delete(merchant);
        }
    }
}
