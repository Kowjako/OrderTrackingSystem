using System.ComponentModel;

namespace OrderTrackingSystem.Logic.DTO
{
    public sealed class SellDTO
    {
        public string Number { get; set; }
        [DisplayName("Data wysyłki")]
        public string Data { get; set; }
        [DisplayName("Dni do odbioru")]
        public string Dni { get; set; }
        public string Odbiorca { get; set; }
        public string Kwota { get; set; }
    }
}
