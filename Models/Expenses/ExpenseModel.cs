﻿using CashTrack.Data.Entities;
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
        public Merchant Merchant { get; set; }
        public string Notes { get; set; }
        public ExpenseCatagory Catagory { get; set; }
        public ExpenseMainCatagory MainCatagory { get; set;  }
        public List<Tag> Tags { get; set; }
    }
}