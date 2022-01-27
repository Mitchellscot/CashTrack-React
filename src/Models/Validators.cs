using CashTrack.Models.AuthenticationModels;
using CashTrack.Models.ExpenseModels;
using CashTrack.Models.MainCategoryModels;
using CashTrack.Models.MerchantModels;
using CashTrack.Models.SubCategoryModels;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.MerchantRepository;
using CashTrack.Repositories.SubCategoriesRepository;
using FluentValidation;
using System;
using System.Linq;

namespace CashTrack.Validators;

/* AUTHENTICATION */
public class AuthenticationValidator : AbstractValidator<AuthenticationModels.Request>
{
    public AuthenticationValidator()
    {
        RuleFor(a => a.Name).NotEmpty().WithMessage("What's your name again?").MaximumLength(25);
        RuleFor(a => a.Password).NotEmpty().WithMessage("Forget your password?").MaximumLength(50);
    }
}

/*  EXPENSES */
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

/* MERCHANTS */
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
/* SUB CATEGORIES */
public class SubCategoryValidator : AbstractValidator<SubCategoryModels.Request>
{
    public SubCategoryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
    }
}

public class AddEditSubCategoryValidator : AbstractValidator<AddEditSubCategory>
{
    public AddEditSubCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(25);
    }
}
/* MAIN CATEGORY */
public class AddEditMainCategoryValidator : AbstractValidator<AddEditMainCategory>
{
    public AddEditMainCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(25);
    }
}