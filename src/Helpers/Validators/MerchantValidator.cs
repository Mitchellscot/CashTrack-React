using CashTrack.Models.MerchantModels;
using FluentValidation;

namespace CashTrack.Helpers.Validators
{
    public class MerchantValidator : AbstractValidator<MerchantModels.Request>
    {
        public MerchantValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(5, 100);
        }
    }
    public class AddEditMerchantValidator : AbstractValidator<AddEditMerchant>
    {
        public AddEditMerchantValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        }
    }
}
