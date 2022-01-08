using FluentValidation;
using OrderTrackingSystem.Logic.DataAccessLayer;

namespace OrderTrackingSystem.Logic.Validators
{
    class CustomerValidator : AbstractValidator<Customers>
    {
        public CustomerValidator()
        {
            RuleFor(x => x.Age).GreaterThan((byte)0);
            RuleFor(x => x.Balance).GreaterThan((decimal)0);
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
        }
    }
}
