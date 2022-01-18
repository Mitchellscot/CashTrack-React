using CashTrack.Data;
using CashTrack.Models.ExpenseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using CashTrack.Helpers.Exceptions;
using CashTrack.Models.Common;
using AutoMapper;

namespace CashTrack.Repositories.ExpenseRepository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ExpenseRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Commit()
        {
            //only return if more than one row was affected
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<ExpenseModels.Response> GetExpenseByIdAsync(int id)
        {
            var singleExpense = await _context.Expenses
                .Include(x => x.expense_tags)
                .ThenInclude(x => x.tag)
                .Include(x => x.merchant)
                .Include(x => x.category)
                .ThenInclude(x => x.main_category)
                .SingleOrDefaultAsync(x => x.id == id);
            if (singleExpense == null)
            {
                throw new ExpenseNotFoundException(id.ToString());
            }
            var response = new ExpenseModels.Response
            {
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
            DateOptions.SpecificMonthAndYear => await GetExpensesFromMonthAndYearAsync(request),
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

        private async Task<ExpenseModels.Response> GetAllExpensesAsync(ExpenseModels.Request request)
        {
            try
            {

                var expenseTransactions = await _context.Expenses
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForAllExpenses(request.PageSize),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForAllExpenses(int pageSize)
        {
            var query = await _context.Expenses.ToArrayAsync();
            var totalNumberOfRecords = (decimal)query.Count();
            var totalPages = Math.Ceiling(totalNumberOfRecords / pageSize);
            return (int)totalPages;
        }

        private async Task<ExpenseModels.Response> GetExpensesFromSpecificDateAsync(ExpenseModels.Request request)
        {
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date == request.BeginDate.ToUniversalTime())
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForSpecificDate(request.PageSize, request.BeginDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForSpecificDate(int pageSize, DateTimeOffset date)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date == date.ToUniversalTime())
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<ExpenseModels.Response> GetExpensesFromMonthAndYearAsync(ExpenseModels.Request request)
        {
            var monthBeginDate = GetMonthBeginDate(request.BeginDate).ToUniversalTime();
            var monthEndDate = GetMonthEndDate(request.BeginDate).ToUniversalTime();
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date >= monthBeginDate && x.purchase_date <= monthEndDate)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForMonthAndYear(request.PageSize, monthBeginDate, monthEndDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForMonthAndYear(int pageSize, DateTimeOffset monthBeginDate, DateTimeOffset monthEndDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date >= monthBeginDate && x.purchase_date <= monthEndDate)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private DateTimeOffset GetMonthBeginDate(DateTimeOffset date)
        {
            var beginingOfMonth = new DateTimeOffset(
                date.Year, date.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return beginingOfMonth;
        }
        private DateTimeOffset GetMonthEndDate(DateTimeOffset date)
        {
            var monthEndDate = GetLastDayOfMonth(date);
            var endingOfMonth = new DateTimeOffset(
                date.Year, date.Month, monthEndDate, 0, 0, 0, new TimeSpan(0, 0, 0)
                );
            return endingOfMonth;
        }
        private int GetLastDayOfMonth(DateTimeOffset date) => date.Month switch
        {
            4 or 6 or 9 or 11 => 30,
            1 or 3 or 5 or 7 or 8 or 10 or 12 => 31,
            2 => DateTime.IsLeapYear(date.Year) ? 29 : 28,
            _ => throw new ArgumentException($"Unable to determine the end of the month {date}", nameof(date.Month))
        };
        private async Task<ExpenseModels.Response> GetExpensesForQuarterAsync(ExpenseModels.Request request)
        {
            var quarterDates = GetQuarterDatesFromDate(request.BeginDate);
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date >= quarterDates.startDate && x.purchase_date <= quarterDates.endDate)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();

                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForExpenseQuarter(request.PageSize, quarterDates.startDate, quarterDates.endDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForExpenseQuarter(int pageSize, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date >= startDate && x.purchase_date <= endDate)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal (DateTimeOffset startDate, DateTimeOffset endDate) GetQuarterDatesFromDate(DateTimeOffset date) => date.Month switch
        {
            1 or 2 or 3 => (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 3, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            4 or 5 or 6 => (startDate: new DateTimeOffset(date.Year, 4, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 6, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            7 or 8 or 9 => (startDate: new DateTimeOffset(date.Year, 7, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 9, 30, 0, 0, 0, new TimeSpan(0, 0, 0))),
            10 or 11 or 12 => (startDate: new DateTimeOffset(date.Year, 10, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0))),
            _ => throw new ArgumentException($"Unable to determine quarter from given date {date}", nameof(date))
        };
        private async Task<ExpenseModels.Response> GetExpensesForAYearAsync(ExpenseModels.Request request)
        {
            var yearDates = GetYearDatesFromDate(request.BeginDate);
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date >= yearDates.startDate && x.purchase_date <= yearDates.endDate)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();

                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForExpenseYear(request.PageSize, yearDates.startDate, yearDates.endDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForExpenseYear(int pageSize, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date >= startDate && x.purchase_date <= endDate)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private (DateTimeOffset startDate, DateTimeOffset endDate) GetYearDatesFromDate(DateTimeOffset date)
        {
            return (startDate: new DateTimeOffset(date.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0)), endDate: new DateTimeOffset(date.Year, 12, 31, 0, 0, 0, new TimeSpan(0, 0, 0)));
        }

        private async Task<ExpenseModels.Response> GetExpensesByDateRange(ExpenseModels.Request request)
        {
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date >= request.BeginDate.ToUniversalTime() && x.purchase_date <= request.EndDate.ToUniversalTime())
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForDateRange(request.PageSize, request.BeginDate.ToUniversalTime(), request.EndDate.ToUniversalTime()),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForDateRange(int pageSize, DateTimeOffset beginDate, DateTimeOffset endDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date >= beginDate.ToUniversalTime() && x.purchase_date <= endDate.ToUniversalTime())
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ExpenseModels.Response> GetExpensesFromLast30Days(ExpenseModels.Request request)
        {
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date > DateTimeOffset.UtcNow.AddDays(-30) && x.purchase_date < DateTimeOffset.UtcNow)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForLast30Days(request.PageSize),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForLast30Days(int pageSize)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date > DateTimeOffset.UtcNow.AddDays(-30) && x.purchase_date < DateTimeOffset.UtcNow)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<ExpenseModels.Response> GetExpensesFromCurrentMonth(ExpenseModels.Request request)
        {
            var beginMonthDate = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date > beginMonthDate && x.purchase_date < DateTimeOffset.UtcNow)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForExpenseCurrentMonth(request.PageSize, beginMonthDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForExpenseCurrentMonth(int pageSize, DateTimeOffset beginMonthDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date > beginMonthDate && x.purchase_date < DateTimeOffset.UtcNow)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<ExpenseModels.Response> GetExpensesFromCurrentQuarter(ExpenseModels.Request request)
        {
            var beginQuarterDate = GetQuarterDatesFromDate(DateTimeOffset.UtcNow);
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date > beginQuarterDate.startDate && x.purchase_date < DateTimeOffset.UtcNow)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForExpenseCurrentQuarter(request.PageSize, beginQuarterDate.startDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForExpenseCurrentQuarter(int pageSize, DateTimeOffset startDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date > startDate && x.purchase_date < DateTimeOffset.UtcNow)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<ExpenseModels.Response> GetExpensesFromCurrentYear(ExpenseModels.Request request)
        {
            var beginYearDate = new DateTimeOffset(DateTime.Now.Year, 1, 1, 0, 0, 0, new TimeSpan(0, 0, 0));
            try
            {
                var expenseTransactions = await _context.Expenses
                    .Where(x => x.purchase_date > beginYearDate && x.purchase_date < DateTimeOffset.UtcNow)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category) 
                    .ToArrayAsync();
                var response = new ExpenseModels.Response
                {
                    TotalPages = await GetTotalPagesForExpenseCurrentYear(request.PageSize, beginYearDate),
                    PageNumber = request.PageNumber,
                    Expenses = _mapper.Map<ExpenseTransaction[]>(expenseTransactions)
                };
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> GetTotalPagesForExpenseCurrentYear(int pageSize, DateTimeOffset beginYearDate)
        {
            try
            {
                var getTotalPages = await _context.Expenses
                    .Where(x => x.purchase_date > beginYearDate && x.purchase_date < DateTimeOffset.UtcNow)
                    .ToArrayAsync();
                return (int)Math.Ceiling((decimal)getTotalPages.Count() / (decimal)pageSize);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
