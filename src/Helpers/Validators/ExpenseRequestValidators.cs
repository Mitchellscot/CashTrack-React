using CashTrack.Models.ExpenseModels;
using FluentValidation;
using System;

namespace CashTrack.Helpers.Validators
{
    public class ExpenseRequestValidators : AbstractValidator<ExpenseModels.Request>
    {
        public ExpenseRequestValidators()
        {
            RuleFor(x => x.DateOptions).IsInEnum().NotEmpty().WithMessage("Date Options must be specificied in query string. Valid options are 1 through 12.");
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
            RuleFor(x => x.BeginDate).Must(beginDate => beginDate > new DateTime(2011, 12, 31)).WithMessage("There are no expenses available before that date."); //TODO: Make a call to the DB find find earliest date so this isn't hardcoded.
            RuleFor(x => x.BeginDate).Must(beginDate => beginDate < DateTime.Today.AddDays(1)).WithMessage("The Begin Date cannot be in the future.");
            RuleFor(x => x.EndDate).Must(endDate => endDate < DateTime.Today.AddDays(1)).WithMessage("You can't search future dates");
            RuleFor(x => x.EndDate).Must(endDate => endDate > new DateTime(2011, 12, 31)).WithMessage("The end date cannot be before 2012.");
        }
    }
}