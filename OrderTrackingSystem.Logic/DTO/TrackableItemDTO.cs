using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class TrackableItemDTO
    {
        [Browsable(false)]
        public int Id { get; set; }

        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Numer { get; set; }
        [Display(Name = "CreateDate", ResourceType = typeof(Properties.Resources))]
        public string Data { get; set; }
        [Display(Name = "Buyer", ResourceType = typeof(Properties.Resources))]
        public string Nabywca { get; set; }
        [Display(Name = "Seller", ResourceType = typeof(Properties.Resources))]
        public string Sprzedawca { get; set; }
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public string Kwota { get; set; }

        [Browsable(false)]
        public bool IsOrder { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int CustomerId { get; set; }
    }
}
