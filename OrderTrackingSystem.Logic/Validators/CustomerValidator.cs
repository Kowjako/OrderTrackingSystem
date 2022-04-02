using FluentValidation;
using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Logic.Validators
{
    public class CustomerValidator : AbstractValidator<Customers>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Age).GreaterThan((byte)0).WithMessage("Niepoprawna wartość wieku");
            RuleFor(x => x.Balance).GreaterThan(0).WithMessage("Konto nie może mieć ujemnej wartości");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Adres e-mail jest niepoprawny");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Imię nie może być puste");
            RuleFor(x => x.Surname).NotEmpty().WithMessage("Nazwisko nie może być puste");
            RuleFor(x => x.Number).NotEmpty().WithMessage("Numer nie może być pusty");
        }
    }
}
