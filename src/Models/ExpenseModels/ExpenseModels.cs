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
        public record ExpenseQuickView
        { 
            public int Id { get; set; }
            public string PurchaseDate { get; set; }
            public decimal Amount { get; set; }
            public string SubCategory { get; set; }
        }
        public record AnnualExpenseStatistics
        {
            public int Year { get; set; }
            public int Count { get; set; }
            public decimal Average { get; set; }
            public decimal Min { get; set; }
            public decimal Max { get; set; }
            public decimal Total { get; set; }
        }
        public class ExpenseStatisticsAggregator
        {
            public decimal Max { get; set; }
            public decimal Min { get; set; }
            public decimal Total { get; set; }
            public int Count { get; set; }
            public decimal Average { get; set; }
            public ExpenseStatisticsAggregator()
            {
                Max = decimal.MinValue;
                Min = decimal.MaxValue;
            }
            public ExpenseStatisticsAggregator Accumulate(Expenses e)
            {
                Total += e.amount;
                Count++;
                Max = Math.Max(Max, e.amount);
                Min = Math.Min(Min, e.amount);
                return this;
            }
            public ExpenseStatisticsAggregator Compute()
            {
                Average = Math.Round(Total / Count, 2);
                return this;
            }
        }
        public record ExpenseTotals
        {
            public decimal TotalSpentThisMonth { get; set; }
            public decimal TotalSpentThisYear { get; set; }
            public decimal TotalSpentAllTime { get; set; }
        }
        public record ExpenseTotalsAggregator
        {
            public decimal TotalSpentThisMonth { get; set; }
            public decimal TotalSpentThisYear { get; set; }
            public decimal TotalSpentAllTime { get; set; }
            public ExpenseTotalsAggregator()
            {
                this.TotalSpentThisMonth = 0;
                this.TotalSpentThisYear = 0;
                this.TotalSpentAllTime = 0;
            }
            public ExpenseTotalsAggregator Accumulate(Expenses e)
            {
                if (e.purchase_date.Month == DateTimeOffset.UtcNow.Month && e.purchase_date.Year == DateTimeOffset.UtcNow.Year)
                {
                    TotalSpentThisMonth += e.amount;
                }
                if (e.purchase_date.Year == DateTimeOffset.UtcNow.Year)
                { 
                    TotalSpentThisYear += e.amount;
                }
                TotalSpentAllTime += e.amount;
                return this;
            }
            public ExpenseTotals Compute()
            { 
                return new ExpenseTotals()
                {
                    TotalSpentThisMonth = this.TotalSpentThisMonth,
                    TotalSpentThisYear = this.TotalSpentThisYear,
                    TotalSpentAllTime = this.TotalSpentAllTime
                };
            }
        }
    }

}
