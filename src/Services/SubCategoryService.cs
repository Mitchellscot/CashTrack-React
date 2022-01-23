using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
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
    Task<SubCategoryDetail> GetSubCategoryDetailsAsync(int id);
    Task<SubCategories> CreateSubCategoryAsync(AddEditSubCategory request);
    Task<bool> UpdateSubCategoryAsync(AddEditSubCategory request);
    Task<bool> DeleteSubCategoryAsync(int id);
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
    public async Task<SubCategories> CreateSubCategoryAsync(AddEditSubCategory request)
    {
        var categories = await _subCategoryRepo.Find(x => true);
        if (categories.Any(x => x.sub_category_name == request.Name))
            throw new DuplicateCategoryNameException(request.Name);

        var subCategoryEntity = _mapper.Map<SubCategories>(request);

        if (!await _subCategoryRepo.Create(subCategoryEntity))
            throw new Exception("Couldn't save category to the database");

        return subCategoryEntity;
    }
    public async Task<bool> UpdateSubCategoryAsync(AddEditSubCategory request)
    {
        var category = _mapper.Map<SubCategories>(request);
        return await _subCategoryRepo.Update(category);
    }
    public async Task<bool> DeleteSubCategoryAsync(int id)
    {
        var category = await _subCategoryRepo.FindById(id);
        return await _subCategoryRepo.Delete(category);
    }
    public Task<SubCategoryDetail> GetSubCategoryDetailsAsync(int id)
    {
        throw new NotImplementedException();
    }
}

public class SubCategoryMapperProfile : Profile
{
    public SubCategoryMapperProfile()
    {
        CreateMap<SubCategories, SubCategoryListItem>()
            .ForMember(c => c.Id, o => o.MapFrom(src => src.id))
            .ForMember(c => c.Name, o => o.MapFrom(src => src.sub_category_name))
            .ForMember(c => c.MainCategoryName, o => o.MapFrom(src => src.main_category.main_category_name))
            .ForMember(c => c.Id, o => o.MapFrom(src => src.id))
            .ReverseMap();

        CreateMap<AddEditSubCategory, SubCategories>()
            .ForMember(c => c.id, o => o.MapFrom(src => src.Id))
            .ForMember(c => c.sub_category_name, o => o.MapFrom(src => src.Name))
            .ForMember(c => c.main_categoryid, o => o.MapFrom(src => src.MainCategoryId))
            .ForMember(c => c.notes, o => o.MapFrom(src => src.Notes))
            .ForMember(c => c.in_use, o => o.MapFrom(src => src.InUse))
            .ReverseMap();
    }
}