using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Services.ExpenseRepository
{
    public interface IExpenseService
    {
        Task<bool> Commit();
        Task<Expenses[]> GetExpensesAsync(Expense.Request request);
        Task<Expenses> GetExpenseByIdAsync(int id);
        int GetTotalPageCount(int pageSize);
    }
}
