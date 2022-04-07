using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class OrderDTO
    {
        [Display(Name = "Number", ResourceType = typeof(Properties.Resources))]
        public string Numer { get; set; }
        [Display(Name = "PayType", ResourceType = typeof(Properties.Resources))]
        public string Oplata { get; set; }
        [Display(Name = "DeliveryType", ResourceType = typeof(Properties.Resources))]
        public string Dostawa { get; set; }
        [Display(Name = "Complaint", ResourceType = typeof(Properties.Resources))]
        public string Rezygnacja { get; set; }
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public string Kwota { get; set; }
        [Display(Name = "Shop", ResourceType = typeof(Properties.Resources))]
        public string Sklep { get; set; }
        [Browsable(false)]
        public int PickupId { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int CustomerId { get; set; }
    }
}
