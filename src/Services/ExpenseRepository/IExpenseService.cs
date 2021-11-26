using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.Expenses;

namespace CashTrack.Services.ExpenseRepository
{
    public interface IExpenseService
    {
        Task<bool> Commit(); //save changes
        Task<Data.Entities.Expenses[]> GetExpenses(int pageNumber, int pageSize);
        Task<Data.Entities.Expenses> GetExpenseById(int id);


    }
}
