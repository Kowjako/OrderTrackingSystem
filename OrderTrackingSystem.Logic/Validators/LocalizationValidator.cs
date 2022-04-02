using FluentValidation;
using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Logic.Validators
{
    class LocalizationValidator : AbstractValidator<Localizations>
    {
        public LocalizationValidator()
        {
            RuleFor(i => i.City).NotNull().NotEmpty().WithMessage("Miasto nie może być puste");
            RuleFor(i => i.Country).NotNull().NotEmpty().WithMessage("Kraj nie może być pusty");
            RuleFor(i => i.Flat).GreaterThan((byte)0).WithMessage("Numer lokalu musi być większy od 0");
            RuleFor(i => i.House).GreaterThan((byte)0).WithMessage("Numer budynku musi być wiekszy od 0");
            RuleFor(i => i.Street).NotNull().NotEmpty().WithMessage("Ulica nie może być pusta");
        }
    }
}
