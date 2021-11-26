using CashTrack.Data.Entities;
using CashTrack.Models.TagModels;
using System;
using System.Collections.Generic;

namespace CashTrack.Models.ExpenseModels
{
    public class Expense
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public string Merchant { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
    }
}
