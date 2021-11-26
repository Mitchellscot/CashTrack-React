using CashTrack.Data.Entities;
using System;
using System.Collections.Generic;

namespace CashTrack.Models.Expenses
{
    public class Expense : Transaction
    {
        public override int Id { get; set; }
        public override DateTime PurchaseDate { get; set; }
        public override decimal Amount { get; set; }
        public override string Notes { get; set; }
        public string Merchant { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
    }
}
