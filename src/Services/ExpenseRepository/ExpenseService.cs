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

        public async Task<Expenses[]> GetExpenses(Expense.Request request)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Where(x => x.purchase_date > DateTime.Today.AddDays(-30) && x.purchase_date < DateTime.Today)
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((request.PageNumber -1) * request.PageSize)
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

        public async Task<Expenses[]> ChoseExpensesBasedOnDateOptions(Expense.Request request) => request.DateOptions switch
        {
            DateOptions.None => await GetAllExpenses(request),
            //Get all expenses on a given date. BeginDate Required. Default is current date.
            //DateOptions.DateRange => c(), // based on given date range, begindate and enddate required. Default is current date.
            //DateOptions.Last30Days => d(), // based on today's date minus thirty. No dates required 
            //DateOptions.LastYear => e(), //based on today's date. No dates required.
            //DateOptions.CurrentMonth => f(), // based on today's date. No dates required.
            //DateOptions.CurrentYear => g(), // based on today's date. No dates required.
            //DateOptions.SpecificYear => h(), // Requires begin date but not end date.
            //DateOptions.SpecificMonthAndYear => i(), // required begin date but not end date.
            //DateOptions.CurrentQuarter => j(), // based on today's date. No dates required.
            //DateOptions.SpecificQuarter => k(), //requires a begin date but not an end date.
            _ => throw new ArgumentException($"DateOption type not supported {request.DateOptions}", nameof(request.DateOptions))
        };

        public async Task<Expenses[]> GetAllExpenses(Expense.Request request)
        {
            try
            {

                var expenses = await _context.Expenses
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.merchant)
                    //.Include(x => x.category)
                    //.ThenInclude(x => x.main_category)
                    //.Include(x => x.expense_tags)
                    //.ThenInclude(x => x.tag)

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
