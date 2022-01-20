using System;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        //Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        Task<Expenses> GetExpenseById(int id);
        Task<Expenses[]> GetAllExpensesPagination(int pageNumber, int pageSize);
        Task<decimal> GetCountOfAllExpenses();
        Task<Expenses[]> GetExpensesFromSpecificDatePagination(DateTimeOffset beginDate, int pageNumber, int pageSize);
        Task<decimal> GetCountOfExpensesForSpecificDate(DateTimeOffset date);
        Task<Expenses[]> GetExpensesBetweenTwoDatesPagination(DateTimeOffset beginDate, DateTimeOffset endDate, int pageNumber, int pageSize);
        Task<decimal> GetCountOfExpensesBetweenTwoDates(DateTimeOffset beginDate, DateTimeOffset endDate);
        Task<bool> CreateExpense(Expenses expense);
        Task<bool> UpdateExpense(Expenses expense);
        Task<bool> DeleteExpense(Expenses expense);
    }
}