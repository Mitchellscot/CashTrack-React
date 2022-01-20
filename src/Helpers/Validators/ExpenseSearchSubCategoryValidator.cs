using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.SubCategoriesRepository.cs;
using FluentValidation;
using System.Linq;

namespace CashTrack.Helpers.Validators
{
    public class ExpenseSearchSubCategoryValidator : AbstractValidator<ExpenseModels.SubCategorySearchRequest>
    {
        public ExpenseSearchSubCategoryValidator(ISubCategoryRepository _categoryRepo)
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
            RuleFor(x => x.SubCategoryId).GreaterThan(0).WithMessage("Must Provide a Sub Category Id");
            RuleFor(x => x.SubCategoryId).MustAsync(async (model, value, _) =>
            {
                return (await _categoryRepo.GetAllSubCategoriesAsync()).Any(x => x.id == value);
            }).WithMessage("Invalid Category Id");
        }
    }
}
