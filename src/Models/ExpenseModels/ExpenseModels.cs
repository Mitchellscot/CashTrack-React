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
        }
        public record Response
        {
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
            public ExpenseTransaction[] Expenses { get; set; }
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
    }

}
