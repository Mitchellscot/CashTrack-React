using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseModels.Response> GetExpenseByIdAsync(int id);
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
    }
}
