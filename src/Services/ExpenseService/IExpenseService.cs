using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseModels.Response> GetExpenseByIdAsync(int id);
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        //Might not actually need these as they are not public methods.
        //Task<ExpenseModels.Response> GetAllExpensesAsync(ExpenseModels.Request request);
        //Task<ExpenseModels.Response> GetExpensesFromSpecificDateAsync(ExpenseModels.Request request);
        //Task<ExpenseModels.Response> GetExpensesForSpecificMonthAsync(ExpenseModels.Request request);
    }
}
