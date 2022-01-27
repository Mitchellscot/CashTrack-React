using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.IncomeCategoryModels;
using CashTrack.Repositories.IncomeCategoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.IncomeCategoryService;

public interface IIncomeCategoryService
{
    Task<IncomeCategoryResponse> GetIncomeCategoriesAsync(IncomeCategoryRequest request);
}
public class IncomeCategoryService : IIncomeCategoryService
{
    private readonly IIncomeCategoryRepository _repo;
    private readonly IMapper _mapper;

    public IncomeCategoryService(IIncomeCategoryRepository repo, IMapper mapper) => (_repo, _mapper) = (repo, mapper);

    public async Task<IncomeCategoryResponse> GetIncomeCategoriesAsync(IncomeCategoryRequest request)
    {
        Expression<Func<IncomeCategories, bool>> returnAll = (IncomeCategories x) => true;
        Expression<Func<IncomeCategories, bool>> searchCategories = (IncomeCategories x) => x.category.ToLower().Contains(request.Query);

        var predicate = request.Query == null ? returnAll : searchCategories;

        var categories = await _repo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
        var count = await _repo.GetCount(predicate);

        var response = new IncomeCategoryResponse(request.PageNumber, request.PageSize, count, _mapper.Map<IncomeCategoryListItem[]>(categories));

        return response;
    }
}

public class IncomeCategoryMapperProfile : Profile
{
    public IncomeCategoryMapperProfile()
    {
        //Maybe add a property on the List Item that associates the item with a number of incomes ???? 
        //You would have to get rid of this map then.
        CreateMap<IncomeCategories, IncomeCategoryListItem>()
            .ForMember(x => x.Id, o => o.MapFrom(src => src.id))
            .ForMember(x => x.Name, o => o.MapFrom(src => src.category))
            .ReverseMap();
    }
}

