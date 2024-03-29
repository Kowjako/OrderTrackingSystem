﻿using FluentValidation;
using FluentValidation.Results;
using System;
using System.Text;

namespace OrderTrackingSystem.Logic.Validators
{
    public static class ValidatorWrapper
    {
        private static ValidationResult Result;
        public static bool IsValid => Result.IsValid;
        public static string ErrorMessage => GenerateErrorMessage();

        public static void Validate<T>(AbstractValidator<T> validator, T obj) => Result = validator.Validate(obj);

        public static bool ValidateWithResult<T>(AbstractValidator<T> validator, T obj)
        {
            Validate(validator,obj);
            return IsValid;
        }

        private static string GenerateErrorMessage()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Result.Errors.Count; i++)
            {
                sb.Append(Result.Errors[i].ErrorMessage + (i == Result.Errors.Count - 1 ? string.Empty : Environment.NewLine));
            }
            return sb.ToString();
        }
    }
}
