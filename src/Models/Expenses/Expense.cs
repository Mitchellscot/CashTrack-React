using CashTrack.Data.Entities;
using System;
using System.Collections.Generic;

namespace CashTrack.Models.expenses
{
    public class Expense : Transaction
    {
        public override int Id { get; set; }
        public override DateTime PurchaseDate { get; set; }
        public override decimal Amount { get; set; }
        public override string Notes { get; set; }
        public Merchants Merchant { get; set; }
        public List<Tag> Tags { get; set; }
        public ExpenseSubCategory SubCategory { get; set; }
        public ExpenseMainCategory MainCategory { get; set; }
    }
}
