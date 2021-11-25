using CashTrack.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CashTrack.Models.Expenses
{
    public class ExpenseModel
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public Merchants Merchant { get; set; }
        public string Notes { get; set; }
        public ExpenseSubCategories category { get; set; }
        //figure this out when you start to map DTOS
        //public ExpenseMaincategory Maincategory { get; set;  }
        public List<Tag> Tags { get; set; }
    }
}
