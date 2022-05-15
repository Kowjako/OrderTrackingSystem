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
        [Display(Name = "Price", ResourceType = typeof(Properties.Resources))]
        public decimal Cena { get; set; }
        [Display(Name = "Amount", ResourceType = typeof(Properties.Resources))]
        public decimal Amount { get; set; }

        [Browsable(false)]
        public decimal Rabat { get; set; }
    }
}
