using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class SellDTO
    {
        public string Numer { get; set; }
        [DisplayName("Data wysyłki")]
        public string Data { get; set; }
        [DisplayName("Dni do odbioru")]
        public string Dni { get; set; }
        public string Odbiorca { get; set; }
        public string Kwota { get; set; }
        [Browsable(false)]
        public int CustomerId { get; set; }
        [Browsable(false)]
        public int SellerId { get; set; }
        [Browsable(false)]
        public int? PickupDays { get; set; }
    }
}
