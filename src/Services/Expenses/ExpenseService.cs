using AutoMapper;
using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using CashTrack.Models.expenses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public Task<Expense[]> GetAllExpenses()
        {
            throw new NotImplementedException();
        }

        //public async Task<Expense[]> GetAllExpenses(int pageSize = 25, int pageNumber = 1)
        //{


        //    IQueryable<Expense> query = _context.;
        //    query = query.Take(100).OrderBy(x => x.PurchaseDate)
        //        .Include(x => x.category)
        //        .Include(x => x.Merchant);
        //    //query = query.Where(e => e.Merchant.Name.Contains("Costco"));
        //    return await query.ToArrayAsync();
        //}

        public async Task<Expenses> GetExpenseById(int id)
        {

            IQueryable<Expenses> query = _context.Expenses;
            var singleExpense = await query.FirstOrDefaultAsync(x => x.id == id);
            return singleExpense;
        }

        Task<Expenses[]> IExpenseService.GetAllExpenses()
        {
            throw new NotImplementedException();
        }

        Task<Expenses> IExpenseService.GetExpenseById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
