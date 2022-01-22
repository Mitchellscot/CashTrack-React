using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseTransaction> GetExpenseByIdAsync(int id);
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        Task<ExpenseModels.Response> GetExpensesByNotesAsync(ExpenseModels.NotesSearchRequest request);
        Task<ExpenseModels.Response> GetExpensesByAmountAsync(ExpenseModels.AmountSearchRequest request);
        Task<Expenses> CreateExpenseAsync(AddEditExpense request);
        Task<bool> UpdateExpenseAsync(AddEditExpense request);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
