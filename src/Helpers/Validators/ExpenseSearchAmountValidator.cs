using CashTrack.Models.ExpenseModels;
using FluentValidation;

namespace CashTrack.Helpers.Validators
{
    public class ExpenseSearchAmountValidator : AbstractValidator<ExpenseModels.AmountSearchRequest>
    {
        public ExpenseSearchAmountValidator()
        {
            RuleFor(x => x.Query).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
        }
    }
}
