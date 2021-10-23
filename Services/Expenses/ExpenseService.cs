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
    public class ExpenseService : IExpenseService
    {
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly AppDbContext _context;
        private readonly ILogger<ExpenseService> _logger;

        public ExpenseService(
            IOptions<AppSettings> appSettings, IMapper mapper, AppDbContext context, ILogger<ExpenseService> logger)
        {
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._context = context;
            this._logger = logger;
        }

        public async Task<bool> Commit()
        {
            //only return if more than one row was affected
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Expense[]> GetAllExpenses()
        {
            IQueryable<Expense> query = _context.Expenses;
            //query = query.Where(e => e.Catagory.Id == 14);
            query = query.Where(e => e.Merchant.Name.Contains("Costco"));
            return await query.ToArrayAsync();
        }
    }
}
