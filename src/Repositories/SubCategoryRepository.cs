using CashTrack.Data;
using CashTrack.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories.SubCategoriesRepository;

public interface ISubCategoryRepository : IRepository<SubCategories>
{
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
            return await _context.SubCategories.Where(predicate).ToArrayAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Task<SubCategories> FindById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<SubCategories[]> FindWithPagination(Expression<Func<SubCategories, bool>> predicate, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(SubCategories entity)
    {
        throw new NotImplementedException();
    }
}

