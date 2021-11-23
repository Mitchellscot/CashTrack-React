using CashTrack.Data.Entities;
using System;

namespace CashTrack.Models.Expenses
{
    public class Expense : Transaction
    {
        public override int Id { get; set; }
        public override DateTime PurchaseDate { get; set; }
        public override decimal Amount { get; set; }
        public override string Notes { get; set; }
        public Merchants Merchant { get; set; }

    }
}
