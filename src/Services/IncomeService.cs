using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Common;
using CashTrack.Models.IncomeModels;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.IncomeRepository;
using CashTrack.Services.Common;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.IncomeService;

public interface IIncomeService
{
    Task<IncomeResponse> GetIncomeAsync(IncomeRequest request);
}
public class IncomeService : IIncomeService
{
    private readonly IMapper _mapper;
    private readonly IIncomeRepository _repo;
    private readonly IExpenseRepository _expenseRepo;

    public IncomeService(IIncomeRepository repo, IMapper mapper, IExpenseRepository expenseRepo) => (_repo, _mapper, _expenseRepo) = (repo, mapper, expenseRepo);

    public async Task<IncomeResponse> GetIncomeAsync(IncomeRequest request)
    {
        var predicate = GetPredicate(request);
        var expenses = await _repo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
        var count = await _repo.GetCount(predicate);
        var amount = await _repo.GetAmountOfIncome(predicate);

        return new IncomeResponse(request.PageNumber, request.PageSize, count, _mapper.Map<IncomeListItem[]>(expenses), amount);
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
            x.income_date >= DateHelpers.GetQuarterDatesFromDate(request.BeginDate).startDate,
        //5
        DateOptions.SpecificYear => (Incomes x) =>
            x.income_date >= DateHelpers.GetYearDatesFromDate(request.BeginDate).startDate &&
            x.income_date <= DateHelpers.GetYearDatesFromDate(request.EndDate).endDate,
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

    }
}