using CashTrack.Models.ExpenseModels;
using FluentValidation;

namespace CashTrack.Helpers
{
    public class ExpenseValidators : AbstractValidator<Expense>
    {
        public ExpenseValidators()
        {
            RuleFor(x => x.PurchaseDate).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
        }
    }
}
