using CashTrack.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using CashTrack.Helpers.Exceptions;
using CashTrack.Data.Entities;
using System.Linq.Expressions;

namespace CashTrack.Repositories.ExpenseRepository;

public interface IExpenseRepository : IRepository<Expenses>
{
    Task<decimal> GetAmountOfExpenses(Expression<Func<Expenses, bool>> predicate);
    Task<Expenses[]> GetExpensesAndCategories(Expression<Func<Expenses, bool>> predicate);
}
public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _context;
    public ExpenseRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Expenses[]> Find(Expression<Func<Expenses, bool>> predicate)
    {
        try
        {
            return await _context.Expenses.Where(predicate).ToArrayAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public async Task<Expenses> FindById(int id)
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
    public async Task<Expenses[]> FindWithPagination(Expression<Func<Expenses, bool>> predicate, int pageNumber, int pageSize)
    {
        try
        {
            var expenses = await _context.Expenses
                    .Where(predicate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .OrderBy(x => x.purchase_date)
                    .ThenBy(x => x.id)
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
    public async Task<bool> Create(Expenses entity)
    {
        try
        {
            await _context.Expenses.AddAsync(entity);
            return await (_context.SaveChangesAsync()) > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Update(Expenses entity)
    {
        try
        {
            _context.ChangeTracker.Clear();
            var Entity = _context.Expenses.Attach(entity);
            Entity.State = EntityState.Modified;
            return await (_context.SaveChangesAsync()) > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Delete(Expenses entity)
    {
        try
        {
            _context.Expenses.Remove(entity);
            return await (_context.SaveChangesAsync()) > 0;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> GetAmountOfExpenses(Expression<Func<Expenses, bool>> predicate)
    {
        try
        {
            return (decimal)await _context.Expenses
            .Where(predicate)
            .SumAsync(x => x.amount);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<Expenses[]> GetExpensesAndCategories(Expression<Func<Expenses, bool>> predicate)
    {
        try
        {
            var expenses = await _context.Expenses
                .Where(predicate)
                .Include(x => x.category)
                .ToArrayAsync();
            return expenses;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<int> GetCount(Expression<Func<Expenses, bool>> predicate)
    {
        try
        {
            return await _context.Expenses
            .Where(predicate)
            .CountAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
