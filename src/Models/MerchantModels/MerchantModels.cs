using System.Collections.Generic;
using CashTrack.Models.ExpenseModels;

namespace CashTrack.Models.MerchantModels
{
    public record MerchantModels
    {
        public class Request
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 25;
            public string SearchTerm { get; set; } = null;
        }
        public class Response
        {
            public int PageNumber { get; set; }
            public int TotalPages { get; set; }
            public Merchant[] Merchants { get; set; }
        }
        public record Merchant
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public bool IsOnline { get; set; }
            public int NumberOfExpenses { get; set; }
        }
        public record MerchantDetail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool SuggestOnLookup { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Notes { get; set; }
            public bool IsOnline { get; set; }
            public ExpenseModels.ExpenseModels.ExpenseTotals ExpenseTotals { get; set; }
            public string MostUsedCategory { get; set; }
            public List<ExpenseModels.ExpenseModels.AnnualExpenseStatistics> AnnualExpenseStatistics { get; set; }
            public Dictionary<string, int> PurchaseCategoryOccurances { get; set; }
            public Dictionary<string, decimal> PurchaseCategoryTotals { get; set; }
            public List<ExpenseModels.ExpenseModels.ExpenseQuickView> RecentExpenses { get; set; }
        }


    }
}
