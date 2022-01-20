using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseTransaction> GetExpenseByIdAsync(int id);
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        Task<AddEditExpense> CreateUpdateExpenseAsync(AddEditExpense request);
        Task<bool> DeleteExpenseAsync(int id);
        Task<ExpenseModels.Response> GetExpensesByNotesAsync(ExpenseModels.NotesSearchRequest request);
        Task<ExpenseModels.Response> GetExpensesByAmountAsync(ExpenseModels.AmountSearchRequest request);
        Task<ExpenseModels.Response> GetExpensesBySubCategoryAsync(ExpenseModels.SubCategorySearchRequest request);


    }
}
