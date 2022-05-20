using OrderTrackingSystem.Logic.HelperClasses.DTOAttributes;
using System;
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

        [ShortDateField]
        [Display(Name = "CreateDate", ResourceType = typeof(Properties.Resources))]
        public DateTime Data { get; set; }
        [Display(Name = "Buyer", ResourceType = typeof(Properties.Resources))]
        public string Nabywca { get; set; }
        [Display(Name = "Seller", ResourceType = typeof(Properties.Resources))]
        public string Sprzedawca { get; set; }

        [MoneyField(2)]
        [Display(Name = "Value", ResourceType = typeof(Properties.Resources))]
        public decimal Kwota { get; set; }

        [Browsable(false)]
        public bool IsOrder { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int CustomerId { get; set; }
    }
}
