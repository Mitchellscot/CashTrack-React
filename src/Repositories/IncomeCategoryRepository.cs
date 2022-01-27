using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories.IncomeCategoryRepository;

public interface IIncomeCategoryRepository : IRepository<IncomeCategories>
{
    Task<decimal> GetCountOfIncomeCategories(Expression<Func<IncomeCategories, bool>> predicate);
}

public class IncomeCategoryRepository : IIncomeCategoryRepository
{
    private readonly AppDbContext _context;

    public IncomeCategoryRepository(AppDbContext context) => _context = context;

    public Task<bool> Create(IncomeCategories entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(IncomeCategories entity)
    {
        throw new NotImplementedException();
    }

    public Task<IncomeCategories[]> Find(Expression<Func<IncomeCategories, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<IncomeCategories> FindById(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IncomeCategories[]> FindWithPagination(Expression<Func<IncomeCategories, bool>> predicate, int pageNumber, int pageSize)
    {
        try
        {
            var categories = await _context.IncomeCategories
                .Where(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(x => x.category)
                .ToArrayAsync();
            return categories;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<decimal> GetCountOfIncomeCategories(Expression<Func<IncomeCategories, bool>> predicate)
    {
        try
        {
            return (decimal)await _context.IncomeCategories.CountAsync(predicate);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<bool> Update(IncomeCategories entity)
    {
        throw new NotImplementedException();
    }
}

