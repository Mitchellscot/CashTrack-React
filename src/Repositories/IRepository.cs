using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CashTrack.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T[]> FindWithPagination(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
        Task<T[]> Find(Expression<Func<T, bool>> predicate);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
    }
}
