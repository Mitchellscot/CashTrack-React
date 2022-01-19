using AutoMapper;
using CashTrack.Models.Common;
using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.ExpenseRepository;
using System;
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
        public async Task<ExpenseModels.Response> GetExpenseByIdAsync(int id)
        {
            var singleExpense = await _expenseRepo.GetExpenseById(id);

            var response = new ExpenseModels.Response
            {
                PageNumber = 1,
                TotalPages = 1,
                Expenses = new[] { _mapper.Map<ExpenseTransaction>(singleExpense) }
            };
            return response;
        }
        public async Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request) => request.DateOptions switch
        {
            //1
            DateOptions.All => await GetAllExpensesAsync(request),
            //2
            //Required Begin Date
            DateOptions.SpecificDate => await GetExpensesFromSpecificDateAsync(request),
            //3
            // required begin date
            DateOptions.SpecificMonthAndYear => await GetExpensesForSpecificMonthAsync(request),
            //4
            //requires a begin date
            DateOptions.SpecificQuarter => await GetExpensesForQuarterAsync(request),
            //5
            // Requires begin date
            DateOptions.SpecificYear => await GetExpensesForAYearAsync(request),
            //6
            //begindate and enddate required.
            DateOptions.DateRange => await GetExpensesByDateRange(request),
            //7
            DateOptions.Last30Days => await GetExpensesFromLast30Days(request),
            //8
            DateOptions.CurrentMonth => await GetExpensesFromCurrentMonth(request),
            //9
            DateOptions.CurrentQuarter => await GetExpensesFromCurrentQuarter(request),
            //10
            DateOptions.CurrentYear => await GetExpensesFromCurrentYear(request),

            _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))

        };
        internal async Task<ExpenseModels.Response> GetAllExpensesAsync(ExpenseModels.Request request)
        {
            var expenseTransactions = await _expenseRepo.GetAllExpensesPagination(request.PageNumber, request.PageSize);
            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal async Task<int> GetTotalPages(int pageSize, DateTimeOffset? beginDate = null, DateTimeOffset? endDate = null)
        {
            decimal numberOfRecords = 0;
            if (beginDate == null && endDate == null)
            {
                numberOfRecords = await _expenseRepo.GetCountOfAllExpenses();
            }
            else if (beginDate != null && endDate == null)
            {
                numberOfRecords = await _expenseRepo.GetCountOfExpensesForSpecificDate(beginDate.Value);
            }
            else if (beginDate != null && endDate != null)
            {
                numberOfRecords = await _expenseRepo.GetCountOfExpensesBetweenTwoDates(beginDate.Value, endDate.Value);
            }
            var totalPages = (int)Math.Ceiling(numberOfRecords / (decimal)pageSize);
            return totalPages == 0 ? 1 : totalPages;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromSpecificDateAsync(ExpenseModels.Request request)
        {
            var expenseTransactions = await _expenseRepo.GetExpensesFromSpecificDatePagination(request.BeginDate, request.PageNumber, request.PageSize);
            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, request.BeginDate),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesForSpecificMonthAsync(ExpenseModels.Request request)
        {
            var monthBeginDate = GetMonthBeginDate(request.BeginDate).ToUniversalTime();
            var monthEndDate = GetMonthEndDate(request.BeginDate).ToUniversalTime();

            var expenseTransactions = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(monthBeginDate, monthEndDate, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, monthBeginDate, monthEndDate),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;

        }
        internal DateTimeOffset GetMonthBeginDate(DateTimeOffset date)
        {
            var beginingOfMonth = new DateTimeOffset(
                date.Year, date.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return beginingOfMonth;
        }
        internal DateTimeOffset GetMonthEndDate(DateTimeOffset date)
        {
            var monthEndDate = GetLastDayOfMonth(date);
            var endingOfMonth = new DateTimeOffset(
                date.Year, date.Month, monthEndDate, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return endingOfMonth;
        }
        internal int GetLastDayOfMonth(DateTimeOffset date) => date.Month switch
        {
            4 or 6 or 9 or 11 => 30,
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            2 => DateTime.IsLeapYear(date.Year) ? 29 : 28,
            _ => throw new ArgumentException($"Unable to determine the end of the month {date}", nameof(date.Month))
        };
        internal async Task<ExpenseModels.Response> GetExpensesForQuarterAsync(ExpenseModels.Request request)
        {
            var quarterDates = GetQuarterDatesFromDate(request.BeginDate);

            var expenseTransactions = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(quarterDates.startDate, quarterDates.endDate, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, quarterDates.startDate, quarterDates.endDate),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;

        }
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetQuarterDatesFromDate(DateTimeOffset date) => date.Month switch
        {
            1 or 2 or 3 => (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 3, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            4 or 5 or 6 => (startDate: new DateTimeOffset(date.Year, 4, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 6, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            7 or 8 or 9 => (startDate: new DateTimeOffset(date.Year, 7, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 9, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            10 or 11 or 12 => (startDate: new DateTimeOffset(date.Year, 10, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            _ => throw new ArgumentException($"Unable to determine quarter from given date {date}", nameof(date))
        };
        internal async Task<ExpenseModels.Response> GetExpensesForAYearAsync(ExpenseModels.Request request)
        {
            var yearDates = GetYearDatesFromDate(request.BeginDate);

            var expenseTransactions = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(yearDates.startDate, yearDates.endDate, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, yearDates.startDate, yearDates.endDate),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
            };
            return response;
        }
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetYearDatesFromDate(DateTimeOffset date)
        {
            return (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0)));
        }
        internal async Task<ExpenseModels.Response> GetExpensesByDateRange(ExpenseModels.Request request)
        {
            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(request.BeginDate.ToUniversalTime(), request.EndDate.ToUniversalTime(), request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, request.BeginDate.ToUniversalTime(), request.EndDate.ToUniversalTime()),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromLast30Days(ExpenseModels.Request request)
        {
            var thirtyDaysAgo = DateTimeOffset.UtcNow.AddDays(-30);
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(thirtyDaysAgo, today, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, thirtyDaysAgo, today),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromCurrentMonth(ExpenseModels.Request request)
        {
            var beginMonthDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(beginMonthDate, today, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, beginMonthDate, today),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromCurrentQuarter(ExpenseModels.Request request)
        {
            var beginQuarterDate = GetQuarterDatesFromDate(DateTimeOffset.UtcNow);
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(beginQuarterDate.startDate, today, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, beginQuarterDate.startDate, today),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
        internal async Task<ExpenseModels.Response> GetExpensesFromCurrentYear(ExpenseModels.Request request)
        {
            var beginYearDate = new DateTimeOffset(DateTime.Now.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
            var today = DateTimeOffset.UtcNow;

            var expenses = await _expenseRepo.GetExpensesBetweenTwoDatesPagination(beginYearDate, today, request.PageNumber, request.PageSize);

            var response = new ExpenseModels.Response
            {
                TotalPages = await GetTotalPages(request.PageSize, beginYearDate, today),
                PageNumber = request.PageNumber,
                Expenses = _mapper.Map<ExpenseTransaction[]>(expenses)
            };
            return response;
        }
    }
}