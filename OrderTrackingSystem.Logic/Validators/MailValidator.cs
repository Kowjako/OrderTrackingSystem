using FluentValidation;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Validators
{
    public class MailValidator : AbstractValidator<MailDTO>
    {
        public MailValidator()
        {
            RuleFor(p => p.Caption).Length(1, 255).WithMessage("Tytuł musi być od 0 do 255 znaków");
            RuleFor(p => p.Content).NotNull().NotEmpty().WithMessage("Treść nie może być pusta");
            RuleFor(p => p.ReceiverId).NotNull().NotEqual(-1).WithMessage("Należy wskazać odbiorcę");
        }
    }
}
