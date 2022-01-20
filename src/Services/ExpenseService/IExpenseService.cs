﻿using CashTrack.Data.Entities;
using CashTrack.Models.ExpenseModels;
using System.Threading.Tasks;

namespace CashTrack.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<ExpenseTransaction> GetExpenseByIdAsync(int id);
        Task<ExpenseModels.Response> GetExpensesAsync(ExpenseModels.Request request);
        Task<Expenses> CreateUpdateExpenseAsync(AddEditExpense request);
        Task<bool> DeleteExpenseAsync(int id);
    }
}