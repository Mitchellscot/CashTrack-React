using CashTrack.Models.ExpenseModels;
using FluentValidation;

namespace CashTrack.Helpers.Validators
{
    public class ExpenseSearchNotesValidator : AbstractValidator<ExpenseModels.NotesSearchRequest>
    {
        public ExpenseSearchNotesValidator()
        {
            RuleFor(x => x.SearchTerm).NotEmpty();
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
        }
    }
}
