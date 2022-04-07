using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class ProductDTO
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Display(Name = "UniqueName", ResourceType = typeof(Properties.Resources))]
        public string Nazwa { get; set; }
        [Display(Name = "Netto", ResourceType = typeof(Properties.Resources))]
        public string Netto { get; set; }
        [Display(Name = "VAT", ResourceType = typeof(Properties.Resources))]
        public string VAT { get; set; }
        [Display(Name = "Seller", ResourceType = typeof(Properties.Resources))]
        public string Sprzedawca { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Properties.Resources))]
        public string Kategoria { get; set; }
        [Display(Name = "Discount", ResourceType = typeof(Properties.Resources))]
        public string Rabat { get; set; }

        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int SubCategoryId { get; set; }
    }
}
