using CashTrack.Models.Common;
using CashTrack.Models.ExpenseModels;
using FluentValidation;
using System;
using System.Globalization;

namespace CashTrack.Helpers
{
    public class ExpenseValidators : AbstractValidator<Expense.Request>
    {
        public ExpenseValidators()
        {
            //CultureInfo enUS = new CultureInfo("en-US");
            //DateTime date;
            //Transform(from: x => x.BeginDate, to: value => DateTime.TryParse(value, out date) ? date : null).
            RuleFor(x => x.DateOptions).IsInEnum().NotEmpty();
            RuleFor(x => x.PageNumber).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
            RuleFor(x => x.QuarterOptions).InclusiveBetween(0, 4);
            RuleFor(x => x.BeginDate).Must(beginDate => beginDate > new DateTime(2011, 12, 31)).WithMessage("There are no expenses available before that date.");
            RuleFor(x => x.BeginDate).Must(beginDate => beginDate < DateTime.Today.AddDays(1)).WithMessage("The Begin Date cannot be in the future.");
            RuleFor(x => x.EndDate).Must(endDate => endDate < DateTime.Today.AddDays(1)).WithMessage("You can't search future dates");
            RuleFor(x => x.EndDate).Must(endDate => endDate > new DateTime(2011, 12, 31)).WithMessage("The end date cannot be before 2012.");
        }
    }
}
