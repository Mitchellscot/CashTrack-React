using CashTrack.Models.ExpenseModels;
using CashTrack.Repositories.ExpenseRepository;
using FluentValidation;
using System;
using System.Linq;

namespace CashTrack.Helpers.Validators
{
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
}