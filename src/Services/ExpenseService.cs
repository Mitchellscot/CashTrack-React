﻿using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Common;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.TagModels;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Services.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService;

public interface IExpenseService
{
    Task<ExpenseListItem> GetExpenseByIdAsync(int id);
    Task<ExpenseResponse> GetExpensesAsync(ExpenseRequest request);
    Task<ExpenseResponse> GetExpensesByNotesAsync(ExpenseRequest request);
    Task<ExpenseResponse> GetExpensesByAmountAsync(AmountSearchRequest request);
    Task<Expenses> CreateExpenseAsync(AddEditExpense request);
    Task<bool> UpdateExpenseAsync(AddEditExpense request);
    Task<bool> DeleteExpenseAsync(int id);
}
public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepo;
    private readonly IMapper _mapper;

    public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
    {
        _expenseRepo = expenseRepository;
        _mapper = mapper;
    }
    public async Task<ExpenseListItem> GetExpenseByIdAsync(int id)
    {
        //This would get displayed in a modal... doesn't need to be a lot of info, just enough to edit or whatever.
        var singleExpense = await _expenseRepo.FindById(id);
        return _mapper.Map<ExpenseListItem>(singleExpense);
    }
    public async Task<ExpenseResponse> GetExpensesAsync(ExpenseRequest request)
    {
        var predicate = GetPredicate(request);
        var expenses = await _expenseRepo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
        var count = await _expenseRepo.GetCount(predicate);
        var amount = await _expenseRepo.GetAmountOfExpenses(predicate);

        return new ExpenseResponse(request.PageNumber, request.PageSize, count, _mapper.Map<ExpenseListItem[]>(expenses), amount);
    }
    public async Task<ExpenseResponse> GetExpensesByNotesAsync(ExpenseRequest request)
    {
        Expression<Func<Expenses, bool>> predicate = x => x.notes.ToLower().Contains(request.Query.ToLower());
        var expenses = await _expenseRepo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
        var count = await _expenseRepo.GetCount(predicate);
        var amount = await _expenseRepo.GetAmountOfExpenses(predicate);

        return new ExpenseResponse(request.PageNumber, request.PageSize, count, _mapper.Map<ExpenseListItem[]>(expenses), amount);
    }
    public async Task<ExpenseResponse> GetExpensesByAmountAsync(AmountSearchRequest request)
    {
        Expression<Func<Expenses, bool>> predicate = x => x.amount == request.Query;
        var expenses = await _expenseRepo.FindWithPagination(x => x.amount == request.Query, request.PageNumber, request.PageSize);
        var count = await _expenseRepo.GetCount(predicate);
        var amount = await _expenseRepo.GetAmountOfExpenses(predicate);
        return new ExpenseResponse(request.PageNumber, request.PageSize, count, _mapper.Map<ExpenseListItem[]>(expenses), amount);
    }
    public async Task<Expenses> CreateExpenseAsync(AddEditExpense request)
    {
        if (request.Id != null)
            throw new ArgumentException("Request must not contain an id in order to create an expense.");

        var expense = _mapper.Map<Expenses>(request);
        //I manually set the id here because when I use the test database it messes with the id autogeneration
        expense.id = ((int)await _expenseRepo.GetCount(x => true)) + 1;
        var success = await _expenseRepo.Create(expense);
        if (!success)
            throw new Exception("Couldn't save expense to the database.");

        return expense;
    }
    public async Task<bool> UpdateExpenseAsync(AddEditExpense request)
    {
        if (request.Id == null)
            throw new ArgumentException("Need an id to update an expense");

        var expense = _mapper.Map<Expenses>(request);
        return await _expenseRepo.Update(expense);
    }
    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _expenseRepo.FindById(id);

        return await _expenseRepo.Delete(expense);
    }
    /***** HELPERS *****/
    internal Expression<Func<Expenses, bool>> GetPredicate(ExpenseRequest request) => request.DateOptions switch
    {
        //1
        DateOptions.All => (Expenses x) => true,
        //2
        DateOptions.SpecificDate => (Expenses x) =>
            x.purchase_date == request.BeginDate.ToUniversalTime(),
        //3
        DateOptions.SpecificMonthAndYear => (Expenses x) =>
           x.purchase_date >= DateHelpers.GetMonthDatesFromDate(request.BeginDate).startDate &&
           x.purchase_date <= DateHelpers.GetMonthDatesFromDate(request.BeginDate).endDate,
        //4
        DateOptions.SpecificQuarter => (Expenses x) =>
            x.purchase_date >= DateHelpers.GetQuarterDatesFromDate(request.BeginDate).startDate,
        //5
        DateOptions.SpecificYear => (Expenses x) =>
            x.purchase_date >= DateHelpers.GetYearDatesFromDate(request.BeginDate).startDate &&
            x.purchase_date <= DateHelpers.GetYearDatesFromDate(request.EndDate).endDate,
        //6
        DateOptions.DateRange => (Expenses x) =>
            x.purchase_date >= request.BeginDate.ToUniversalTime() &&
            x.purchase_date <= request.EndDate.ToUniversalTime(),
        //7
        DateOptions.Last30Days => (Expenses x) =>
            x.purchase_date >= DateTimeOffset.UtcNow.AddDays(-30),
        //8
        DateOptions.CurrentMonth => (Expenses x) =>
            x.purchase_date >= DateHelpers.GetCurrentMonth() &&
            x.purchase_date <= DateTimeOffset.UtcNow,
        //9
        DateOptions.CurrentQuarter => (Expenses x) =>
            x.purchase_date >= DateHelpers.GetCurrentQuarter() &&
            x.purchase_date <= DateTimeOffset.UtcNow,
        //10
        DateOptions.CurrentYear => (Expenses x) =>
            x.purchase_date >= DateHelpers.GetCurrentYear() &&
            x.purchase_date <= DateTimeOffset.UtcNow,

        _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))

    };
}
public class ExpenseMapperProfile : Profile
{
    public ExpenseMapperProfile()
    {

        CreateMap<Expenses, ExpenseListItem>()
            .ForMember(e => e.Id, o => o.MapFrom(src => src.id))
            .ForMember(e => e.PurchaseDate, o => o.MapFrom(src => src.purchase_date))
            .ForMember(e => e.Amount, o => o.MapFrom(src => src.amount))
            .ForMember(e => e.Notes, o => o.MapFrom(src => src.notes))
            .ForMember(e => e.Merchant, o => o.MapFrom(src => src.merchant.name))
            .ForMember(e => e.SubCategory, o => o.MapFrom(src => src.category.sub_category_name))
            .ForMember(e => e.MainCategory, o => o.MapFrom(src => src.category.main_category.main_category_name))
            .ForMember(e => e.Tags, o => o.MapFrom(
                src => src.expense_tags.Select(a => new TagModel() { Id = a.tag_id, TagName = a.tag.tag_name })));

        CreateMap<AddEditExpense, Expenses>()
            .ForMember(e => e.id, o => o.MapFrom(src => src.Id))
            .ForMember(e => e.purchase_date, o => o.MapFrom(src => src.PurchaseDate.ToUniversalTime()))
            .ForMember(e => e.amount, o => o.MapFrom(src => src.Amount))
            .ForMember(e => e.notes, o => o.MapFrom(src => src.Notes))
            .ForMember(e => e.merchantid, o => o.MapFrom(src => src.MerchantId))
            .ForMember(e => e.categoryid, o => o.MapFrom(src => src.SubCategoryId))
            .ReverseMap();

        //goes it Tags Service when created
        CreateMap<Tags, TagModel>()
            .ForMember(t => t.Id, o => o.MapFrom(src => src.id))
            .ForMember(t => t.TagName, o => o.MapFrom(src => src.tag_name))
            .ReverseMap();
    }
}
