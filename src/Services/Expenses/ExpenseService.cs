using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using CashTrack.Models.Expenses;
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

namespace CashTrack.Services.expenses
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

        public async Task<Expenses[]> GetExpenses(int pageNumber, int pageSize)
        {
            try
            {
                var expenses = await _context.Expenses
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToArrayAsync();
                
                return expenses;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Expenses> GetExpenseById(int id)
        {

            var singleExpense = await _context.Expenses.SingleOrDefaultAsync(x => x.id == id);
            if (singleExpense == null)
            {
                throw new ExpenseNotFoundException(id.ToString());
            }
            return singleExpense;

        }
    }
}
