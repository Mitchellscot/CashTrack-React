using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.SubCategoryModels;
using CashTrack.Repositories;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.SubCategoriesRepository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.SubCategoryService;

public interface ISubCategoryService
{
    Task<SubCategoryModels.Response> GetSubCategoriesAsync(SubCategoryModels.Request request);
}
public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepo;
    private readonly IMapper _mapper;
    private readonly IExpenseRepository _expenseRepo;

    public SubCategoryService(IExpenseRepository expenseRepo, ISubCategoryRepository subCategoryRepo, IMapper mapper) => (_subCategoryRepo, _mapper, _expenseRepo) = (subCategoryRepo, mapper, expenseRepo);

    public async Task<SubCategoryModels.Response> GetSubCategoriesAsync(SubCategoryModels.Request request)
    {
        Expression<Func<SubCategories, bool>> returnAll = (SubCategories s) => true;
        Expression<Func<SubCategories, bool>> searchCategories = (SubCategories s) => s.sub_category_name.ToLower().Contains(request.SearchTerm.ToLower());

        var predicate = request.SearchTerm == null ? returnAll : searchCategories;

        var categories = await _subCategoryRepo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
        var count = await _subCategoryRepo.GetCountOfSubCategories(predicate);

        var categoryViewModels = categories.Select(c => new SubCategoryListItem
        {
            Id = c.id,
            Name = c.sub_category_name,
            MainCategoryName = c.main_category.main_category_name,
            NumberOfExpenses = (int)_expenseRepo.GetCountOfExpenses(x => x.categoryid == c.id).Result
        }).ToArray();

        var response = new SubCategoryModels.Response()
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(count / request.PageSize),
            TotalSubCategories = (int)count,
            SubCategories = categoryViewModels
        };
        return response;
    }
}

