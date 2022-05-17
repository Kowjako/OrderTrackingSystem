using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class SellDTO
    {
        [Display(Name= "Number", ResourceType = typeof(Properties.Resources))]
        public string Numer { get; set; }
        [Display(Name = "SendDate", ResourceType = typeof(Properties.Resources))]
        public string Data { get; set; }
        [Display(Name = "DaysToGet", ResourceType = typeof(Properties.Resources))]
        public string Dni { get; set; }
        [Display(Name = "Receiver", ResourceType = typeof(Properties.Resources))]
        public string Odbiorca { get; set; }

        [Money(2)]
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public decimal Kwota { get; set; }

        [Browsable(false)]
        public int CustomerId { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int? PickupDays { get; set; }
    }
}
