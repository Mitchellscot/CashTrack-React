using CashTrack.Common.Exceptions;
using CashTrack.Data;
using CashTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories.IncomeRepository;
public interface IIncomeRepository : IRepository<Incomes>
{
    Task<decimal> GetAmountOfIncome(Expression<Func<Incomes, bool>> predicate);
}
public class IncomeRepository : IIncomeRepository
{
    private readonly AppDbContext _ctx;
    public IncomeRepository(AppDbContext ctx) => _ctx = ctx;

    public Task<bool> Create(Incomes entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Incomes entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Incomes[]> Find(Expression<Func<Incomes, bool>> predicate)
    {
        try
        {
            return await _ctx.Incomes.Where(predicate).ToArrayAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Incomes> FindById(int id)
    {
        try
        {
            var income = await _ctx.Incomes
                .Include(x => x.source)
                .Include(x => x.category)
                .SingleOrDefaultAsync(x => x.id == id);
            if (income == null)
                throw new IncomeNotFoundException(id.ToString());
            return income;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Incomes[]> FindWithPagination(Expression<Func<Incomes, bool>> predicate, int pageNumber, int pageSize)
    {
        try
        {
            var income = await _ctx.Incomes
                    .Where(predicate)
                    .OrderByDescending(x => x.income_date)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Include(x => x.source)
                    .Include(x => x.category)
                    .ToArrayAsync();
            return income;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<decimal> GetAmountOfIncome(Expression<Func<Incomes, bool>> predicate)
    {
        try
        {
            return (decimal)await _ctx.Incomes
            .Where(predicate)
            .SumAsync(x => x.amount);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<int> GetCount(Expression<Func<Incomes, bool>> predicate)
    {
        try
        {
            return await _ctx.Incomes
            .Where(predicate)
            .CountAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<bool> Update(Incomes entity)
    {
        throw new NotImplementedException();
    }
}

