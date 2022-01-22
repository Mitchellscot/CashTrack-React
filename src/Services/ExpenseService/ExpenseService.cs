using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Common;
using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.ExpenseRepository;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepo;
        private readonly IMapper _mapper;

        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepo = expenseRepository;
            _mapper = mapper;
        }
        public async Task<ExpenseTransaction> GetExpenseByIdAsync(int id)
        {
            var singleExpense = await _expenseRepo.GetExpenseById(id);

            return _mapper.Map<ExpenseTransaction>(singleExpense);
        }
        public async Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request)
        {
            var predicate = GetPredicate(request);
            var expenses = await _expenseRepo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
            var countOfExpenses = await _expenseRepo.GetCountOfExpenses(predicate);

            return BuildResponse(expenses, countOfExpenses, request.PageNumber, request.PageSize);
        }
        public async Task<ExpenseModels.Response> GetExpensesByNotesAsync(ExpenseModels.NotesSearchRequest request)
        {
            Expression<Func<Expenses, bool>> predicate = x => x.notes.Contains(request.SearchTerm);
            var expenses = await _expenseRepo.FindWithPagination(predicate, request.PageNumber, request.PageSize);
            var count = await _expenseRepo.GetCountOfExpenses(predicate);

            return BuildResponse(expenses, count, request.PageNumber, request.PageSize);
        }
        public async Task<ExpenseModels.Response> GetExpensesByAmountAsync(ExpenseModels.AmountSearchRequest request)
        {
            Expression<Func<Expenses, bool>> predicate = x => x.amount == request.Query;
            var expenses = await _expenseRepo.FindWithPagination(x => x.amount == request.Query, request.PageNumber, request.PageSize);
            var count = await _expenseRepo.GetCountOfExpenses(predicate);

            return BuildResponse(expenses, count, request.PageNumber, request.PageSize);

        }
        public async Task<Expenses> CreateUpdateExpenseAsync(AddEditExpense request)
        {
            var expense = _mapper.Map<Expenses>(request);

            var success = false;
            if (request.Id == null)
            {
                //I manually set the id here because when I use the test database it messes with the id autogeneration
                expense.id = ((int)await _expenseRepo.GetCountOfExpenses(x => true)) + 1;
                success = await _expenseRepo.Create(expense);
            }
            else
            {
                success = await _expenseRepo.Update(expense);
            }

            if (!success)
                throw new Exception("Couldn't save expense to the database.");

            return expense;
        }
        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expense = await _expenseRepo.GetExpenseById(id);

            return await _expenseRepo.Delete(expense);
        }

        /***** HELPERS *****/
        internal ExpenseModels.Response BuildResponse(Expenses[] expenses, decimal countOfExpenses, int pageNumber, int pageSize)
        {
            var response = new ExpenseModels.Response
            {
                TotalPages = (int)Math.Ceiling(countOfExpenses / (decimal)pageSize),
                PageSize = pageSize,
                TotalExpenses = (int)countOfExpenses,
                PageNumber = pageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal Expression<Func<Expenses, bool>> GetPredicate(ExpenseModels.Request request) => request.DateOptions switch
        {
            //1
            DateOptions.All => (Expenses x) => true,
            //2
            DateOptions.SpecificDate => (Expenses x) =>
                x.purchase_date == request.BeginDate.ToUniversalTime(),
            //3
            DateOptions.SpecificMonthAndYear => (Expenses x) =>
               x.purchase_date >= GetMonthDatesFromDate(request.BeginDate).startDate &&
               x.purchase_date <= GetMonthDatesFromDate(request.BeginDate).endDate,
            //4
            DateOptions.SpecificQuarter => (Expenses x) =>
                x.purchase_date >= GetQuarterDatesFromDate(request.BeginDate).startDate,
            //5
            DateOptions.SpecificYear => (Expenses x) =>
                x.purchase_date >= GetYearDatesFromDate(request.BeginDate).startDate &&
                x.purchase_date <= GetYearDatesFromDate(request.EndDate).endDate,
            //6
            DateOptions.DateRange => (Expenses x) =>
                x.purchase_date >= request.BeginDate.ToUniversalTime() &&
                x.purchase_date <= request.EndDate.ToUniversalTime(),
            //7
            DateOptions.Last30Days => (Expenses x) =>
                x.purchase_date >= DateTimeOffset.UtcNow.AddDays(-30),
            //8
            DateOptions.CurrentMonth => (Expenses x) =>
                x.purchase_date >= new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0)) &&
                x.purchase_date <= DateTimeOffset.UtcNow,
            //9
            DateOptions.CurrentQuarter => (Expenses x) =>
                x.purchase_date >= GetQuarterDatesFromDate(DateTime.UtcNow).startDate &&
                x.purchase_date <= DateTimeOffset.UtcNow,
            //10
            DateOptions.CurrentYear => (Expenses x) =>
                x.purchase_date >= new DateTimeOffset(DateTime.Now.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)) &&
                x.purchase_date <= DateTimeOffset.UtcNow,

            _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))

        };
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetMonthDatesFromDate(DateTimeOffset date)
        {
            var beginingOfMonth = new DateTimeOffset(
                date.Year, date.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            var monthEndDate = GetLastDayOfMonth(date);
            var endingOfMonth = new DateTimeOffset(
                date.Year, date.Month, monthEndDate, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return (beginingOfMonth.ToUniversalTime(), endingOfMonth.ToUniversalTime());
        }
        internal int GetLastDayOfMonth(DateTimeOffset date) => date.Month switch
        {
            4 or 6 or 9 or 11 => 30,
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            2 => DateTime.IsLeapYear(date.Year) ? 29 : 28,
            _ => throw new ArgumentException($"Unable to determine the end of the month {date}", nameof(date.Month))
        };
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetQuarterDatesFromDate(DateTimeOffset date) => date.Month switch
        {
            1 or 2 or 3 => (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 3, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            4 or 5 or 6 => (startDate: new DateTimeOffset(date.Year, 4, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 6, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            7 or 8 or 9 => (startDate: new DateTimeOffset(date.Year, 7, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 9, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            10 or 11 or 12 => (startDate: new DateTimeOffset(date.Year, 10, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            _ => throw new ArgumentException($"Unable to determine quarter from given date {date}", nameof(date))
        };
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetYearDatesFromDate(DateTimeOffset date)
        {
            return (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0)));
        }
    }
}