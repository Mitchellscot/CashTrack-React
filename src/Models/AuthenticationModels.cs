using FluentValidation;

namespace CashTrack.Models.AuthenticationModels;

public class AuthenticationValidator : AbstractValidator<AuthenticationModels.Request>
{
    public AuthenticationValidator()
    {
        RuleFor(a => a.Name).NotEmpty().WithMessage("What's your name again?").MaximumLength(25);
        RuleFor(a => a.Password).NotEmpty().WithMessage("Forget your password?").MaximumLength(50);
    }
}
public class AuthenticationModels
{
    public record Request(string Name, string Password);
    //automapper requires a parameterless constructor. This does the trick.
    public record Response()
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Token { get; init; }
    };
}

