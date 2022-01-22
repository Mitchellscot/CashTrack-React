using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Repositories.ExpenseRepository
{
    public interface IExpenseRepository : IRepository<Expenses>
    {
        Task<decimal> GetCountOfExpenses(Expression<Func<Expenses, bool>> predicate);
    }
}