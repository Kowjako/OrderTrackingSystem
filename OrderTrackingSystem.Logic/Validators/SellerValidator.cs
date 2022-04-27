using FluentValidation;
using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Logic.Validators
{
    public class SellerValidator : AbstractValidator<Sellers>
    {
        public SellerValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Adres e-mail jest niepoprawny");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nazwa nie może być pusta");
            RuleFor(x => x.Number).NotEmpty().WithMessage("Numer nie może być pusty");
            RuleFor(x => x.TIN).MinimumLength(10).MaximumLength(10).WithMessage("NIP musi być 10-cio cyfrowy");
        }
    }
}
