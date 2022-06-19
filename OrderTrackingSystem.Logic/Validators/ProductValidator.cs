using FluentValidation;
using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Logic.Validators
{
    public class ProductValidator : AbstractValidator<Products>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).MinimumLength(1).MaximumLength(100).WithMessage("Nazwa powinna być długości [1-100]");
            RuleFor(x => x.PriceNetto).GreaterThan(0m).WithMessage("Cena musi być większa od zera");
            RuleFor(x => x.VAT).InclusiveBetween((byte)0, (byte)23).WithMessage("Stawka VAT musi być od [0;23] %");
            RuleFor(x => x.Weight).GreaterThan(0m).WithMessage("Waga musi być większa od zera");
            RuleFor(x => x.Discount).GreaterThanOrEqualTo((byte)0).WithMessage("Zniżka nie może być ujemna");
            RuleFor(x => x.Category).NotEqual(-1).WithMessage("Należy wybrać kategorię");
        }
    }
}
