using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CartProductDTO
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Nazwa { get; set; }

        [MoneyField(2)]
        [Display(Name = "Price", ResourceType = typeof(Properties.Resources))]
        public decimal Cena { get; set; }

        [AmountField(2)]
        [Display(Name = "Amount", ResourceType = typeof(Properties.Resources))]
        public decimal Amount { get; set; }

        [PercentageField]
        [Browsable(false)]
        public decimal Rabat { get; set; }
    }
}
