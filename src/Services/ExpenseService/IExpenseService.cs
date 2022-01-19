using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseModels.Response> GetExpenseByIdAsync(int id);
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        Task<ExpenseModels.Response> GetAllExpensesAsync(ExpenseModels.Request request);
        Task<ExpenseModels.Response> GetExpensesFromSpecificDateAsync(ExpenseModels.Request request);
    }
}
