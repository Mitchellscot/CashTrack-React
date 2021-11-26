using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.expenses;

namespace CashTrack.Services.expenses
{
    public interface IExpenseService
    {
        Task<bool> Commit(); //save changes
        Task<Expenses[]> GetAllExpenses();
        Task<Expenses> GetExpenseById(int id);

    }
}
