using CashTrack.Data.Entities;
using CashTrack.Models.Common;
using CashTrack.Models.TagModels;
using System;
using System.Collections.Generic;

namespace CashTrack.Models.ExpenseModels
{
    public class ExpenseModels
    {
        public record Request
        {
            public DateOptions DateOptions { get; set; }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 25;
            public DateTimeOffset BeginDate { get; set; } = DateTimeOffset.UtcNow;
            public DateTimeOffset EndDate { get; set; } = DateTimeOffset.UtcNow;
            public string SearchTerm { get; set; } = null;
        }
        public record Response
        {
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
            public int PageSize { get; set; }
            public int TotalExpenses { get; set; }
            public decimal TotalAmount { get; set; }
            public ExpenseTransaction[] Expenses { get; set; }
        }
        public record NotesSearchRequest
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 25;
            public string SearchTerm { get; set; } = null;
        }
        public record AmountSearchRequest
        {
            private decimal _query;
            public decimal Query
            {
                get { return _query; }
                set
                {
                    _query = Decimal.Round(value, 2);
                }
            }
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 25;
        }
    }
    public record AddEditExpense
    {
        public int? Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public int? MerchantId { get; set; }
        //figure this out after you get Tags CRUD set up
        //public ICollection<Tag> Tags { get; set; }
        public int SubCategoryId { get; set; }
    }
    public record ExpenseTransaction
    {
        public int Id { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public string Merchant { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public string SubCategory { get; set; }
        public string MainCategory { get; set; }
    }
    public record ExpenseQuickView
    {
        public int Id { get; set; }
        public string PurchaseDate { get; set; }
        public decimal Amount { get; set; }
        public string SubCategory { get; set; }
    }
}
