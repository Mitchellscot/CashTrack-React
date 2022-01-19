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
using CashTrack.Data.Entities;

namespace CashTrack.Repositories.ExpenseRepository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;
        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Commit()
        {
            //only return if more than one row was affected
            return (await _context.SaveChangesAsync()) > 0;
        }
        public async Task<Expenses> GetExpenseById(int id)
        {
            try
            {
                var expense = await _context.Expenses
                    .Include(x => x.expense_tags)
                    .ThenInclude(x => x.tag)
                    .Include(x => x.merchant)
                    .Include(x => x.category)
                    .ThenInclude(x => x.main_category)
                    .SingleOrDefaultAsync(x => x.id == id);
                if (expense == null)
                    throw new ExpenseNotFoundException(id.ToString());

                return expense;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Expenses[]> GetAllExpensesPagination(int pageNumber, int pageSize)
        {
            try
            {
                var expenses = await _context.Expenses
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
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
        public async Task<decimal> GetCountOfAllExpenses()
        {
            try
            {
                return (decimal)await _context.Expenses.CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Expenses[]> GetExpensesFromSpecificDatePagination(DateTimeOffset beginDate, int pageNumber, int pageSize)
        {
            try
            {
                var expenses = await _context.Expenses
                    .Where(x => x.purchase_date == beginDate.ToUniversalTime())
                    .OrderByDescending(x => x.purchase_date)
                    .ThenByDescending(x => x.id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
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
        public async Task<decimal> GetCountOfExpensesForSpecificDate(DateTimeOffset date)
        {
            try
            {
                return (decimal)await _context.Expenses.Where(x => x.purchase_date == date.ToUniversalTime()).CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Expenses[]> GetExpensesBetweenTwoDatesPagination(DateTimeOffset beginDate, DateTimeOffset endDate, int pageNumber, int pageSize)
        {
            try
            {
                var expenses = await _context.Expenses
                .Where(x => x.purchase_date >= beginDate && x.purchase_date <= endDate)
                .OrderBy(x => x.purchase_date)
                .ThenBy(x => x.id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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
        public async Task<decimal> GetCountOfExpensesBetweenTwoDates(DateTimeOffset beginDate, DateTimeOffset endDate)
        {
            try
            {
                return (decimal)await _context.Expenses
                .Where(x => x.purchase_date >= beginDate && x.purchase_date <= endDate)
                .CountAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
