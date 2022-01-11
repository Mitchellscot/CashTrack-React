using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<bool> Commit();
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        Task<ExpenseModels.Response> GetExpenseByIdAsync(int id);
    }
}
