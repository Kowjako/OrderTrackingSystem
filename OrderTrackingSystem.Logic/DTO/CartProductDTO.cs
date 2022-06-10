using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class CartProductDTO
    {
        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Name { get; set; }

        [MoneyField(2)]
        [Display(Name = "Price", ResourceType = typeof(Properties.Resources))]
        public decimal Price { get; set; }

        [AmountField(2)]
        [Display(Name = "Amount", ResourceType = typeof(Properties.Resources))]
        public decimal Amount { get; set; }

        #region Non-browsable

        [Browsable(false)]
        public int Id { get; set; }

        [PercentageField]
        [Browsable(false)]
        public decimal Discount { get; set; }

        #endregion
    }
}
