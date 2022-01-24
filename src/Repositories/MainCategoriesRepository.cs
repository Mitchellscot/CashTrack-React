using CashTrack.Data;
using CashTrack.Data.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories.MainCategoriesRepository
{
    public interface IMainCategoriesRepository : IRepository<MainCategories>
    {

    }
    public class MainCategoriesRepository : IMainCategoriesRepository
    {
        private readonly AppDbContext _context;
        public MainCategoriesRepository(AppDbContext dbContext) => (_context) = (dbContext);

        public Task<bool> Create(MainCategories entity)
        {
            //Create a check here, if there are 25 main categories, throw an error. That's too many.
            throw new NotImplementedException();
        }

        public Task<bool> Delete(MainCategories entity)
        {
            throw new NotImplementedException();
        }

        public Task<MainCategories[]> Find(Expression<Func<MainCategories, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<MainCategories> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MainCategories[]> FindWithPagination(Expression<Func<MainCategories, bool>> predicate, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(MainCategories entity)
        {
            throw new NotImplementedException();
        }
    }
}
