using FluentValidation;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Validators
{
    public class VoucherValidator : AbstractValidator<VoucherDTO>
    {
        public VoucherValidator()
        {
            RuleFor(x => x.ExpireDate).GreaterThan(DateTime.Now).WithMessage("Data musi być większa do obecnej daty");
            RuleFor(x => x.Value).GreaterThan(0).WithMessage("Kwota bonu ma być większa od 0");
        }
    }
}
