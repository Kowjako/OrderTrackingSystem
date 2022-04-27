using FluentValidation;
using OrderTrackingSystem.Logic.DataAccessLayer;
using OrderTrackingSystem.Logic.DTO;

namespace OrderTrackingSystem.Logic.Validators
{
    public class LocalizationValidator : AbstractValidator<LocalizationDTO>
    {
        public LocalizationValidator()
        {
            RuleFor(i => i.Miasto).NotNull().NotEmpty().WithMessage("Miasto nie może być puste");
            RuleFor(i => i.Kraj).NotNull().NotEmpty().WithMessage("Kraj nie może być pusty");
            RuleFor(i => i.Mieszkanie).GreaterThan((byte)0).WithMessage("Numer lokalu musi być większy od 0");
            RuleFor(i => i.Budynek).GreaterThan((byte)0).WithMessage("Numer budynku musi być wiekszy od 0");
            RuleFor(i => i.Ulica).NotNull().NotEmpty().WithMessage("Ulica nie może być pusta");
        }
    }

    public class LocalizationValidatorDAL : AbstractValidator<Localizations>
    {
        public LocalizationValidatorDAL()
        {
            RuleFor(i => i.City).NotNull().NotEmpty().WithMessage("Miasto nie może być puste");
            RuleFor(i => i.Country).NotNull().NotEmpty().WithMessage("Kraj nie może być pusty");
            RuleFor(i => i.Flat).GreaterThan((byte)0).WithMessage("Numer lokalu musi być większy od 0");
            RuleFor(i => i.House).GreaterThan((byte)0).WithMessage("Numer budynku musi być wiekszy od 0");
            RuleFor(i => i.Street).NotNull().NotEmpty().WithMessage("Ulica nie może być pusta");
        }
    }
}
