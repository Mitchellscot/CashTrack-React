using System.Collections.Generic;
using CashTrack.Helpers.Aggregators;
using CashTrack.Models.ExpenseModels;
using FluentValidation;

namespace CashTrack.Models.MerchantModels
{
    public class MerchantValidator : AbstractValidator<MerchantModels.Request>
    {
        public MerchantValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
        }
    }
    public class AddEditMerchantValidator : AbstractValidator<AddEditMerchant>
    {
        public AddEditMerchantValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }

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
            public int PageSize { get; set; } = 25;
            public int TotalPages { get; set; }
            public decimal TotalMerchants { get; set; }
            public MerchantListItem[] Merchants { get; set; }
        }
    }
    public record AddEditMerchant
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool SuggestOnLookup { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public bool IsOnline { get; set; }
        public string Notes { get; set; }
    }
    public record MerchantListItem
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
        public ExpenseTotals ExpenseTotals { get; set; }
        public string MostUsedCategory { get; set; }
        public List<AnnualExpenseStatistics> AnnualExpenseStatistics { get; set; }
        public Dictionary<string, int> PurchaseCategoryOccurances { get; set; }
        public Dictionary<string, decimal> PurchaseCategoryTotals { get; set; }
        public List<ExpenseQuickView> RecentExpenses { get; set; }
    }
}
