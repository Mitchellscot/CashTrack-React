using CashTrack.Data;
using CashTrack.Models.UserModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CashTrack.Helpers.Validators
{
    public class AuthenticationValidator : AbstractValidator<Authentication.Request>
    {
        public AuthenticationValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("What's your name again?").MaximumLength(25);
            RuleFor(a => a.Password).NotEmpty().WithMessage("Forget your password?").MaximumLength(50);
        }
    }
}
