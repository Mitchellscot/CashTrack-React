using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories.SubCategoriesRepository;

public interface ISubCategoryRepository : IRepository<SubCategories>
{
    Task<decimal> GetCountOfSubCategories(Expression<Func<SubCategories, bool>> predicate);
}
public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly AppDbContext _context;
    public SubCategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> Create(SubCategories entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(SubCategories entity)
    {
        throw new NotImplementedException();
    }

    public async Task<SubCategories[]> Find(Expression<Func<SubCategories, bool>> predicate)
    {
        try
        {
            return await _context.SubCategories.Where(predicate).Include(x => x.main_category).ToArrayAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<SubCategories> FindById(int id)
    {
        try
        {
            var category = await _context.SubCategories
                .Include(x => x.main_category)
                .SingleOrDefaultAsync();
            if (category == null)
                throw new CategoryNotFoundException(id.ToString());
            return category;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<SubCategories[]> FindWithPagination(Expression<Func<SubCategories, bool>> predicate, int pageNumber, int pageSize)
    {
        try
        {
            var categories = await _context.SubCategories
                .Where(predicate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(x => x.sub_category_name)
                .Include(x => x.main_category)
                .ToArrayAsync();
            return categories;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<decimal> GetCountOfSubCategories(Expression<Func<SubCategories, bool>> predicate)
    {
        try
        {
            var categories = (decimal)await _context.SubCategories
                .CountAsync(predicate);
            return categories;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<bool> Update(SubCategories entity)
    {
        throw new NotImplementedException();
    }
}

