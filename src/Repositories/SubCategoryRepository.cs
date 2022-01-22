using CashTrack.Data;
using CashTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories.SubCategoriesRepository;

public interface ISubCategoryRepository : IRepository<ExpenseSubCategories>
{
}
public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly AppDbContext _context;
    public SubCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> Create(ExpenseSubCategories entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(ExpenseSubCategories entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ExpenseSubCategories[]> Find(Expression<Func<ExpenseSubCategories, bool>> predicate)
    {
        try
        {
            return await _context.ExpenseSubCategories.Where(predicate).ToArrayAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<ExpenseSubCategories> FindById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ExpenseSubCategories[]> FindWithPagination(Expression<Func<ExpenseSubCategories, bool>> predicate, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(ExpenseSubCategories entity)
    {
        throw new NotImplementedException();
    }
}

