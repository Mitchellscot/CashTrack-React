using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Common;
using CashTrack.Models.IncomeModels;
using CashTrack.Repositories.IncomeRepository;
using CashTrack.Services.Common;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.IncomeService;

public interface IIncomeService
{
    Task<IncomeResponse> GetIncomeAsync(IncomeRequest request);
    Task<IncomeListItem> GetIncomeByIdAsync(int id);
    Task<AddEditIncome> CreateIncomeAsync(AddEditIncome request);
    Task<bool> UpdateIncomeAsync(AddEditIncome request);
    Task<bool> DeleteIncomeAsync(int id);
}
public class IncomeService : IIncomeService
{
    private readonly IMapper _mapper;
    private readonly IIncomeRepository _repo;

    public IncomeService(IIncomeRepository repo, IMapper mapper) => (_repo, _mapper) = (repo, mapper);

    public async Task<AddEditIncome> CreateIncomeAsync(AddEditIncome request)
    {
        if (request.Id != null)
            throw new ArgumentException("Request must not contain an id in order to create an income.");

        var income = _mapper.Map<Incomes>(request);

        income.id = ((int)await _repo.GetCount(x => true)) + 1;
        var success = await _repo.Create(income);
        if (!success)
            throw new Exception("Couldn't save income to the database.");

        request.Id = income.id;

        return request;
    }

    public async Task<bool> DeleteIncomeAsync(int id)
    {
        var income = await _repo.FindById(id);

        return await _repo.Delete(income);
    }

    public async Task<IncomeResponse> GetIncomeAsync(IncomeRequest request)
    {
        var predicate = GetPredicate(request);
        var expenses = await _repo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
        var count = await _repo.GetCount(predicate);
        var amount = await _repo.GetAmountOfIncome(predicate);

        return new IncomeResponse(request.PageNumber, request.PageSize, count, _mapper.Map<IncomeListItem[]>(expenses), amount);
    }

    public async Task<IncomeListItem> GetIncomeByIdAsync(int id)
    {
        //Change this to income detail in the future, once you know what you want it to look like.
        var singleExpense = await _repo.FindById(id);
        return _mapper.Map<IncomeListItem>(singleExpense);
    }

    public async Task<bool> UpdateIncomeAsync(AddEditIncome request)
    {
        if (request.Id == null)
            throw new ArgumentException("Need an id to update an income");

        var income = _mapper.Map<Incomes>(request);
        return await _repo.Update(income);
    }

    /***** HELPERS *****/
    internal Expression<Func<Incomes, bool>> GetPredicate(IncomeRequest request) => request.DateOptions switch
    {
        //1
        DateOptions.All => (Incomes x) => true,
        //2
        DateOptions.SpecificDate => (Incomes x) =>
            x.income_date == request.BeginDate.ToUniversalTime(),
        //3
        DateOptions.SpecificMonthAndYear => (Incomes x) =>
           x.income_date >= DateHelpers.GetMonthDatesFromDate(request.BeginDate).startDate &&
           x.income_date <= DateHelpers.GetMonthDatesFromDate(request.BeginDate).endDate,
        //4
        DateOptions.SpecificQuarter => (Incomes x) =>
            x.income_date >= DateHelpers.GetQuarterDatesFromDate(request.BeginDate).startDate &&
            x.income_date <= DateHelpers.GetQuarterDatesFromDate(request.BeginDate).endDate,
        //5
        DateOptions.SpecificYear => (Incomes x) =>
            x.income_date >= DateHelpers.GetYearDatesFromDate(request.BeginDate).startDate &&
            x.income_date <= DateHelpers.GetYearDatesFromDate(request.BeginDate).endDate,
        //6
        DateOptions.DateRange => (Incomes x) =>
            x.income_date >= request.BeginDate.ToUniversalTime() &&
            x.income_date <= request.EndDate.ToUniversalTime(),
        //7
        DateOptions.Last30Days => (Incomes x) =>
            x.income_date >= DateTimeOffset.UtcNow.AddDays(-30),
        //8
        DateOptions.CurrentMonth => (Incomes x) =>
            x.income_date >= DateHelpers.GetCurrentMonth() &&
            x.income_date <= DateTimeOffset.UtcNow,
        //9
        DateOptions.CurrentQuarter => (Incomes x) =>
            x.income_date >= DateHelpers.GetCurrentQuarter() &&
            x.income_date <= DateTimeOffset.UtcNow,
        //10
        DateOptions.CurrentYear => (Incomes x) =>
            x.income_date >= DateHelpers.GetCurrentYear() &&
            x.income_date <= DateTimeOffset.UtcNow,

        _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))

    };
}

public class IncomeMapperProfile : Profile
{
    public IncomeMapperProfile()
    {
        CreateMap<Incomes, IncomeListItem>()
            .ForMember(x => x.Id, o => o.MapFrom(x => x.id))
            .ForMember(x => x.IncomeDate, o => o.MapFrom(x => x.income_date))
            .ForMember(x => x.Amount, o => o.MapFrom(x => x.amount))
            .ForMember(x => x.Category, o => o.MapFrom(x => x.category.category))
            .ForMember(x => x.Source, o => o.MapFrom(x => x.source.source))
            .ReverseMap();

        CreateMap<AddEditIncome, Incomes>()
            .ForMember(x => x.income_date, o => o.MapFrom(src => src.IncomeDate.ToUniversalTime()))
            .ForMember(x => x.id, o => o.MapFrom(src => src.Id))
            .ForMember(x => x.amount, o => o.MapFrom(src => src.Amount))
            .ForMember(x => x.categoryid, o => o.MapFrom(src => src.CategoryId))
            .ForMember(x => x.sourceid, o => o.MapFrom(src => src.SourceId))
            .ForMember(x => x.notes, o => o.MapFrom(src => src.Notes))
            .ReverseMap();
    }
}