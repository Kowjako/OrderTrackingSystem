using FluentValidation;
using OrderTrackingSystem.Logic.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderTrackingSystem.Logic.Validators
{
    public class OrderValidator : AbstractValidator<OrderDTO>
    {
        public OrderValidator()
        {
            RuleFor(x => x.PickupDTO).NotNull().WithMessage("Należy dodać punkt odbioru");
            RuleFor(x => x.Dostawa).NotEqual("-1").WithMessage("Należy wybrać typ dostawy");
            RuleFor(x => x.Oplata).NotEqual("-1").WithMessage("Należy wybrać typ opłaty");
            RuleForEach(x => x.CartProducts).SetValidator(new CartProductValidator());
        }
    }

    public class CartProductValidator : AbstractValidator<CartProductDTO>
    {
        public CartProductValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Ilość produktu musi być większa od zera");
            RuleFor(x => x.Cena).GreaterThan(0).WithMessage("Cena musi być większa od zera");
            RuleFor(x => x.Rabat).GreaterThanOrEqualTo(0).WithMessage("Rabat nie może być ujemny");
        }
    }
}
