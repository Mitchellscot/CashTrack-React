using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using CashTrack.Models.ExpenseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using CashTrack.Helpers.Exceptions;
using Npgsql;
using CashTrack.Models.Common;

namespace CashTrack.Services.ExpenseRepository
{
    public class ExpenseService : IExpenseService
    {
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(
            IOptions<AppSettings> appSettings, AppDbContext context, ILogger<ExpenseService> logger)
        {
            this._appSettings = appSettings.Value;
            this._context = context;
            this._logger = logger;
        }

        public async Task<bool> Commit()
        {
            //only return if more than one row was affected
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Expenses> GetExpenseById(int id)
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
            return singleExpense;
        }

        public async Task<Expenses[]> GetExpenses(Expense.Request request) => request.DateOptions switch
        {
            //1
            //No requirements
            DateOptions.All => await GetAllExpenses(request),

            //2
            //Required Begin Date
            DateOptions.SpecificDate => await GetExpensesFromSpecificDate(request),

            //3
            // required begin date
            DateOptions.SpecificMonthAndYear => await GetExpensesFromMonthAndYear(request),

            //requires a begin date
            //DateOptions.SpecificQuarter => k(), 

            // Requires begin date
            //DateOptions.SpecificYear => h(), 

            //begindate and enddate required.
            DateOptions.DateRange => await GetExpensesByDateRange(request),

            //No dates required
            DateOptions.Last30Days => await GetExpensesFromLast30Days(request),

            //No dates required.
            //DateOptions.LastQuarter => e(), 

            //No dates required.
            //DateOptions.LastYear => e(), 

            //No dates required.
            //DateOptions.CurrentMonth => f(), 

            // based on today's date. No dates required.
            //DateOptions.CurrentQuarter => j(), 

            //No dates required.
            //DateOptions.CurrentYear => g(), 

            _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))

        };

        private async Task<Expenses[]> GetAllExpenses(Expense.Request request)
        {
            try
            {
                var expenses = await _context.Expenses
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
                return expenses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Expenses[]> GetExpensesFromSpecificDate(Expense.Request request)
        {
            try
            {
                var expenses = await _context.Expenses
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
                return expenses;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<Expenses[]> GetExpensesFromMonthAndYear(Expense.Request request)
        {
            var monthBeginDate = GetMonthBeginDate(request.BeginDate).ToUniversalTime();
            var monthEndDate = GetMonthEndDate(request.BeginDate).ToUniversalTime();
            _logger.LogInformation($"{monthBeginDate} - {monthEndDate}");
            try
            {
                var expenses = await _context.Expenses
                    .Where(x => x.purchase_date >= monthBeginDate && x.purchase_date <= monthEndDate)
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
                return expenses;
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

        private async Task<Expenses[]> GetExpensesByDateRange(Expense.Request request)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Where(x => x.purchase_date >= request.BeginDate.ToUniversalTime() && x.purchase_date <= request.EndDate.ToUniversalTime())
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
                return expenses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<Expenses[]> GetExpensesFromLast30Days(Expense.Request request)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Where(x => x.purchase_date > DateTimeOffset.Now.AddDays(-30) && x.purchase_date < DateTimeOffset.Now)
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
                return expenses;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
