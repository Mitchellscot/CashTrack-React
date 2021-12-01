using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Services.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<bool> Commit();
        Task<Expense.Response> GetExpensesAsync(Expense.Request request);
        Task<Expense.Response> GetExpenseByIdAsync(int id);
    }
}
