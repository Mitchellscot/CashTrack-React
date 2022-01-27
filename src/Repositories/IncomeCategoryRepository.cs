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

    public Task<IncomeCategories[]> FindWithPagination(Expression<Func<IncomeCategories, bool>> predicate, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Update(IncomeCategories entity)
    {
        throw new NotImplementedException();
    }
}

