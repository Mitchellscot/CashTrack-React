using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashTrack.Data.Entities;
using CashTrack.Models.Expenses;

namespace CashTrack.Services.Expenses
{
    public interface IExpenseService
    {
        Task<bool> Commit(); //save changes
        Task<Expense[]> GetAllExpenses();

    }
}
