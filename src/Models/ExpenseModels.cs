using CashTrack.Models.Common;
using CashTrack.Models.TagModels;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.MerchantRepository;
using CashTrack.Repositories.SubCategoriesRepository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CashTrack.Models.ExpenseModels;

public class AddEditExpenseValidators : AbstractValidator<AddEditExpense>
{
    public AddEditExpenseValidators(ISubCategoryRepository _categoryRepo, IMerchantRepository _merchantRepo)
    {
        RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PurchaseDate).NotEmpty();
        RuleFor(x => x.PurchaseDate).Must(x => x < DateTime.Today.AddDays(1)).WithMessage("The Purchase Date cannot be in the future.");
        RuleFor(x => x.SubCategoryId).NotEmpty().GreaterThan(0).WithMessage("Must provide a category ID");
        RuleFor(x => x.SubCategoryId).MustAsync(async (model, value, _) =>
        {
            return (await _categoryRepo.Find(x => true)).Any(x => x.id == value);
        }).WithMessage("Invalid Category Id");

        When(x => x.MerchantId != null,
            () =>
            {
                RuleFor(x => x.MerchantId).GreaterThan(0).MustAsync(async (model, value, _) =>
                {
                    return ((int)await _merchantRepo.GetCountOfMerchants(x => true)) > value;
                }).WithMessage("Invalid Merchant Id");
            });
    }
}
public class ExpenseRequestValidators : AbstractValidator<ExpenseModels.Request>
{
    public ExpenseRequestValidators(IExpenseRepository expenseRepository)
    {
        var earliestExpense = expenseRepository.Find(x => true).Result.OrderBy(x => x.purchase_date).Select(x => x.purchase_date).FirstOrDefault();

        RuleFor(x => x.DateOptions).IsInEnum().NotEmpty().WithMessage("Date Options must be specificied in query string. Valid options are 1 through 12.");
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
        RuleFor(x => x.BeginDate).Must(beginDate => beginDate >= earliestExpense).WithMessage("There are no expenses available before that date.");
        RuleFor(x => x.BeginDate).Must(beginDate => beginDate < DateTime.Today.AddDays(1)).WithMessage("The Begin Date cannot be in the future.");
        RuleFor(x => x.EndDate).Must(endDate => endDate < DateTime.Today.AddDays(1)).WithMessage("You can't search future dates");
        RuleFor(x => x.EndDate).Must(endDate => endDate > earliestExpense).WithMessage($"The end date cannot be before {earliestExpense.DateTime.ToShortDateString()}.");
    }
}
public class ExpenseSearchAmountValidator : AbstractValidator<ExpenseModels.AmountSearchRequest>
{
    public ExpenseSearchAmountValidator()
    {
        RuleFor(x => x.Query).NotEmpty().GreaterThan(0);
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
    }
}
public class ExpenseSearchNotesValidator : AbstractValidator<ExpenseModels.NotesSearchRequest>
{
    public ExpenseSearchNotesValidator()
    {
        RuleFor(x => x.SearchTerm).NotEmpty();
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
    }
}

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
        public ExpenseListItem[] Expenses { get; set; }
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
public record ExpenseListItem
{
    public int Id { get; set; }
    public DateTimeOffset PurchaseDate { get; set; }
    public decimal Amount { get; set; }
    public string Notes { get; set; }
    public string Merchant { get; set; }
    public ICollection<TagModel> Tags { get; set; }
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

