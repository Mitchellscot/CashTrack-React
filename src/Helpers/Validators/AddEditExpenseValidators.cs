using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.MerchantRepository;
using CashTrack.Repositories.SubCategoriesRepository.cs;
using FluentValidation;
using System;
using System.Linq;

namespace CashTrack.Helpers.Validators
{
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
                return (await _categoryRepo.GetAllSubCategoriesAsync()).Any(x => x.id == value);
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
}
