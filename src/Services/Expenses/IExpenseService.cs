using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.Expenses;

namespace CashTrack.Services.expenses
{
    public interface IExpenseService
    {
        Task<bool> Commit(); //save changes
        Task<Expenses[]> GetExpenses(int pageNumber, int pageSize);
        Task<Expenses> GetExpenseById(int id);

    }
}
