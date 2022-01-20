using System;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Repositories.ExpenseRepository
{
    public interface IExpenseRepository
    {
        Task<Expenses> GetExpenseById(int id);
        Task<Expenses[]> GetAllExpensesPagination(int pageNumber, int pageSize);
        Task<decimal> GetCountOfAllExpenses();
        Task<Expenses[]> GetExpensesFromSpecificDatePagination(DateTimeOffset beginDate, int pageNumber, int pageSize);
        Task<decimal> GetCountOfExpensesForSpecificDate(DateTimeOffset date);
        Task<Expenses[]> GetExpensesBetweenTwoDatesPagination(DateTimeOffset beginDate, DateTimeOffset endDate, int pageNumber, int pageSize);
        Task<decimal> GetCountOfExpensesBetweenTwoDates(DateTimeOffset beginDate, DateTimeOffset endDate);
        Task<Expenses[]> GetExpensesForNotesSearchPagination(string searchTerm, int pageNumber, int pageSize);
        Task<decimal> GetCountOfExpensesForNotesSearch(string searchTerm);
        Task<Expenses[]> GetExpensesForAmountSearchPagination(decimal amount, int pageNumber, int pageSize);
        Task<decimal> GetCountOfExpensesForAmountSearch(decimal amount);
        Task<bool> CreateExpense(Expenses expense);
        Task<bool> UpdateExpense(Expenses expense);
        Task<bool> DeleteExpense(Expenses expense);
    }
}