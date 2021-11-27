using CashTrack.Data;
using CashTrack.Models.UserModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CashTrack.Helpers.Validators
{
    public class AuthenticationValidator : AbstractValidator<Authentication.Request>
    {
        //contains error checking for unique names... can take out later if you want
        private readonly AppDbContext _context;
        public AuthenticationValidator(AppDbContext context)
        {
            context = _context;

            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("What's your name again?")
                .MaximumLength(25)
                .MustAsync(async (model, value, x) =>
                    {
                        return !(await _context.Users.AnyAsync(c => c.first_name == value));
                    })
                 .WithMessage("Name must be unique");
            ;
            RuleFor(a => a.Password).NotEmpty().WithMessage("Forget your password?").MaximumLength(50);
        }
    }
}
