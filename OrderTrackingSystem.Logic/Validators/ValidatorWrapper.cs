using FluentValidation;
using FluentValidation.Results;

namespace OrderTrackingSystem.Logic.Validators
{
    public class ValidatorWrapper
    {
        public static ValidationResult Validate<T>(AbstractValidator<T> validator, T obj)
        {
            return validator.Validate(obj);
        }
    }
}
