using AutoMapper;
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

namespace CashTrack.Services.Expenses
{
    public class ExpenseService //: IExpenseService
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

        //public async Task<Expense[]> GetAllExpenses()
        //{
        //    IQueryable<Expense> query = _context.Expenses;
        //    query = query.Take(100).OrderBy(x => x.PurchaseDate)
        //        .Include(x => x.category)
        //        .Include(x => x.Merchant);
        //    //query = query.Where(e => e.Merchant.Name.Contains("Costco"));
        //    return await query.ToArrayAsync();
        //}

        //public async Task<Expense> GetExpenseById(int id)
        //{
        //    IQueryable<Expense> query = _context.Expenses;
        //    return await query.FirstOrDefaultAsync(x => x.Id == id);
        //}
    }
}
