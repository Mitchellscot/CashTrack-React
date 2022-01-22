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
        Task<Expenses> GetExpenseById(int id);
        Task<decimal> GetCountOfAllExpenses();
        Task<decimal> GetCountOfExpensesForSpecificDate(DateTimeOffset date);
        Task<decimal> GetCountOfExpensesBetweenTwoDates(DateTimeOffset beginDate, DateTimeOffset endDate);
        Task<decimal> GetCountOfExpensesForNotesSearch(string searchTerm);
        Task<decimal> GetCountOfExpensesForAmountSearch(decimal amount);
        Task<decimal> GetCountOfExpenses(Expression<Func<Expenses, bool>> predicate);
    }
}