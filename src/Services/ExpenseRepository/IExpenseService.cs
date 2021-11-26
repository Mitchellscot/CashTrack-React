using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Services.ExpenseRepository
{
    public interface IExpenseService
    {
        Task<bool> Commit(); //save changes
        Task<Expenses[]> GetExpenses(int pageNumber, int pageSize);
        Task<Expenses> GetExpenseById(int id);
        Task<Expenses[]> GetExpensesByDate();
    }
}
